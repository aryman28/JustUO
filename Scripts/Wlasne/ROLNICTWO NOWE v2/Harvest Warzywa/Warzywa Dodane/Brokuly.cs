using System;
using Server.Network;

namespace Server.Items
{
	public class Brokuly : Food
	{
		[Constructable]
		public Brokuly() : this( 1 )
		{
		}

		[Constructable]
		public Brokuly( int amount ) : base( amount, 0xC78 )
		{
			Weight = 0.3;
			FillFactor = 1;
			Hue = 0x48F;
			Name = "Brokuly";
		}

		public Brokuly( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}