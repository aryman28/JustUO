using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class KolorW³osówGump : Gump
	{
		public KolorW³osówGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);
			this.AddLabel(290, 63, 137, @"KOLOR OW£OSIENIA");

			this.AddButton(130, 113, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(130, 143, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
                        this.AddButton(130, 173, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
                        this.AddButton(130, 203, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
                        this.AddButton(130, 233, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(130, 263, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
                        this.AddButton(130, 293, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
                        this.AddButton(130, 323, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
                        this.AddButton(130, 353, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
////Prawa Strona
			this.AddButton(350, 113, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
			this.AddButton(350, 143, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
                        this.AddButton(350, 173, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);
                        this.AddButton(350, 233, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);
                        this.AddButton(350, 263, 4502, 4502, (int)Buttons.Button14, GumpButtonType.Reply, 0);
                        this.AddButton(350, 323, 4502, 4502, (int)Buttons.Button15, GumpButtonType.Reply, 0);

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button16, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button17, GumpButtonType.Reply, 0);

                        this.AddLabel(120, 100, 567, @"KOLORY DLA KA¯DEJ RASY:");   
			this.AddLabel(180, 130, 931, @"SZARE");
			this.AddLabel(180, 160, 543, @"BR¥ZOWE");
			this.AddLabel(180, 190, 637, @"CIEMNO BR¥ZOWE");
			this.AddLabel(180, 220, 51, @"BL¥D");
			this.AddLabel(180, 250, 53, @"JASNY BL¥D");
                        this.AddLabel(180, 280, 148, @"CIEMNY BL¥D");
                        this.AddLabel(180, 310, 902, @"CZARNE");
                        this.AddLabel(180, 340, 915, @"SIWE");
                        this.AddLabel(180, 370, 32, @"RUDE");
////Prawa Strona
                        this.AddLabel(370, 100, 567, @"KOLORY DLA ELFÓW:");   
			this.AddLabel(400, 130, 88, @"B£ÊKITNE");
			this.AddLabel(400, 160, 38, @"OGNISTE");
			this.AddLabel(400, 190, 1259, @"Z£OCISTE");

			this.AddLabel(370, 220, 567, @"KOLORY DLA MROCZNYCH ELFÓW:");
                        this.AddLabel(400, 250, 2051, @"KRUCZO CZARNE");
                        this.AddLabel(400, 280, 2045, @"ŒNIE¯NO BIA£E");

                        this.AddLabel(370, 310, 567, @"KOLOR DLA DRIAD:");
                        this.AddLabel(400, 340, 567, @"ZIELONE");

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
			Button14,
			Button15,
			Button16,			
			Button17,
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
				pm.HairHue = 931;
                                pm.FacialHairHue = 931;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
         		        break; 
				}
				
				case 2: 
				{
				pm.HairHue = 543;
                                pm.FacialHairHue = 543;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 3: 
				{
				pm.HairHue = 637;
                                pm.FacialHairHue = 637;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 4: 
				{
				pm.HairHue = 51;
                                pm.FacialHairHue = 51;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 5: 
				{
				pm.HairHue = 53;
                                pm.FacialHairHue = 53;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 6: 
				{
				pm.HairHue = 148;
                                pm.FacialHairHue = 148;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 7: 
				{
				pm.HairHue = 902;
                                pm.FacialHairHue = 902;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 8: 
				{
				pm.HairHue = 915;
                                pm.FacialHairHue = 915;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 9: 
				{
				pm.HairHue = 32;
                                pm.FacialHairHue = 32;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
				break; 
				}

				case 10: 
				{
if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.Pó³Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.LeœnyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf )
{
				pm.HairHue = 88;
                                pm.FacialHairHue = 88;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
}
else
{
m.SendMessage( 0x35, "Nie jesteœ elfem!" );
m.SendGump( new KolorW³osówGump());
}
				break; 
				}

				case 11: 
				{
if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.Pó³Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.LeœnyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf )
{
				pm.HairHue = 38;
                                pm.FacialHairHue = 38;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
}
else
{
m.SendMessage( 0x35, "Nie jesteœ elfem!" );
m.SendGump( new KolorW³osówGump());
}
				break; 
				}

				case 12: 
				{
if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.Pó³Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.LeœnyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf )
{
				pm.HairHue = 1259;
                                pm.FacialHairHue = 1259;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
}
else
{
m.SendMessage( 0x35, "Nie jesteœ elfem!" );
m.SendGump( new KolorW³osówGump());
}
				break; 
				}

				case 13: 
				{
if ( pm.Race1 == Race1.MrocznyElf || pm.Race1 == Race1.Drow || pm.Race1 == Race1.KrwawyElf )
{
				pm.HairHue = 2051;
                                pm.FacialHairHue = 2051;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
}
else
{
m.SendMessage( 0x35, "Nie jesteœ mrocznym elfem!" );
m.SendGump( new KolorW³osówGump());
}
				break; 
				}

				case 14: 
				{
if ( pm.Race1 == Race1.MrocznyElf || pm.Race1 == Race1.Drow || pm.Race1 == Race1.KrwawyElf )
{
				pm.HairHue = 2045;
                                pm.FacialHairHue = 2045;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
}
else
{
m.SendMessage( 0x35, "Nie jesteœ mrocznym elfem!" );
m.SendGump( new KolorW³osówGump());
}
				break; 
				}

				case 15: 
				{
if ( pm.Race1 == Race1.Driada )
{
				pm.HairHue = 567;
                                pm.FacialHairHue = 567;
                                m.CloseGump( typeof ( KolorW³osówGump ) );
                                m.SendGump( new KolorCia³aGump());
}
else
{
m.SendMessage( 0x35, "Nie jesteœ driad¹!" );
m.SendGump( new KolorW³osówGump());
}
				break; 
				}

			        case 16:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 17:
			        {
				pm.HairHue = 0;
                                pm.FacialHairHue = 0;
                                m.SendGump( new P³eæGump());
                                break;
                                } 
      }
    }
  }
}