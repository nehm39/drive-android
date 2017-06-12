using DriveApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi
{
    public static class GeoLocUtils
    {
        private static double startLat = 49.590990;
        private static double endLat = 50.620239;
        private static double startLng = 20.975404;
        private static double endLng = 23.154203;

        private static Random random = new Random();

        public static double calculateDistance(GeoPoint gp1, GeoPoint gp2)
        {
            double lat1 = gp1.Latitude;
            double lon1 = gp1.Longitude;
            double lat2 = gp2.Latitude;
            double lon2 = gp2.Longitude;

            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344; // K
            //if (unit == 'K')
            //{
            //    dist = dist * 1.609344;
            //}
            //else if (unit == 'N')
            //{
            //    dist = dist * 0.8684;
            //}
            return (dist);
        }

        public static GeoPoint randLocationInRange()
        {
            return new GeoPoint(getRandomCoordinate(startLat, endLat), getRandomCoordinate(startLng, endLng));
        }

        private static double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private static double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private static double getRandomCoordinate(double minimum, double maximum)
        {
            double number = random.NextDouble() * (maximum - minimum) + minimum;
            return Math.Round(number, 6);
        }
    }
}