using System;

namespace Server.Mobiles
{
    public class HealerGuildmaster : BaseGuildmaster
    {
        [Constructable]
        public HealerGuildmaster()
            : base("- Mistrz Gildii Uzdrowicieli")
        {
            this.SetSkill(SkillName.Anatomia, 120.0, 150.0);
            this.SetSkill(SkillName.Leczenie, 120.0, 150.0);
            this.SetSkill(SkillName.Kryminalistyka, 75.0, 98.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 120.0, 150.0);
            this.SetSkill(SkillName.MowaDuchow, 65.0, 88.0);
        }

        public HealerGuildmaster(Serial serial)
            : base(serial)
        {
        }

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.HealersGuild;
            }
        }
        public override VendorShoeType ShoeType
        {
            get
            {
                return VendorShoeType.Sandals;
            }
        }
        public override void InitOutfit()
        {
            base.InitOutfit();

            this.AddItem(new Server.Items.Robe(Utility.RandomYellowHue()));
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