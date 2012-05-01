using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ActivEarth.Objects.Profile
{
    public partial class DisplayWeatherControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {  

        }

        public bool GetCurrentConditions(string location)
        {
            XmlDocument xmlConditions = new XmlDocument();
            xmlConditions.Load(string.Format("http://www.google.com/ig/api?weather={0}", location));

            if (xmlConditions.SelectSingleNode("xml_api_reply/weather/problem_cause") != null)
            {
                return false;
            }
            
            else 
            {  
               Condition.Text = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/condition").Attributes["data"].InnerText;
               TempF.Text = string.Format("{0}°F", xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/temp_f").Attributes["data"].InnerText);
               Humidity.Text = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/humidity").Attributes["data"].InnerText;
               Wind.Text = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/wind_condition").Attributes["data"].InnerText;
               ConditionImage.ImageUrl = "http://www.google.com" + xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/icon").Attributes["data"].InnerText;
               return true;
            }
        }
    }
}