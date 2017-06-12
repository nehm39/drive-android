using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DriveApi;
using System.IO;
using System.Web.Script.Serialization;
using DriveApi.Model;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;

namespace DriveApiTests
{
    [TestClass]
    public class ServiceTests
    {
        private static readonly string API_URL = "http://drivingapptmp-001-site1.anytempurl.com/Service.svc{0}";

        [TestMethod]
        public void GET_authenticate()
        {
            string url = string.Format(API_URL, "/authenticate");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers = LoginHeaders;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);
            
            object obj = jsonToObject<FullUser>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(FullUser));
        }

        [TestMethod]
        public void POST_user()
        {
            string url = string.Format(API_URL, "/user");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Post;
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string username = "testUser" + DateTime.Now.ToUnixTimeStampUTC().ToString();
            FullUser user = new FullUser()
            {
                City = "Rzeszów",
                Id = 0,
                Mail = username + "@example.com",
                Password = "pass",
                UserName = username,
                VehicleMake = "Opel",
                VehicleModel = "Astra",
                Birthdate = "1992-08-09"
            };

            string postJson = objectToJsonNew<FullUser>(user);
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());
            writer.Write(postJson);
            writer.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<FullUser>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(FullUser));
        }

        [TestMethod]
        public void PUT_user()
        {
            string url = string.Format(API_URL, "/user");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Put;
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            string username = "test";
            FullUser user = new FullUser()
            {
                City = "Rzeszów",
                Id = 0,
                Mail = username + "@example.com",
                Password = null,
                UserName = username,
                VehicleMake = "Opel",
                VehicleModel = "Insignia",
                Birthdate = "1990-01-06"
            };

            httpWebRequest.Headers.Add("Username", username);
            httpWebRequest.Headers.Add("Password", "test");

            string postJson = objectToJsonNew<FullUser>(user);
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());
            writer.Write(postJson);
            writer.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<FullUser>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(FullUser));
        }

        [TestMethod]
        public void GET_user()
        {
            string url = string.Format(API_URL, "/user");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers = LoginHeaders;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<FullUser>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(FullUser));
        }

        [TestMethod]
        public void POST_users_nearby()
        {
            string url = string.Format(API_URL, "/users/nearby");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Post;
            httpWebRequest.Headers = LoginHeaders;            
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            UsersNearbyFilter filter = new UsersNearbyFilter(50.041272, 21.999231, 40);

            string postJson = objectToJsonNew<UsersNearbyFilter>(filter);
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());
            writer.Write(postJson);
            writer.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<List<UserForMap>>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(List<UserForMap>));
        }

        [TestMethod]
        public void PUT_user_location()
        {
            string url = string.Format(API_URL, "/user/location");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Put;
            httpWebRequest.Headers = LoginHeaders;
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            GeoPoint gp = new GeoPoint(50.121716, 22.061823);

            string postJson = objectToJsonNew<GeoPoint>(gp);
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream(), System.Text.Encoding.ASCII);
            writer.Write(postJson);
            writer.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<Status>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(Status));
        }

        [TestMethod]
        public void GET_users_search()
        {
            string url = string.Format(API_URL, "/users/search/test");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers = LoginHeaders;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<List<UserForProfile>>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(List<UserForProfile>));
        }

        [TestMethod]
        public void GET_events()
        {
            string url = string.Format(API_URL, "/events");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers = LoginHeaders;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<List<Event>>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(List<Event>));
        }

        [TestMethod]
        public void GET_events_for_city()
        {
            string url = string.Format(API_URL, "/events/Rzeszów");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers = LoginHeaders;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<List<Event>>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(List<Event>));
        }

        [TestMethod]
        public void POST_events_nearby()
        {
            string url = string.Format(API_URL, "/events/nearby");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Post;
            httpWebRequest.Headers = LoginHeaders;
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            EventsNearbyFilter filter = new EventsNearbyFilter()
            {
                Latitude = 50.041272,
                Longitude = 21.999231,
                MaxDistance = 40,
                MaxDaysLeft = 80
            };

            string postJson = objectToJsonNew<EventsNearbyFilter>(filter);
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());
            writer.Write(postJson);
            writer.Close();

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<List<Event>>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(List<Event>));
        }

        [TestMethod]
        public void GET_news()
        {
            string url = string.Format(API_URL, "/news");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers = LoginHeaders;

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string jsonString = streamToString(httpWebResponse);

            object obj = jsonToObject<List<NewsContainer>>(jsonString);
            Assert.IsInstanceOfType(obj, typeof(List<NewsContainer>));
        }

        #region Serialization methods and LoginHeaders
        private T jsonToObject<T>(string jsonString)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            var jsonAddResponse = jsSerializer.Deserialize<T>(jsonString);

            return jsonAddResponse;
        }

        private string objectToJson(object obj)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            return jsSerializer.Serialize(obj);
        }

        private string objectToJsonNew<T>(object obj)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer dcjs = new DataContractJsonSerializer(typeof(T));
            dcjs.WriteObject(ms, obj);
            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }

        private string streamToString(HttpWebResponse httpWebResponse)
        {
            using (StreamReader sr = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                return sr.ReadToEnd();
            }
        }

        public WebHeaderCollection LoginHeaders 
        { 
            get
            {
                WebHeaderCollection whc = new WebHeaderCollection();
                whc.Add("Username", "geo0");
                whc.Add("Password", "pass");
                return whc;
            }
        }
        #endregion
    }
}
