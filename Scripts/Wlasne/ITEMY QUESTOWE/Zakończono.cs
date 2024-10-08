using System;

namespace Server.Items
{
    public class Zakończono : Item
    {
        [Constructable]
        public Zakończono()
            : base(0x1D9F)
        {
            this.Weight = 0.0;
        }

        public Zakończono(Serial serial)
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