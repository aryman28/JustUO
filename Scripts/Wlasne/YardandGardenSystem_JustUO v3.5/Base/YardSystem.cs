using System.Collections.Generic;
using System;
using Server.Commands;
using Server.Items;

using Server.Items.Crops;

namespace Server.ACC.YS
{
    public class YardSystem
    {
        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(StartTimer);
        }

        public static List<Item> OrphanedYardItems = new List<Item>();

        public static void AddOrphanedItem(Item item)
        {
            if (OrphanedYardItems == null)
            {
                OrphanedYardItems = new List<Item>();
            }

            if (item == null)
            {
                return;
            }

            OrphanedYardItems.Add(item);
        }

        public static void StartTimer(WorldSaveEventArgs args)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(YardSettings.SecondsToCleanup), CleanYards);
        }

        public static void CleanYards()
        {
            if (OrphanedYardItems == null || OrphanedYardItems.Count <= 0)
            {
                return;
            }

            Console.WriteLine();
            Console.WriteLine(String.Format("Cleaning {0} Orphaned Yard Items...", OrphanedYardItems.Count));
            for (int i = 0; i < OrphanedYardItems.Count; i++)
            {
                if (OrphanedYardItems[i] is YardItem)
                {
                    YardItem item = (YardItem)OrphanedYardItems[i];
                    if (item == null)
                    {
                        continue;
                    }
                    item.FindHouseOfPlacer();
                    if (item.House == null)
                    {
                        //item.Refund();
                          item.Delete();
                    }
                }

                else if (OrphanedYardItems[i] is YardFountain)
                {
                    YardFountain item = (YardFountain)OrphanedYardItems[i];
                    if (item == null)
                    {
                        continue;
                    }
                    item.FindHouseOfPlacer();
                    if (item.House == null)
                    {
                        //item.Refund();
                          item.Delete();
                    }
                }

                else if (OrphanedYardItems[i] is YardTreeMulti)
                {
                    YardTreeMulti item = (YardTreeMulti)OrphanedYardItems[i];
                    if (item == null)
                    {
                        continue;
                    }
                    item.FindHouseOfPlacer();
                    if (item.House == null)
                    {
                        //item.Refund();
                          item.Delete();
                    }
                }
            }

            OrphanedYardItems.Clear();
        }
    }
}
