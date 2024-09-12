using System;
using System.Collections.Generic;

namespace Server.Mobiles 
{ 
    public class HairStylist : BaseVendor 
    { 
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public HairStylist()
            : base("- Fryzjer")
        { 
            this.SetSkill(SkillName.Alchemia, 80.0, 100.0);
            this.SetSkill(SkillName.Magia, 90.0, 110.0);
            this.SetSkill(SkillName.OcenaSmaku, 85.0, 100.0);
        }

        public HairStylist(Serial serial)
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
            this.m_SBInfos.Add(new SBHairStylist()); 
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