using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class BagOfDruidReagents : Bag
	{

		[Constructable]
		public BagOfDruidReagents() : this( 1 ) 
		{ 
			Movable = true; 
			Hue = 0x48C; 
			Name = "bag of druid reagents";
		}

		[Constructable]
		public BagOfDruidReagents( int amount )
		{

			DropItem( new DestroyingAngel  ( 100 ) );
			DropItem( new PetrifiedWood ( 100 ) );
			DropItem( new Pumice ( 100 ) );
			DropItem( new SpringWater ( 100 ) );
            DropItem(new FertileEarth(100));
            DropItem(new FenMoss(100));
		}

		public BagOfDruidReagents( Serial serial ) : base( serial )
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
 
    
