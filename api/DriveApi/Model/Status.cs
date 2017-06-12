using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class Status
    {
        private Boolean showUser;
        private string message;

        public Status()
        {

        }

        public Status(Boolean showUser, string message)
        {
            this.showUser = showUser;
            this.message = message;
        }

        [DataMember]
        public bool ShowUser
        {
            get
            {
                return showUser;
            }

            set
            {
                showUser = value;
            }
        }

        [DataMember]
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }
    }
}