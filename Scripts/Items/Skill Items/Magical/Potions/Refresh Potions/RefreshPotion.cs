using System;

namespace Server.Items
{
    public class RefreshPotion : BaseRefreshPotion
    {
        [Constructable]
        public RefreshPotion()
            : base(PotionEffect.Refresh)
        {
        }

        public RefreshPotion(Serial serial)
            : base(serial)
        {
        }

        public override double Refresh
        {
            get
            {
                return 0.25;
            }
        }

        public override double Delay
        {
            get
            {
                return 30.0;
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