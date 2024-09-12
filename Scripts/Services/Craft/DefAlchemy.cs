using System;
using Server.Items;

namespace Server.Engines.Craft
{
    public class DefAlchemy : CraftSystem
    {
        public override SkillName MainSkill
        {
            get
            {
                return SkillName.Alchemia;
            }
        }

        public override int GumpTitleNumber
        {
            get
            {
                return 1044001;
            }// <CENTER>ALCHEMY MENU</CENTER>
        }

        private static CraftSystem m_CraftSystem;

        public static CraftSystem CraftSystem
        {
            get
            {
                if (m_CraftSystem == null)
                    m_CraftSystem = new DefAlchemy();

                return m_CraftSystem;
            }
        }

        public override double GetChanceAtMin(CraftItem item)
        {
            return 0.0; // 0%
        }

        private DefAlchemy()
            : base(1, 1, 1.25)// base( 1, 1, 3.1 )
        {
        }

        public override int CanCraft(Mobile from, BaseTool tool, Type itemType)
        {
            if (tool == null || tool.Deleted || tool.UsesRemaining < 0)
                return 1044038; // You have worn out your tool!
            else if (!BaseTool.CheckAccessible(tool, from))
                return 1044263; // The tool must be on your person to use.

            return 0;
        }

        public override void PlayCraftEffect(Mobile from)
        {
            from.PlaySound(0x242);
        }

        private static readonly Type typeofPotion = typeof(BasePotion);

        public static bool IsPotion(Type type)
        {
            return typeofPotion.IsAssignableFrom(type);
        }

        public override int PlayEndingEffect(Mobile from, bool failed, bool lostMaterial, bool toolBroken, int quality, bool makersMark, CraftItem item)
        {
            if (toolBroken)
                from.SendLocalizedMessage(1044038); // You have worn out your tool

            if (failed)
            {
                if (IsPotion(item.ItemType))
                {
                    from.AddToBackpack(new Bottle());
                    return 500287; // You fail to create a useful potion.
                }
                else
                {
                    return 1044043; // You failed to create the item, and some of your materials are lost.
                }
            }
            else
            {
                from.PlaySound(0x240); // Sound of a filling bottle

                if (IsPotion(item.ItemType))
                {
                    if (quality == -1)
                        return 1048136; // You create the potion and pour it into a keg.
                    else
                        return 500279; // You pour the potion into a bottle...
                }
                else
                {
                    return 1044154; // You create the item.
                }
            }
        }

        public override void InitCraftList()
        {
            int index = -1;

            // Refresh Potion
            index = this.AddCraft(typeof(RefreshPotion), "Mikstury odświeżania", "Mikstura odświeżenia", -25, 25.0, typeof(BlackPearl), 1044353, 1, 1044361);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(TotalRefreshPotion), "Mikstury odświeżania", "Mikstura pełnego odświeżenia", 25.0, 75.0, typeof(BlackPearl), 1044353, 5, 1044361);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Agility Potion
            index = this.AddCraft(typeof(AgilityPotion), "Mikstury zręczności", "Mikstura zręczności", 15.0, 65.0, typeof(Bloodmoss), 1044354, 1, 1044362);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(GreaterAgilityPotion), "Mikstury zręczności", "Duża mikstura zręczności", 35.0, 85.0, typeof(Bloodmoss), 1044354, 3, 1044362);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Nightsight Potion
            index = this.AddCraft(typeof(NightSightPotion), "Mikstura widz. w ciemn.", "Mikstura widz. w ciemn.", -25.0, 25.0, typeof(SpidersSilk), 1044360, 1, 1044368);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Heal Potion
            index = this.AddCraft(typeof(LesserHealPotion), "Mikstury leczenia", "Mała mikstura leczenia", -25.0, 25.0, typeof(Ginseng), 1044356, 1, 1044364);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(HealPotion), "Mikstury leczenia", "Średnia mikstura leczenia", 15.0, 65.0, typeof(Ginseng), 1044356, 3, 1044364);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(GreaterHealPotion), "Mikstury leczenia", "Duża mikstura leczenia", 55.0, 105.0, typeof(Ginseng), 1044356, 7, 1044364);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Strength Potion[restart]
            index = this.AddCraft(typeof(StrengthPotion), "Mikstury siły", "Mikstury siły", 25.0, 75.0, typeof(MandrakeRoot), 1044357, 2, 1044365);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(GreaterStrengthPotion), "Mikstury siły", "Duża mikstury siły", 45.0, 95.0, typeof(MandrakeRoot), 1044357, 5, 1044365);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Poison Potion
            index = this.AddCraft(typeof(LesserPoisonPotion), "Mikstury trucizny", "Mała mikstura trucizny", -5.0, 45.0, typeof(Nightshade), 1044358, 1, 1044366);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(PoisonPotion), "Mikstury trucizny", "Średnia mikstura trucizny", 15.0, 65.0, typeof(Nightshade), 1044358, 2, 1044366);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(GreaterPoisonPotion), "Mikstury trucizny", "Duża mikstura trucizny", 55.0, 105.0, typeof(Nightshade), 1044358, 4, 1044366);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(DeadlyPoisonPotion), "Mikstury trucizny", "Mikstura śmiertelnej trucizny", 90.0, 140.0, typeof(Nightshade), 1044358, 8, 1044366);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            //index = this.AddCraft(typeof(ScouringToxin), "Mikstury trucizny", 1112292, 75.0, 100.0, typeof(ToxicVenomSac), 1112291, 1, 1044366);
            //this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Cure Potion
            index = this.AddCraft(typeof(LesserCurePotion), "Mikstury odtrucia", "Mała mikstura odtrucia", -10.0, 40.0, typeof(Garlic), 1044355, 1, 1044363);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(CurePotion), "Mikstury odtrucia", "Mikstura odtrucia", 25.0, 75.0, typeof(Garlic), 1044355, 3, 1044363);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(GreaterCurePotion), "Mikstury odtrucia", "Duża mikstura odtrucia", 65.0, 115.0, typeof(Garlic), 1044355, 6, 1044363);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            // Explosion Potion
            index = this.AddCraft(typeof(LesserExplosionPotion), "Mikstury wybuchowe", "Mała mikstura wybuchowa", 5.0, 55.0, typeof(SulfurousAsh), 1044359, 3, 1044367);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(ExplosionPotion), "Mikstury wybuchowe", "Mikstura wybuchowa", 35.0, 85.0, typeof(SulfurousAsh), 1044359, 5, 1044367);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);
            index = this.AddCraft(typeof(GreaterExplosionPotion), "Mikstury wybuchowe", "Duża mikstura wybuchowa", 65.0, 115.0, typeof(SulfurousAsh), 1044359, 10, 1044367);
            this.AddRes(index, typeof(Bottle), 1044529, 1, 500315);

            if (Core.SE)
            {
                index = this.AddCraft(typeof(SmokeBomb), "Mikstury wybuchowe", "Bomba dymna", 90.0, 120.0, typeof(Eggs), 1044477, 1, 1044253);
                this.AddRes(index, typeof(Ginseng), 1044356, 3, 1044364);
                this.SetNeededExpansion(index, Expansion.SE);
            }

            #region Mondain's Legacy
            // region Nekromancja (Core.ML?)
            index = this.AddCraft(typeof(ConflagrationPotion), "Mikstury wybuchowe", "Mikstuta pożogi", 55.0, 105.0, typeof(Bottle), 1044529, 1, 500315);
            this.AddRes(index, typeof(GraveDust), 1023983, 5, 1044253);

            index = this.AddCraft(typeof(GreaterConflagrationPotion), "Mikstury wybuchowe", "Duża mikstura pożogi", 70.0, 120.0, typeof(Bottle), 1044529, 1, 500315);
            this.AddRes(index, typeof(GraveDust), 1023983, 10, 1044253);

            //index = this.AddCraft(typeof(ConfusionBlastPotion), "Mikstury wybuchowe", 1072106, 55.0, 105.0, typeof(Bottle), 1044529, 1, 500315);
            //this.AddRes(index, typeof(PigIron), 1023978, 5, 1044253);

            //index = this.AddCraft(typeof(GreaterConfusionBlastPotion), "Mikstury wybuchowe", 1072109, 70.0, 120.0, typeof(Bottle), 1044529, 1, 500315);
            //this.AddRes(index, typeof(PigIron), 1023978, 10, 1044253);

            // Earthen Mixtures
            if (Core.ML)
            {
                index = this.AddCraft(typeof(InvisibilityPotion), "Różne", "Mikstura niewidzialności", 65.0, 115.0, typeof(Bottle), 1044529, 1, 500315);
                this.AddRes(index, typeof(Bloodmoss), 1044354, 4, 1044362);
                this.AddRes(index, typeof(Nightshade), 1044358, 4, 1044366);
                //this.AddRecipe(index, (int)TinkerRecipes.InvisibilityPotion);
                this.SetNeededExpansion(index, Expansion.ML);

                //index = this.AddCraft(typeof(ParasiticPotion), 1074832, 1072942, 65.0, 115.0, typeof(Bottle), 1044529, 1, 500315);
                //this.AddRes(index, typeof(ParasiticPlant), 1073474, 5, 1044253);
                //this.AddRecipe(index, (int)TinkerRecipes.ParasiticPotion);
                //this.SetNeededExpansion(index, Expansion.ML);

                //index = this.AddCraft(typeof(DarkglowPotion), 1074832, 1072943, 65.0, 115.0, typeof(Bottle), 1044529, 1, 500315);
                //this.AddRes(index, typeof(LuminescentFungi), 1073475, 5, 1044253);
                //this.AddRecipe(index, (int)TinkerRecipes.DarkglowPotion);
                //this.SetNeededExpansion(index, Expansion.ML);

                //index = this.AddCraft(typeof(HoveringWisp), 1074832, 1072881, 65.0, 115.0, typeof(CapturedEssence), 1032686, 4, 1044253);
                //this.AddRecipe(index, (int)TinkerRecipes.HoveringWisp);
                //this.SetNeededExpansion(index, Expansion.ML);
            }
            #endregion

            #region Stygian Abyss
            /* Plant Pigments/Natural Dyes*/

            if (Core.SA)
            {
                index = this.AddCraft(typeof(PlantPigment), "Różne", 1112132, 33.0, 83.0, typeof(PlantClippings), 1112131, 1, 1044253);
                this.AddRes(index, typeof(Bottle), 1023854, 1, 1044253);
                this.SetNeededExpansion(index, Expansion.SA);

                index = this.AddCraft(typeof(NaturalDye), "Różne", 1112136, 75.0, 100.0, typeof(PlantPigment), 1112132, 1, 1044253);
                this.AddRes(index, typeof(ColorFixative), 1112135, 1, 1044253);
                this.SetNeededExpansion(index, Expansion.SA);

                index = this.AddCraft(typeof(ColorFixative), "Różne", 1112135, 75.0, 100.0, typeof(SilverSerpentVenom), 1112173, 1, 1044253);
                this.AddRes(index, typeof(BaseBeverage), 1044476, 1, 1044253);//TODO correct Consumption of BaseBeverage...
                this.SetNeededExpansion(index, Expansion.SA);

                index = this.AddCraft(typeof(SoftenedReeds), "Różne", 1112249, 75.0, 100.0, typeof(DryReeds), 1112248, 1, 1112250);
                this.AddRes(index, typeof(ScouringToxin), 1112292, 2, 1112326);
            }
            #endregion
        }
    }
}