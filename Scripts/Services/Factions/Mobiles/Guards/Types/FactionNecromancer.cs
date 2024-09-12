using System;
using Server.Items;

namespace Server.Factions
{
    public class FactionNecromancer : BaseFactionGuard
    {
        [Constructable]
        public FactionNecromancer()
            : base("the necromancer")
        {
            this.GenerateBody(false, false);
            this.Hue = 1;

            this.SetStr(151, 175);
            this.SetDex(61, 85);
            this.SetInt(151, 175);

            this.SetResistance(ResistanceType.Physical, 40, 60);
            this.SetResistance(ResistanceType.Fire, 40, 60);
            this.SetResistance(ResistanceType.Cold, 40, 60);
            this.SetResistance(ResistanceType.Energy, 40, 60);
            this.SetResistance(ResistanceType.Poison, 40, 60);

            this.VirtualArmor = 32;

            this.SetSkill(SkillName.WalkaObuchami, 110.0, 120.0);
            this.SetSkill(SkillName.WalkaMieczami, 110.0, 120.0);
            this.SetSkill(SkillName.Taktyka, 110.0, 120.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 110.0, 120.0);
            this.SetSkill(SkillName.Leczenie, 110.0, 120.0);
            this.SetSkill(SkillName.Anatomia, 110.0, 120.0);

            this.SetSkill(SkillName.Magia, 110.0, 120.0);
            this.SetSkill(SkillName.Intelekt, 110.0, 120.0);
            this.SetSkill(SkillName.Medytacja, 110.0, 120.0);

            Item shroud = new Item(0x204E);
            shroud.Layer = Layer.OuterTorso;

            this.AddItem(this.Immovable(this.Rehued(shroud, 1109)));
            this.AddItem(this.Newbied(this.Rehued(new GnarledStaff(), 2211)));

            this.PackItem(new Bandage(Utility.RandomMinMax(30, 40)));
            this.PackStrongPotions(6, 12);
        }

        public FactionNecromancer(Serial serial)
            : base(serial)
        {
        }

        public override GuardAI GuardAI
        {
            get
            {
                return GuardAI.Magic | GuardAI.Smart | GuardAI.Bless | GuardAI.Curse;
            }
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