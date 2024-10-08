using System;

namespace Server.Mobiles
{
    public class MerchantGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public MerchantGuildmaster()
            : base("- Mistrz Gildii Kupiectwa")
        {
            this.SetSkill(SkillName.Identyfikacja, 120.0, 150.0);
            this.SetSkill(SkillName.WiedzaOUzbrojeniu, 120.0, 150.0);
        }

        public MerchantGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.MerchantsGuild;
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