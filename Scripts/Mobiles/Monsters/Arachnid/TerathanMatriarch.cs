using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Zw�oki terathana")]
    public class TerathanMatriarch : BaseCreature
    {
        [Constructable]
        public TerathanMatriarch()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "W�adczyni terathan";
            this.Body = 72;
            this.BaseSoundID = 599;

            this.SetStr(316, 405);
            this.SetDex(96, 115);
            this.SetInt(366, 455);

            this.SetHits(190, 243);

            this.SetDamage(11, 14);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 30, 40);
            this.SetResistance(ResistanceType.Cold, 35, 45);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 35, 45);

            this.SetSkill(SkillName.Intelekt, 90.1, 100.0);
            this.SetSkill(SkillName.Magia, 90.1, 100.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 90.1, 100.0);
            this.SetSkill(SkillName.Taktyka, 50.1, 70.0);
            this.SetSkill(SkillName.Boks, 60.1, 80.0);

            this.Fame = 10000;
            this.Karma = -10000;

            this.PackItem(new SpidersSilk(5));
            this.PackNecroReg(Utility.RandomMinMax(4, 10));
        }

        public TerathanMatriarch(Serial serial)
            : base(serial)
        {
        }

        public override int TreasureMapLevel
        {
            get
            {
                return 4;
            }
        }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.TerathansAndOphidians;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Average, 2);
            this.AddLoot(LootPack.MedScrolls, 2);
            this.AddLoot(LootPack.Potions);
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