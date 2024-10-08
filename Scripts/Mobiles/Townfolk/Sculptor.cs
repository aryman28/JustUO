using System;
using Server.Items;

namespace Server.Mobiles
{
    public class Sculptor : BaseCreature
    {
        [Constructable]
        public Sculptor()
            : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            this.InitStats(31, 41, 51);

            this.SpeechHue = Utility.RandomDyedHue();
            //this.Title = "the sculptor";
            this.Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                this.Body = 0x191;
                this.Name = NameList.RandomName("female");
                this.Title = "- Rzečbiarka";
                this.AddItem(new Kilt(Utility.RandomNeutralHue()));
            }
            else
            {
                this.Body = 0x190;
                this.Name = NameList.RandomName("male");
                this.Title = "- Rzečbiarz";
                this.AddItem(new LongPants(Utility.RandomNeutralHue()));
            }

            this.AddItem(new Doublet(Utility.RandomNeutralHue()));
            this.AddItem(new HalfApron());

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();

            pack.DropItem(new Gold(250, 300));

            pack.Movable = false;

            this.AddItem(pack);
        }

        public Sculptor(Serial serial)
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

            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}