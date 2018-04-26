# Demo application for a Serverless IoT home automation showcase from the session at JAX 2018 in Mainz, Germany.

This involves

- Azure Functions
- Azure Cosmos DB
- Azure Storage
- Azure Event Grid
- Azure Event Hub
- Azure IoT Hub
- Azure Application Insights
- Azure Notification Hub 
- plus an Angular 5 app for the browser, Cordova, Electron and as a PWA. 
    
Oh, and an ESP8266 Arduino-compatible sketch.

# Configuration
In order to be able to run the demo application, you need to setup all required Azure Resources. After that you need to edit the configuration files

- HomeAutomation-App\src\environments\environment.*.ts
- HomeAutomation-Functions\local.settings.json
- HomeAutomation-Simulator\App.config


# Structure

## HomeAutomation-App
Contains the Angular 5 application. Requires Node.js. Use `npm i` and `npm run start`.

## HomeAutomation-Common 
Contains common C# code that is shared between projects.

## HomeAutomation-Functions
Contains the Azure Functions C# code.

## HomeAutomation-Simulator
Contains an old school Windows Forms application (yes, all the code is in MainForm.cs :-)) and acts as an IoT device simulator.

## HomeAutomationSketch
Contains an Ardunio Sketch which can be used with Arduino IDE.

## IdentityServer4.Demo 
Contains a demo IdentityServer 4 used by the client application and the functions.

# Contact us
Christian Weyer 

- Mail: christian.weyer@thinktecture.com
- Twitter: https://twitter.com/ChristianWeyer
