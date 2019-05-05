using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.Models
{
    public class Art
    {
        public int id { get; set; }
        public String Creator { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public String Medium { get; set; }
        public String Origin { get; set; }
        public String Link { get; set; }
        public int Price { get; set; }
        public String Date { get; set; }
        public Art(int id = 0, String Creator = "", String Title = "", String Description = "", String Medium = "", String Origin = "", String link = "../Content/images/paintings/train.jpg", String date = "", int Price = 0)
        {
            this.id = id;
            this.Creator = Creator;
            this.Title = Title;
            this.Description = Description;
            this.Medium = Medium;
            this.Origin = Origin;
            this.Link = link;
            this.Date = date;
            this.Price = Price;
        }
    }
}