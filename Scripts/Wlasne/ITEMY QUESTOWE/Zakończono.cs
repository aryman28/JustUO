using System;

namespace Server.Items
{
    public class Zako�czono : Item
    {
        [Constructable]
        public Zako�czono()
            : base(0x1D9F)
        {
            this.Weight = 0.0;
        }

        public Zako�czono(Serial serial)
            : base(serial)
        {
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