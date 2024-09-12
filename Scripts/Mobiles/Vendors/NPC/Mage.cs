using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class Mage : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public Mage()
            : base("- Mag")
        {
            this.SetSkill(SkillName.Intelekt, 65.0, 88.0);
            this.SetSkill(SkillName.Inskrypcja, 60.0, 83.0);
            this.SetSkill(SkillName.Magia, 64.0, 100.0);
            this.SetSkill(SkillName.Medytacja, 60.0, 83.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 65.0, 88.0);
            this.SetSkill(SkillName.Boks, 36.0, 68.0);
        }

        public Mage(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.MagesGuild;
            }
        }
        public override VendorShoeType ShoeType
        {
            get
            {
                return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals;
            }
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
            this.m_SBInfos.Add(new SBMage());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            this.AddItem(new Server.Items.Robe(Utility.RandomBlueHue()));
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