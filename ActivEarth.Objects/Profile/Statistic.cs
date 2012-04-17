namespace ActivEarth.Objects.Profile
{
    /// <summary>
    /// Enumeration of the types of statistics that can be accessed for a User.
    /// </summary>
    /// <remarks>
    /// When adding a new value to the enum, simply add it to the end.  Do not
    /// change the numeric values of any of the existing statistic entries.
    /// </remarks>
    public enum Statistic
    {
        Steps = 0,
        WalkDistance = 1,
        BikeDistance = 2,
        RunDistance = 3,
        GasSavings = 4,
        ChallengesCompleted = 5,
        AggregateDistance = 6,
        AggregateTime = 7,
        WalkTime = 8,
        BikeTime = 9,
        RunTime = 10
    };
}