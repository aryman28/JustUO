using System;

namespace Server.Mobiles
{
    public class MageGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public MageGuildmaster()
            : base("- Mistrz Gildii Magów")
        {
            this.SetSkill(SkillName.Intelekt, 120.0, 150.0);
            this.SetSkill(SkillName.Inskrypcja, 65.0, 88.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 120.0, 150.0);
            this.SetSkill(SkillName.Magia, 90.0, 100.0);
            this.SetSkill(SkillName.Boks, 60.0, 83.0);
            this.SetSkill(SkillName.Medytacja, 120.0, 150.0);
            this.SetSkill(SkillName.WalkaObuchami, 36.0, 68.0);
        }

        public MageGuildmaster(Serial serial)
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
        public override void InitOutfit()
        {
            base.InitOutfit();

            this.AddItem(new Server.Items.Robe(Utility.RandomBlueHue()));
            this.AddItem(new Server.Items.GnarledStaff());
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