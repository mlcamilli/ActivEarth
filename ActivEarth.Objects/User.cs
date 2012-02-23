using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects
{
    public class User
    {
        public String UserName { get; set; }
        public String Email { get; set; }
        public int UserID { get; set; }
        public int ProfileID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public Char Gender { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public int? Age { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
    }
}