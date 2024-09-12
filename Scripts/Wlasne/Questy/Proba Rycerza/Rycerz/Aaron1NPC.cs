//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 27.08.2024 12:20:26
//===============================================================================


using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
	public class Aaron : MondainQuester
	{
		[Constructable]
		public Aaron() : base("Aaron","- Rycerz ")
		{
		}

		public Aaron(Serial serial) : base(serial)
		{
		}

		public override Type[] Quests { get { return new Type[] { typeof(ProbaRycerzaCzesc1) }; } }

		public override void InitBody()
		{
			this.Race = Race.Human;
			this.Hue = Hue = Utility.RandomSkinHue();
			this.HairItemID = Race.RandomHair(Female);
			this.HairHue = Race.RandomHairHue();
			this.FacialHairItemID = Race.RandomFacialHair(Female);
			if (FacialHairItemID != 0)
				FacialHairHue = Race.RandomHairHue();
			this.Body = 0x190;
		}

		public override void InitOutfit()
        {
            // Usuni�cie istniej�cego ekwipunku
            AddItem(new RingmailChest { Hue = 1153 }); // Niebiesko-b��kitny odcie�
            AddItem(new RingmailLegs { Hue = 1153 });
            AddItem(new RingmailArms { Hue = 1153 });
            AddItem(new RingmailGloves { Hue = 1153 });
            AddItem(new Kaptur{ Hue = 1153 });
			AddItem(new Cloak{ Hue = 1153 });
            AddItem(new WoodenShield { Hue = 1153 }); // Drewniana tarcza w tym samym odcieniu

            // Dodanie dodatkowych akcesori�w, np. miecza
            AddItem(new Longsword()); // Opcjonalnie dodajemy miecz, mo�na zmieni� na inn� bro�
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
