using System;
using Server.Items;

namespace Server.Factions
{
    public class FactionSorceress : BaseFactionGuard
    {
        [Constructable]
        public FactionSorceress()
            : base("the sorceress")
        {
            this.GenerateBody(true, false);

            this.SetStr(126, 150);
            this.SetDex(61, 85);
            this.SetInt(126, 150);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 30, 50);
            this.SetResistance(ResistanceType.Fire, 30, 50);
            this.SetResistance(ResistanceType.Cold, 30, 50);
            this.SetResistance(ResistanceType.Energy, 30, 50);
            this.SetResistance(ResistanceType.Poison, 30, 50);

            this.VirtualArmor = 24;

            this.SetSkill(SkillName.WalkaObuchami, 100.0, 110.0);
            this.SetSkill(SkillName.Boks, 100.0, 110.0);
            this.SetSkill(SkillName.Taktyka, 100.0, 110.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 100.0, 110.0);
            this.SetSkill(SkillName.Leczenie, 100.0, 110.0);
            this.SetSkill(SkillName.Anatomia, 100.0, 110.0);

            this.SetSkill(SkillName.Magia, 100.0, 110.0);
            this.SetSkill(SkillName.Intelekt, 100.0, 110.0);
            this.SetSkill(SkillName.Medytacja, 100.0, 110.0);

            this.AddItem(this.Immovable(this.Rehued(new WizardsHat(), 1325)));
            this.AddItem(this.Immovable(this.Rehued(new Sandals(), 1325)));
            this.AddItem(this.Immovable(this.Rehued(new LeatherGorget(), 1325)));
            this.AddItem(this.Immovable(this.Rehued(new LeatherGloves(), 1325)));
            this.AddItem(this.Immovable(this.Rehued(new LeatherLegs(), 1325)));
            this.AddItem(this.Immovable(this.Rehued(new Skirt(), 1325)));
            this.AddItem(this.Immovable(this.Rehued(new FemaleLeatherChest(), 1325)));
            this.AddItem(this.Newbied(this.Rehued(new QuarterStaff(), 1310)));

            this.PackItem(new Bandage(Utility.RandomMinMax(30, 40)));
            this.PackStrongPotions(6, 12);
        }

        public FactionSorceress(Serial serial)
            : base(serial)
        {
        }

        public override GuardAI GuardAI
        {
            get
            {
                return GuardAI.Magic | GuardAI.Bless | GuardAI.Curse;
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