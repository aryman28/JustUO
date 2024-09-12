using System;

namespace Server.Items
{
    public class GargishRing : BaseRing
    {
        [Constructable]
        public GargishRing()
            : base(0x4212)
        {
            this.Name = "Demoni Pierœcieñ";
            this.Weight = 0.1;
        }

        public GargishRing(Serial serial)
            : base(serial)
        {
        }

        public override int InitMinHits
        {
            get
            {
                return 12;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 12;
            }
        }

        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
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