using System;

namespace Server.Mobiles
{
    [CorpseName("a Chiikkaha the Toothed corpse")]
    public class Chiikkaha : RatmanMage
    {
        [Constructable]
        public Chiikkaha()
        {
            Name = "Chiikkaha the Toothed";

            SetStr(450, 476);
            SetDex(157, 179);
            SetInt(251, 275);

            SetHits(400, 425);

            SetDamage(10, 17);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 45);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 100);

            SetSkill(SkillName.Intelekt, 70.1, 80.0);
            SetSkill(SkillName.Magia, 70.1, 90.0);
            SetSkill(SkillName.ObronaPrzedMagia, 65.1, 96.0);
            SetSkill(SkillName.Taktyka, 50.1, 75.0);
            SetSkill(SkillName.Boks, 50.1, 75.0);

            Fame = 7500;
            Karma = -7500;
        }

        public Chiikkaha(Serial serial)
            : base(serial)
        {
        }

        public override bool AllureImmune
        {
            get
            {
                return true;
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