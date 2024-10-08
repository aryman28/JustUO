using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Zw�oki potwora")]
    public class DarknightCreeper : BaseCreature
    {
        [Constructable]
        public DarknightCreeper()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = NameList.RandomName("darknight creeper");
            this.Body = 313;
            this.BaseSoundID = 0xE0;

            this.SetStr(301, 330);
            this.SetDex(101, 110);
            this.SetInt(301, 330);

            this.SetHits(4000);

            this.SetDamage(22, 26);

            this.SetDamageType(ResistanceType.Physical, 85);
            this.SetDamageType(ResistanceType.Poison, 15);

            this.SetResistance(ResistanceType.Physical, 60);
            this.SetResistance(ResistanceType.Fire, 60);
            this.SetResistance(ResistanceType.Cold, 100);
            this.SetResistance(ResistanceType.Poison, 90);
            this.SetResistance(ResistanceType.Energy, 75);

            this.SetSkill(SkillName.Wykrywanie, 80.0);
            this.SetSkill(SkillName.Intelekt, 118.1, 120.0);
            this.SetSkill(SkillName.Magia, 112.6, 120.0);
            this.SetSkill(SkillName.Medytacja, 150.0);
            this.SetSkill(SkillName.Zatruwanie, 120.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 90.1, 90.9);
            this.SetSkill(SkillName.Taktyka, 100.0);
            this.SetSkill(SkillName.Boks, 90.1, 90.9);
            this.SetSkill(SkillName.Nekromancja, 120.1, 130.0);
            this.SetSkill(SkillName.MowaDuchow, 120.1, 130.0);

            this.Fame = 22000;
            this.Karma = -22000;

            this.VirtualArmor = 34;
        }

        public DarknightCreeper(Serial serial)
            : base(serial)
        {
        }

        public override bool IgnoreYoungProtection
        {
            get
            {
                return Core.ML;
            }
        }
        public override bool BardImmune
        {
            get
            {
                return !Core.SE;
            }
        }
        public override bool Unprovokable
        {
            get
            {
                return Core.SE;
            }
        }
        public override bool AreaPeaceImmune
        {
            get
            {
                return Core.SE;
            }
        }
        public override bool BleedImmune
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
        public override int TreasureMapLevel
        {
            get
            {
                return 1;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (!this.Summoned && !this.NoKillAwards && DemonKnight.CheckArtifactChance(this))
                DemonKnight.DistributeArtifact(this);
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

            if (this.BaseSoundID == 471)
                this.BaseSoundID = 0xE0;
        }
    }
}