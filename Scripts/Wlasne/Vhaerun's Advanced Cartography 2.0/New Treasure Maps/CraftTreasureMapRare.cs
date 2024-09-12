using System;

namespace Server.Items
{
	public class CraftTreasureMapRare : TreasureMap
	{

		[Constructable]
		public CraftTreasureMapRare(): this( 2, Map.Ilshenar )
		{
		}

		[Constructable]
		public CraftTreasureMapRare( int Level, Map map) : base( Level, map )
		{
			Name = "Stara mapa skarbu";
			ItemID = 0x14EB;
			Hue = 0x6BC;
		}

		public CraftTreasureMapRare( Serial serial ) : base( serial )
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