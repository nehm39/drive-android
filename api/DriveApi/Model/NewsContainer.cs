using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class NewsContainer
    {
        private List<News> news;
        private List<UserForMap> users;
        private List<Event> events;

        public List<News> News
        {
            get { return news; }
            set { news = value; }
        }

        public List<UserForMap> Users
        {
            get { return users; }
            set { users = value; }
        }

        public List<Event> Events
        {
            get { return events; }
            set { events = value; }
        }

    }
}