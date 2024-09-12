using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class Kartograf : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public Kartograf()
            : base("- Kartograf")
        {
            this.SetSkill(SkillName.Kartografia, 90.0, 100.0);
        }

        public Kartograf(Serial serial)
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
            this.m_SBInfos.Add(new SBKartograf());

            if (this.IsTokunoVendor)
                this.m_SBInfos.Add(new SBSEHats());
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