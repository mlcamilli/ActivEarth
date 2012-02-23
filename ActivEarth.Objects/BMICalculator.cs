using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActivEarth.Objects
{
    public class BMICalculator
    {
        public static double CalculateBMI(int height, int weight)
        {
            return (weight*703)/(height*height);
        }

        public static string GetBMIResult(double bmi)
        {
            if (bmi < 18.5)
            {
                return "underweight";
            }
            if (bmi >= 18.5 && bmi < 25)
            {
                return "normal";
            }
            if (bmi >= 25 && bmi < 30)
            {
                return "overweight";
            }
            return "obese";
        }
    }
}