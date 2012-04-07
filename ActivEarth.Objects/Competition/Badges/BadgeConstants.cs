using System;

namespace ActivEarth.Objects.Competition.Badges
{
    /// <summary>
    /// Consists of the constants for each badge.
    /// </summary>
    public class BadgeConstants
    {
        /// <summary>
        /// Constants for the Steps badge.
        /// </summary>
        public class Steps
        {
            public static readonly float[] REQUIREMENTS = { 0, 30000, 300000, 900000, 1800000, 2850000, 4050000, 5550000, 7200000, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {   "~/Images/Competition/Badges/Steps/None.png",
                                                      "~/Images/Competition/Badges/Steps/Bronze.png",
                                                      "~/Images/Competition/Badges/Steps/Silver.png",
                                                      "~/Images/Competition/Badges/Steps/Gold.png",
                                                      "~/Images/Competition/Badges/Steps/Platinum.png",
                                                      "~/Images/Competition/Badges/Steps/Ruby.png",
                                                      "~/Images/Competition/Badges/Steps/Sapphire.png",
                                                      "~/Images/Competition/Badges/Steps/Emerald.png",
                                                      "~/Images/Competition/Badges/Steps/Diamond.png"};
            public static readonly string FORMAT = "{0}";
        }

        /// <summary>
        /// Constants for the Walking Distance badge.
        /// </summary>
        public class WalkDistance
        {
            public static readonly float[] REQUIREMENTS = { 0, 20, 150, 450, 900, 1425, 2025, 2775, 3600, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {   "~/Images/Competition/Badges/Distance_Walked/None.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Bronze.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Silver.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Gold.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Platinum.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Ruby.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Sapphire.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Emerald.png",
                                                      "~/Images/Competition/Badges/Distance_Walked/Diamond.png"};
            public static readonly string FORMAT = "{0:0.0}";
        }

        /// <summary>
        /// Constants for the Biking Distance badge.
        /// </summary>
        public class BikeDistance
        {
            public static readonly float[] REQUIREMENTS = { 0, 20, 150, 450, 900, 1425, 2025, 2775, 3600, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {   "~/Images/Competition/Badges/Distance_Biked/None.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Bronze.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Silver.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Gold.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Platinum.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Ruby.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Sapphire.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Emerald.png",
                                                      "~/Images/Competition/Badges/Distance_Biked/Diamond.png"};
            public static readonly string FORMAT = "{0:0.0}";
        }

        /// <summary>
        /// Constants for the Running Distance badge.
        /// </summary>
        public class RunDistance
        {
            public static readonly float[] REQUIREMENTS = { 0, 20, 150, 450, 900, 1425, 2025, 2775, 3600, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {   "~/Images/Competition/Badges/Distance_Ran/None.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Bronze.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Silver.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Gold.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Platinum.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Ruby.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Sapphire.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Emerald.png",
                                                      "~/Images/Competition/Badges/Distance_Ran/Diamond.png"};
            public static readonly string FORMAT = "{0:0.0}";
        }

        /// <summary>
        /// Constants for the Challenges Completed badge.
        /// </summary>
        public class Challenges
        {
            //Change REQUIRMENTS to correct values
            public static readonly float[] REQUIREMENTS = { 0, 1, 5, 15, 30, 45, 65, 90, 120, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {   "~/Images/Competition/Badges/Challenges_Completed/None.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Bronze.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Silver.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Gold.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Platinum.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Ruby.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Sapphire.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Emerald.png",
                                                      "~/Images/Competition/Badges/Challenges_Completed/Diamond.png"};
            public static readonly string FORMAT = "{0}";
        }


        /// <summary>
        /// Constants for the Gas Savings badge.
        /// </summary>
        public class GasSavings
        {
            //Change REQUIRMENTS to correct values
            public static readonly float[] REQUIREMENTS = { 0, 10.00f, 20.00f, 40.00f, 60.00f, 80.00f, 100.00f, 200.00f, 400.00f, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {   "~/Images/Competition/Badges/Gas_Savings/None.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Bronze.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Silver.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Gold.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Platinum.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Ruby.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Sapphire.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Emerald.png",
                                                      "~/Images/Competition/Badges/Gas_Savings/Diamond.png"};
            public static readonly string FORMAT = "${0:0.00}";
        }
    }
}