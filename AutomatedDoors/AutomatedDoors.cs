﻿using System;
using System.Collections.Generic;
using System.IO; 
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;

namespace AutomatedDoors
{
    public class AutomatedDoors : Mod
    {
        public static ModConfig AutomatedDoorsConfig { get; private set; }

        public override void Entry(params object[] objects)
        {
            runConfig();
            Console.WriteLine("AutomatedDoors Has Loaded");
            GameEvents.UpdateTick += Events_UpdateTick;
            TimeEvents.DayOfMonthChanged += Events_NewDay;
        }

        void runConfig()
        {
            AutomatedDoorsConfig = new ModConfig().InitializeConfig(BaseConfigPath);
        }
    }
    
    public class ModConfig : Config
    {
        public int openDoors { get; set; }
        public int closeDoors { get; set; }
        public bool openRainyDays { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            openDoors = 6000;
            closeDoors = 1800;
            openRainyDays = false;
            return this as T;
        }
    }
}