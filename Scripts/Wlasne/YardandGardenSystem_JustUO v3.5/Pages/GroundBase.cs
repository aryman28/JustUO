using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using System.Collections;
using Server.Misc;
using System.Text;
using Server.Targeting;
using Server.Multis;
using System.Collections.Generic;

namespace Server.ACC.YS
{
	public class GroundBase : Gump
	{
		int price = 0;
		int itemid;
		string TGold = "";
		YardWand m_Wand;

		public GroundBase(YardWand wand, Mobile owner, int id, int p)
			: base( wand.xstart, wand.ystart )
		{
			m_Wand = wand;

			itemid = id;
			computeGold( owner );
			price = p;
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;

//Page 0
			this.AddPage(0);

			this.AddBackground(59, 55, 300, 300, 3600);					//MainGround
			this.AddBackground(34, 0, 350, 50, 3600);						//TitleGround
			this.AddBackground(385, 209, 150, 200, 3600);				//PicGround

			this.AddBackground(59, 358, 300, 50, 3600);					//PriceGround
			this.AddBackground(372, 93, 165, 50, 3600);					//PlaceGround
			this.AddBackground(372, 143, 165, 50, 3600);					//GoldGround

			this.AddButton(472, 102, 2642, 2643, (int)Buttons.Place, GumpButtonType.Reply, 0);
			this.AddLabel(398, 109, 197, "DODAJ");
			this.AddItem(455, 98, 6022);										//LPGrass
			this.AddItem(489, 98, 6024);										//RPGrass

			this.AddLabel(136, 16, 68, @"SYSTEM OGROD�W");
			this.AddLabel(116, 375, 37, @"Cena : ");
			this.AddLabel(166, 375, 37, price + " Monet");
			this.AddLabel(387, 160, 48, "Monet : " + TGold);

			this.AddItem(337, 110, 6019);										//TGrass
			this.AddItem(337, 155, 6019);										//BGrass
			this.AddItem(510, 183, 6024);										//FGrass
			this.AddItem(328, 190, 3317);										//TLog1
			this.AddItem(348, 195, 3318);										//TLog2
			this.AddItem(371, 221, 3319);										//TLog3
			this.AddItem(339, 354, 3316);										//BLog1
			this.AddItem(362, 338, 3315);										//BLog2

			this.AddItem(0, 8, 3497);											//LTree
			this.AddItem(330, 8, 3497);										//RTree
			this.AddItem(334, 266, 3312);										//RBVine
			this.AddItem(334, 192, 3312);										//RMVine
			this.AddItem(334, 118, 3312);										//RTVine
			this.AddItem(39, 266, 3308);										//LBVine
			this.AddItem(39, 192, 3308);										//LMVine
			this.AddItem(39, 118, 3308);										//LTVine

			this.AddItem(35, 325, 3310);										//LPVine
			this.AddItem(307, 325, 3314);										//RPVine
			this.AddButton( 490, 365, 22124, 22125, (int)Buttons.Settings, GumpButtonType.Reply, 0 );

//Page 1
			this.AddPage(1);
			this.AddButton(136, 74, 1210, 2084, 	(int) Buttons.Normal, GumpButtonType.Reply, 0);
			this.AddButton(181, 76, 2362, 2084, 	(int) Buttons.Water, GumpButtonType.Reply, 0);
			this.AddButton(226, 76, 2360, 2084, 	(int) Buttons.Lava, GumpButtonType.Reply, 0);
			this.AddButton(271, 76, 2361, 2084, 	(int) Buttons.Swamp, GumpButtonType.Reply, 0);
			this.AddHtml(86, 96, 246, 243, "<basefont color=#99AA22>To jest zak�adka grunt�w.  Ze wzgl�du na ogrom przedmiot�w w tej zak�adce, zosta�y one podzielone na 4 kategorie (przyciski powy�ej).  Kolory : Srebrny = Normalne, Niebieski = Woda, Czerwony = Lawa, Zielony = Bagno.  Aby powr�cic do tej zak�adki wci�nij przycisk ponownie.</basefont>", false, false);
			this.AddButton(30, 115, 10830, 2361, 	(int)Buttons.Plants, GumpButtonType.Reply, 0);
			this.AddButton(30, 155, 10850, 2360, 	(int)Buttons.Trees, GumpButtonType.Reply, 0);
			this.AddButton(30, 195, 2362, 10810, 	(int)Buttons.Ground, GumpButtonType.Reply, 0);

			if(id != 0)
			this.AddItem(410, 235, id);										//Choice Pic
//End Page 1

		}

		public enum Buttons
		{
			Exit,
			Settings,
			Place,
			Normal,
			Water,
			Lava,
			Swamp,
			Plants,
			Trees,
			Ground,

		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;
			switch( info.ButtonID )
			{
				case (int)Buttons.Place:		{if(itemid != 0)Place( from, itemid, price);break;}
				case (int)Buttons.Settings:		{from.SendGump(new YGSettingsGump(m_Wand,from));break;}

				case (int)Buttons.Plants:		{from.SendGump(new Plants1(m_Wand,from,itemid,price));break;}
				case (int)Buttons.Trees:		{from.SendGump(new Trees1(m_Wand,from,itemid,price));break;}
				case (int)Buttons.Ground:		{from.SendGump(new YardGump(from,m_Wand));break;}

				case (int)Buttons.Normal:		{from.SendGump(new Ground1(m_Wand,from,itemid,price));break;}
				case (int)Buttons.Water:		{from.SendGump(new Water1(m_Wand,from,itemid,price));break;}
				case (int)Buttons.Lava:			{from.SendGump(new Lava1(m_Wand,from,itemid,price));break;}
				case (int)Buttons.Swamp:		{from.SendGump(new Swamp1(m_Wand,from,itemid,price));break;}
			}
		}

		public void computeGold( Mobile from )
		{
			int Gold = 0;
			foreach( Item item in from.Backpack.Items )
			{
				if( item is Gold )
				{
					Gold += item.Amount;
				}
				else if( item is Container )
                {
                    List<Item> list = item.Items;
					for( int i=0; i<list.Count; i++ )
					{
						Item inbag = (Item)list[i];
						if( inbag is Gold )
						{
							Gold += inbag.Amount;
						}
					}
				}
			}
			TGold = makeReal(Gold);
		}

		public string makeReal( int amount )
		{
			if( amount < 1000 )
			{
				return amount.ToString();
			}
			else if( amount >= 1000 && amount < 1000000 )
			{
				string samount = amount.ToString();

				StringBuilder sb = new StringBuilder(samount);
				sb.Insert(samount.Length - 3, "," );
				samount = sb.ToString();
				return samount;
			}
			else if( amount >= 1000000 && amount < 1000000000 )
			{
				string samount = amount.ToString();

				StringBuilder sb = new StringBuilder(samount);
				sb.Insert(samount.Length - 6, "," );
				samount = sb.ToString();
				StringBuilder sb2 = new StringBuilder(samount);
				sb2.Insert(samount.Length - 3, "," );
				samount = sb2.ToString();

				return samount;
			}
			return "Null";
		}

		public void Place( Mobile from, int itemid, int p )
		{
			from.SendMessage( "Gdzie chcesz doda� przedmiot" );
			from.Target = new YardTarget( m_Wand, from, itemid, p, 4 );
		}
	}
}
