using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class ZarostGump : Gump
	{
		public ZarostGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);
			this.AddLabel(290, 63, 137, @"WYBÓR ZAROSTU");

			this.AddButton(130, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(130, 153, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
                        this.AddButton(130, 223, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
                        this.AddButton(130, 293, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
                        this.AddButton(130, 363, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(130, 433, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
////Prawa Strona
			this.AddButton(350, 83, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			this.AddButton(350, 153, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);

                        this.AddLabel(180, 100, 567, @"D£UGIE Z W¥SEM");   
                        this.AddImage(205, 23, 0x75A);
			this.AddLabel(180, 170, 567, @"KRÓTKA Z W¥SEM");
                        this.AddImage(205, 93, 0x75E);
			this.AddLabel(180, 240, 567, @"KOZIA Z W¥SEM");
                        this.AddImage(205, 163, 0x75F);
			this.AddLabel(180, 310, 567, @"W¥SY");
                        this.AddImage(205, 233, 0x75C);
			this.AddLabel(180, 380, 567, @"BRODA D£UGIE");
                        this.AddImage(205, 303, 0x75B);
			this.AddLabel(180, 450, 567, @"BRODA KRÓTKA");
                        this.AddImage(205, 373, 0x75D);
////Prawa Strona
                        this.AddLabel(400, 100, 567, @"BRODA KOZIA");   
                        this.AddImage(425, 23, 0x759);
			this.AddLabel(400, 170, 567, @"BEZ BRODY");

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
				pm.FacialHairItemID = 0x204C;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());     
         		        break; 
				}
				
				case 2: 
				{
				m.FacialHairItemID = 0x204B;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 3: 
				{
				m.FacialHairItemID = 0x204D;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 4: 
				{
				m.FacialHairItemID = 0x2041;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 5: 
				{
				m.FacialHairItemID = 0x203E;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 6: 
				{
				m.FacialHairItemID = 0x203F;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 7: 
				{
				m.FacialHairItemID = 0x2040;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 8: 
				{
if ( pm.Race1 == Race1.Krasnolud || pm.Race1 == Race1.SkalnyKrasnolud || pm.Race1 == Race1.GórskiKrasnolud || pm.Race1 == Race1.Duergar || pm.Race1 == Race1.Pó³Krasnolud )
{
m.SendMessage( 0x35, "Krasnolud Musi Mieæ Brodê!" );
m.SendGump( new ZarostGump());
}
else
{
				m.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( ZarostGump ) );
                                m.SendGump( new KolorW³osówGump());
}
				break; 
				}

			        case 9:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 10:
			        {
                                pm.FacialHairItemID = 0x0;
                                m.SendGump( new FryzuraMêskaGump());
                                break;
                                } 
        //break;
        }
    }
  }
}