using System;

namespace Server.Mobiles
{
    public class EvilHealer : BaseHealer
    {
        [Constructable]
        public EvilHealer()
        {
            this.Title = "- Uzdrowiciel (chaos)";

            this.Karma = -10000;

            this.SetSkill(SkillName.Kryminalistyka, 80.0, 100.0);
            this.SetSkill(SkillName.MowaDuchow, 80.0, 100.0);
            this.SetSkill(SkillName.WalkaMieczami, 80.0, 100.0);
        }

        public EvilHealer(Serial serial)
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
        public override bool AlwaysMurderer
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
            if (Core.AOS && m.Criminal)
            {
                this.Say(501222); // Thou art a criminal.  I shall not resurrect thee.
                return false;
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
        }
    }
}