using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class HolyMage : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public HolyMage()
            : base("- Kap³an")
        {
            this.SetSkill(SkillName.Intelekt, 65.0, 88.0);
            this.SetSkill(SkillName.Inskrypcja, 60.0, 83.0);
            this.SetSkill(SkillName.Magia, 64.0, 100.0);
            this.SetSkill(SkillName.Medytacja, 60.0, 83.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 65.0, 88.0);
            this.SetSkill(SkillName.Boks, 36.0, 68.0);
        }

        public HolyMage(Serial serial)
            : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos
        {
            get
            {
                return this.m_SBInfos;
            }
        }
        public override void InitSBInfo()
        {
            this.m_SBInfos.Add(new SBHolyMage());
        }

        public Item ApplyHue(Item item, int hue)
        {
            item.Hue = hue;

            return item;
        }

        public override void InitOutfit()
        {
            this.AddItem(this.ApplyHue(new Robe(), 0x47E));
            this.AddItem(this.ApplyHue(new ThighBoots(), 0x47E));
            this.AddItem(this.ApplyHue(new BlackStaff(), 0x47E));

            if (this.Female)
            {
                this.AddItem(this.ApplyHue(new LeatherGloves(), 0x47E));
                this.AddItem(this.ApplyHue(new GoldNecklace(), 0x47E));
            }
            else
            {
                this.AddItem(this.ApplyHue(new PlateGloves(), 0x47E));
                this.AddItem(this.ApplyHue(new PlateGorget(), 0x47E));
            }

            switch ( Utility.Random(this.Female ? 2 : 1) )
            {
                case 0:
                    this.HairItemID = 0x203C;
                    break;
                case 1:
                    this.HairItemID = 0x203D;
                    break;
            }

            this.HairHue = 0x47E;

            this.PackGold(100, 200);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}