using System;
using Server.Items;

namespace Server.Mobiles 
{
    public class HireThief : BaseHire 
    {
        [Constructable] 
        public HireThief()
        {
            this.SpeechHue = Utility.RandomDyedHue();
            this.Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool()) 
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName("female");

                switch ( Utility.Random(2) )
                {
                    case 0:
                        this.AddItem(new Skirt(Utility.RandomNeutralHue()));
                        break;
                    case 1:
                        this.AddItem(new Kilt(Utility.RandomNeutralHue()));
                        break;
                }
            }
            else 
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName("male");
                this.AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }
            this.Title = "- Z�odziej (najemnik)";
            this.HairItemID = this.Race.RandomHair(this.Female);
            this.HairHue = this.Race.RandomHairHue();
            this.Race.RandomFacialHair(this);

            this.SetStr(81, 95);
            this.SetDex(86, 100);
            this.SetInt(61, 75);

            this.SetDamage(10, 23);

            this.SetSkill(SkillName.Okradanie, 66.0, 97.5);
            this.SetSkill(SkillName.Uspokajanie, 65.0, 87.5);
            this.SetSkill(SkillName.ObronaPrzedMagia, 25.0, 47.5);
            this.SetSkill(SkillName.Leczenie, 65.0, 87.5);
            this.SetSkill(SkillName.Taktyka, 65.0, 87.5);
            this.SetSkill(SkillName.WalkaSzpadami, 65.0, 87.5);
            this.SetSkill(SkillName.Parowanie, 45.0, 60.5);
            this.SetSkill(SkillName.Wlamywanie, 65, 87);
            this.SetSkill(SkillName.Ukrywanie, 65, 87);
            this.SetSkill(SkillName.Zagladanie, 65, 87);	
            this.Fame = 100;
            this.Karma = 0;

            this.AddItem(new Sandals(Utility.RandomNeutralHue()));
            this.AddItem(new Dagger());
            switch ( Utility.Random(2) )
            {
                case 0:
                    this.AddItem(new Doublet(Utility.RandomNeutralHue()));
                    break;
                case 1:
                    this.AddItem(new Shirt(Utility.RandomNeutralHue()));
                    break;
            }
		
            this.PackGold(0, 25);
        }

        public HireThief(Serial serial)
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