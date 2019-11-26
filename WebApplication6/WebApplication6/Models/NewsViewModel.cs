using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string spot { get; set; }
        public string content { get; set; }
        public string Image { get; set; }
        public string Editor { get; set; }
        public DateTime CreatDate { get; set; }
        public Nullable<int> EditorId { get; set; }
    }
}