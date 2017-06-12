using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class UsersNearbyFilter : GeoPoint
    {
        private int maxDistance;

        public int MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }

        public UsersNearbyFilter(double latitude, double longitude, int maxDistance)
        {
            this.MaxDistance = maxDistance;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}