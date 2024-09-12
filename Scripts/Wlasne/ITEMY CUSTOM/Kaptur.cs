using System;
using System.Collections.Generic;
using Server.Engines.Craft;
using Server.Network;
using Server.Mobiles;

namespace Server.Items
{

    public class Kaptur : BaseHat
    {

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#0070FF>Demony nie mog¹ nosiæ<BASEFONT COLOR=#FFFFFF>" );
                }

        public override bool CanBeWornByGargoyles
        {
            get
            {
                return false;
            }
        }
        public override int BasePhysicalResistance
        {
            get
            {
                return 5;
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
                return 5;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 5;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 5;
            }
        }

        public override int InitMinHits
        {
            get
            {
                return 20;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 30;
            }
        }

        [Constructable]
        public Kaptur()
            : this(0)
        {
        }

        [Constructable]
        public Kaptur(int hue)
            : base(0x1519, hue)
        {
            this.Weight = 1.0;
            this.Name = "Kaptur";
        }

        public Kaptur(Serial serial)
            : base(serial)
        {
        }

        public override bool OnEquip( Mobile m )
        { 

            Mobile from = m as PlayerMobile;
            PlayerMobile pm = (PlayerMobile)from;

             if ( pm.DisplayRaceTitle == true )
             {
                pm.DisplayRaceTitle = false;                 
             }                                    
             return base.OnEquip( pm );
        }
            
        public override void OnRemoved( object parent )
        {

	//PlayerMobile from = (PlayerMobile)parent;
          PlayerMobile from = parent as PlayerMobile;

	     if (from == null)
             {
	     return;
             }
             else if ( from.DisplayRaceTitle == false )
             {
                from.DisplayRaceTitle = true;                 
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