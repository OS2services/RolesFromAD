using Serilog;
using System;
using System.Dynamic;
using Newtonsoft.Json;
using WebSocketSharp;
using Aula_Dagtilbud_AD_Integration.ActiveDirectory;

namespace Aula_Dagtilbud_AD_Integration.WebSockets
{
    public class WSCommunication
    {
        private static string VERSION = "1.0.0";

        private WebSocket socket;

        public ADStub adStub { get; set; }

        public ILogger Logger { get; set; }

        public WSCommunication()
        {
            socket = new WebSocket(Settings.GetStringValue("webSocketUrl"));
        }

        internal void Connect()
        {
            if (!socket.IsAlive)
            {
                Logger.Information("Attempting to connect");
                socket.Connect();
            }
        }

        internal void Disconnect()
        {
            socket.Close();
        }

        public void Init()
        {
            socket.OnMessage += (sender, e) =>
            {
                if (e.IsText)
                {
                    try
                    {
                        dynamic message = JsonConvert.DeserializeObject(e.Data);

                        LogRequest(message);

                        if (!ValidateMessage(message))
                        {
                            Logger.Error("Invalid signature on message: " + message.transactionUuid);
                            return;
                        }

                        string command = (string)message.command;
                        switch (command)
                        {
                            case "AUTHENTICATE":
                                Reply((string)message.transactionUuid, (string)message.command, true);
                                break;
                            case "GET_USERS_WITH_ROLE":
                                Reply((string)message.transactionUuid, (string)message.command, true, adStub.GetUsersWithRole((string)message.payload));
                                break;
                            default:
                                Logger.Error("Unknown request: " + message.command);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Get error on request");
                    }
                }
            };

            socket.OnClose += (sender, e) =>
            {
                Logger.Information("Connection closed by other side");
            };

            Connect();
        }

        private bool ValidateMessage(dynamic message)
        {
            string command = (string)message.command;

            switch (command)
            {
                case "AUTHENTICATE":
                    {
                        string mac = HMacUtil.Encode(((string)message.transactionUuid + "." + (string)message.command));
                        return Equals(mac, (string)message.signature);
                    }
                case "GET_USERS_WITH_ROLE":
                    {
                        string mac = HMacUtil.Encode(((string)message.transactionUuid + "." + (string)message.command + "." + (string)message.payload));
                        return Equals(mac, (string)message.signature);
                    }
                default:
                    Logger.Information("Unknown command: " + (string)message.command);
                    break;
            }

            return false;
        }

        internal void Reply(string transactionUuid, string command, bool valid, dynamic payload = null)
        {
            dynamic response = new ExpandoObject();
            response.transactionUuid = transactionUuid;
            response.command = command;
            response.status = (valid) ? "true" : "false";
            response.clientVersion = VERSION;
            response.signature = HMacUtil.Encode(transactionUuid + "." + command + "." + (valid ? "true" : "false"));
            response.payload = payload;

            socket.Send(JsonConvert.SerializeObject(response));

            LogResponse(response);
        }

        private void LogResponse(dynamic response)
        {
            Logger.Information("Response for " + response.command + " (" + response.transactionUuid + "): result=" + response.status);
        }

        private void LogRequest(dynamic request)
        {
            Logger.Information("Request for " + request.command + " (" + request.transactionUuid + ")");
        }
    }
}
