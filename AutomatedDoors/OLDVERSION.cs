// Decompiled with JetBrains decompiler
// Type: AnimalDoorAutomatic.AnimalDoorAutomatic
// Assembly: AnimalDoorAutomatic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 4930DD5B-285C-4645-8A7D-08A20A5149E8
// Assembly location: C:\Users\Disiesel\Downloads\AnimalDoorAutomatic.dll

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buildings;
using StardewValley.Locations;
using System;
using System.Collections.Generic;
using System.IO;

namespace AnimalDoorAutomatic
{
    public class AnimalDoorAutomatic : Mod
    {
        private bool gotExecuted1;
        private bool gotExecuted2;
        public int opendoortime;
        public int closedoortime;
        public bool OpenOnRainyDays;

        public virtual string Name
        {
            get
            {
                return "AnimalDoorAutomatic";
            }
        }

        public virtual string Authour
        {
            get
            {
                return "Phate";
            }
        }

        public virtual string Version
        {
            get
            {
                return "1.1";
            }
        }

        public virtual string Description
        {
            get
            {
                return "Automatic opens and closes all Animal Doors at desired Time";
            }
        }

        public AnimalDoorAutomatic()
        {
            base.Vector();
        }

        public virtual void Entry(params object[] objects)
        {
            Console.WriteLine("{0} loaded.", (object)base.get_Name());
            this.config();
            GameEvents.add_UpdateTick(new EventHandler(this.Events_DrawTick));
            TimeEvents.add_DayOfMonthChanged(new EventHandler<EventArgsIntChanged>(this.Events_TimeOfMonthChanged));
        }

        private void Events_TimeOfMonthChanged(object sender, EventArgs e)
        {
            this.gotExecuted1 = false;
            this.gotExecuted2 = false;
        }

        private void Events_DrawTick(object sender, EventArgs e)
        {
            if (!(bool)Game1.hasLoadedGame)
                return;
            if (!this.gotExecuted1 && (Game1.timeOfDay == this.opendoortime && !Game1.get_IsWinter()))
            {
                if (this.OpenOnRainyDays)
                {
                    using (List<Building>.Enumerator enumerator = ((List<Building>)((BuildableGameLocation)Game1.getFarm()).buildings).GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Building current = enumerator.Current;
                            if (current.animalDoorOpen == 0)
                            {
                                // ISSUE: explicit reference operation
                                // ISSUE: cast to a reference type
                                // ISSUE: explicit reference operation
                                // ISSUE: explicit reference operation
                                // ISSUE: cast to a reference type
                                // ISSUE: explicit reference operation
                                current.doAction(new Vector((float)(^ (Point &) @current.animalDoor).X + (float)current.tileX, (float)(^ (Point &) @current.animalDoor).Y + (float)current.tileY), (Farmer)Game1.player);
                                this.gotExecuted1 = true;
                            }
                        }
                    }
                }
                else if (Game1.isRaining == null || Game1.isLightning == 0)
                {
                    using (List<Building>.Enumerator enumerator = ((List<Building>)((BuildableGameLocation)Game1.getFarm()).buildings).GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Building current = enumerator.Current;
                            if (current.animalDoorOpen == 0)
                            {
                                // ISSUE: explicit reference operation
                                // ISSUE: cast to a reference type
                                // ISSUE: explicit reference operation
                                // ISSUE: explicit reference operation
                                // ISSUE: cast to a reference type
                                // ISSUE: explicit reference operation
                                current.doAction(new Vector2((float)(^ (Point &) @current.animalDoor).X + (float)current.tileX, (float)(^ (Point &) @current.animalDoor).Y + (float)current.tileY), (Farmer)Game1.player);
                                this.gotExecuted1 = true;
                            }
                        }
                    }
                }
            }
            if (!this.gotExecuted2 && Game1.timeOfDay == this.closedoortime)
            {
                using (List<Building>.Enumerator enumerator = ((List<Building>)((BuildableGameLocation)Game1.getFarm()).buildings).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Building current = enumerator.Current;
                        if ((bool)current.animalDoorOpen)
                        {
                            // ISSUE: explicit reference operation
                            // ISSUE: cast to a reference type
                            // ISSUE: explicit reference operation
                            // ISSUE: explicit reference operation
                            // ISSUE: cast to a reference type
                            // ISSUE: explicit reference operation
                            current.doAction(new Vector2((float)(^ (Point &) @current.animalDoor).X + (float)current.tileX, (float)(^ (Point &) @current.animalDoor).Y + (float)current.tileY), (Farmer)Game1.player);
                            this.gotExecuted2 = true;
                        }
                    }
                }
            }
        }

        private void config()
        {
            string path = "Mods\\AnimalDoorAutomatic.ini";
            StreamReader streamReader;
            try
            {
                streamReader = File.OpenText(path);
                Console.WriteLine("Loaded AnimalDoorAutomatic config from " + path);
            }
            catch
            {
                Console.WriteLine("AnimalDoorAutomatic config doesn't exist! Creating one for you.");
                StreamWriter streamWriter = new StreamWriter(path);
                streamWriter.WriteLine("OpenDoors=600");
                streamWriter.WriteLine("CloseDoors=1800");
                streamWriter.WriteLine("OpenOnRainyDays=false");
                streamWriter.Close();
                streamReader = File.OpenText(path);
            }
            if (!int.TryParse(streamReader.ReadLine().Split('=')[1], out this.opendoortime) || !int.TryParse(streamReader.ReadLine().Split('=')[1], out this.closedoortime) || !bool.TryParse(streamReader.ReadLine().Split('=')[1], out this.OpenOnRainyDays))
            {
                Console.WriteLine("Couldn't parse AnimalDoorAutomatic config! Using default values.");
                streamReader.Close();
            }
            else
            {
                if (this.opendoortime < 600 || this.closedoortime < 600)
                {
                    this.opendoortime = this.opendoortime < 600 ? 600 : this.opendoortime;
                    this.closedoortime = this.closedoortime < 600 ? 600 : this.closedoortime;
                    Console.WriteLine("You cannot open/close the Doors earlier than 6am.");
                }
                else if (this.opendoortime > 2600 || this.closedoortime > 2600)
                {
                    this.opendoortime = this.opendoortime > 2600 ? 2600 : this.opendoortime;
                    this.closedoortime = this.closedoortime > 2600 ? 2600 : this.closedoortime;
                    Console.WriteLine("You cannot open/close the Doors later than 2am the next day.");
                }
                Console.WriteLine(string.Concat(new object[4]
                {
          (object) "Doors open on: ",
          (object) this.opendoortime,
          (object) ". Doors closing on: ",
          (object) this.closedoortime
                }));
                if (this.OpenOnRainyDays)
                    Console.WriteLine("Animal Doors open now on Rainy Days, too!");
                else
                    Console.WriteLine("Animal Doors doesn't open on Rainy Days!");
                streamReader.Close();
            }
        }
    }
}
