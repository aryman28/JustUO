using System;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class Ranger : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
        [Constructable]
        public Ranger()
            : base("- Stra¿nik Lasu")
        {
            this.SetSkill(SkillName.Obozowanie, 55.0, 78.0);
            this.SetSkill(SkillName.Wykrywanie, 65.0, 88.0);
            this.SetSkill(SkillName.Ukrywanie, 45.0, 68.0);
            this.SetSkill(SkillName.Lucznictwo, 65.0, 88.0);
            this.SetSkill(SkillName.Tropienie, 65.0, 88.0);
            this.SetSkill(SkillName.Weterynaria, 60.0, 83.0);
        }

        public Ranger(Serial serial)
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
            this.m_SBInfos.Add(new SBRanger());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            this.AddItem(new Server.Items.Shirt(Utility.RandomNeutralHue()));
            this.AddItem(new Server.Items.LongPants(Utility.RandomNeutralHue()));
            this.AddItem(new Server.Items.Bow());
            this.AddItem(new Server.Items.ThighBoots(Utility.RandomNeutralHue()));
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