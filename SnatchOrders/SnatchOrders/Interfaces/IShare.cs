using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Interfaces
{
    public interface IShare
    {
        void ShareMessageToApps(string message);
    }
}
