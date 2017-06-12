using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class UserForList : User
    {
        private string city;

        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
            }
        }
    }
}