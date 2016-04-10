using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StardewValley.Buildings;
using StardewValley;
using StardewModdingAPI;
using System.Collections.Generic;
using System;

namespace SMAPIAutomatedDoors
{
    public class AutomatedDoorsConfig : Config
    {
        public int openDoorsTime { get; set; }
        public int closeDoorsTime { get; set; }
        public bool openRainyDays { get; set; }
        public bool openWinter { get; set; }
        public Dictionary<string, bool> buildings { get; set; }
        public Keys configKey;

        public override T GenerateDefaultConfig<T>()
        {
            openDoorsTime = 620;
            closeDoorsTime = 1810;
            configKey = Keys.L;
            openRainyDays = false;
            openWinter = false;
            buildings = new Dictionary<string, bool>();

            return this as T;
        }
    }
}
