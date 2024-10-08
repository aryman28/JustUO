using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a tangling root corpse")]
    public class TanglingRoots : BaseCreature
    {
        [Constructable]
        public TanglingRoots()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a tangling root";
            this.Body = 743;
            this.BaseSoundID = 684;

            this.SetStr(150, 157);
            this.SetDex(55, 60);
            this.SetInt(30, 35);

            this.SetHits(200, 232);
            this.SetStam(55, 60);

            this.SetDamage(10, 23);

            this.SetDamageType(ResistanceType.Physical, 60);
            this.SetDamageType(ResistanceType.Poison, 40);

            this.SetResistance(ResistanceType.Physical, 15, 20);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 20, 30);

            this.SetSkill(SkillName.ObronaPrzedMagia, 15.1, 18.5);
            this.SetSkill(SkillName.Taktyka, 45.1, 59.5);
            this.SetSkill(SkillName.Boks, 45.1, 60.0);

            this.Fame = 1000;
            this.Karma = -1000;

            this.VirtualArmor = 18;

            if (0.25 > Utility.RandomDouble())
                this.PackItem(new Board(10));
            else
                this.PackItem(new Log(10));

            this.PackItem(new MandrakeRoot(3));
        }

        public TanglingRoots(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lesser;
            }
        }
        public override bool DisallowAllMoves
        {
            get
            {
                return true;
            }
        }
        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Meager);
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

            if (this.BaseSoundID == 352)
                this.BaseSoundID = 684;
        }
    }
}