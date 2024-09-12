using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class FryzuraDamskaGump : Gump
	{
		public FryzuraDamskaGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);
			this.AddLabel(290, 63, 137, @"WYBÓR FRYZURY");

			this.AddButton(130, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(130, 153, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
                        this.AddButton(130, 223, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
                        this.AddButton(130, 293, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
                        this.AddButton(130, 363, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(130, 433, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
////Prawa Strona
			this.AddButton(350, 83, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			this.AddButton(350, 153, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
                        this.AddButton(350, 223, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
                        this.AddButton(350, 293, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
                        this.AddButton(350, 363, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);

                        this.AddLabel(180, 100, 567, @"W£OSY D£UGIE");   
                        this.AddImage(205, 23, 0x72D);
			this.AddLabel(180, 170, 567, @"W£OSY KRÓTRKIE");
                        this.AddImage(205, 93, 0x737);
			this.AddLabel(180, 240, 567, @"W£OSY W KUC");
                        this.AddImage(205, 163, 0x735);
			this.AddLabel(180, 310, 567, @"AFRO");
                        this.AddImage(205, 233, 0x72F);
			this.AddLabel(180, 380, 567, @"NA PAZIA");
                        this.AddImage(205, 303, 0x734);
			this.AddLabel(180, 450, 567, @"IROKEZ");
                        this.AddImage(205, 373, 0x733);
////Prawa Strona
                        this.AddLabel(400, 100, 567, @"DWA WARKOCZE");   
                        this.AddImage(425, 23, 0x72C);
			this.AddLabel(400, 170, 567, @"KOK");
                        this.AddImage(425, 93, 0x730);
			this.AddLabel(400, 240, 567, @"LOKI");
                        this.AddImage(425, 163, 0x731);
			this.AddLabel(400, 310, 567, @"CZUPRYNA");
                        this.AddImage(425, 233, 0x736);
			this.AddLabel(400, 380, 567, @"£YSINA");
                        //this.AddImage(425, 303, 0x0);

                        this.AddLabel(210, 540, 137, @"WYJDZ");
                        this.AddLabel(410, 540, 137, @"WRÓÆ");
		}
		
		public enum Buttons
		{
			Close,
			Button1,
			Button2,
			Button3,
			Button4,
			Button5,
			Button6,
			Button7,
			Button8,
			Button9,
			Button10,
			Button11,
			Button12,
			Button13,
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{                  
			   Mobile m = sender.Mobile;
               
			if (m == null)
				return;

            PlayerMobile pm = (PlayerMobile)m;
            pm.PlaySound(0x2DF);
             
			switch ( info.ButtonID )
			{
				case 0:
				{
				m.SendMessage( "" );
				break;
				}

				case 1: 
				{						            
				pm.HairItemID = 0x203C;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
         		        break; 
				}
				
				case 2: 
				{
				m.HairItemID = 0x203B;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 3: 
				{
				m.HairItemID = 0x203D;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 4: 
				{
				m.HairItemID = 0x2047;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 5: 
				{
				m.HairItemID = 0x2045;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 6: 
				{
				m.HairItemID = 0x2044;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 7: 
				{
				m.HairItemID = 0x2049;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 8: 
				{
				m.HairItemID = 0x204A;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 9: 
				{
				m.HairItemID = 0x0;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 10: 
				{
				m.HairItemID = 0x2048;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

				case 11: 
				{
				m.HairItemID = 0x0;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( FryzuraDamskaGump ) );
                                m.SendGump( new KolorW³osówGump()); 
				break; 
				}

			        case 12:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 13:
			        {
                                m.HairItemID = 0x0;
                                m.SendGump( new P³eæGump());
                                break;
                                } 
      }
    }
  }
}