using DeviceLibrary;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly())
    .Build();

var deviceConnectionStrings = config.GetSection("deviceConnectionStrings")
    .GetChildren()
    .Select(x => x.Value)
    .ToList();

var deviceManager = new DeviceManager();

await deviceManager.TurnOnDeviceAsync(deviceConnectionStrings);

// Keep the simulation alive
Console.ReadLine();

await deviceManager.TurnOffDevicesAsync();
