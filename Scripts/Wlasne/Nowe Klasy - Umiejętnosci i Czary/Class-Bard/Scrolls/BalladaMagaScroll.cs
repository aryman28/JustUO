using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class MagesBalladScroll : SpellScroll
	{
		[Constructable]
		public MagesBalladScroll() : this( 1 )
		{
                  Name = "Mages Ballad sheet music";
		}

		[Constructable]
		public MagesBalladScroll( int amount ) : base( 361, 0x14ED, amount )
		{
                  Name = "Mages Ballad sheet music";
		          Hue = 0x96;
        }

		public MagesBalladScroll( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			from.SendMessage( "The sheet music must be in your music book." );
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

		//public override Item Dupe( int amount )
		//{
			//return base.Dupe( new MagesBalladScroll( amount ), amount );
		//}
	}
}