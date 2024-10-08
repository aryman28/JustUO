using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Anlorzen zw�oki")]
    public class Anlorzen : BaseCreature
    {
        [Constructable]
        public Anlorzen()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Anlorzen";
            Body = 11;
            BaseSoundID = 1170;

            SetStr(196, 220);
            SetDex(126, 145);
            SetInt(286, 310);

            SetHits(1118, 1132);

            SetDamage(15, 17);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Poison, 80);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.Intelekt, 65.1, 80.0);
            SetSkill(SkillName.Magia, 65.1, 80.0);
            SetSkill(SkillName.Medytacja, 65.1, 80.0);
            SetSkill(SkillName.ObronaPrzedMagia, 45.1, 60.0);
            SetSkill(SkillName.Taktyka, 55.1, 70.0);
            SetSkill(SkillName.Boks, 60.1, 75.0);

            Fame = 5000;
            Karma = -5000;

            QLPoints = 10;

            VirtualArmor = 56;

            PackItem(new DaemonBone(5));
        }

        public Anlorzen(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.10)
                c.DropItem(new VoidOrb());

            if (Utility.RandomDouble() < 0.20)
                c.DropItem(new VoidEssence());
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

            if (BaseSoundID == 263)
                BaseSoundID = 1170;
        }
    }
}