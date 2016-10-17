using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class TypeMapping
{
    public static Type GetType(String type)
    {
        if (type.Equals("TrafficCone"))
        {
            return typeof(TrafficCone);
        }
        return null;
    }
}
