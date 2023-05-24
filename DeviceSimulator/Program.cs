using DeviceLibrary;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets("1e7e8041-8ea9-4c2b-af36-f3e54580cb94")
    .Build();

var deviceConnectionStrings = config.GetSection("deviceConnectionStrings")
    .GetChildren()
    .Select(x => x.Value)
    .ToList();

var deviceManager = new DeviceManager();

await deviceManager.TurnOnDeviceAsync(deviceConnectionStrings);

// Keep the simulation alive
Console.ReadLine();

await deviceManager.TurnOffDevices();
