﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SnatchOrders.Models
{
    public enum MenuItemType
    {
        Home,
        MailSettings,
        Items,
        Reports,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
