using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley;
using StardewModdingAPI;
using System.Collections.Generic;
using System;

namespace AutomatedDoors
{
    public class AutomatedDoorsConfig
    {
        public int timeDoorsOpen { get; set; } = 620;
        public int timeDoorsClose { get; set; } = 1810;
        public bool openOnRainyDays { get; set; } = false;
        public bool openInWinter { get; set; } = false;
        public Dictionary<string, Dictionary<string, bool>> buildings { get; set; } = new Dictionary<string, Dictionary<string, bool>>();
    }
}
