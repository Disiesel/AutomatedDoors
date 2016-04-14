using System;
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
}