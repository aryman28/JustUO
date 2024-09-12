using System;

namespace Server.Items
{
    public class Zakoñczono : Item
    {
        [Constructable]
        public Zakoñczono()
            : base(0x1D9F)
        {
            this.Weight = 0.0;
        }

        public Zakoñczono(Serial serial)
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