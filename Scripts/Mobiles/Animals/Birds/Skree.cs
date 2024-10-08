using System;

namespace Server.Mobiles
{
    [CorpseName("a skree corpse")]
    public class Skree : BaseCreature
    {
        [Constructable]
        public Skree()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a skree";
            Body = 733; 

            SetStr(297, 330);
            SetDex(96, 124);
            SetInt(188, 260);

            SetHits(205, 300);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 45, 55);
            SetResistance(ResistanceType.Cold, 25, 40);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 25, 40);

            SetSkill(SkillName.Intelekt, 90.6, 100.0);
            SetSkill(SkillName.Magia, 90.2, 114.2);
            SetSkill(SkillName.Medytacja, 65.3, 75.0);
            SetSkill(SkillName.ObronaPrzedMagia, 75.1, 90.0);
            SetSkill(SkillName.Taktyka, 20.2, 24.7);
            SetSkill(SkillName.Boks, 20.2, 34.8);

            Tamable = true;
            ControlSlots = 4;
            MinTameSkill = 95.1;

            QLPoints = 15;
        }

        public Skree(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override MeatType MeatType
        {
            get
            {
                return MeatType.Bird;
            }
        }
        public override int Hides
        {
            get
            {
                return 5;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override int GetIdleSound()
        {
            return 1585;
        }

        public override int GetAngerSound()
        {
            return 1582;
        }

        public override int GetHurtSound()
        {
            return 1584;
        }

        public override int GetDeathSound()
        {
            return 1583;
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