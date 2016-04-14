<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.IO; 
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using Microsoft.Xna.Framework;

namespace AutomatedDoors
{

    public class AutomatedDoors : Mod
    {
        private bool gotFired1 = false;
        private bool gotFired2 = false ;
        public int openDoors;
        public int closeDoors;
        public bool openRainyDays;

        public static AutomatedDoorsConfig ModConfig { get; private set; }

        public override void Entry(params object[] objects)
        {
            ModConfig = new AutomatedDoorsConfig();
            ModConfig = ModConfig.InitializeConfig(BaseConfigPath);
            GameEvents.UpdateTick += Events_UpdateTick;
            TimeEvents.DayOfMonthChanged += Events_NewDay;
            Console.WriteLine("AutomatedDoors Has Loaded");
        }

        public void Events_NewDay(object sender, EventArgs e)
        {
            gotFired1 = false;
            gotFired2 = false; 
        }

        public void Events_UpdateTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
            return;
            if (!gotFired1 && Game1.timeOfDay == ModConfig.openDoors && !Game1.IsWinter)
                {
                if (ModConfig.openRainyDays == true)
                    {
                        using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Building current = enumerator.Current;
                                if (current.animalDoorOpen == false)
                                {
                                    current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                                    gotFired1 = true;
                                }
                            }
                        }
 
                    }

                 else if (Game1.isRaining == false && Game1.isLightning == false)
                    {
                        using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Building current = enumerator.Current;
                                if (current.animalDoorOpen == false)
                                {
                                    current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                                    gotFired1 = true;
                                }
                            }
                        }
                    }
                }
             if (!gotFired2 && Game1.timeOfDay >= ModConfig.closeDoors)
                {
                    using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Building current = enumerator.Current;
                            if (current.animalDoorOpen)
                            {
                                current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                                gotFired2 = true;
                            }
                        }
                    }
                }
        
        }
                
    }
    
    public class AutomatedDoorsConfig : Config
    {
        public int openDoors { get; set; }
        public int closeDoors { get; set; }
        public bool openRainyDays { get; set; }

        public override T GenerateDefaultConfig<T>()
        {
            openDoors = 600;
            closeDoors = 1800;
            openRainyDays = false;
            return this as T;
        }
    }
=======
﻿using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SMAPIAutomatedDoors;

namespace AutomatedDoors
{

    public class AutomatedDoors : Mod
    {
        public static AutomatedDoorsConfig ModConfig { get; protected set; }

        private bool openDoorsEventFired;
        private bool closeDoorsEventFired;
        public int openDoorsTime;
        public int closeDoorsTime;
        public bool openRainyDays;
        public bool openWinter;
        public Dictionary<string, bool> buildings;

        public override void Entry(params object[] objects)
        {
            ModConfig = new AutomatedDoorsConfig().InitializeConfig(BaseConfigPath);
            GameEvents.UpdateTick += Events_UpdateTick;
            TimeEvents.DayOfMonthChanged += Events_NewDay;
            Console.WriteLine("AutomatedDoors Loaded ...... [OK]");
        }

        public void Events_NewDay(object sender, EventArgs e)
        {
            openDoorsEventFired = false;
            closeDoorsEventFired = false; 
        }

        public void Events_UpdateTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
            {
                return;
            }
            
            if (!openDoorsEventFired && Game1.timeOfDay == ModConfig.openDoorsTime && Game1.IsWinter == ModConfig.openWinter)
                {
                    if (ModConfig.openRainyDays == true)
                    {
                        OpenBuildingDoors();
                    }
                    else if (Game1.isRaining == false && Game1.isLightning == false)
                    {
                        OpenBuildingDoors();
                    }
                }
             if (!closeDoorsEventFired && Game1.timeOfDay >= ModConfig.closeDoorsTime)
                {
                    CloseBuildingDoors();
                }
            
            
        }

        public void OpenBuildingDoors()
        {
            using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Building current = enumerator.Current;
                    if ( !ModConfig.buildings.ContainsKey(current.nameOfIndoors) )
                    {
                        ModConfig.buildings.Add(current.nameOfIndoors, true);

                        if (current.animalDoorOpen == false)
                        {
                            current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                            openDoorsEventFired = true;
                        }
                    }
                    else if (ModConfig.buildings.ContainsKey(current.nameOfIndoors))
                    {
                        if (current.animalDoorOpen == false && ModConfig.buildings[current.nameOfIndoors] == true )
                        {
                            current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                            openDoorsEventFired = true;
                        }
                    }
                }
            }

            ModConfig.UpdateConfig<AutomatedDoorsConfig>();
            ModConfig.WriteConfig();
        }

        public void CloseBuildingDoors()
        {
            using (List<Building>.Enumerator enumerator = Game1.getFarm().buildings.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Building current = enumerator.Current;
                    if (current.animalDoorOpen)
                    {
                        current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                        closeDoorsEventFired = true;
                    }
                }
            }
        }
    }
>>>>>>> 65e2ef5545c4b4ba75cee138b37b05c5265dcfc0
}