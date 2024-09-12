using System;

namespace Server.Items
{
    [FlipableAttribute(0x13d5, 0x13dd)]
    public class GiermekRekawice : BaseArmor
    {
        public override int LabelNumber{ get{ return 1080160; } } // Thank you Paradyme
		
		public override SetItem SetID{ get{ return SetItem.Giermek; } }
		public override int Pieces{ get{ return 5; } }

        [Constructable]
        public GiermekRekawice()
            : base(0x13D5)
        {
            this.Weight = 1.0;
            this.Name = "Skórzane Utwardzane Rêkawice";
            SetHue = 0x1E8;
            SetAttributes.BonusHits = 5;
            SetPhysicalBonus = 5;
			SetFireBonus = 2;
			SetColdBonus = 2;
			SetPoisonBonus = 2;
			SetEnergyBonus = 2;
            //Identified = true;
        }

        public GiermekRekawice(Serial serial)
            : base(serial)
        {
        }

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
                return 4;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 4;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 80;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 100;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 25;
            }
        }
        public override int OldStrReq
        {
            get
            {
                return 25;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 16;
            }
        }
        public override ArmorMaterialType MaterialType
        {
            get
            {
                return ArmorMaterialType.Studded;
            }
        }
        public override CraftResource DefaultResource
        {
            get
            {
                return CraftResource.RegularLeather;
            }
        }
        public override ArmorMeditationAllowance DefMedAllowance
        {
            get
            {
                return ArmorMeditationAllowance.Half;
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (this.Weight == 2.0)
                this.Weight = 1.0;
        }
    }
}