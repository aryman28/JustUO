using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Zw�oki paj�ka")]
    public class WolfSpider : BaseCreature
    {
        [Constructable]
        public WolfSpider()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Paj�k - tropiciel";
            this.Body = 736;
            this.Hue = 0;

            this.SetStr(225, 268);
            this.SetDex(145, 165);
            this.SetInt(285, 310);

            this.SetHits(150, 200);
	    this.SetMana(285, 310);
	    this.SetStam(145, 165);

            this.SetDamage(15, 18);

            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Poison, 30);

            this.SetResistance(ResistanceType.Physical, 30, 35);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 100);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.Anatomia, 80.0, 90.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 60.0, 75.0);
            this.SetSkill(SkillName.Zatruwanie, 62.3, 77.2);
            this.SetSkill(SkillName.Taktyka, 84.1, 95.9);
            this.SetSkill(SkillName.Boks, 80.2, 90.0);

            this.Fame = 5000;
            this.Karma = -5000;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = 59.1;

            this.AddItem(new Gold(300));
            this.PackItem(new SpidersSilk(8));
            this.PackMagicItems(1, 2);
        }

        public WolfSpider(Serial serial)
            : base(serial)
        {
        }

        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Arachnid;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Regular;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return Poison.Regular;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Gems, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.15)
                c.DropItem(new BottleIchor());
        }

        public override int GetIdleSound()
        {
            return 1605;
        }

        public override int GetAngerSound()
        {
            return 1602;
        }

        public override int GetHurtSound()
        {
            return 1604;
        }

        public override int GetDeathSound()
        {
            return 1603;
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