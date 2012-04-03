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
            public static readonly string[] IMAGES = {  "http://activearth/badgeNone.png",
                                                        "http://activearth/badgeBronze.png",
                                                        "http://activearth/badgeSilver.png",
                                                        "http://activearth/badgeGold.png",
                                                        "http://activearth/badgePlatinum.png",
                                                        "http://activearth/badgeRuby.png",
                                                        "http://activearth/badgeSapphire.png",
                                                        "http://activearth/badgeEmerald.png",
                                                        "http://activearth/badgeDiamond.png"};
            public static readonly string FORMAT = "{0}";
        }

        /// <summary>
        /// Constants for the Walking Distance badge.
        /// </summary>
        public class WalkDistance
        {
            public static readonly float[] REQUIREMENTS = { 0, 20, 150, 450, 900, 1425, 2025, 2775, 3600, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {  "http://activearth/badgeNone.png",
                                                        "http://activearth/badgeBronze.png",
                                                        "http://activearth/badgeSilver.png",
                                                        "http://activearth/badgeGold.png",
                                                        "http://activearth/badgePlatinum.png",
                                                        "http://activearth/badgeRuby.png",
                                                        "http://activearth/badgeSapphire.png",
                                                        "http://activearth/badgeEmerald.png",
                                                        "http://activearth/badgeDiamond.png"};
            public static readonly string FORMAT = "{0:0.0}";
        }

        /// <summary>
        /// Constants for the Biking Distance badge.
        /// </summary>
        public class BikeDistance
        {
            public static readonly float[] REQUIREMENTS = { 0, 20, 150, 450, 900, 1425, 2025, 2775, 3600, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {  "http://activearth/badgeNone.png",
                                                        "http://activearth/badgeBronze.png",
                                                        "http://activearth/badgeSilver.png",
                                                        "http://activearth/badgeGold.png",
                                                        "http://activearth/badgePlatinum.png",
                                                        "http://activearth/badgeRuby.png",
                                                        "http://activearth/badgeSapphire.png",
                                                        "http://activearth/badgeEmerald.png",
                                                        "http://activearth/badgeDiamond.png"};
            public static readonly string FORMAT = "{0:0.0}";
        }

        /// <summary>
        /// Constants for the Running Distance badge.
        /// </summary>
        public class RunDistance
        {
            public static readonly float[] REQUIREMENTS = { 0, 20, 150, 450, 900, 1425, 2025, 2775, 3600, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {  "http://activearth/badgeNone.png",
                                                        "http://activearth/badgeBronze.png",
                                                        "http://activearth/badgeSilver.png",
                                                        "http://activearth/badgeGold.png",
                                                        "http://activearth/badgePlatinum.png",
                                                        "http://activearth/badgeRuby.png",
                                                        "http://activearth/badgeSapphire.png",
                                                        "http://activearth/badgeEmerald.png",
                                                        "http://activearth/badgeDiamond.png"};
            public static readonly string FORMAT = "{0:0.0}";
        }

        /// <summary>
        /// Constants for the Challenges Completed badge.
        /// </summary>
        public class Challenges
        {
            public static readonly float[] REQUIREMENTS = { 0, 1, 5, 15, 30, 45, 65, 90, 120, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {  "http://activearth/badgeNone.png",
                                                        "http://activearth/badgeBronze.png",
                                                        "http://activearth/badgeSilver.png",
                                                        "http://activearth/badgeGold.png",
                                                        "http://activearth/badgePlatinum.png",
                                                        "http://activearth/badgeRuby.png",
                                                        "http://activearth/badgeSapphire.png",
                                                        "http://activearth/badgeEmerald.png",
                                                        "http://activearth/badgeDiamond.png"};
            public static readonly string FORMAT = "{0}";
        }

        /// <summary>
        /// Constants for the Gas Savings badge.
        /// </summary>
        public class GasSavings
        {
            public static readonly float[] REQUIREMENTS = { 0, 10, 20, 30, 40, 50, 60, 70, 80, float.PositiveInfinity };
            public static readonly int[] REWARDS = { 0, 50, 100, 250, 400, 650, 800, 1050, 1200, 0 };
            public static readonly string[] IMAGES = {  "http://activearth/badgeNone.png",
                                                        "http://activearth/badgeBronze.png",
                                                        "http://activearth/badgeSilver.png",
                                                        "http://activearth/badgeGold.png",
                                                        "http://activearth/badgePlatinum.png",
                                                        "http://activearth/badgeRuby.png",
                                                        "http://activearth/badgeSapphire.png",
                                                        "http://activearth/badgeEmerald.png",
                                                        "http://activearth/badgeDiamond.png"};
            public static readonly string FORMAT = "${0:0.00}";
        }

    }
}