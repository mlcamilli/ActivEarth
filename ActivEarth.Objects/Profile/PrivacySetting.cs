using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects.Profile
{
    public class PrivacySetting
    {
        public ProfileVisibility profileVisibility{ get; set;}
        public bool email{ get; set; }
        public bool gender{ get; set;}
        public bool age{ get; set; }
        public bool weight{ get; set; }
        public bool height{ get; set; }
        public bool location{ get; set; }
        public bool group{ get; set; }
    }
}