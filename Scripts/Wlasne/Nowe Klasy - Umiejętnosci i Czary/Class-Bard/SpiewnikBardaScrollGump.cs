using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Spells.Song;
using Server.Prompts;
using Server.Commands;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Commands
{ 
	public class Spiewnik
	{ 
		public static void Initialize () 
		{
			//EventSink.Login += new LoginEventHandler(EventSink_Login); 
			CommandSystem.Register( "Spiewnik", AccessLevel.Player, new CommandEventHandler( Spiewnik_OnCommand ) );
		}

		[Usage( "Spiewnik" )]
		[Description( "Used to look at someone who is near you or to change your's character description." )]
		public static void Spiewnik_OnCommand( CommandEventArgs e )
		{
			Mobile from = e.Mobile;				
			from.CloseGump( typeof( SpiewnikBardaScrollGump ) );
			from.SendGump( new SpiewnikBardaScrollGump( from, new SpiewnikBardaScroll() ) );
			
		}
	}
}

namespace Server.Gumps
{
	public class SpiewnikBardaScrollGump : Gump
	{
		private SpiewnikBardaScroll m_Scroll;

		public SpiewnikBardaScrollGump( Mobile from, SpiewnikBardaScroll scroll ) : base( 0, 0 )
		{
			m_Scroll = scroll;

			int mB01BalladaMaga = m_Scroll.mB01BalladaMaga;
			int mB02BohaterskaEtiuda = m_Scroll.mB02BohaterskaEtiuda;
			int mB03MagicznyFinal = m_Scroll.mB03MagicznyFinal;
			int mB04OgnistaZemsta = m_Scroll.mB04OgnistaZemsta;
			int mB05PiesnZywiolow = m_Scroll.mB05PiesnZywiolow;
			int mB06PonuryZywiol = m_Scroll.mB06PonuryZywiol;
			int mB07RzekaZycia = m_Scroll.mB07RzekaZycia;
			int mB08TarczaOdwagi = m_Scroll.mB08TarczaOdwagi;
			
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(32, 14, 250, 518, 9380);

			this.AddImage(60, 45, 20528);
			this.AddLabel(60, 90, 76, @"Ballada o Magu");
			this.AddImage(60, 110, 20529);
			this.AddLabel(60, 155, 76, @"Boh. Etiuda");
			this.AddImage(60, 175, 20530);
			this.AddLabel(60, 220, 76, @"Magiczny Fina³");
			this.AddImage(60, 240, 20531);
			this.AddLabel(60, 285, 76, @"Ognista Zemsta");
			this.AddImage(60, 305, 20532);
			this.AddLabel(60, 350, 76, @"Pieœñ ¯ywio³ów");
			this.AddImage(60, 370, 20533);
			this.AddLabel(60, 415, 76, @"Ponury ¯ywio³");
			this.AddImage(60, 435, 20534);
			this.AddLabel(60, 480, 76, @"Rzeka ¯ycia");
			
			this.AddImage(170, 45, 20535);
			this.AddLabel(170, 90,  76, @"Tarcza Odwagi");
			

			if ( mB01BalladaMaga == 1 ) { this.AddButton( 110, 55,  4023, 4023, 1, GumpButtonType.Reply, 1); }
			if ( mB02BohaterskaEtiuda == 1 ) { this.AddButton( 110, 120,  4023, 4023, 2, GumpButtonType.Reply, 1); }
			if ( mB03MagicznyFinal == 1 ) { this.AddButton( 110, 185,  4023, 4023, 3, GumpButtonType.Reply, 1); }
			if ( mB04OgnistaZemsta  == 1 ) { this.AddButton( 110, 250,  4023, 4023, 4, GumpButtonType.Reply, 1); }
			if ( mB05PiesnZywiolow == 1 ) { this.AddButton( 110, 315,  4023, 4023, 5, GumpButtonType.Reply, 1); }
			if ( mB06PonuryZywiol == 1 ) { this.AddButton( 110, 380,  4023, 4023, 6, GumpButtonType.Reply, 1); }
			if ( mB07RzekaZycia == 1 ) { this.AddButton( 110, 445,  4023, 4023, 7, GumpButtonType.Reply, 1); }
			if ( mB08TarczaOdwagi == 1 ) { this.AddButton( 220, 55,  4023, 4023, 8, GumpButtonType.Reply, 1); }
						

			if ( mB01BalladaMaga == 0 ) { this.AddButton( 110, 55,  4020, 4020, 1, GumpButtonType.Reply, 1); }
			if ( mB02BohaterskaEtiuda == 0 ) { this.AddButton( 110, 120,  4020, 4020, 2, GumpButtonType.Reply, 1); }
			if ( mB03MagicznyFinal == 0 ) { this.AddButton( 110, 185,  4020, 4020, 3, GumpButtonType.Reply, 1); }
			if ( mB04OgnistaZemsta  == 0 ) { this.AddButton( 110, 250, 4020, 4020, 4, GumpButtonType.Reply, 1); }
			if ( mB05PiesnZywiolow == 0 ) { this.AddButton( 110, 315,  4020, 4020, 5, GumpButtonType.Reply, 1); }
			if ( mB06PonuryZywiol == 0 ) { this.AddButton( 110, 380,  4020, 4020, 6, GumpButtonType.Reply, 1); }
			if ( mB07RzekaZycia == 0 ) { this.AddButton( 110, 445,  4020, 4020, 7, GumpButtonType.Reply, 1); }
			if ( mB08TarczaOdwagi == 0 ) { this.AddButton( 220, 55,  4020, 4020, 8, GumpButtonType.Reply, 1); }
			

			this.AddButton(220, 510, 4023, 4023, 9, GumpButtonType.Reply, 1); // TOOLBAR
			this.AddLabel(60, 510, 38, @"Otwórz pasek pieœni");
		}

	public override void OnResponse( NetState state, RelayInfo info )
	{
		Mobile from = state.Mobile;

		switch ( info.ButtonID )
		{
			case 0:
			{
				break;
			}
			case 1 : { if ( m_Scroll.mB01BalladaMaga == 0 ) { m_Scroll.mB01BalladaMaga = 1; } else { m_Scroll.mB01BalladaMaga = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 2 : { if ( m_Scroll.mB02BohaterskaEtiuda == 0 ) { m_Scroll.mB02BohaterskaEtiuda = 1; } else { m_Scroll.mB02BohaterskaEtiuda = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 3 : { if ( m_Scroll.mB03MagicznyFinal == 0 ) { m_Scroll.mB03MagicznyFinal = 1; } else { m_Scroll.mB03MagicznyFinal = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 4 : { if ( m_Scroll.mB04OgnistaZemsta== 0 ) { m_Scroll.mB04OgnistaZemsta= 1; } else { m_Scroll.mB04OgnistaZemsta= 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 5 : { if ( m_Scroll.mB05PiesnZywiolow == 0 ) { m_Scroll.mB05PiesnZywiolow = 1; } else { m_Scroll.mB05PiesnZywiolow = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 6 : { if ( m_Scroll.mB06PonuryZywiol  == 0 ) { m_Scroll.mB06PonuryZywiol  = 1; } else { m_Scroll.mB06PonuryZywiol = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 7 : { if ( m_Scroll.mB07RzekaZycia == 0 ) { m_Scroll.mB07RzekaZycia = 1; } else { m_Scroll.mB07RzekaZycia = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			case 8 : { if ( m_Scroll.mB08TarczaOdwagi == 0 ) { m_Scroll.mB08TarczaOdwagi = 1; } else { m_Scroll.mB08TarczaOdwagi = 0; } from.SendGump( new SpiewnikBardaScrollGump( from, m_Scroll ) ); break; }
			
			case 9:
			{
				from.CloseGump( typeof( SpiewnikBarda ) );
				from.SendGump( new SpiewnikBarda( from, m_Scroll ) );
				break;
			}
		}
	}}

	public class SpiewnikBarda : Gump
	{
		public static bool HasSpell( Mobile from, int spellID )
		{
			Spellbook book = Spellbook.Find( from, spellID );
			return ( book != null && book.HasSpell( spellID ) );
		}

		private SpiewnikBardaScroll m_Scroll;

		public SpiewnikBarda( Mobile from, SpiewnikBardaScroll scroll ) : base( 0, 0 )
		{
			m_Scroll = scroll;
			this.Closable=false;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImage(0, 0, 11056, 47);
			//this.AddPage(0);
			//this.AddBackground(3, 3, 424, 199, 9380);
			//this.AddImage(32, 43, 11053);
			//this.AddLabel(146, 60, 76, @"PieÅ›ni Barda");
			int dby = 50;

			if ( HasSpell( from, 351 ) && m_Scroll.mB01BalladaMaga == 1){this.AddButton(dby, 5, 20528, 20528, 1, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 352 ) && m_Scroll.mB02BohaterskaEtiuda == 1){this.AddButton(dby, 5, 20529, 20529, 2, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 353 ) && m_Scroll.mB03MagicznyFinal == 1){this.AddButton(dby, 5, 20530, 20530, 3, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 354 ) && m_Scroll.mB04OgnistaZemsta== 1){this.AddButton(dby, 5, 20531, 20531, 4, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 355 ) && m_Scroll.mB05PiesnZywiolow == 1){this.AddButton(dby, 5, 20532, 20532, 5, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 356 ) && m_Scroll.mB06PonuryZywiol == 1){this.AddButton(dby, 5, 20533, 20533, 6, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 357 ) && m_Scroll.mB07RzekaZycia == 1){this.AddButton(dby, 5, 20534, 20534, 7, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 358 ) && m_Scroll.mB08TarczaOdwagi == 1){this.AddButton(dby , 5, 20535, 20535, 8, GumpButtonType.Reply, 1); dby = dby + 45;}
			
		}
		
		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 
			Mobile from = state.Mobile; 
			switch ( info.ButtonID ) 
			{
				case 0: { break; }
				case 1 : { if ( HasSpell( from, 351 ) ) { new BalladaMaga( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 2 : { if ( HasSpell( from, 352 ) ) { new BohaterskaEtiuda ( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 3 : { if ( HasSpell( from, 352 ) ) { new MagicznyFinal( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 4 : { if ( HasSpell( from, 354 ) ) { new OgnistaZemsta( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 5 : { if ( HasSpell( from, 355 ) ) { new PiesnZywiolow( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 6 : { if ( HasSpell( from, 356 ) ) { new PonuryZywiol( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 7 : { if ( HasSpell( from, 357 ) ) { new RzekaZycia( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				case 8 : { if ( HasSpell( from, 358 ) ) { new TarczaOdwagi( from, null ).Cast(); from.SendGump( new SpiewnikBarda( from, m_Scroll ) ); } break; }
				
			}
		}
	}
}