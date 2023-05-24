# WebApp

This is a Blazor Server web app that interacts with Azure IoT Hub to control and monitor devices. It provides a user interface to manage multiple devices and perform actions such as turning on/off automatic watering and displaying device measurements.

## Index page

It is responsible for displaying the list of devices and initializing the `ServiceClient` to communicate with Azure IoT Hub.

### Initialization

The `OnInitialized` method is called when the component is initialized. In this method, the `ServiceClient` is created using the provided IoT Hub connection string and also the device IDs are retrieved from the configuration file and stored in the `deviceIds` property. 

## DeviceCard component

This component represents a card that displays device information, allows control of device functionalities, and displays measurements.

### Properties

- `[Parameter] DeviceId`: The ID of the device associated with the card.
- `[Parameter] ServiceClient`: The `ServiceClient` instance used for communication with Azure IoT Hub.

### Initialization

The `OnParametersSetAsync` method is called whenever the component's parameters are set or updated. In this method, the information about the state whether the automatic watering is on or off is retrieved from the device.

### Methods

- `PlotLastMeasurementsAsync()`: This method retrieves the last measurements from the device and plots the measurements graph within the component.
- `TurnAutomaticWateringOnAsync()`: This method sends a command to turn on automatic watering for the associated device.
- `TurnAutomaticWateringOffAsync()`: This method sends a command to turn off automatic watering for the associated device.

## Usage

Before running the application you need to provide IoT Hub connection string to the User Secrets. You can do so in Visual Studio by right clicking on the project and clicking on Manage User Secrets. The file will open where IoT Hub connection string can be added.

The file should look like this:

```json
{
	"iotHubConnectionString": "your_iot_hub_connection_string"
}
```

Next you should open appsettings.json and add your device ids:
```json
"deviceIds": [
    "your_device_id",
    "your_another_device_id"
]
```

After this, if the devices are running, you can start the web app.
