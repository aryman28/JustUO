//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 01.09.2024 06:20:38
//===============================================================================


using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class Daerth : MondainQuester
	{
		[Constructable]
		public Daerth() : base("Daerth","- Upad³y Rycerz")
		{
		}

		public Daerth(Serial serial) : base(serial)
		{
		}

		public override Type[] Quests { get { return new Type[] { typeof(DrogaZemstyCzesc2) }; } }

		public override void InitBody()
        {
            this.Race = Race.Human;
            this.Hue = Utility.RandomSkinHue();
            this.HairItemID = Race.RandomHair(Female);
            this.HairHue = Race.RandomHairHue();
            this.FacialHairItemID = Race.RandomFacialHair(Female);
            if (FacialHairItemID != 0)
                FacialHairHue = Race.RandomHairHue();
            this.Body = 0x190;
        }

        public override void InitOutfit()
        {
            // Usuniêcie istniej¹cego ekwipunku i dodanie nowego
            AddItem(new PlateChest { Hue = 1109 }); // Niebiesko-b³êkitny odcieñ
            AddItem(new PlateLegs { Hue = 1109 });
            AddItem(new PlateArms { Hue = 1109 });
            AddItem(new PlateGloves { Hue = 1109 });
            //AddItem(new HoodedShroudOfShadows { Hue = 1153 }); // Kaptur w odcieniu pasuj¹cym do zestawu
            AddItem(new Cloak { Hue = 1157 });
            AddItem(new ChaosShield { Hue = 1157 }); // Drewniana tarcza w tym samym odcieniu

            // Dodanie dodatkowych akcesoriów, np. miecza
            AddItem(new Longsword()); // Opcjonalnie dodajemy miecz, mo¿na zmieniæ na inn¹ broñ
        }
		
		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
