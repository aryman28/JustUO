using System;
using Server;

namespace Server.Items
{
	public class szatazakonnika : BaseOuterTorso
	{
		public override string DefaultName 
		{ 
			get { return "Szata Zakonnika"; } 
		}
		
		[Constructable]
		public szatazakonnika() : base( 0x2683 )
		{
      Hue = 1150;
      LootType = LootType.Blessed;
      Weight = 3.0;
		}

		public szatazakonnika( Serial serial ) : base( serial )
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