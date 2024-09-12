using System;

namespace Server.Mobiles
{
    public class BardGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public BardGuildmaster()
            : base("- Mistrz Gildii Bardów")
        {
            this.SetSkill(SkillName.Lucznictwo, 80.0, 100.0);
            this.SetSkill(SkillName.Manipulacja, 80.0, 100.0);
            this.SetSkill(SkillName.Muzykowanie, 120.0, 150.0);
            this.SetSkill(SkillName.Uspokajanie, 80.0, 100.0);
            this.SetSkill(SkillName.Prowokacja, 80.0, 100.0);
            this.SetSkill(SkillName.WalkaMieczami, 80.0, 100.0);
        }

        public BardGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.BardsGuild;
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