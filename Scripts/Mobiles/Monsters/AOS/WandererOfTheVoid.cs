using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Zw³oki potwora")]
    public class WandererOfTheVoid : BaseCreature
    {
        [Constructable]
        public WandererOfTheVoid()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Wêdruj¹ca pustka";
            this.Body = 316;
            this.BaseSoundID = 377;

            this.SetStr(111, 200);
            this.SetDex(101, 125);
            this.SetInt(301, 390);

            this.SetHits(351, 400);

            this.SetDamage(11, 13);

            this.SetDamageType(ResistanceType.Physical, 0);
            this.SetDamageType(ResistanceType.Cold, 15);
            this.SetDamageType(ResistanceType.Energy, 85);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 40, 50);
            this.SetResistance(ResistanceType.Poison, 50, 75);
            this.SetResistance(ResistanceType.Energy, 40, 50);

            this.SetSkill(SkillName.Intelekt, 60.1, 70.0);
            this.SetSkill(SkillName.Magia, 60.1, 70.0);
            this.SetSkill(SkillName.Medytacja, 60.1, 70.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 50.1, 75.0);
            this.SetSkill(SkillName.Taktyka, 60.1, 70.0);
            this.SetSkill(SkillName.Boks, 60.1, 70.0);

            this.Fame = 20000;
            this.Karma = -20000;

            this.VirtualArmor = 44;

            int count = Utility.RandomMinMax(2, 3);

            for (int i = 0; i < count; ++i)
                this.PackItem(new TreasureMap(3, Map.Trammel));
        }

        public WandererOfTheVoid(Serial serial)
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
        public override Poison PoisonImmune
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
                return Core.AOS ? 4 : 1;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich);
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