using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DriveApi.Model;
using System.Data.SqlClient;
using System.Configuration;
using System.ServiceModel.Web;

namespace DriveApi
{

    public class Service : IService
    {
        private static readonly string userNameHeader = "Username";
        private static readonly string userPasswordHeader = "Password";
        private SqlConnection sqlConnection;

        public Service()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SQLConnectionString"].ConnectionString);
        }

        public object authenticate()
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0) return DBUtils.getUserForProfile(sqlConnection, loggedUserId);
            else
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;
                return new Status(false, null);
            }
        }

        private int runAuthenticate()
        {
            string user = WebOperationContext.Current.IncomingRequest.Headers[userNameHeader];
            string pass = WebOperationContext.Current.IncomingRequest.Headers[userPasswordHeader];
            return checkCredentials(user, pass);
        }

        public object createUser(FullUser user)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Conflict;
            if (user.UserName.Length > 50) return new Status(true, "Zbyt długa nazwa użytkownika.");
            if (user.Password.Length > 50) return new Status(true, "Zbyt długie hasło.");
            if (user.Mail.Length > 50) return new Status(true, "Zbyt długi adres e-mail.");
            if (DBUtils.userNameExists(sqlConnection, user.UserName)) return new Status(true, "Użytkownik z taką nazwą już istnieje.");
            if (DBUtils.userMailExists(sqlConnection, user.Mail)) return new Status(true, "Użytkownik z takim adresem e-mail już istnieje.");

            int id = DBUtils.addUser(sqlConnection, user);
            WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            return new Status(false, "Dodano do bazy.");
        }

        public object getUserById(string id)
        {
            int loggedUserId = runAuthenticate();
            int userId = Int32.Parse(id);
            if (loggedUserId > 0 && loggedUserId == userId)
            {
                return DBUtils.getSimpleUser(sqlConnection, userId);
            } 
            else
                return getAuthorizationErrorStatus(true);
        }

        public object getUser()
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0)
            {
                return DBUtils.getUserForProfile(sqlConnection, loggedUserId);
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public object updateUser(FullUser user)
        {
            int loggedUserId = runAuthenticate();
            user.Id = loggedUserId;
            if (loggedUserId > 0 && DBUtils.updateUser(sqlConnection, user))
            {
                return new Status(true, "Profil został zaktualizowany.");
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public object getUserLocation()
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0)
            {
                return DBUtils.getUserLocation(sqlConnection, loggedUserId);
            }
            else
                return getAuthorizationErrorStatus(true);
        }


        public object getUsersNearby(UsersNearbyFilter usersFilter)
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0)
            {
                if (usersFilter != null)
                    return DBUtils.getUsersNearby(sqlConnection, usersFilter, loggedUserId);
                else
                {
                    //GeoPoint userLocation = DBUtils.getUserLocation(sqlConnection, loggedUserId);
                    //return DBUtils.getUsersNearby(sqlConnection, userLocation, maxDistance, loggedUserId);
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Status(false, "");
                }
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public object updateUserLocation(GeoPoint geoPoint)
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0 && DBUtils.updateUserLocation(sqlConnection, loggedUserId, geoPoint))
            {
                return new Status(true, "Pozycja zaktualizowana.");
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public object getUsersByName(string username)
        {

            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0)
            {
                return DBUtils.getUsersByName(sqlConnection, username);
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public int checkCredentials(string user, string pass)
        {
            if (!string.IsNullOrWhiteSpace(user) && !string.IsNullOrWhiteSpace(pass))
                return DBUtils.checkCredentials(sqlConnection, user, pass);
            else
                return -1;
        }

        #region Event
        public object getEvents()
        {
            if (runAuthenticate() > 0)
            {
                return DBUtils.getEvents(sqlConnection);
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public object getEventsForCity(string city)
        {
            if (runAuthenticate() > 0 && city != null)
            {
                return DBUtils.getEventsForCity(sqlConnection, city);
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        public object getEventsNearby(EventsNearbyFilter eventsFilter)
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0)
            {
                if (eventsFilter != null)
                    return DBUtils.getEventsNearby(sqlConnection, eventsFilter);
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return new Status(false, "Uzupełnij wszystkie parametry.");
                }
            }
            else
                return getAuthorizationErrorStatus(true);
        }
        #endregion

        public object getNews()
        {
            int loggedUserId = runAuthenticate();
            if (loggedUserId > 0)
            {
                GeoPoint userLocation = DBUtils.getUserLocation(sqlConnection, loggedUserId);
                NewsContainer nc = new NewsContainer();
                nc.Events = DBUtils.getEventsNearby(sqlConnection, new EventsNearbyFilter(userLocation.Latitude, userLocation.Longitude, 10, 34));
                nc.Users = DBUtils.getUsersNearby(sqlConnection, new UsersNearbyFilter(userLocation.Latitude, userLocation.Longitude, 34), loggedUserId);
                nc.News = DBUtils.getNews(sqlConnection);
                return nc;
            }
            else
                return getAuthorizationErrorStatus(true);
        }

        // TODO do usunięcia
        public string rnd()
        {
            if (DBUtils.rnd(sqlConnection))
                return "ok";
            else
                return "not ok";
        }

        private Status getAuthorizationErrorStatus(bool withHttpStatusCode)
        {
            if (withHttpStatusCode)
                WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.Unauthorized;

            return new Status(true, "Błąd autoryzacji.");
        }

        public string getDate()
        {
            return DateTime.Now.ToString();
        }
    }
}
