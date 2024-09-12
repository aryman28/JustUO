using System;

namespace Server.Mobiles
{
    public class RangerGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public RangerGuildmaster()
            : base("- Mistrz Gildii Stra¿ników Lasu")
        {
            this.SetSkill(SkillName.WiedzaOBestiach, 64.0, 100.0);
            this.SetSkill(SkillName.Obozowanie, 75.0, 98.0);
            this.SetSkill(SkillName.Ukrywanie, 120.0, 150.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 75.0, 98.0);
            this.SetSkill(SkillName.Taktyka, 65.0, 88.0);
            this.SetSkill(SkillName.Lucznictwo, 120.0, 150.0);
            this.SetSkill(SkillName.Tropienie, 120.0, 150.0);
            this.SetSkill(SkillName.Zakradanie, 120.0, 150.0);
            this.SetSkill(SkillName.WalkaSzpadami, 36.0, 68.0);
            this.SetSkill(SkillName.Zielarstwo, 36.0, 68.0);
            this.SetSkill(SkillName.WalkaMieczami, 45.0, 68.0);
        }

        public RangerGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.RangersGuild;
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