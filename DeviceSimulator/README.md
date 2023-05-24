# DeviceSimulator

The DeviceSimulator project is an application that utilizes the [DeviceLibrary](../DeviceLibrary/README.md) class library to simulate and manage devices capable of measuring soil moisture and performing automatic watering. It reads device connection strings from a configuration source and uses the DeviceManager class to turn on and off the devices.

## Usage

You can right click on the project in Visual Studio and click on Manage User Secrets. The file will open where device connection strings can be added.

The file can look like this:

```json
{
	"deviceConnectionStrings": [
		"your_device_connection_string",
		"your_another_device_connection_string"
	]
}
```

Then you can run the program. The DeviceSimulator will turn on the devices based on the provided connection strings and keep the simulation alive until you press Enter. Upon pressing Enter, the devices will be turned off.
