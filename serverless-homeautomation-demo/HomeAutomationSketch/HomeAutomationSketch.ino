#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>
#include <WiFiUdp.h>

#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>

#include "config.h"

static bool messagePending = false;
static bool messageSending = false;

const char *connectionString = ""; // Connectionstring to IoT Hub
const char *ssid = ""; // SSID of Wifi
const char *pass = ""; // Password of Wifi

static int interval = INTERVAL;

void initSerial()
{
	Serial.begin(115200);
	Serial.setDebugOutput(true);
	Serial.println("Serial successfully initiated.");
}

void initWifi()
{
	Serial.printf("Attempting to connect to SSID: %s.\r\n", ssid);

	WiFi.mode(WIFI_STA);
	WiFi.begin(ssid, pass);

	Serial.printf("Connected to wifi %s.\r\n", ssid);
}

void initTime()
{
	time_t epochTime;
	configTime(0, 0, "pool.ntp.org", "time.nist.gov");

	while (true)
	{
		epochTime = time(NULL);

		if (epochTime == 0)
		{
			Serial.println("Fetching NTP epoch time failed! Waiting 2 seconds to retry.");
			delay(2000);
		}
		else
		{
			Serial.printf("Fetched NTP epoch time is: %lu.\r\n", epochTime);
			break;
		}
	}
}

static IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;

void setup()
{
	pinMode(LED_PIN, OUTPUT);

	initSerial();
	delay(2000);

	initWifi();
	initTime();
	initSensor();

	iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(connectionString, MQTT_Protocol);

	if (iotHubClientHandle == NULL)
	{
		Serial.println("Failed on IoTHubClient_CreateFromConnectionString.");
		while (1);
	}

	IoTHubClient_LL_SetMessageCallback(iotHubClientHandle, receiveMessageCallback, NULL);
	IoTHubClient_LL_SetDeviceMethodCallback(iotHubClientHandle, deviceMethodCallback, NULL);
	IoTHubClient_LL_SetDeviceTwinCallback(iotHubClientHandle, twinCallback, NULL);
}

static int messageCount = 1;
unsigned long previousMillis = 0;

void loop()
{
	unsigned long currentMillis = millis();

	if (currentMillis - previousMillis >= interval)
	{
		previousMillis = currentMillis;

		if (!messagePending && messageSending)
		{
			char messagePayload[MESSAGE_MAX_LEN];
			bool temperatureAlert = readMessage(messageCount, messagePayload);

			sendMessage(iotHubClientHandle, messagePayload, temperatureAlert);

			messageCount++;
		}
	}

	IoTHubClient_LL_DoWork(iotHubClientHandle);
	delay(10); // <- fixes some issues with WiFi stability
}
