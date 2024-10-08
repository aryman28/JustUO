using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Anlorlem - zw�oki")]
    public class Anlorlem : BaseCreature
    {
        [Constructable]
        public Anlorlem()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Anlorlem";
            Body = 72;
            Hue = 2071;
            BaseSoundID = 644;

            SetStr(416, 505);
            SetDex(96, 115);
            SetInt(366, 455);

            SetHits(250, 303);

            SetDamage(11, 13);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 35, 45);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Intelekt, 90.1, 100.0);
            SetSkill(SkillName.Magia, 90.1, 100.0);
            SetSkill(SkillName.Medytacja, 5.4, 25.0);
            SetSkill(SkillName.ObronaPrzedMagia, 90.1, 100.0);
            SetSkill(SkillName.Taktyka, 50.1, 70.0);
            SetSkill(SkillName.Boks, 60.1, 80.0);

            Fame = 16000;
            Karma = -16000;

            QLPoints = 20;

            VirtualArmor = 50;

            PackItem(new DaemonBone(15));
        }

        public Anlorlem(Serial serial)
            : base(serial)
        {
        }

        public override bool BardImmune
        {
            get
            {
                return !Core.AOS;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return true;
            }
        }
        public override bool ReacquireOnMovement
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Greater;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.5)
                c.DropItem(new VoidEssence(2));

            if (Utility.RandomDouble() < 0.20)
            { 
                c.DropItem(new VoidOrb());
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