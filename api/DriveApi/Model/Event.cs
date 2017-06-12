using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class Event
    {
        private int id;
        private string name;
        private string description;
        private double latitude;
        private double longitude;
        private string address;
        private string city;
        private Int32 date;

        public Event()
        {

        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public Int32 Date
        {
            get { return date; }
            set { date = value; }
        }
    }
}