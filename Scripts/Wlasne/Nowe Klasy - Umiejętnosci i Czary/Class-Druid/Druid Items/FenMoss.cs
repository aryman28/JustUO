using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class FenMoss : BaseReagent, ICommodity
	{
        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return (Core.ML); } }

		[Constructable]
		public FenMoss() : this( 1 )
		{
		}

		[Constructable]
		public FenMoss( int amount ) : base( 0x26B7, amount )
		{
			Name = "fen moss";
			Hue = 460;
		}

		public FenMoss( Serial serial ) : base( serial )
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