using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi
{
    public static class DateTimeExtension
    {
        public static Int32 ToUnixTimeStampUTC(this DateTime dt)
        {
            Int32 unixTimeStamp;
            DateTime zuluTime = dt.ToUniversalTime();
            DateTime unixEpoch = new DateTime(1970, 1, 1).ToUniversalTime();
            unixTimeStamp = (Int32)(zuluTime.Subtract(unixEpoch)).TotalSeconds;
            return unixTimeStamp;
        }
    }
}