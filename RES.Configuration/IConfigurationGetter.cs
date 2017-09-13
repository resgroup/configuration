using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace RES.Configuration
{
    public interface IConfigurationGetter
    {
        string Get(string setting);
    }
}