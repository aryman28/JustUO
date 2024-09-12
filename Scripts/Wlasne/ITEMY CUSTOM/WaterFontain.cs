using System;
using System.Collections;
using Server.Multis;

namespace Server.Items
{
	public class WaterFountain : BaseAddon, IWaterSource
	{
		public override BaseAddonDeed Deed{ get{ return new WaterFountainDeed(); } }

		[Constructable]
		public WaterFountain()
		{
                int itemID = 0x1731;

		AddComponent( new AddonComponent( itemID++ ), -2, +1, 0 );
		AddComponent( new AddonComponent( itemID++ ), -1, +1, 0 );
		AddComponent( new AddonComponent( itemID++ ), +0, +1, 0 );
		AddComponent( new AddonComponent( itemID++ ), +1, +1, 0 );

		AddComponent( new AddonComponent( itemID++ ), +1, +0, 0 );
		AddComponent( new AddonComponent( itemID++ ), +1, -1, 0 );
		AddComponent( new AddonComponent( itemID++ ), +1, -2, 0 );

		AddComponent( new AddonComponent( itemID++ ), +0, -2, 0 );
		AddComponent( new AddonComponent( itemID++ ), +0, -1, 0 );
		AddComponent( new AddonComponent( itemID++ ), +0, +0, 0 );

		AddComponent( new AddonComponent( itemID++ ), -1, +0, 0 );
		AddComponent( new AddonComponent( itemID++ ), -2, +0, 0 );

		AddComponent( new AddonComponent( itemID++ ), -2, -1, 0 );
		AddComponent( new AddonComponent( itemID++ ), -1, -1, 0 );

		AddComponent( new AddonComponent( itemID++ ), -1, -2, 0 );
		AddComponent( new AddonComponent( ++itemID ), -2, -2, 0 );	
		Name = "fontanna";
		}

		public WaterFountain( Serial serial ) : base( serial )
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

		public int Quantity
		{
			get{ return 50; }
			set{}
		}
	}

	public class WaterFountainDeed : BaseAddonDeed
	{
		public override BaseAddon Addon{ get{ return new WaterFountain(); } }
		public override int LabelNumber{ get{ return 1025453; } } 

		[Constructable]
		public WaterFountainDeed()
		{
		}

		public WaterFountainDeed( Serial serial ) : base( serial )
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