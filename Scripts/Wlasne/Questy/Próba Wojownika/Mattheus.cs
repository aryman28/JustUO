using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class Mattheus : MondainQuester
    {
        [Constructable]
        public Mattheus() : base("Mattheus", "- Wojownik [Dowódca]")
        {
        }

        public Mattheus(Serial serial) : base(serial)
        {
        }

		public override bool DisallowAllMoves{ get{ return true; } }

        public override Type[] Quests { get { return new Type[] { typeof(ProbaGiermka) }; } }

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
            // Usuniêcie istniej¹cego ekwipunku
            AddItem(new RingmailChest { Hue = 0x5A9 }); // Niebiesko-b³êkitny odcieñ
            AddItem(new RingmailLegs { Hue = 0x5A9 });
            AddItem(new RingmailArms { Hue = 0x5A9 });
            AddItem(new RingmailGloves { Hue = 0x5A9 });
            AddItem(new Kaptur{ Hue = 0x5A9 });
			AddItem(new Cloak{ Hue = 0x5A9 });
            AddItem(new WoodenShield { Hue = 0x5A9 }); // Drewniana tarcza w tym samym odcieniu

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
