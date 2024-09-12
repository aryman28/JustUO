using System;

namespace Server.Mobiles
{
    public class WarriorGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public WarriorGuildmaster()
            : base("- Mistrz Gildii Wojowników")
        {
            this.SetSkill(SkillName.WiedzaOUzbrojeniu, 75.0, 98.0);
            this.SetSkill(SkillName.Parowanie, 85.0, 100.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 60.0, 83.0);
            this.SetSkill(SkillName.Taktyka, 120.0, 150.0);
            this.SetSkill(SkillName.WalkaMieczami, 120.0, 150.0);
            this.SetSkill(SkillName.WalkaObuchami, 120.0, 150.0);
            this.SetSkill(SkillName.WalkaSzpadami, 120.0, 150.0);
        }

        public WarriorGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.WarriorsGuild;
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