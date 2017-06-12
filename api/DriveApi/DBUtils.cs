using DriveApi.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DriveApi
{
    public static class DBUtils
    {
        private static readonly string USERS_TABLE = "dbo.users";
        private static readonly string USERS_COLUMN_ID = "id";
        private static readonly string USERS_COLUMN_NAME = "user_name";
        private static readonly string USERS_COLUMN_PASSWORD = "password";
        private static readonly string USERS_COLUMN_MAIL = "mail";
        private static readonly string USERS_COLUMN_CITY = "city";
        private static readonly string USERS_COLUMN_LAT = "lat";
        private static readonly string USERS_COLUMN_LNG = "lng";
        private static readonly string USERS_COLUMN_VEHICLE_MAKE = "vehicle_make";
        private static readonly string USERS_COLUMN_VEHICLE_MODEL = "vehicle_model";
        private static readonly string USERS_COLUMN_BDATE = "birthdate";

        private static readonly string USERS_PROCEDURE = "user_distance";
        private static readonly string USERS_PROC_LAT = "@orig_lat";
        private static readonly string USERS_PROC_LNG = "@orig_lng";
        private static readonly string USERS_PROC_MAX_DISTANCE = "@max_distance";
        private static readonly string USERS_PROC_ID = "@id";

        private static readonly string addUserCommand = "INSERT INTO " + USERS_TABLE + " VALUES(" + nameWithAt(USERS_COLUMN_NAME) + ", " + nameWithAt(USERS_COLUMN_PASSWORD) + ", " + nameWithAt(USERS_COLUMN_MAIL) + ", " + nameWithAt(USERS_COLUMN_CITY) + ", " + nameWithAt(USERS_COLUMN_LAT) + ", " + nameWithAt(USERS_COLUMN_LNG) + ", " + nameWithAt(USERS_COLUMN_VEHICLE_MAKE) + ", " + nameWithAt(USERS_COLUMN_VEHICLE_MODEL) + ", " + nameWithAt(USERS_COLUMN_BDATE) + ")";
        private static readonly string getUserCommand = "SELECT * FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_ID + " = " + nameWithAt(USERS_COLUMN_ID);
        private static readonly string updateUserCommand = "UPDATE " + USERS_TABLE + " SET " + USERS_COLUMN_NAME + " = " + nameWithAt(USERS_COLUMN_NAME) + ", " + USERS_COLUMN_PASSWORD + " = " + nameWithAt(USERS_COLUMN_PASSWORD) + ", " + USERS_COLUMN_MAIL + " = " + nameWithAt(USERS_COLUMN_MAIL) + ", " + USERS_COLUMN_CITY + " = " + nameWithAt(USERS_COLUMN_CITY) + ", " + USERS_COLUMN_LAT + " = " + nameWithAt(USERS_COLUMN_LAT) + ", " + USERS_COLUMN_LNG + " = " + nameWithAt(USERS_COLUMN_LNG) + " WHERE " + USERS_COLUMN_ID + " = " + nameWithAt(USERS_COLUMN_ID);
        private static readonly string checkIfUserIdExistsCommand = "SELECT count(1) FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_ID + " = " + nameWithAt(USERS_COLUMN_ID);
        private static readonly string checkIfUserNameExistsCommand = "SELECT count(1) FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_NAME + " = " + nameWithAt(USERS_COLUMN_NAME);
        private static readonly string checkIfUserMailExistsCommand = "SELECT count(1) FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_MAIL + " = " + nameWithAt(USERS_COLUMN_MAIL);
        private static readonly string checkUserCredentials = "SELECT TOP 1 " + USERS_COLUMN_ID + " FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_NAME + " = " + nameWithAt(USERS_COLUMN_NAME) + " AND " + USERS_COLUMN_PASSWORD + " = " + nameWithAt(USERS_COLUMN_PASSWORD);
        private static readonly string getUserLocationCommand = "SELECT TOP 1 " + USERS_COLUMN_LAT + ", " + USERS_COLUMN_LNG + " FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_ID + " = " + nameWithAt(USERS_COLUMN_ID);
        private static readonly string updateUserLocationCommand = "UPDATE " + USERS_TABLE + " SET " + USERS_COLUMN_LAT + " = " + nameWithAt(USERS_COLUMN_LAT) + ", " + USERS_COLUMN_LNG + " = " + nameWithAt(USERS_COLUMN_LNG) + " WHERE " + USERS_COLUMN_ID + " = " + nameWithAt(USERS_COLUMN_ID);
        private static readonly string getUsersByNameCommand = "SELECT TOP 50 " + USERS_COLUMN_ID + ", " + USERS_COLUMN_NAME + ", " + USERS_COLUMN_MAIL + ", " + USERS_COLUMN_CITY + ", " + USERS_COLUMN_VEHICLE_MAKE + ", " + USERS_COLUMN_VEHICLE_MODEL + ", " + USERS_COLUMN_BDATE + " FROM " + USERS_TABLE + " WHERE " + USERS_COLUMN_NAME + " LIKE " + nameWithAt(USERS_COLUMN_NAME);

        private static readonly string EVENTS_TABLE = "dbo.events";
        private static readonly string EVENTS_COLUMN_ID = "id";
        private static readonly string EVENTS_COLUMN_NAME = "name";
        private static readonly string EVENTS_COLUMN_DESC = "description";
        private static readonly string EVENTS_COLUMN_LAT = "lat";
        private static readonly string EVENTS_COLUMN_LNG = "lng";
        private static readonly string EVENTS_COLUMN_ADDRESS = "address";
        private static readonly string EVENTS_COLUMN_CITY = "city";
        private static readonly string EVENTS_COLUMN_DATE = "date";

        private static readonly string EVENTS_PROCEDURE = "event_distance";
        private static readonly string EVENTS_PROC_LAT = "@orig_lat";
        private static readonly string EVENTS_PROC_LNG = "@orig_lng";
        private static readonly string EVENTS_PROC_MAX_DISTANCE = "@max_distance";
        private static readonly string EVENTS_PROC_MAX_DAYS_LEFT = "@max_days_left";

        private static readonly string getEventsCommand = "SELECT * FROM " + EVENTS_TABLE;
        private static readonly string getEventsForCityCommand = "SELECT * FROM " + EVENTS_TABLE + " WHERE " + EVENTS_COLUMN_CITY + " = " + nameWithAt(EVENTS_COLUMN_CITY);

        private static readonly string NEWS_TABLE = "dbo.news";
        private static readonly string getNewsCommand = "SELECT * FROM " + NEWS_TABLE;


        private static string nameWithAt(string name)
        {
            return "@" + name;
        }

        public static Boolean userNameExists(SqlConnection sqlConnection, string name)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.checkIfUserNameExistsCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_NAME), name);
            sqlConnection.Open();
            int count = (int)cmd.ExecuteScalar();
            sqlConnection.Close();
            if (count > 0) return true;
            else return false;
        }

        public static Boolean userMailExists(SqlConnection sqlConnection, string mail)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.checkIfUserMailExistsCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_MAIL), mail);
            sqlConnection.Open();
            int count = (int)cmd.ExecuteScalar();
            sqlConnection.Close();
            if (count > 0) return true;
            else return false;
        }

        public static int addUser(SqlConnection sqlConnection, FullUser user)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.addUserCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_NAME), user.UserName);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_MAIL), user.Mail);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_PASSWORD), user.Password);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_CITY), user.City != null ? user.City : (object)DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LAT), DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LNG), DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_VEHICLE_MAKE), user.VehicleMake != null ? user.VehicleMake : (object)DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_VEHICLE_MODEL), user.VehicleModel != null ? user.VehicleModel : (object)DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_BDATE), user.Birthdate != null ? user.Birthdate : (object)DBNull.Value);
            sqlConnection.Open();
            int id = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return id;
        }

        public static FullUser getSimpleUser(SqlConnection sqlConnection, int id)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.getUserCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_ID), id);
            sqlConnection.Open();

            FullUser user = null;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if(reader.Read())
                {
                    user = new FullUser();
                    user.Id = reader.GetInt32(0);
                    user.UserName = reader.GetString(1);
                    user.Mail = reader.GetString(3);
                    if(!reader.IsDBNull(4))
                        user.City = reader.GetString(4);
                }
            }
            sqlConnection.Close();

            return user;
        }

        public static UserForProfile getUserForProfile(SqlConnection sqlConnection, int id)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.getUserCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_ID), id);
            sqlConnection.Open();

            UserForProfile user = null;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    user = new UserForProfile();
                    user.Id = reader.GetInt32(0);
                    user.UserName = reader.GetString(1);
                    user.Mail = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        user.City = reader.GetString(4);
                    if (!reader.IsDBNull(7))
                        user.VehicleMake = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        user.VehicleModel = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        user.Birthdate = reader.GetDateTime(9).ToShortDateString();
                }
            }
            sqlConnection.Close();

            return user;
        }

        public static GeoPoint getUserLocation(SqlConnection sqlConnection, int id)
        {
            SqlCommand cmd = new SqlCommand(DBUtils.getUserLocationCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_ID), id);
            sqlConnection.Open();

            GeoPoint point = null;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    point = new GeoPoint();
                    point.Latitude = (double) reader.GetDecimal(0);
                    point.Longitude = (double) reader.GetDecimal(1);
                }
            }
            sqlConnection.Close();
            return point;
        }

        public static bool updateUserLocation(SqlConnection sqlConnection, int id, GeoPoint geoPoint)
        {
            SqlCommand cmd = new SqlCommand(DBUtils.updateUserLocationCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_ID), id);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LAT), geoPoint.Latitude);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LNG), geoPoint.Longitude);
            sqlConnection.Open();
            int count = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return count > 0;
        }

        public static bool updateUser(SqlConnection sqlConnection, FullUser user)
        {
            sqlConnection.Close();

            string cmdString = "UPDATE " + USERS_TABLE + " SET ";

            if (user.UserName != null)
                cmdString += USERS_COLUMN_NAME + " = " + nameWithAt(USERS_COLUMN_NAME) + ", ";
            if (user.Password != null)
                cmdString += USERS_COLUMN_PASSWORD + " = " + nameWithAt(USERS_COLUMN_PASSWORD) + ", ";
            if (user.Mail != null)
                cmdString += USERS_COLUMN_MAIL + " = " + nameWithAt(USERS_COLUMN_MAIL) + ", ";

            cmdString += USERS_COLUMN_CITY + " = " + nameWithAt(USERS_COLUMN_CITY) + ", " + USERS_COLUMN_VEHICLE_MAKE + " = " + nameWithAt(USERS_COLUMN_VEHICLE_MAKE) + ", " + USERS_COLUMN_VEHICLE_MODEL + " = " + nameWithAt(USERS_COLUMN_VEHICLE_MODEL) + ", " + USERS_COLUMN_BDATE + " = " + nameWithAt(USERS_COLUMN_BDATE) + " WHERE " + USERS_COLUMN_ID + " = " + nameWithAt(USERS_COLUMN_ID); // ", " + USERS_COLUMN_LAT + " = " + nameWithAt(USERS_COLUMN_LAT) + ", " + USERS_COLUMN_LNG + " = " + nameWithAt(USERS_COLUMN_LNG) +

            SqlCommand cmd = new SqlCommand(cmdString, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_ID), user.Id);
            if (user.UserName != null)
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_NAME), user.UserName);
            if (user.Mail != null)
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_MAIL), user.Mail);
            if (user.Password != null)
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_PASSWORD), user.Password);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_CITY), user.City != null ? user.City : (object)DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_VEHICLE_MAKE), user.VehicleMake != null ? user.VehicleMake : (object)DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_VEHICLE_MODEL), user.VehicleModel != null ? user.VehicleModel : (object)DBNull.Value);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_BDATE), user.Birthdate != null ? user.Birthdate : (object)DBNull.Value);
            //cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LAT), DBNull.Value);
            //cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LNG), DBNull.Value);
            sqlConnection.Open();
            int count = cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return count > 0;
        }

        public static int checkCredentials(SqlConnection sqlConnection, string username, string password)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.checkUserCredentials, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_NAME), username);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_PASSWORD), password);
            sqlConnection.Open();

            int userId;
            try
            {
                userId = (int)cmd.ExecuteScalar();
            }
            catch(Exception)
            {
                userId = -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            return userId;
        }

        public static List<UserForMap> getUsersNearby(SqlConnection sqlConnection, UsersNearbyFilter usersFilter, int id)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(USERS_PROCEDURE, sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(USERS_PROC_LAT, usersFilter.Latitude);
            cmd.Parameters.AddWithValue(USERS_PROC_LNG, usersFilter.Longitude);
            cmd.Parameters.AddWithValue(USERS_PROC_MAX_DISTANCE, usersFilter.MaxDistance);
            cmd.Parameters.AddWithValue(USERS_PROC_ID, id);
            sqlConnection.Open();

            List<UserForMap> users = new List<UserForMap>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserForMap user = new UserForMap();
                    user.Id = reader.GetInt32(0);
                    user.UserName = reader.GetString(1);
                    user.Mail = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        user.City = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        user.Latitude = (double)reader.GetDecimal(5);
                    if (!reader.IsDBNull(6))
                        user.Longitude = (double)reader.GetDecimal(6);
                    if (!reader.IsDBNull(7))
                        user.VehicleMake = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        user.VehicleModel = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        user.Birthdate = reader.GetDateTime(9).ToShortDateString();
                    if (!reader.IsDBNull(10))
                        user.Distance = Math.Round(reader.GetDouble(10), 3, MidpointRounding.AwayFromZero);

                    users.Add(user);
                }
            }
            sqlConnection.Close();

            return users;
        }

        public static List<UserForProfile> getUsersByName(SqlConnection sqlConnection, string username)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.getUsersByNameCommand, sqlConnection);
            cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_NAME), "%" + username + "%");
            sqlConnection.Open();

            List<UserForProfile> users = new List<UserForProfile>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    UserForProfile user = new UserForProfile();
                    user.Id = reader.GetInt32(0);
                    user.UserName = reader.GetString(1);
                    user.Mail = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        user.City = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        user.VehicleMake = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        user.VehicleModel = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        user.Birthdate = reader.GetDateTime(6).ToShortDateString();

                    users.Add(user);
                }
            }
            sqlConnection.Close();

            return users;
        }

        #region Event
        public static List<Event> getEvents(SqlConnection sqlConnection)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.getEventsCommand, sqlConnection);
            sqlConnection.Open();

            List<Event> events = new List<Event>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    events.Add(parseEvent(reader));
                }
            }
            sqlConnection.Close();

            return events;
        }

        public static List<Event> getEventsForCity(SqlConnection sqlConnection, string city)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.getEventsForCityCommand, sqlConnection);
            cmd.Parameters.AddWithValue(EVENTS_COLUMN_CITY, city);
            sqlConnection.Open();

            List<Event> events = new List<Event>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    events.Add(parseEvent(reader));
                }
            }
            sqlConnection.Close();

            return events;
        }

        public static List<Event> getEventsNearby(SqlConnection sqlConnection, EventsNearbyFilter filter)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(EVENTS_PROCEDURE, sqlConnection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(EVENTS_PROC_LAT, filter.Latitude);
            cmd.Parameters.AddWithValue(EVENTS_PROC_LNG, filter.Longitude);
            cmd.Parameters.AddWithValue(EVENTS_PROC_MAX_DISTANCE, filter.MaxDistance);
            cmd.Parameters.AddWithValue(EVENTS_PROC_MAX_DAYS_LEFT, filter.MaxDaysLeft);
            sqlConnection.Open();

            List<Event> events = new List<Event>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    events.Add(parseEvent(reader));
                }
            }
            sqlConnection.Close();

            return events;
        }

        private static Event parseEvent(SqlDataReader reader)
        {
            Event eventData = new Event();
            eventData.Id = reader.GetInt32(0);
            eventData.Name = reader.GetString(1);
            if (!reader.IsDBNull(2))
                eventData.Description = reader.GetString(2);
            eventData.Latitude = (double)reader.GetDecimal(3);
            eventData.Longitude = (double)reader.GetDecimal(4);
            if (!reader.IsDBNull(5))
                eventData.Address = reader.GetString(5);
            eventData.City = reader.GetString(6);
            eventData.Date = reader.GetDateTime(7).ToUnixTimeStampUTC();

            return eventData;
        }
        #endregion

        public static List<News> getNews(SqlConnection sqlConnection)
        {
            sqlConnection.Close();

            SqlCommand cmd = new SqlCommand(DBUtils.getNewsCommand, sqlConnection);
            sqlConnection.Open();

            List<News> news = new List<News>();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    news.Add(parseNews(reader));
                }
            }
            sqlConnection.Close();

            return news;
        }

        private static News parseNews(SqlDataReader reader)
        {
            News news = new News();
            news.Id = reader.GetInt32(0);
            news.Title = reader.GetString(1);
            news.Content = reader.GetString(2);
            news.PublishDate = reader.GetDateTime(3).ToUnixTimeStampUTC();

            return news;
        }

        // TODO Do usuniecia
        public static bool rnd(SqlConnection sqlConnection)
        {
            GeoPoint startPoint = new GeoPoint(50.041272, 21.999231);

            sqlConnection.Close();

            sqlConnection.Open();
            SqlTransaction sqlTran = sqlConnection.BeginTransaction();
            SqlCommand cmd = sqlConnection.CreateCommand();
            cmd.CommandText = "INSERT INTO " + USERS_TABLE + " VALUES(" + nameWithAt(USERS_COLUMN_NAME) + ", " + nameWithAt(USERS_COLUMN_PASSWORD) + ", " + nameWithAt(USERS_COLUMN_MAIL) + ", " + nameWithAt(USERS_COLUMN_CITY) + ", " + nameWithAt(USERS_COLUMN_LAT) + ", " + nameWithAt(USERS_COLUMN_LNG) + ", @distance)";
            cmd.Transaction = sqlTran;

            for (int i = 0; i < 10000; i++)
            {
                GeoPoint randomPoint = GeoLocUtils.randLocationInRange();
                double distance = GeoLocUtils.calculateDistance(startPoint, randomPoint);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_NAME), "geo" + i);
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_MAIL), "a@b.pl");
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_PASSWORD), "pass");
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_CITY), "Rzeszów");
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LAT), randomPoint.Latitude);
                cmd.Parameters.AddWithValue(nameWithAt(USERS_COLUMN_LNG), randomPoint.Longitude);
                cmd.Parameters.AddWithValue("@distance", distance);
                cmd.ExecuteNonQuery();
            }

            sqlTran.Commit();

            sqlConnection.Close();

            return true;
        }
    }
}