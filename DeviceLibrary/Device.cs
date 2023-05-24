using Microsoft.Azure.Devices.Client;
using System.Text;

namespace DeviceLibrary
{
	/// <summary>
	/// Represents a device with capabilities of measuring the soil moisture and automatic watering.
	/// </summary>
	internal class Device : IAsyncDisposable
    {
        private readonly int humidityThresholdForWatering = 500; // if humidity is less than this value, watering may start
        private readonly int lastMeasurementsMaxCount = 42;
        private readonly TimeSpan wateringDuration = TimeSpan.FromSeconds(2);
        private readonly TimeSpan measurementsInterval = TimeSpan.FromSeconds(5);
        private Queue<int> lastMeasurements = new();
        private Random random = new();
        private bool isAutomaticWateringOn = true;
        private bool isOn;
        private string connectionString;
        private DeviceClient deviceClient;

		/// <summary>
		/// The ID of the device.
		/// </summary>
		internal string Id { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Device"/> class.
		/// </summary>
		/// <param name="connectionString">The device connection string.</param>
		/// <param name="deviceClient">The device client.</param>
		private Device(string connectionString, DeviceClient deviceClient)
        {
            this.connectionString = connectionString;
            this.deviceClient = deviceClient;

            if (!TryExtractDeviceId(this.connectionString, out string deviceIdTmp))
            {
                throw new Exception(
                    $"Provided parameter {nameof(connectionString)} does not include device id or is malformed.");
            }
            Id = deviceIdTmp;
        }

		public async ValueTask DisposeAsync()
        {
            await deviceClient.CloseAsync();

            await deviceClient.DisposeAsync();

            // doc string in Dispose says that DisposeAsync should be called before it
            deviceClient.Dispose();
        }

		/// <summary>
		/// Creates a device instance from the specified device connection string asynchronously.
		/// </summary>
		/// <param name="connectionString">The device connection string.</param>
		/// <returns>A task representing the asynchronous operation. The task result contains the created device instance.</returns>
		internal static async Task<Device> CreateDeviceFromAsync(string connectionString)
        {
            var deviceClient = DeviceClient.CreateFromConnectionString(connectionString);

            var device = new Device(connectionString, deviceClient);

            await deviceClient.SetMethodHandlerAsync(
                methodName: "GetLastMeasurementsAsync", 
                methodHandler: async (_, _) => await device.GetLastMeasurementsAsync(), 
                userContext: null
            );
            await deviceClient.SetMethodHandlerAsync(
                methodName:"TurnAutomaticWateringOnAsync", 
                methodHandler: async (_, _) => await device.TurnAutomaticWateringOnAsync(), 
                userContext: null
            );
            await deviceClient.SetMethodHandlerAsync(
                methodName:"TurnAutomaticWateringOffAsync", 
                methodHandler: async (_, _) => await device.TurnAutomaticWateringOffAsync(), 
                userContext: null
            );
            await deviceClient.SetMethodHandlerAsync(
                methodName:"GetIsAutomaticWateringOnAsync", 
                methodHandler: async (_, _) => await device.GetIsAutomaticWateringOnAsync(), 
                userContext: null
            );

            return device;
        }

		/// <summary>
		/// Turns on the device.
		/// </summary>
		internal void TurnOn()
        {
            TurnOnAsync();
        }

		/// <summary>
		/// Turns off the device asynchronously.
		/// </summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		internal async Task TurnOffAsync()
        {
            isOn = false;

            await Console.Out.WriteLineAsync($"Device '{Id}' has been turned OFF");

            await deviceClient.CloseAsync();
        }

		private async void TurnOnAsync()
        {
            await Task.Run(async () =>
            {
                isOn = true;

                await Console.Out.WriteLineAsync($"Device '{Id}' has been turned ON");

                while (isOn)
                {
                    var measuredValue = GetMeasuredValue();

                    await Console.Out.WriteLineAsync($"Device '{Id}' has measured value: {measuredValue}");

                    AddToLastMeasurements(measuredValue);

                    if (isAutomaticWateringOn && measuredValue < humidityThresholdForWatering)
                    {
                        await Console.Out.WriteLineAsync(
                            $"Device '{Id}' has started watering (this will take " +
                            $"{wateringDuration.TotalSeconds} {(wateringDuration.TotalSeconds == 1 ? "second" : "seconds")})."
                        );

                        await Task.Delay(wateringDuration);
                    }

                    await Task.Delay(measurementsInterval);
                }
            });
        }

        private void AddToLastMeasurements(int measuredValue)
        {
            if (lastMeasurements.Count >= lastMeasurementsMaxCount)
            {
                lastMeasurements.Dequeue();
            }

            lastMeasurements.Enqueue(measuredValue);
        }

        private int GenerateRandomNumber() => random.Next(maxValue: 1024);

        private int GetMeasuredValue() => GenerateRandomNumber();

        private static bool TryExtractDeviceId(string connectionString, out string deviceId)
        {
            deviceId = "";

            var connectionStringParts = connectionString.Split(';');

            foreach (var part in connectionStringParts)
            {
                if (part.Contains("DeviceId="))
                {
                    var keyAndValue = part.Split("=");
                    if (keyAndValue.Count() != 2)
                    {
                        return false;
                    }

                    deviceId = keyAndValue.ElementAt(1);
                    return true;
                }
            }

            return false;
        }

        private MethodResponse PrepareMethodResponseWithPayload(object? payload)
        {
            var responseJson = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            var response = new MethodResponse(
                result: Encoding.UTF8.GetBytes(responseJson),
                status: 200
            );

            return response;
        }

        private async Task<MethodResponse> GetLastMeasurementsAsync()
        {
            var response = PrepareMethodResponseWithPayload(lastMeasurements.ToList());

            return await Task.FromResult(response);
        }

        private async Task<MethodResponse> TurnAutomaticWateringOnAsync()
        {
            var response = new MethodResponse(status: 200);

            if (isAutomaticWateringOn)
            {
                await Console.Out.WriteLineAsync($"Device '{Id}' is requested to turn ON automatic watering, but it is already turned ON.");

                return await Task.FromResult(response);
            }
            else
            {
                isAutomaticWateringOn = true;

                await Console.Out.WriteLineAsync($"Automatic watering of the device with id '{Id}' has been turned ON.");

                return await Task.FromResult(response);
            }
        }

        private async Task<MethodResponse> TurnAutomaticWateringOffAsync()
        {
            var response = new MethodResponse(status: 200);

            if (!isAutomaticWateringOn)
            {
                await Console.Out.WriteLineAsync($"Device '{Id}' is requested to turn OFF automatic watering, but it is already turned OFF.");

                return await Task.FromResult(response);
            }
            else
            {
                isAutomaticWateringOn = false;

                await Console.Out.WriteLineAsync($"Automatic watering of the device '{Id}' has been turned OFF.");

                return await Task.FromResult(response);
            }
        }

        private async Task<MethodResponse> GetIsAutomaticWateringOnAsync()
        {
            var resposne = PrepareMethodResponseWithPayload(isAutomaticWateringOn);

            return await Task.FromResult(resposne);
        }
    }
}
