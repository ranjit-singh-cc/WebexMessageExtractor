using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebexMessageExtractor {
    public static class HelperClass {

        public static string BearerToken = "";
        public static int MaxMessageCount = 1000;

        public static IDictionary<string, string> GetRooms() {
            IDictionary<string, string> rooms = new Dictionary<string, string>();
            string response = ExecuteApis("rooms", queryString: string.Format("sortBy=lastactivity&max=1000"));
            if(string.IsNullOrEmpty(response) == false) {
                JObject responseObj = ParseJsonObject(response);
                if(responseObj != null) {
                    JArray itemArr = responseObj["items"] as JArray;
                    for(int i = 0; i < itemArr.Count; i++) {
                        JObject item = itemArr[i] as JObject;
                        if(item != null && item["id"] != null) {
                            string id = item["id"].ToString();
                            if(rooms.ContainsKey(id) == false)
                                rooms.Add(item["id"].ToString(), item["title"].ToString());
                        }
                    }
                }
            }

            return rooms;
        }

        public static string GetMessageData(string roomId) {
            return ExecuteApis("messages", queryString: string.Format("roomId={0}&max={1}", roomId, MaxMessageCount));
        }

        public static string ExecuteApis(string endPoint, string postData = null, string queryString = null, string method = "GET") {
            string responseData = string.Empty;
            try {
                if(string.IsNullOrEmpty(queryString) == false)
                    queryString = "?" + queryString;

                HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create($"https://webexapis.com/v1/{endPoint}{queryString}");
                webRequest.Method = method;
                webRequest.Timeout = 120 * 1000;
                webRequest.Headers["Authorization"] = $"Bearer {BearerToken}";

                if(string.IsNullOrEmpty(postData) == false) {
                    webRequest.MediaType = "multipart/form-data";
                    webRequest.ContentType = "application/json";

                    byte[] byteArr = Encoding.UTF8.GetBytes(postData);
                    webRequest.ContentLength = byteArr.Length;
                    using(Stream dataStream = webRequest.GetRequestStream()) {
                        dataStream.Write(byteArr, 0, byteArr.Length);
                    }
                }

                using(WebResponse webResponse = webRequest.GetResponse()) {
                    using(Stream responseStream = webResponse.GetResponseStream()) {
                        if(responseStream != null) {
                            using(StreamReader responseReader = new StreamReader(responseStream)) {
                                responseData = responseReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch(WebException ex) {
                LoggingHelper.Error(ex);
            }
            catch(Exception ex) {
                LoggingHelper.Error(ex);
            }

            return responseData;
        }

        public static void SetProtocol() {
            if((ServicePointManager.SecurityProtocol & SecurityProtocolType.Ssl3) == SecurityProtocolType.Ssl3)
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            if((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls) == SecurityProtocolType.Tls)
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Tls;
            if((ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls12) != SecurityProtocolType.Tls12)
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        public static JObject ParseJsonObject(string text) {
            JObject responseObj = null;
            try {
                responseObj = JObject.Parse(text);
            }
            catch(Exception ex) {
                LoggingHelper.Error(ex: ex);
            }

            return responseObj;
        }

    }
}
