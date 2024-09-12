using System;

namespace Server.Items
{
    [FlipableAttribute(0x27A8, 0x27F3)]
    public class MieczDrewniany : BaseSword
    {
        [Constructable]
        public MieczDrewniany()
            : base(0x27A8)
        {
            this.Name = "Drewniany Miecz";
            this.Weight = 7.0;
        }

        public override void AddNameProperty( ObjectPropertyList list )
		{ 
		    base.AddNameProperty( list );
		    list.Add( "<BASEFONT COLOR=#FACC2E>(Specjalne umiejêtnoœci wy³¹czone)<BASEFONT COLOR=#FFFFFF>" );	
        }

        public MieczDrewniany(Serial serial)
            : base(serial)
        {
        }

        /*public override WeaponAbility PrimaryAbility
        {
            get
            {
                return WeaponAbility.Feint;
            }
        }
        public override WeaponAbility SecondaryAbility
        {
            get
            {
                return WeaponAbility.NerveStrike;
            }
        }*/
        public override int AosStrengthReq
        {
            get
            {
                return 20;
            }
        }
        public override int AosMinDamage
        {
            get
            {
                return 1;
            }
        }
        public override int AosMaxDamage
        {
            get
            {
                return 1;
            }
        }
        public override int AosSpeed
        {
            get
            {
                return 53;
            }
        }
        public override float MlSpeed
        {
            get
            {
                return 2.00f;
            }
        }
        public override int OldStrengthReq
        {
            get
            {
                return 20;
            }
        }
        public override int OldMinDamage
        {
            get
            {
                return 1;
            }
        }
        public override int OldMaxDamage
        {
            get
            {
                return 1;
            }
        }
        public override int OldSpeed
        {
            get
            {
                return 53;
            }
        }
        public override int DefHitSound
        {
            get
            {
                return 0x536;
            }
        }
        public override int DefMissSound
        {
            get
            {
                return 0x23A;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 25;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 50;
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