using System;
using Server.Network;

namespace Server.Items
{
	public class Ziemniak : Food
	{
		[Constructable]
		public Ziemniak() : this( 1 )
		{
		}

		[Constructable]
		public Ziemniak( int amount ) : base( amount, 0xC5D )
		{
			FillFactor = 1;
			Hue = 0x6C0;
			Name = "Ziemniak";
                        Weight = 1.0;
		}

		public Ziemniak( Serial serial ) : base( serial )
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