using System;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AutomatedDoors
{

    public class AutomatedDoors : Mod
    {
        private AutomatedDoorsConfig _config;

        private bool openDoorsEventFired;
        private bool closeDoorsEventFired;

        public override void Entry(IModHelper helper)
        {
            _config = Helper.ReadConfig<AutomatedDoorsConfig>();
            TimeEvents.DayOfMonthChanged += Events_NewDay;
            GameEvents.OneSecondTick += Events_OneSecondTick;
        }

        public void Events_NewDay(object sender, EventArgs e)
        {
            openDoorsEventFired = false;
            closeDoorsEventFired = false; 
        }

        public void Events_OneSecondTick(object sender, EventArgs e)
        {
            if (!Game1.hasLoadedGame)
            {
                return;
            }
            
            if (!openDoorsEventFired && Game1.timeOfDay == _config.timeDoorsOpen) //&& Game1.IsWinter == _config.openInWinter
            {
                    if (_config.openOnRainyDays == true)
                    {
                        OpenBuildingDoors();
                    }
                    else if (Game1.isRaining == false && Game1.isLightning == false)
                    {
                        OpenBuildingDoors();
                    }
                }
             if (!closeDoorsEventFired && Game1.timeOfDay >= _config.timeDoorsClose)
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
                    if ( !_config.buildings.ContainsKey(current.nameOfIndoors) )
                    {
                        _config.buildings.Add(current.nameOfIndoors, true);

                        if (current.animalDoorOpen == false)
                        {
                            current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                            openDoorsEventFired = true;
                        }
                    }
                    else if (_config.buildings.ContainsKey(current.nameOfIndoors))
                    {
                        if (current.animalDoorOpen == false && _config.buildings[current.nameOfIndoors] == true )
                        {
                            current.doAction(new Vector2(current.animalDoor.X + current.tileX, current.animalDoor.Y + current.tileY), Game1.player);
                            openDoorsEventFired = true;
                        }
                    }
                }
            }
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