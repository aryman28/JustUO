using System;

namespace Server.Mobiles
{
    public class Healer : BaseHealer
    {
        [Constructable]
        public Healer()
        {
            this.Title = "- Uzdrowiciel";

            if (!Core.AOS)
                this.NameHue = 0x35;

            this.SetSkill(SkillName.Kryminalistyka, 80.0, 100.0);
            this.SetSkill(SkillName.MowaDuchow, 80.0, 100.0);
            this.SetSkill(SkillName.WalkaMieczami, 80.0, 100.0);
        }

        public Healer(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach
        {
            get
            {
                return true;
            }
        }
        public override bool IsActiveVendor
        {
            get
            {
                return true;
            }
        }
        public override bool IsInvulnerable
        {
            get
            {
                return true;
            }
        }
        public override bool CheckTeach(SkillName skill, Mobile from)
        {
            if (!base.CheckTeach(skill, from))
                return false;

            return (skill == SkillName.Kryminalistyka) ||
                   (skill == SkillName.Leczenie) ||
                   (skill == SkillName.MowaDuchow) ||
                   (skill == SkillName.WalkaMieczami);
        }

        public override void InitSBInfo()
        {
            this.SBInfos.Add(new SBHealer());
        }

        public override bool CheckResurrect(Mobile m)
        {
            if (m.Criminal)
            {
                this.Say(501222); // Thou art a criminal.  I shall not resurrect thee.
                return false;
            }
            else if (m.Kills >= 5)
            {
                this.Say(501223); // Thou'rt not a decent and good person. I shall not resurrect thee.
                return false;
            }
            else if (m.Karma < 0)
            {
                this.Say(501224); // Thou hast strayed from the path of virtue, but thou still deservest a second chance.
            }

            return true;
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

            if (Core.AOS && this.NameHue == 0x35)
                this.NameHue = -1;
        }
    }
}