# ComfileTech.ComfilePi.CP_IO22_A4_2.Demo

This is a .NET Framework Winforms application to demonstrate features of the [CP-IO22-A4-2](https://comfiletech.com/raspberry-pi-panel-pc/cp-io22-a4-2-digital-analog-i-o-board-for-the-cpi-s-series/) IO accessory board for COMFILE Technology's ComfilePi industrial touchscreen panel PCs, and program it.

Execution on a ComfilePi panel PC requires [Mono](https://gitlab.winehq.org/mono/mono), which should already be installed by default on the ComfilePi panel PCs.

This application uses the following .NET libraries to control the GPIO and I2C devices on the CP-IO22-A4-2 board.
* [System.Device.Gpio](https://www.nuget.org/packages/System.Device.Gpio/)
* [Iot.Device.Bindings](https://www.nuget.org/packages/Iot.Device.Bindings/)

Although the `ComfileTech.ComfilePi.CP_IO22_A4_2.Demo` project is a .NET Framework Winforms application, the `ComfileTech.ComfilePi.CP_IO22_A4_2` project is a .NET Standard library, so it can also be used in more recent .NET 8+ applications.

## Deploying to a ComfilePi Panel PC

The `ComfileTech.ComfilePi.CP_IO22_A4_2.Demo` project has 2 different launch profiles, which can be found in the `Properties/launchSettings.json` file.  
* The `Windows` profile is for running the application on a Windows development machine to verify the appearance of the application. 
* The `ComfilePi` profile will deploy the application to a ComfilePi panel PC over SSH.  

The ComfilePi panel PC must have SSH enabled.  Adjust the `ComfilePiUser` and `ComfilePiIpAddress` properties on the `ComfileTech.ComfilePi.CP_IO22_A4_2.Demo.csroj` file to adjust the deployment for your ComfilePi's unique configuration.

## Debugging the Application on a ComfilePi Panel PC

Debugging mono applications running on a ComfilePi panel PC from within Visual Studio is not currently possible without 3rd party Visual Studio extensions, however, the `ComfilePi` launch profile should provide a convenient development experience from within Visual Studio.
