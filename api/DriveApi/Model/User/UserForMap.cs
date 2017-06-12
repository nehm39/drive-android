using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class UserForMap : UserForProfile
    {
        private double latitude;
        private double longitude;
        private double distance = -1;

        [DataMember]
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

        [DataMember]
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

        [DataMember]
        public double Distance
        {
            get
            {
                return distance;
            }

            set
            {
                distance = value;
            }
        }
    }
}