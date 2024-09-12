using System;
using Server;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{
	public class  SpiewnikBardaScroll: Item
	{
		public  int mB01BalladaMaga = 0;
		public 	int mB02BohaterskaEtiuda = 0;
		public 	int mB03MagicznyFinal = 0;
		public 	int mB04OgnistaZemsta = 0;
		public 	int mB05PiesnZywiolow = 0;
		public 	int mB06PonuryZywiol = 0;
		public 	int mB07RzekaZycia = 0;
		public 	int mB08TarczaOdwagi = 0;
		
		[CommandProperty(AccessLevel.GameMaster)]
		public int B01BalladaMaga { get { return mB01BalladaMaga; } set { mB01BalladaMaga = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B02BohaterskaEtiuda { get { return mB02BohaterskaEtiuda; } set { mB02BohaterskaEtiuda = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B03MagicznyFinal { get { return mB03MagicznyFinal; } set { mB03MagicznyFinal = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B04OgnistaZemsta { get { return mB04OgnistaZemsta; } set { mB04OgnistaZemsta = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B05PiesnZywiolow { get { return mB05PiesnZywiolow; } set { mB05PiesnZywiolow = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B06PonuryZywiol{ get { return mB06PonuryZywiol; } set { mB06PonuryZywiol= value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B07RzekaZycia { get { return mB07RzekaZycia; } set { mB07RzekaZycia = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int B08TarczaOdwagi { get { return mB08TarczaOdwagi; } set { mB08TarczaOdwagi = value; } }
		

		[Constructable]
		public SpiewnikBardaScroll() : base( 0x14F0 )
		{
			LootType = LootType.Blessed;
			Hue = 0x2F;
			Name = "Pieœni Barda";
		}

		public SpiewnikBardaScroll( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
			}
			else
			{
				from.CloseGump( typeof( SpiewnikBardaScrollGump ) );
				from.SendGump( new SpiewnikBardaScrollGump( from, this ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write(mB01BalladaMaga);
			writer.Write(mB02BohaterskaEtiuda);
			writer.Write(mB03MagicznyFinal);
			writer.Write(mB04OgnistaZemsta);
			writer.Write(mB05PiesnZywiolow);
			writer.Write(mB06PonuryZywiol);
			writer.Write(mB07RzekaZycia);
			writer.Write(mB08TarczaOdwagi);
			
            
            
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			mB01BalladaMaga = reader.ReadInt();
			mB02BohaterskaEtiuda = reader.ReadInt();
			mB03MagicznyFinal = reader.ReadInt();
			mB04OgnistaZemsta = reader.ReadInt();
			mB05PiesnZywiolow = reader.ReadInt();
			mB06PonuryZywiol= reader.ReadInt();
			mB07RzekaZycia = reader.ReadInt();
			mB08TarczaOdwagi = reader.ReadInt();
			

		}
	}
}