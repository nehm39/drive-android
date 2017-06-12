using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class User
    {
        [OptionalField]
        private int id;
        private string userName;
        private string vehicleMake;
        private string vehicleModel;

        public User()
        {

        }

        public User(int id, string userName)
        {
            this.id = id;
            this.userName = userName;
        }

        [DataMember]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        [DataMember]
        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        [DataMember]
        public string VehicleMake
        {
            get
            {
                return vehicleMake;
            }

            set
            {
                vehicleMake = value;
            }
        }

        [DataMember]
        public string VehicleModel
        {
            get
            {
                return vehicleModel;
            }

            set
            {
                vehicleModel = value;
            }
        }
    }
}