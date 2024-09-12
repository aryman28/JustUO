using System.Collections.Generic;
using System;
using Server.Commands;
using Server.Items;

using Server.Items.Crops;

namespace Server.ACC.YS
{
    public class YardFarmSystem
    {
        public static void Initialize()
        {
            EventSink.WorldSave += new WorldSaveEventHandler(StartTimer);
        }

        public static List<Item> FarmYardItems = new List<Item>();

        public static void AddFarmItem(Item item)
        {
            if (FarmYardItems == null)
            {
                FarmYardItems = new List<Item>();
            }

            if (item == null)
            {
                return;
            }

            FarmYardItems.Add(item);
        }

        public static void StartTimer(WorldSaveEventArgs args)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(YardSettings.SecondsToCleanup), CleanYardFarm);
        }

        public static void CleanYardFarm()
        {
            if (FarmYardItems == null || FarmYardItems.Count <= 0)
            {
                return;
            }

            for (int i = 0; i < FarmYardItems.Count; i++)
            {

                if (FarmYardItems[i] is YardFarm)
                {
                    YardFarm item = (YardFarm)FarmYardItems[i];
                    if (item == null)
                    {
                        continue;
                    }
                    item.FindHouseOfPlacer();
                    item.FindCropOfPlacer();
                    if (item.House == null)
                    {
                          item.Delete();
                    }
                    if (item.Crop == null)
                    {
                          item.Hue = 0;
                    }
                }

            }

            FarmYardItems.Clear();
        }
    }
}
