using System;
using Server;

namespace Server.Items
{
	public class szatadruida : BaseOuterTorso
	{
		public override string DefaultName 
		{ 
			get { return "Szata Druida"; } 
		}
		
		[Constructable]
		public szatadruida() : base( 0x2683 )
		{
      Hue = 62;
      LootType = LootType.Blessed;
      Weight = 3.0;
		}

		public szatadruida( Serial serial ) : base( serial )
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