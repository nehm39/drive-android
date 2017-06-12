using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class UserForProfile : UserForList
    {
        private string mail;
        private string birthdate;

        public string Mail
        {
            get
            {
                return mail;
            }

            set
            {
                mail = value;
            }
        }

        public string Birthdate
        {
            get
            {
                return birthdate;
            }

            set
            {
                birthdate = value;
            }
        }
    }
}