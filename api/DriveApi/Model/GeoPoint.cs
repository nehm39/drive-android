using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class GeoPoint
    {
        private double latitude;
        private double longitude;

        public GeoPoint(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public GeoPoint()
        {

        }

        public double Latitude
        {
            get
            {
                return latitude;
            }

            set
            {
                latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }

            set
            {
                longitude = value;
            }
        }
    }
}