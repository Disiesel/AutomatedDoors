using System;
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
}