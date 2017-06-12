using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class EventsNearbyFilter : GeoPoint
    {
        private int maxDaysLeft;
        private int maxDistance;

        public EventsNearbyFilter()
        {
            //
        }

        public EventsNearbyFilter(double latitude, double longitude, int maxDaysLeft, int maxDistance)
        {
            this.MaxDaysLeft = maxDaysLeft;
            this.MaxDistance = maxDistance;
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public int MaxDaysLeft
        {
            get { return maxDaysLeft; }
            set { maxDaysLeft = value; }
        }

        public int MaxDistance
        {
            get { return maxDistance; }
            set { maxDistance = value; }
        }
    }
}