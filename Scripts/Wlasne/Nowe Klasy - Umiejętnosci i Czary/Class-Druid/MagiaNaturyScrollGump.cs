using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Spells.Druid;
using Server.Prompts;

namespace Server.Gumps
{
	public class MagiaNaturyScrollGump : Gump
	{
		private MagiaNaturyScroll m_Scroll;

		public MagiaNaturyScrollGump( Mobile from, MagiaNaturyScroll scroll ) : base( 0, 0 )
		{
			m_Scroll = scroll;

			int mD01BarkSkinSpell = m_Scroll.mD01BarkSkinSpell;
			int mD02CircleOfThornsSpell = m_Scroll.mD02CircleOfThornsSpell;
			int mD03EnchantedGroveSpell = m_Scroll.mD03EnchantedGroveSpell;
			int mD04ForestKinSpell = m_Scroll.mD04ForestKinSpell;
			int mD05GraspingRootsSpell = m_Scroll.mD05GraspingRootsSpell;
			int mD06HibernateSpell = m_Scroll.mD06HibernateSpell;
			int mD07HollowReedSpell = m_Scroll.mD07HollowReedSpell;
			int mD08HurricaneSpell = m_Scroll.mD08HurricaneSpell;
			int mD09LureStoneSpell = m_Scroll.mD09LureStoneSpell;
			int mD10ManaSpringSpell = m_Scroll.mD10ManaSpringSpell;
			int mD11MushroomGatewaySpell = m_Scroll.mD11MushroomGatewaySpell;
			int mD12RestorativeSoilSpell = m_Scroll.mD12RestorativeSoilSpell;
			int mD13ShieldOfEarthSpell = m_Scroll.mD13ShieldOfEarthSpell;
			int mD14SpringOfLifeSpell = m_Scroll.mD14SpringOfLifeSpell;



			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(32, 14, 250, 518, 9380);

			this.AddImage(60, 45, 23008);
			this.AddLabel(60, 90, 76, @"DÍbowa skÛra");
			this.AddImage(60, 110, 21045);
			this.AddLabel(60, 155, 76, @"Ciernisty okrπg");
			this.AddImage(60, 175, 21020);
			this.AddLabel(60, 220, 76, @"ZaklÍty gaj");
			this.AddImage(60, 240, 21047);
			this.AddLabel(60, 285, 76, @"Duchy natury");
			this.AddImage(60, 305, 21042);
			this.AddLabel(60, 350, 76, @"Wiπrzπce korzenie");
			this.AddImage(60, 370, 24004);
			this.AddLabel(60, 415, 76, @"Uúpienie");
			this.AddImage(60, 435, 21041);
			this.AddLabel(60, 480, 76, @"Wzmocnienie");
			
			this.AddImage(170, 45, 24014);
			this.AddLabel(170, 90,  76, @"Huragan");
			this.AddImage(170, 110, 24011);
			this.AddLabel(170, 155, 76, @"Kuszπcy kamie≈Ñ");
            this.AddImage(170, 175, 2270);
			this.AddLabel(170, 220, 76, @"Wzmocnienie");
			this.AddImage(170, 240, 21044);
			this.AddLabel(170, 285, 76, @"èrÛd≥o many");
			this.AddImage(170, 305, 23014);
			this.AddLabel(170, 350, 76, @"Grzybowy portal");
			this.AddImage(170, 370, 21043);
			this.AddLabel(170, 415, 76, @"Krzaczasty mur");
            this.AddImage(170, 435, 21028);
			this.AddLabel(170, 480, 76, @"èrÛd≥o øycia");

			if ( mD01BarkSkinSpell == 1 ) { this.AddButton( 110, 55,  4023, 4023, 1, GumpButtonType.Reply, 1); }
			if ( mD02CircleOfThornsSpell == 1 ) { this.AddButton( 110, 120,  4023, 4023, 2, GumpButtonType.Reply, 1); }
			if ( mD03EnchantedGroveSpell == 1 ) { this.AddButton( 110, 185,  4023, 4023, 3, GumpButtonType.Reply, 1); }
			if ( mD04ForestKinSpell  == 1 ) { this.AddButton( 110, 250,  4023, 4023, 4, GumpButtonType.Reply, 1); }
			if ( mD05GraspingRootsSpell == 1 ) { this.AddButton( 110, 315,  4023, 4023, 5, GumpButtonType.Reply, 1); }
			if ( mD06HibernateSpell == 1 ) { this.AddButton( 110, 380,  4023, 4023, 6, GumpButtonType.Reply, 1); }
			if ( mD07HollowReedSpell == 1 ) { this.AddButton( 110, 445,  4023, 4023, 7, GumpButtonType.Reply, 1); }
			if ( mD08HurricaneSpell == 1 ) { this.AddButton( 220, 55,  4023, 4023, 8, GumpButtonType.Reply, 1); }
			if ( mD09LureStoneSpell  == 1 ) { this.AddButton( 220, 120,  4023, 4023, 9, GumpButtonType.Reply, 1); }
			if ( mD10ManaSpringSpell == 1 ) { this.AddButton( 220, 185,  4023, 4023, 10, GumpButtonType.Reply, 1); }
			if ( mD11MushroomGatewaySpell == 1 ) { this.AddButton( 220, 250,  4023, 4023, 11, GumpButtonType.Reply, 1); }
			if ( mD12RestorativeSoilSpell == 1 ) { this.AddButton( 220, 315,  4023, 4023, 12, GumpButtonType.Reply, 1); }
			if ( mD13ShieldOfEarthSpell == 1 ) { this.AddButton( 220, 380,  4023, 4023, 13, GumpButtonType.Reply, 1); }
			if ( mD14SpringOfLifeSpell == 1 ) { this.AddButton( 220, 445,  4023, 4023, 14, GumpButtonType.Reply, 1); }
			

			if ( mD01BarkSkinSpell == 0 ) { this.AddButton( 110, 55,  4020, 4020, 1, GumpButtonType.Reply, 1); }
			if ( mD02CircleOfThornsSpell == 0 ) { this.AddButton( 110, 120,  4020, 4020, 2, GumpButtonType.Reply, 1); }
			if ( mD03EnchantedGroveSpell == 0 ) { this.AddButton( 110, 185,  4020, 4020, 3, GumpButtonType.Reply, 1); }
			if ( mD04ForestKinSpell  == 0 ) { this.AddButton( 110, 250, 4020, 4020, 4, GumpButtonType.Reply, 1); }
			if ( mD05GraspingRootsSpell == 0 ) { this.AddButton( 110, 315,  4020, 4020, 5, GumpButtonType.Reply, 1); }
			if ( mD06HibernateSpell == 0 ) { this.AddButton( 110, 380,  4020, 4020, 6, GumpButtonType.Reply, 1); }
			if ( mD07HollowReedSpell == 0 ) { this.AddButton( 110, 445,  4020, 4020, 7, GumpButtonType.Reply, 1); }
			if ( mD08HurricaneSpell == 0 ) { this.AddButton( 220, 55,  4020, 4020, 8, GumpButtonType.Reply, 1); }
			if ( mD09LureStoneSpell  == 0 ) { this.AddButton( 220, 120,  4020, 4020, 9, GumpButtonType.Reply, 1); }
			if ( mD10ManaSpringSpell == 0 ) { this.AddButton( 220, 185,  4020, 4020, 10, GumpButtonType.Reply, 1); }
			if ( mD11MushroomGatewaySpell == 0 ) { this.AddButton( 220, 250,  4020, 4020, 11, GumpButtonType.Reply, 1); }
			if ( mD12RestorativeSoilSpell == 0 ) { this.AddButton( 220, 315,  4020, 4020, 12, GumpButtonType.Reply, 1); }
			if ( mD13ShieldOfEarthSpell == 0 ) { this.AddButton( 220, 380,  4020, 4020, 13, GumpButtonType.Reply, 1); }
			if ( mD14SpringOfLifeSpell == 0 ) { this.AddButton( 220, 445,  4020, 4020, 14, GumpButtonType.Reply, 1); }

			this.AddButton(220, 510, 4023, 4023, 15, GumpButtonType.Reply, 1); // TOOLBAR
			this.AddLabel(60, 510, 38, @"OtwÛrz pasek zaklÍÊ");
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
			case 1 : { if ( m_Scroll.mD01BarkSkinSpell == 0 ) { m_Scroll.mD01BarkSkinSpell = 1; } else { m_Scroll.mD01BarkSkinSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 2 : { if ( m_Scroll.mD02CircleOfThornsSpell == 0 ) { m_Scroll.mD02CircleOfThornsSpell = 1; } else { m_Scroll.mD02CircleOfThornsSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 3 : { if ( m_Scroll.mD03EnchantedGroveSpell == 0 ) { m_Scroll.mD03EnchantedGroveSpell = 1; } else { m_Scroll.mD03EnchantedGroveSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 4 : { if ( m_Scroll.mD04ForestKinSpell== 0 ) { m_Scroll.mD04ForestKinSpell= 1; } else { m_Scroll.mD04ForestKinSpell= 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 5 : { if ( m_Scroll.mD05GraspingRootsSpell == 0 ) { m_Scroll.mD05GraspingRootsSpell = 1; } else { m_Scroll.mD05GraspingRootsSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 6 : { if ( m_Scroll.mD06HibernateSpell  == 0 ) { m_Scroll.mD06HibernateSpell  = 1; } else { m_Scroll.mD06HibernateSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 7 : { if ( m_Scroll.mD07HollowReedSpell == 0 ) { m_Scroll.mD07HollowReedSpell = 1; } else { m_Scroll.mD07HollowReedSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 8 : { if ( m_Scroll.mD08HurricaneSpell == 0 ) { m_Scroll.mD08HurricaneSpell = 1; } else { m_Scroll.mD08HurricaneSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 9 : { if ( m_Scroll.mD09LureStoneSpell  == 0 ) { m_Scroll.mD09LureStoneSpell  = 1; } else { m_Scroll.mD09LureStoneSpell  = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 10 : { if ( m_Scroll.mD10ManaSpringSpell == 0 ) { m_Scroll.mD10ManaSpringSpell = 1; } else { m_Scroll.mD10ManaSpringSpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 11 : { if ( m_Scroll.mD11MushroomGatewaySpell == 0 ) { m_Scroll.mD11MushroomGatewaySpell = 1; } else { m_Scroll.mD11MushroomGatewaySpell = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 12 : { if ( m_Scroll.mD12RestorativeSoilSpell == 0 ) { m_Scroll.mD12RestorativeSoilSpell = 1; } else { m_Scroll.mD12RestorativeSoilSpell= 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 13 : { if ( m_Scroll.mD13ShieldOfEarthSpell  == 0 ) { m_Scroll.mD13ShieldOfEarthSpell  = 1; } else { m_Scroll.mD13ShieldOfEarthSpell  = 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 14 : { if ( m_Scroll.mD14SpringOfLifeSpell == 0 ) { m_Scroll.mD14SpringOfLifeSpell = 1; } else { m_Scroll.mD14SpringOfLifeSpell= 0; } from.SendGump( new MagiaNaturyScrollGump( from, m_Scroll ) ); break; }
			case 15:
			{
				from.CloseGump( typeof( MagiaNatury ) );
				from.SendGump( new MagiaNatury( from, m_Scroll ) );
				break;
			}
		}
	}}

	public class MagiaNatury : Gump
	{
		public static bool HasSpell( Mobile from, int spellID )
		{
			Spellbook book = Spellbook.Find( from, spellID );
			return ( book != null && book.HasSpell( spellID ) );
		}

		private MagiaNaturyScroll m_Scroll;

		public MagiaNatury( Mobile from, MagiaNaturyScroll scroll ) : base( 0, 0 )
		{
			m_Scroll = scroll;
			this.Closable=false;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddImage(0, 0, 11053, 2210);
			//this.AddPage(0);
			//this.AddBackground(3, 3, 424, 199, 9380);
			//this.AddImage(32, 43, 11053);
			//this.AddLabel(146, 60, 76, @"ZaklÍcia Magii Natury");
			int dby = 50;

			if ( HasSpell( from, 312 ) && m_Scroll.mD01BarkSkinSpell == 1){this.AddButton(dby, 5, 23008, 23008, 1, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 305 ) && m_Scroll.mD02CircleOfThornsSpell == 1){this.AddButton(dby, 5, 21045, 21045, 2, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 306 ) && m_Scroll.mD03EnchantedGroveSpell == 1){this.AddButton(dby, 5, 21020, 21020, 3, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 311 ) && m_Scroll.mD04ForestKinSpell== 1){this.AddButton(dby, 5, 21047, 21047, 4, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 304 ) && m_Scroll.mD05GraspingRootsSpell == 1){this.AddButton(dby, 5, 21042, 21042, 5, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 314 ) && m_Scroll.mD06HibernateSpell == 1){this.AddButton(dby, 5, 24004, 24004, 6, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 302 ) && m_Scroll.mD07HollowReedSpell == 1){this.AddButton(dby, 5, 21041, 21041, 7, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 308 ) && m_Scroll.mD08HurricaneSpell == 1){this.AddButton(dby , 5, 24014, 24014, 8, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 307 ) && m_Scroll.mD09LureStoneSpell  == 1){this.AddButton(dby, 5, 24011, 24011, 9, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 313 ) && m_Scroll.mD10ManaSpringSpell == 1){this.AddButton(dby, 5, 2270, 2270, 10, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 309 ) && m_Scroll.mD11MushroomGatewaySpell == 1){this.AddButton(dby, 5, 21044, 21004, 11, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 310 ) && m_Scroll.mD12RestorativeSoilSpell == 1){this.AddButton(dby, 5, 23014, 23014, 12, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 301 ) && m_Scroll.mD13ShieldOfEarthSpell  == 1){this.AddButton(dby, 5, 21043, 21043, 13, GumpButtonType.Reply, 1); dby = dby + 45;}
			if ( HasSpell( from, 303 ) && m_Scroll.mD14SpringOfLifeSpell == 1){this.AddButton(dby, 5, 21028, 21028, 14, GumpButtonType.Reply, 1); dby = dby + 45;}
		}
		
		public override void OnResponse( NetState state, RelayInfo info ) 
		{ 
			Mobile from = state.Mobile; 
			switch ( info.ButtonID ) 
			{
				case 0: { break; }
				case 1 : { if ( HasSpell( from, 312 ) ) { new BarkSkinSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 2 : { if ( HasSpell( from, 305 ) ) { new CircleOfThornsSpell ( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 3 : { if ( HasSpell( from, 306 ) ) { new EnchantedGroveSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 4 : { if ( HasSpell( from, 311 ) ) { new ForestKinSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 5 : { if ( HasSpell( from, 304 ) ) { new GraspingRootsSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 6 : { if ( HasSpell( from, 314 ) ) { new HibernateSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 7 : { if ( HasSpell( from, 302 ) ) { new HollowReedSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 8 : { if ( HasSpell( from, 308 ) ) { new HurricaneSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 9 : { if ( HasSpell( from, 307 ) ) { new LureStoneSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 10 : { if ( HasSpell( from, 313 ) ) { new ManaSpringSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 11 : { if ( HasSpell( from, 309 ) ) { new MushroomGatewaySpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 12 : { if ( HasSpell( from, 310 ) ) { new RestorativeSoilSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 13 : { if ( HasSpell( from, 301 ) ) { new ShieldOfEarthSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
				case 14 : { if ( HasSpell( from, 303 ) ) { new SpringOfLifeSpell( from, null ).Cast(); from.SendGump( new MagiaNatury( from, m_Scroll ) ); } break; }
			}
		}
	}
}