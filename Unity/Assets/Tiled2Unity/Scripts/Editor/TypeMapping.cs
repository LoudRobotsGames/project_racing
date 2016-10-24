using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class TypeMapping
{
    private static int TrafficConeHash = "TrafficCone".GetHashCode();
    private static int StartPointHash = "StartPoint".GetHashCode();
    private static int RaceTrackControllerHash = "RaceTrackController".GetHashCode();

    public static Type GetType(String type)
    {
        int hash = type.GetHashCode();

        if (hash == TrafficConeHash)
            return typeof(TrafficCone);
        else if (hash == StartPointHash)
            return typeof(StartPoint);
        else if (hash == RaceTrackControllerHash)
            return typeof(RaceTrackController);

        return null;
    }
}
