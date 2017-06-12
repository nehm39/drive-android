using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DriveApi.Model
{
    [Serializable]
    public class News
    {
        private int id;
        private string title;
        private string content;
        private Int32 publishDate;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public Int32 PublishDate
        {
            get { return publishDate; }
            set { publishDate = value; }
        }
    }
}