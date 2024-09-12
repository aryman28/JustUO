using System;
using Server.Items;

namespace Server.Mobiles
{
    public class EvilWanderingHealer : BaseHealer
    {
        [Constructable]
        public EvilWanderingHealer()
        {
            this.Title = (Core.AOS) ? "Mroczny ksi¹dz" : "Uzdrowiciel (chaos)";
            this.Karma = -10000;

            this.AddItem(new GnarledStaff());

            this.SetSkill(SkillName.Obozowanie, 80.0, 100.0);
            this.SetSkill(SkillName.Kryminalistyka, 80.0, 100.0);
            this.SetSkill(SkillName.MowaDuchow, 80.0, 100.0);
        }

        public EvilWanderingHealer(Serial serial)
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
        public override bool ClickTitle
        {
            get
            {
                return false;
            }
        }// Do not display title in OnSingleClick
        public override bool CheckTeach(SkillName skill, Mobile from)
        {
            if (!base.CheckTeach(skill, from))
                return false;

            return (skill == SkillName.Anatomia) ||
                   (skill == SkillName.Obozowanie) ||
                   (skill == SkillName.Kryminalistyka) ||
                   (skill == SkillName.Leczenie) ||
                   (skill == SkillName.MowaDuchow);
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

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version < 1 && this.Title == "the wandering healer" && Core.AOS)
                this.Title = "the priest of Mondain";
        }
    }
}