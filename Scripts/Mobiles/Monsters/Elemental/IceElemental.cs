using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an ice elemental corpse")]
    public class IceElemental : BaseCreature
    {
        [Constructable]
        public IceElemental()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "an ice elemental";
            this.Body = 161;
            this.BaseSoundID = 268;

            this.SetStr(156, 185);
            this.SetDex(96, 115);
            this.SetInt(171, 192);

            this.SetHits(94, 111);

            this.SetDamage(10, 21);

            this.SetDamageType(ResistanceType.Physical, 25);
            this.SetDamageType(ResistanceType.Cold, 75);

            this.SetResistance(ResistanceType.Physical, 35, 45);
            this.SetResistance(ResistanceType.Fire, 5, 10);
            this.SetResistance(ResistanceType.Cold, 50, 60);
            this.SetResistance(ResistanceType.Poison, 20, 30);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.Intelekt, 10.5, 60.0);
            this.SetSkill(SkillName.Magia, 10.5, 60.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 30.1, 80.0);
            this.SetSkill(SkillName.Taktyka, 70.1, 100.0);
            this.SetSkill(SkillName.Boks, 60.1, 100.0);

            this.Fame = 4000;
            this.Karma = -4000;

            this.VirtualArmor = 40;

            this.PackItem(new BlackPearl());
            this.PackReg(3);
        }

        public IceElemental(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Average, 2);
            this.AddLoot(LootPack.Gems, 2);
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