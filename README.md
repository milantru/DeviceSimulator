# IoT Device Simulator

This project was made for the [Smart IoT Devices course (NSWI180)](https://is.cuni.cz/studium/predmety/index.php?do=predmet&kod=NSWI180).

It is a project for simulating devices capable of measuring soil moisture. The devices communicate with a web application through cloud service, specifically [Azure IoT Hub](#azure-iot-hub).

The devices periodically measure the soil moisture. If the measured value is below a defined threshold, the irrigation system is activated. The automatic irrigation can be turned on/off from the web application. If it is turned off, even if the measured value is below the threshold, the irrigation will not be triggered. The web application also allows requesting the devices' last measured values, which are displayed in the form of a graph.

The project consists of three components:

1. [DeviceLibrary](./DeviceLibrary/README.md): Contains classes used for the simulation of the running devices.
2. [DeviceSimulator](./DeviceSimulator/README.md): Executes the simulation of the running devices.
3. [WebApp](./WebApp/README.md): Web application providing interface for displaying measured data and for communicating with the devices.

## Azure IoT Hub

The communication between the devices and the web application is facilitated through the cloud service, , which functions based on the request-response principle. In the devices, we register methods that can be called from external sources, such as the web application. The web application can request the invocation of a specific method on a device, e.g. retrieving the last measured values. The method is executed on the device, and the response is returned (in the example's case, the last measured values).

## How to run it

Firstly, please read and follow the instructions in Usage section of both the [DeviceSimulator README](./DeviceSimulator/README.md) and the [WebApp README](./WebApp/README.md).

After that, you can in Visual Studio right click on DeviceSimulator project, click on Debug and then choose either Start New Instance or Start Without Debugging. The simulation of the running devices will start, and you should see a console with the output of your devices.

After that, it is recommended to wait approx. 3 minutes for the device methods to successfully register in the cloud.

Next, you can start the web application (WebApp project) the same way you started the DeviceSimulator.
