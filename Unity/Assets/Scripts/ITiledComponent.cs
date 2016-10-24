using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ITiledComponent
{
    void SetupFromProperties(IDictionary<string, string> props);
}

