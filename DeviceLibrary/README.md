# DeviceLibrary

The DeviceLibrary is a class library that provides functionality for managing devices capable of measuring soil moisture and performing automatic watering. It includes the following classes:

## Device

The `Device` class represents a device with the ability to measure soil moisture and perform automatic watering.

### Properties

- `Id` (string): The ID of the device.

### Constructors

- `Device(string connectionString, DeviceClient deviceClient)`: Initializes a new instance of the Device class with the specified device connection string and device client.

### Methods

- `TurnOn()`: Turns on the device.
- `TurnOffAsync()`: Turns off the device asynchronously.
- `DisposeAsync()`: Disposes of the device resources asynchronously.

### Static Methods
- `CreateDeviceFromAsync(string connectionString)`: Creates a device instance from the specified device connection string asynchronously.

## DeviceManager

The `DeviceManager` class manages devices and provides methods to turn them on and off.

### Methods

- `TurnOnDeviceAsync(string deviceConnectionString)`: Turns on a device asynchronously using the specified device connection string.
- `TurnOnDeviceAsync(IList<string> deviceConnectionStrings)`: Turns on multiple devices asynchronously using the specified device connection strings.
- `TurnOffDevicesAsync()`: Turns off all the devices managed by the device manager asynchronously.

## Usage

To use the DeviceLibrary, follow these steps:

1. Add a reference to the DeviceLibrary in your project.
2. Use the `DeviceManager` class to manage devices and perform operations such as turning them on and off.

Example:

```csharp
// Create an instance of DeviceManager
var deviceManager = new DeviceManager();

// Turn on a device
var deviceConnectionString = "your_device_connection_string";
await deviceManager.TurnOnDeviceAsync(deviceConnectionString);

// Keep the device running
Console.ReadLine();

// Turn off all devices
await deviceManager.TurnOffDevices();
```

**WARNING**: Even after the method `TurnOnDeviceAsync` was awaited successfully it may take some time (approx. 3 minutes) to register device methods in the cloud.

Also please ensure that you have the necessary dependencies, such as `Microsoft.Azure.Devices.Client`, installed in your project before using the DeviceLibrary.

The library was made with the idea of soil moisture measuring devices in mind, but with a little change of code this behavior can be changed. Feel free to explore the DeviceLibrary and customize it according to your specific requirements.
