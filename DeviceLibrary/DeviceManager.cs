namespace DeviceLibrary
{
	/// <summary>
	/// Manages devices and provides methods to turn them on and off.
	/// </summary>
	public class DeviceManager
    {
        private List<Device> devices = new();

		/// <summary>
		/// Turns on a device asynchronously using the specified device connection string.
		/// </summary>
        /// <para>
        /// <remarks>
        /// Even though the method was awaited, it may take some minute until the device methods are successfully 
        /// registered and ready to be used.
        /// </remarks>
        /// </para>
		/// <param name="deviceConnectionString">The device connection string.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task TurnOnDeviceAsync(string deviceConnectionString)
        {
            var device = await Device.CreateDeviceFromAsync(deviceConnectionString);

            device.TurnOn();

            devices.Add(device);
        }

		/// <summary>
		/// Turns on multiple devices asynchronously using the specified device connection strings.
		/// </summary>
		/// <para>
		/// <remarks>
		/// Even though the method was awaited, it may take some minute until the device methods are successfully 
		/// registered and ready to be used.
		/// </remarks>
		/// </para>
		/// <param name="deviceConnectionStrings">The connection strings of the devices.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task TurnOnDeviceAsync(IList<string> deviceConnectionStrings)
        {
            foreach (var connectionString in deviceConnectionStrings)
            {
                var device = await Device.CreateDeviceFromAsync(connectionString);

                device.TurnOn();

                devices.Add(device);
            }
        }

		/// <summary>
		/// Turns off all the devices (and frees the resources) managed by the device manager asynchronously.
		/// </summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		public async Task TurnOffDevices()
        {
            for (int i = devices.Count - 1; i >= 0; i--)
            {
                var device = devices.ElementAt(i);

                await device.TurnOffAsync();

                await device.DisposeAsync();

                devices.RemoveAt(i);
            }
        }
    }
}
