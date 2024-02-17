using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebexMessageExtractor {
    public static class WebexMessageExtractorMain {
        private static Regex _nonAlphaNumeric = new Regex(@"[^a-z0-9]");
        public static void Start(IConfigurationRoot config) {
            //LoggingHelper.InitializeLogFile();
            LoggingHelper.InitializeErrorLogFile();

            HelperClass.BearerToken = config.GetSection("BearerToken").Value;
            if(string.IsNullOrEmpty(HelperClass.BearerToken)) {
                Console.WriteLine("Please enter your Personal Access Token, you will get it from https://developer.webex.com/docs/getting-your-personal-access-token");
                string token = Console.ReadLine();
                HelperClass.BearerToken = token;
            }

            if(string.IsNullOrEmpty(HelperClass.BearerToken)) {
                Console.WriteLine("Personal Access token is mandatory for execution");
                return;
            }

            if(int.TryParse(config.GetSection("MessageCountLimit").Value, out int maxMessageCount))
                HelperClass.MaxMessageCount = maxMessageCount;

            if(DateTime.TryParseExact(config.GetSection("MinimumRecordingDate").Value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime minimumRecordingDate))
                HelperClass.MinimumRecordingDate = minimumRecordingDate;

            if(DateTime.TryParseExact(config.GetSection("MaximumRecordingDate").Value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime maximumRecordingDate))
                HelperClass.MaximumRecordingDate = maximumRecordingDate;

            if(int.TryParse(config.GetSection("MaximumRecordingDataCount").Value, out int maximumRecordingDataCount))
                HelperClass.MaximumRecordingDataCount = maximumRecordingDataCount;

            HelperClass.SetProtocol();

            IList<string> menus = new List<string>();
            IList<string> scripts = new List<string>();

            if(Directory.Exists("ui\\_resources") == false)
                Directory.CreateDirectory("ui\\_resources");


            LoggingHelper.Log("Fetching recordings");
            string recordingData = HelperClass.GetRecordings();
            if(string.IsNullOrEmpty(recordingData) == false) {
                string variableName = "recordingdata";
                string fileName = variableName + ".js";
                recordingData = $"var {variableName} = {recordingData}";
                File.WriteAllText($"ui\\_resources\\{fileName}", recordingData);
                menus.Add($"<div class='left-panel-menu' onclick=\"renderRecordings(this, '{variableName}')\"><a>Recordings</a></div>");
                scripts.Add($"<script async src='_resources\\{fileName}'></script>");

                LoggingHelper.Log("Recording data exported");
            }

            LoggingHelper.Log("Fetching rooms");
            IDictionary<string, string> rooms = HelperClass.GetRooms();
            LoggingHelper.Log($"{rooms.Count} chat room found");
            if(rooms.Count == 0) {
                Console.WriteLine("Please check error log file to see what went wrong");
            }

            int counter = 1;
            int index = 1;
            foreach(string roomId in rooms.Keys) {
                string roomName = rooms[roomId];
                LoggingHelper.Log($"fetching for index: {index++} room: {roomName}");

                string messageData = HelperClass.GetMessageData(roomId);
                if(string.IsNullOrEmpty(messageData) == false && messageData != "{\"items\":[]}") {
                    string variableName = "_var" + _nonAlphaNumeric.Replace(roomName.ToLower(), "");
                    string fileName = variableName + ".js";

                    if(File.Exists(fileName)) {
                        variableName = variableName + counter;
                        fileName = variableName + ".js";
                        counter++;
                    }

                    messageData = $"var {variableName} = {messageData}";
                    File.WriteAllText($"ui\\_resources\\{fileName}", messageData);

                    menus.Add($"<div class='left-panel-menu' onclick=\"renderMessages(this, '{variableName}')\"><a>{roomName}</a></div>");
                    scripts.Add($"<script async src='_resources\\{fileName}'></script>");
                }
            }

            if(menus.Count > 0) {
                string htmlContent = File.ReadAllText("ui\\chats-template-file.html");
                htmlContent = htmlContent.Replace("$$menu_section$$", string.Join('\n', menus));
                htmlContent = htmlContent.Replace("$$script_section$$", string.Join('\n', scripts));
                File.WriteAllText("ui\\webex-chats.output.html", htmlContent);
            }

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
