using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.Models
{
    public class User
    {
        public String user_name { get; set; }
        public int user_id { get; set; }
        public String email { get; set; }
        public String acc_type { get; set; }

        public User(int user_id, String user_name, String email, String acc_type)
        {
            this.user_id = user_id;
            this.user_name = user_name;
            this.email = email;
            this.acc_type = acc_type;
        }
    }
}