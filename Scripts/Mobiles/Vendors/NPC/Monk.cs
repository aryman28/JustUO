using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class Monk : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public Monk()
            : base("- Mnich")
        {
            this.SetSkill(SkillName.Intelekt, 100.0);
            this.SetSkill(SkillName.Taktyka, 70.0, 90.0);
            this.SetSkill(SkillName.Boks, 70.0, 90.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 70.0, 90.0);
            this.SetSkill(SkillName.WalkaObuchami, 70.0, 90.0);
        }

        public Monk(Serial serial)
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
            this.m_SBInfos.Add(new SBMonk());
        }

        public override void InitOutfit()
        {
            this.AddItem(new Sandals());
            this.AddItem(new MonkRobe());
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