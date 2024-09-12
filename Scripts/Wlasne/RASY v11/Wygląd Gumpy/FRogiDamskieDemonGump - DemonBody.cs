using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class RogiDamskieDemonGump : Gump
	{
		public RogiDamskieDemonGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);
			this.AddLabel(290, 63, 137, @"WYBÓR ROGÓW");

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

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);

                        this.AddLabel(180, 100, 567, @"Z£AMANY RÓG");   
                        this.AddImage(205, 23, 0x76C);
			this.AddLabel(180, 170, 567, @"ZAKRZYWIONE");
                        this.AddImage(205, 93, 0x76D);
			this.AddLabel(180, 240, 567, @"KRÊCONE ROGI");
                        this.AddImage(205, 163, 0x76E);
			this.AddLabel(180, 310, 567, @"D£UGIE PROSTE");
                        this.AddImage(205, 233, 0x775);
			this.AddLabel(180, 380, 567, @"D£UGIE ODSTAJ¥CE");
                        this.AddImage(205, 303, 0x776);
			this.AddLabel(180, 450, 567, @"GRUBE ROGI");
                        this.AddImage(205, 373, 0x777);
////Prawa Strona
                        this.AddLabel(400, 100, 567, @"KRÓTKIE OSTRE");   
                        this.AddImage(425, 23, 0x774);
			this.AddLabel(400, 170, 567, @"POJEDYÑCZY RÓG");
                        this.AddImage(425, 93, 0x773);
			this.AddLabel(400, 240, 567, @"£YSINA");
                        //this.AddImage(425, 163, 0x0);

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
				pm.HairItemID = 0x4258;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());     
         		        break; 
				}
				
				case 2: 
				{
				m.HairItemID = 0x4259;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 3: 
				{
				m.HairItemID = 0x425B;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 4: 
				{
				m.HairItemID = 0x425D;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 5: 
				{
				m.HairItemID = 0x425E;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 6: 
				{
				m.HairItemID = 0x425F;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 7: 
				{
				m.HairItemID = 0x425C;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 8: 
				{
				m.HairItemID = 0x425A;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

				case 9: 
				{
				m.HairItemID = 0x0;
                                pm.FacialHairItemID = 0x0;
                                m.CloseGump( typeof ( RogiDamskieDemonGump ) );
                                m.SendGump( new KolorW³osówGump());
				break; 
				}

			        case 10:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 11:
			        {
                                m.HairItemID = 0x0;
                                m.SendGump( new P³eæGump());
                                break;
                                } 

      }
    }
  }
}