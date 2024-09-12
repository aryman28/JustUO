using System;

namespace Server.Mobiles
{
    [CorpseName("zwłoki niedzwiedzia")]
    public class ArcaneFey : BaseCreature
    {
        [Constructable]
        public ArcaneFey()
            : base(AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4)
        {
            this.Name = "czarny niedzwiedz";
            this.Body = 211;
            this.BaseSoundID = 0xA3;

            this.SetStr(80, 100);
            this.SetDex(40, 60);
            this.SetInt(11, 14);

            this.SetHits(70, 90);
            this.SetMana(0);

            this.SetDamage(8, 12);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 70, 100);
            this.SetResistance(ResistanceType.Cold, 10, 15);
            this.SetResistance(ResistanceType.Poison, 5, 10);

            this.SetSkill(SkillName.ObronaPrzedMagia, 60.1, 80.0);
            this.SetSkill(SkillName.Taktyka, 80.1, 100.0);
            this.SetSkill(SkillName.Boks, 80.1, 100.0);

            this.Fame = 0;
            this.Karma = 0;

            this.VirtualArmor = 24;
            this.ControlSlots = 2;

        }

        public ArcaneFey(Serial serial)
            : base(serial)
        {
        }

        public override double DispelDifficulty
        {
            get
            {
                return 70.0;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 20.0;
            }
        }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override bool InitialInnocent
        {
            get
            {
                return true;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}