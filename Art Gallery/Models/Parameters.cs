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
        public int identifier { get; set; }
        public Parameters()
        {
            obj = null;
            identifier = 0;
            sim = null;
            obj1 = null;
        }
    }
}