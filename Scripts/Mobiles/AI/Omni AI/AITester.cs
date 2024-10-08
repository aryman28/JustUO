// Created by Peoharen
using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a corpse")]
    public class AITester : BaseCreature
    {
        [Constructable]
        public AITester()
            : this(12)
        {
        }

        [Constructable]
        public AITester(int i)
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Dark Knight";
            this.Body = 400;
            this.Hue = 1175;

            this.SetStr(150);
            this.SetDex(180);
            this.SetInt(1500);

            this.SetHits(5000);
            this.SetDamage(15, 20);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 50);
            this.SetResistance(ResistanceType.Fire, 50);
            this.SetResistance(ResistanceType.Cold, 50);
            this.SetResistance(ResistanceType.Poison, 50);
            this.SetResistance(ResistanceType.Energy, 50);

            if (i == 0)
            {
                this.SetSkill(SkillName.Intelekt, 120.0);
                this.SetSkill(SkillName.Magia, 120.0);
            }
            else if (i == 1)
            {
                this.SetSkill(SkillName.Nekromancja, 120.0);
                this.SetSkill(SkillName.MowaDuchow, 120.0);
            }
            else if (i == 2)
            {
                this.SetSkill(SkillName.Fanatyzm, 120.0);
                this.SetSkill(SkillName.Parowanie, 120.0);
            }
            else if (i == 3)
            {
                this.SetSkill(SkillName.Skrytobojstwo, 120.0);
                this.SetSkill(SkillName.Ukrywanie, 120.0);
                this.SetSkill(SkillName.Zakradanie, 120.0);
            }
            else if (i == 4)
            {
                this.SetSkill(SkillName.Druidyzm, 120.0);
                this.SetSkill(SkillName.Intelekt, 120.0);
            }
            else if (i == 5)
            {
                this.SetSkill(SkillName.Mistycyzm, 120.0);
            }

            this.SetSkill(SkillName.Anatomia, 100.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 120.0);
            this.SetSkill(SkillName.WalkaMieczami, 100.0);
            this.SetSkill(SkillName.Taktyka, 100.0);

            this.Fame = 32000;
            this.Karma = -32000;

            this.VirtualArmor = 80;

            int hue = 1175;
            GiveItem(this, hue, new DragonHelm());
            GiveItem(this, hue, new PlateGorget());
            GiveItem(this, hue, new DragonChest());
            GiveItem(this, hue, new DragonLegs());
            GiveItem(this, hue, new DragonArms());
            GiveItem(this, hue, new DragonGloves());

            Longsword sword = new Longsword();
            sword.ItemID = 9934;
            GiveItem(this, hue, sword);
        }

        public AITester(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer
        {
            get
            {
                return true;
            }
        }
        public override bool ShowFameTitle
        {
            get
            {
                return false;
            }
        }
        protected override BaseAI ForcedAI
        {
            get
            {
                return new OmniAI(this);
            }
        }
        public static void GiveItem(Mobile to, int hue, Item item)
        {
            if (to == null && item == null)
                return;

            if (hue != 0)
                item.Hue = hue;

            item.Movable = false;
            to.EquipItem(item);
            return;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich, 2);
            this.AddLoot(LootPack.Gems, 50);
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