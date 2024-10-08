using System;

namespace Server.Mobiles
{
    [CorpseName("a fetid essence corpse")]
    public class FetidEssence : BaseCreature
    {
        [Constructable]
        public FetidEssence()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a fetid essence";
            this.Body = 273;

            this.SetStr(101, 150);
            this.SetDex(210, 250);
            this.SetInt(451, 550);

            this.SetHits(551, 650);

            this.SetDamage(21, 25);

            this.SetDamageType(ResistanceType.Physical, 30);
            this.SetDamageType(ResistanceType.Poison, 70);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 40, 50);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 70, 90);
            this.SetResistance(ResistanceType.Energy, 75, 80);

            this.SetSkill(SkillName.Medytacja, 91.4, 99.4);
            this.SetSkill(SkillName.Intelekt, 88.5, 92.3);
            this.SetSkill(SkillName.Magia, 97.9, 101.7);
            this.SetSkill(SkillName.Zatruwanie, 100);
            this.SetSkill(SkillName.Anatomia, 0, 4.5);
            this.SetSkill(SkillName.ObronaPrzedMagia, 103.5, 108.8);
            this.SetSkill(SkillName.Taktyka, 81.0, 84.6);
            this.SetSkill(SkillName.Boks, 81.3, 83.9);

            this.Fame = 3700;  // Guessed
            this.Karma = -3700;  // Guessed
        }

        public FetidEssence(Serial serial)
            : base(serial)
        {
        }

        public override Poison HitPoison
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Deadly;
            }
        }
        public override void GenerateLoot() // Need to verify
        {
            this.AddLoot(LootPack.FilthyRich);
        }

        public override int GetAngerSound()
        {
            return 0x56d;
        }

        public override int GetIdleSound()
        {
            return 0x56b;
        }

        public override int GetAttackSound()
        {
            return 0x56c;
        }

        public override int GetHurtSound()
        {
            return 0x56c;
        }

        public override int GetDeathSound()
        {
            return 0x56e;
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
        /*private class InternalTimer : Timer
        {
        private Mobile m_From;
        private Mobile m_Mobile;
        private int m_Count;
        public InternalTimer( Mobile from, Mobile m ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
        {
        m_From = from;
        m_Mobile = m;
        Priority = TimerPriority.TwoFiftyMS;
        }
        }*/
    }
}