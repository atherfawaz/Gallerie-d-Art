using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Art_Gallery.Models;


namespace Art_Gallery.Models
{
    public class Parameters
    {
        public List<Art> obj { get; set; }
        public List<Art> sim { get; set; }
        public List<Art> obj1 { get; set; }
        public List<User> user_table { get; set; }
        public int identifier { get; set; }
        public float rating { get; set; }
        public int error { get; set; }
        public int purCheck { get; set; }
        public int ratingCheck { get; set; }
        public int likeCheck { get; set; }
        public Parameters()
        {
            obj = null;
            identifier = 0;
            rating = 0;
            sim = null;
            obj1 = null;
            user_table = null;
            error = 0;
            purCheck = 0;
            ratingCheck = 0;
            likeCheck = 0;
        }
    }
}