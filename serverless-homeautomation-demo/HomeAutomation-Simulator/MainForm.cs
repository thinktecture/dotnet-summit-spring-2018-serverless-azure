using HomeAutomation.Common;
using HomeAutomationSimulator.Properties;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeAutomation.Simulator
{
    public partial class MainForm : Form
    {
        private string _hubDeviceId;
        private string _deviceConnectionString;
        private DeviceClient _client = null;
        private bool _connected;

        private string _currentCommand;
        private int _currentPosition;

        private Stopwatch _stopWatch;
        private int _maxTimeToFinish;

        private System.Timers.Timer _messageSendingTimer = new System.Timers.Timer(10000);

        public MainForm()
        {
            InitializeComponent();

            _hubDeviceId = Settings.Default.HubDeviceId;
            _deviceConnectionString = Settings.Default.IotHubConnectionString;
            _maxTimeToFinish = Settings.Default.MaxTimeToFinish;
            lblDevice.Text = _hubDeviceId;

            _stopWatch = new Stopwatch();

            _messageSendingTimer.Elapsed += _messageSendingTimer_Elapsed;
        }

        private void _messageSendingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AppendToLog("New climate data sent.");
            SendMessageToIotHub();
        }

        private void SendMessageToIotHub()
        {
            var climateData = new ClimateData
            {
                DeviceId = _hubDeviceId,
                MessageId = 42,
                DateTime = DateTime.UtcNow.ToLongTimeString(),
                Humidity = new Random().Next(55, 70),
                Temperature = new Random().Next(19, 29)
            };

            var climateDataJson = JsonConvert.SerializeObject(climateData);
            var rawClimateData = Encoding.UTF8.GetBytes(climateDataJson);
            var message = new Microsoft.Azure.Devices.Client.Message(rawClimateData);

            _client.SendEventAsync(message);
        }

        private void AppendToLog(string logText)
        {
            if (InvokeRequired)
            {
                tbLogs.Invoke((MethodInvoker)delegate
                {
                    UpdateLog(logText);
                });
            }
            else
            {
                UpdateLog(logText);
            }
        }

        private void UpdateLog(string logText)
        {
            if (tbLogs.Text.Length == 0)
            {
                tbLogs.Text = logText;
            }
            else
            {
                tbLogs.AppendText("\r\n" + logText);
            }
        }

        private async void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_connected)
                {
                    AppendToLog("Connecting to IoT Hub...");

                    _client = DeviceClient.CreateFromConnectionString(_deviceConnectionString,
                        TransportType.Mqtt_Tcp_Only);

                    await _client.SetMethodHandlerAsync(MethodNames.GetBuildingInfo, GetBuildingInfo, null);
                    await _client.SetMethodHandlerAsync(MethodNames.ControlBlinds, ControlCommand, null);
                    await _client.SetMethodHandlerAsync(MethodNames.GetDeviceState, GetDeviceState, null);
                    await _client.SetMethodHandlerAsync(MethodNames.ControlClimateData, ControlClimateData, null);

                    AppendToLog("Connected to IoT Hub :-)");

                    _connected = !_connected;
                    buttonConnect.Text = "Disconnect";
                }
                else
                {
                    AppendToLog("Disconnecting from IoT Hub...");

                    await _client.SetMethodHandlerAsync(MethodNames.GetBuildingInfo, null, null);
                    await _client.SetMethodHandlerAsync(MethodNames.ControlBlinds, null, null);
                    await _client.SetMethodHandlerAsync(MethodNames.GetDeviceState, null, null);
                    await _client.SetMethodHandlerAsync(MethodNames.ControlClimateData, null, null);

                    await _client.CloseAsync();

                    AppendToLog("Disconnected from IoT Hub!");

                    _connected = !_connected;
                    buttonConnect.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                AppendToLog(String.Format("ERROR: {0}", ex.Message));
            }
        }

        private Task<MethodResponse> ControlClimateData(MethodRequest methodRequest, object userContext)
        {
            var request = JsonConvert.DeserializeObject<ControlClimateDataRequest>(methodRequest.DataAsJson);

            if (request.Command == "start")
            {
                panelClimateControlStatus.BackColor = Color.Green;
                AppendToLog("START sending climate data...");
                _messageSendingTimer.Start();
            }
            else
            {
                panelClimateControlStatus.BackColor = Color.Red;
                AppendToLog("STOP sending climate data...");
                _messageSendingTimer.Stop();
            }

            return Task.FromResult(new MethodResponse(200));
        }

        private Task<MethodResponse> GetBuildingInfo(MethodRequest methodRequest, object userContext)
        {
            AppendToLog("GetBuildingInfo - called...");

            var message = new GetBuildingInfoResponse()
            {
                Rooms = new List<Room>()
                {
                    new Room
                    {
                        Name = "Living room",
                        Devices = new List<Device>()
                        {
                            new Device()
                            {
                                Name = "livingroom-blinds"
                            },
                            new Device()
                            {
                                Name = "livingroom-lights"
                            }
                        }
                    },
                    new Room
                    {
                        Name = "Sleeping room",
                        Devices = new List<Device>()
                        {
                            new Device()
                            {
                                Name = "sleepingroom-lights"
                            }
                        }
                    }
                }
            };

            var result = JsonConvert.SerializeObject(message);
            var resultData = Encoding.UTF8.GetBytes(result);

            // TODO: what about error cases?

            AppendToLog("GetBuildingInfo - returning response...");

            return Task.FromResult(new MethodResponse(resultData, 200));
        }

        private Task<MethodResponse> ControlCommand(MethodRequest methodRequest, object userContext)
        {
            AppendToLog("ControlCommand - called...");

            var request = JsonConvert.DeserializeObject<ControlBlindsRequest>(methodRequest.DataAsJson);
            _currentCommand = request.Command.ToLowerInvariant();

            lblChannel.Invoke((MethodInvoker)delegate
            {
                lblChannel.Text = request.Device;
            });

            lblCommand.Invoke((MethodInvoker)delegate
            {
                if (_currentCommand == "up")
                {
                    lblCommand.Text = "\u2B06";
                }
                else
                {
                    lblCommand.Text = "\u2B07";
                }
            });

            switch (_currentCommand)
            {
                case "up":
                    {
                        if (!_stopWatch.IsRunning)
                        {
                            _stopWatch.Start();
                        }

                        break;
                    }

                case "down":
                    {
                        if (!_stopWatch.IsRunning)
                        {
                            _stopWatch.Start();
                        }

                        break;
                    }

                case "stop":
                    {
                        if (_stopWatch.IsRunning)
                        {
                            _stopWatch.Stop();
                        }

                        break;
                    }

                default:
                    break;
            }

            var message = new ControlBlindsResponse()
            {
                Device = request.Device,
                State = true
            };

            var result = JsonConvert.SerializeObject(message);
            var resultData = Encoding.UTF8.GetBytes(result);

            // TODO: what about error cases?

            AppendToLog("ControlCommand - returning response...");

            return Task.FromResult(new MethodResponse(resultData, 200));
        }

        private Task<MethodResponse> GetDeviceState(MethodRequest methodRequest, object userContext)
        {
            AppendToLog("GetDeviceState - called...");

            var request = JsonConvert.DeserializeObject<GetDeviceStateRequest>(methodRequest.DataAsJson);

            lblChannel.Invoke((MethodInvoker)delegate
            {
                lblChannel.Text = request.Device;
            });

            var elapsed = _stopWatch.ElapsedMilliseconds;

            if (elapsed >= _maxTimeToFinish)
            {
                _stopWatch.Stop();
                _stopWatch.Reset();

                elapsed = 0;

                _currentPosition = _currentCommand == "up" ? 100 : 0;
            }
            else if (elapsed > 0)
            {
                switch (_currentCommand)
                {
                    case "up":
                        {
                            _currentPosition = (int)((((float)elapsed / _maxTimeToFinish)) * 100);
                            break;
                        }

                    case "down":
                        {
                            _currentPosition = (int)((1 - ((float)elapsed / _maxTimeToFinish)) * 100);
                            break;
                        }

                    case "stop":
                        {
                            // TODO: how complex do we want to make it?
                            // ...
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }

            var message = new GetDeviceStateResponse()
            {
                Device = request.Device,
                Position = _currentPosition
            };

            var result = JsonConvert.SerializeObject(message);
            var resultData = Encoding.UTF8.GetBytes(result);

            // TODO: what about error cases?

            AppendToLog("GetChannelState - returning response...");

            if (_currentPosition > 0 && _currentPosition < 100)
            {
                return Task.FromResult(new MethodResponse(resultData, 202));
            }

            return Task.FromResult(new MethodResponse(resultData, 200));
        }
    }
}
