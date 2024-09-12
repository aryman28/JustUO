using System;

namespace Server.Mobiles
{
    public class MinerGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public MinerGuildmaster()
            : base("- Mistrz Gildii Górnictwa")
        {
            this.SetSkill(SkillName.Identyfikacja, 60.0, 83.0);
            this.SetSkill(SkillName.Gornictwo, 120.0, 150.0);
        }

        public MinerGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.MinersGuild;
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