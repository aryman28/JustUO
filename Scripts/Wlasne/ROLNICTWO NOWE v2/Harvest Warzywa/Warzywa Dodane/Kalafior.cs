using System;
using Server.Network;

namespace Server.Items
{
	public class Kalafior : Food
	{
		[Constructable]
		public Kalafior() : this( 1 )
		{
		}
		
		[Constructable]
		public Kalafior( int amount ) : base( 0xD06 )
		{
                        Hue = 0x000;
                        this.FillFactor = 1;
			Name = "kalafior";
			Weight = 1.0;
			Stackable = true;
		}

		public Kalafior( Serial serial ) : base( serial )
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