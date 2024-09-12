//Created with Script Creator By Marak & Rockstar
//Kevin Evans helped too xD
using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class plaszczmaga : Cloak
  {


		public override int BaseColdResistance{ get{ return 1; } } 
		public override int BaseEnergyResistance{ get{ return 1; } } 
		public override int BasePhysicalResistance{ get{ return 1; } } 
		public override int BasePoisonResistance{ get{ return 1; } } 
		public override int BaseFireResistance{ get{ return 1; } } 
      
      [Constructable]
		public plaszczmaga()
		{
			Weight = 2;
          Name = "Plaszcz Mistrza Magii";
          Hue = 1153;
      LootType = LootType.Blessed;
		}

		public plaszczmaga( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
