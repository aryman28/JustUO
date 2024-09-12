using System;

namespace Server.Items
{
    public class MarkOfTravesty : SavageMask, ITokunoDyable
    {
        [Constructable]
        public MarkOfTravesty()
            : base()
        {
            this.Hue = 0x495;
			
            this.Attributes.BonusMana = 8;
            //Attributes.RegenHits = 3;
			
            this.ClothingAttributes.SelfRepair = 3;
			
            switch( Utility.Random(15) )
            {
                case 0: 
                    this.SkillBonuses.SetValues(0, SkillName.Intelekt, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Magia, 10);
                    break;
                case 1: 
                    this.SkillBonuses.SetValues(0, SkillName.WiedzaOBestiach, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Oswajanie, 10);
                    break;
                case 2: 
                    this.SkillBonuses.SetValues(0, SkillName.WalkaMieczami, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Taktyka, 10);
                    break;
                case 3: 
                    this.SkillBonuses.SetValues(0, SkillName.Manipulacja, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Muzykowanie, 10);
                    break;
                case 4: 
                    this.SkillBonuses.SetValues(0, SkillName.WalkaSzpadami, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Taktyka, 10);
                    break;
                case 5: 
                    this.SkillBonuses.SetValues(0, SkillName.Rycerstwo, 10);
                    this.SkillBonuses.SetValues(1, SkillName.ObronaPrzedMagia, 10);
                    break;
                case 6: 
                    this.SkillBonuses.SetValues(0, SkillName.Anatomia, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Leczenie, 10);
                    break;
                case 7: 
                    this.SkillBonuses.SetValues(0, SkillName.Skrytobojstwo, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Zakradanie, 10);
                    break;
                case 8: 
                    this.SkillBonuses.SetValues(0, SkillName.Fanatyzm, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Parowanie, 10);
                    break;
                case 9: 
                    this.SkillBonuses.SetValues(0, SkillName.Lucznictwo, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Taktyka, 10);
                    break;
                case 10: 
                    this.SkillBonuses.SetValues(0, SkillName.WalkaObuchami, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Taktyka, 10);
                    break;
                case 11: 
                    this.SkillBonuses.SetValues(0, SkillName.Nekromancja, 10);
                    this.SkillBonuses.SetValues(1, SkillName.MowaDuchow, 10);
                    break;
                case 12: 
                    this.SkillBonuses.SetValues(0, SkillName.Zakradanie, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Okradanie, 10);
                    break;
                case 13: 
                    this.SkillBonuses.SetValues(0, SkillName.Uspokajanie, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Muzykowanie, 10);
                    break;
                case 14:
                    this.SkillBonuses.SetValues(0, SkillName.Prowokacja, 10);
                    this.SkillBonuses.SetValues(1, SkillName.Muzykowanie, 10);
                    break;
            }
        }

        public MarkOfTravesty(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1074493;
            }
        }// Mark of Travesty
        public override int BasePhysicalResistance
        {
            get
            {
                return 8;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 11;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 20;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 15;
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