using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DriveApi.Model;

namespace DriveApi
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "authenticate")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(UserForProfile))]
        object authenticate();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/{id}")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(FullUser))]
        object getUserById(string id);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(UserForProfile))]
        object getUser();

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat=WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(FullUser))]
        object createUser(FullUser user);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(FullUser))]
        object updateUser(FullUser user);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "users/nearby")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(List<UserForMap>))]
        object getUsersNearby(UsersNearbyFilter usersFilter);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "user/location")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(GeoPoint))]
        object updateUserLocation(GeoPoint geoPoint);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "users/search/{username}")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(List<UserForProfile>))]
        object getUsersByName(string username);

        #region Event
        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "events")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(List<Event>))]
        object getEvents();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "events/{city}")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(List<Event>))]
        object getEventsForCity(string city);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "events/nearby")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(List<Event>))]
        object getEventsNearby(EventsNearbyFilter eventsFilter);
        #endregion

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "news")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(NewsContainer))]
        object getNews();

        // TODO Do usuniecia
        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "rnd")]
        string rnd();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "date")]
        [ServiceKnownType(typeof(Status))]
        [ServiceKnownType(typeof(UserForProfile))]
        string getDate();
    }
}
