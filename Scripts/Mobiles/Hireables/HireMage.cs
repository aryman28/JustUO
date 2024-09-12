using System;
using Server.Items;

namespace Server.Mobiles 
{
    public class HireMage : BaseHire
    {
        [Constructable]
        public HireMage()
            : base(AIType.AI_Mage)
        {
            this.SpeechHue = Utility.RandomDyedHue();
            this.Hue = Utility.RandomSkinHue();
            this.Title = "- Mag (najemnik)";
            if (this.Female = Utility.RandomBool()) 
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName("female");
            }
            else 
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName("male");
                this.AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }

            this.HairItemID = this.Race.RandomHair(this.Female);
            this.HairHue = this.Race.RandomHairHue();
            this.Race.RandomFacialHair(this);

            this.SetStr(61, 75);
            this.SetDex(81, 95);
            this.SetInt(86, 100);

            this.SetDamage(10, 23);

            this.SetSkill(SkillName.Intelekt, 100.0, 125);
            this.SetSkill(SkillName.Magia, 100, 125);
            this.SetSkill(SkillName.Medytacja, 100, 125);
            this.SetSkill(SkillName.ObronaPrzedMagia, 100, 125);
            this.SetSkill(SkillName.Taktyka, 100, 125);
            this.SetSkill(SkillName.WalkaObuchami, 100, 125);

            this.Fame = 100;
            this.Karma = 100;

            this.AddItem(new Shoes(Utility.RandomNeutralHue()));
            this.AddItem(new Shirt());

            this.AddItem(new Robe(Utility.RandomNeutralHue()));
            this.AddItem(new ThighBoots());

            this.PackGold(20, 100);
        }

        public HireMage(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }
        public override void Serialize(GenericWriter writer) 
        {
            base.Serialize(writer);

            writer.Write((int)0);// version 
        }

        public override void Deserialize(GenericReader reader) 
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}