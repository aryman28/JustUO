using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class KolorCia�aGump : Gump
	{
		public KolorCia�aGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);
			this.AddLabel(290, 63, 137, @"KOLOR SK�RY"); ////kolor standardowy 33770

			this.AddButton(130, 113, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(130, 143, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
                        this.AddButton(130, 173, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
                        this.AddButton(130, 203, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
                        this.AddButton(130, 233, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(130, 293, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
                        this.AddButton(130, 323, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
                        this.AddButton(130, 353, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
                        this.AddButton(130, 413, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
////Prawa Strona
			this.AddButton(350, 113, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
			this.AddButton(350, 143, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
                        this.AddButton(350, 173, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);
                        this.AddButton(350, 233, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);
                        this.AddButton(350, 263, 4502, 4502, (int)Buttons.Button14, GumpButtonType.Reply, 0);
                        this.AddButton(350, 323, 4502, 4502, (int)Buttons.Button15, GumpButtonType.Reply, 0);
                        this.AddButton(350, 383, 4502, 4502, (int)Buttons.Button16, GumpButtonType.Reply, 0);
                        this.AddButton(350, 443, 4502, 4502, (int)Buttons.Button17, GumpButtonType.Reply, 0);
                        this.AddButton(350, 473, 4502, 4502, (int)Buttons.Button18, GumpButtonType.Reply, 0);

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button19, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button20, GumpButtonType.Reply, 0);

                        this.AddLabel(130, 100, 567, @"DLA LUDZIOPODOBNYCH:");   

			this.AddLabel(180, 130, 541, @"ZWYK�A");
			this.AddLabel(180, 160, 543, @"BR�ZOWA");
			this.AddLabel(180, 190, 637, @"CIEMNO BR�ZOWE");
			this.AddLabel(180, 220, 541, @"RӯOWA");
			this.AddLabel(180, 250, 546, @"JASNO BR�ZOWA");

                        this.AddLabel(130, 280, 567, @"DLA KRASNOLUD�W:");

                        this.AddLabel(180, 310, 842, @"ZIEMISTA");
                        this.AddLabel(180, 340, 915, @"SZARA");
                        this.AddLabel(180, 370, 651, @"B�OTNISTA");

                        this.AddLabel(130, 400, 567, @"DLA NIEUMAR�YCH:");
                        
                        this.AddLabel(180, 430, 901, @"BLADY");
////Prawa Strona
                        this.AddLabel(370, 100, 567, @"DLA ELF�W:");   

			this.AddLabel(400, 130, 88, @"B��KITNE");
			this.AddLabel(400, 160, 555, @"TRAWIASTA");
			this.AddLabel(400, 190, 241, @"RӯOWA JASNA");

			this.AddLabel(370, 220, 567, @"DLA MROCZNYCH ELF�W:");

                        this.AddLabel(400, 250, 902, @"CIEMNA");
                        this.AddLabel(400, 280, 905, @"CIEMNO SZARA");

                        this.AddLabel(370, 310, 567, @"DLA ORK�W i DRIAD:");
                        
                        this.AddLabel(400, 340, 669, @"ZIELONY");

                        this.AddLabel(370, 370, 567, @"DLA CIENI:");
                        
                        this.AddLabel(400, 400, 97, @"CIEMNO NIEBIESKA");

                        this.AddLabel(370, 430, 567, @"DLA DEMON�W:");
                        
                        this.AddLabel(400, 460, 27, @"CZERWONY");
                        this.AddLabel(400, 490, 0, @"SZARY");

                        this.AddLabel(210, 540, 137, @"WYJDZ");
                        this.AddLabel(410, 540, 137, @"WR��");
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
			Button18,
			Button19,
			Button20,
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{                  
			   Mobile m = sender.Mobile;
               
			if (m == null)
				return;

            PlayerMobile pm = (PlayerMobile)m;
            pm.PlaySound(0x2DF);

Item Robex = m.FindItemOnLayer( Layer.OuterTorso );
if ( m.Player )
{
               pm.RemoveItem(Robex);
               pm.EquipItem(Robex);
}             
			switch ( info.ButtonID )
			{
				case 0:
				{
				m.SendMessage( "" );
				break;
				}

				case 1: 
				{						            
if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.P�Cz�owiek )
{
				pm.Hue = 33770;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie mo�esz wyba� tego koloru sk�ry!" );
m.SendGump( new KolorCia�aGump());
}
         		        break; 
				}
				
				case 2: 
				{
if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.P�Cz�owiek )
{
				pm.Hue = 543;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie mo�esz wyba� tego koloru sk�ry!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 3: 
				{
if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.P�Cz�owiek )
{
				pm.Hue = 637;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie mo�esz wyba� tego koloru sk�ry!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 4: 
				{
if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.P�Cz�owiek )
{
				pm.Hue = 541;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie mo�esz wyba� tego koloru sk�ry!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 5: 
				{
if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.P�Cz�owiek )
{
				pm.Hue = 546;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie mo�esz wyba� tego koloru sk�ry!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 6: 
				{
if ( pm.Race1 == Race1.Krasnolud || pm.Race1 == Race1.SkalnyKrasnolud || pm.Race1 == Race1.G�rskiKrasnolud || pm.Race1 == Race1.Duergar || pm.Race1 == Race1.P�Krasnolud )
{
				pm.Hue = 842;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Krasnoludem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 7: 
				{
if ( pm.Race1 == Race1.Krasnolud || pm.Race1 == Race1.SkalnyKrasnolud || pm.Race1 == Race1.G�rskiKrasnolud || pm.Race1 == Race1.Duergar || pm.Race1 == Race1.P�Krasnolud )
{
				pm.Hue = 915;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Krasnoludem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 8: 
				{
if ( pm.Race1 == Race1.Krasnolud || pm.Race1 == Race1.SkalnyKrasnolud || pm.Race1 == Race1.G�rskiKrasnolud || pm.Race1 == Race1.Duergar || pm.Race1 == Race1.P�Krasnolud )
{
				pm.Hue = 651;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Krasnoludem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 9: 
				{
if ( pm.Race1 == Race1.Wampir || pm.Race1 == Race1.Dampir || pm.Race1 == Race1.Nieumar�y )
{
				pm.Hue = 901;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Nieumar�ym!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 10: 
				{
if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.P�Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.Le�nyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf )
{
				pm.Hue = 88;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Elfem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 11: 
				{
if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.P�Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.Le�nyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf )
{
				pm.Hue = 555;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Elfem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 12: 
				{
if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.P�Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.Le�nyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf )
{
				pm.Hue = 241;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Elfem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 13: 
				{
if ( pm.Race1 == Race1.MrocznyElf || pm.Race1 == Race1.Drow || pm.Race1 == Race1.KrwawyElf )
{
				pm.Hue = 902;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Mrocznym Elfem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 14: 
				{
if ( pm.Race1 == Race1.MrocznyElf || pm.Race1 == Race1.Drow || pm.Race1 == Race1.KrwawyElf )
{
				pm.Hue = 905;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Mrocznym Elfem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 15: 
				{
if ( pm.Race1 == Race1.Ork || pm.Race1 == Race1.P�Ork || pm.Race1 == Race1.Driada )
{
				pm.Hue = 669;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Orkiem ani Driad�!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 16: 
				{
if ( pm.Race1 == Race1.Cie� )
{
				pm.Hue = 97;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Cieniem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 17: 
				{
if ( pm.Race1 == Race1.Demon || pm.Race1 == Race1.P�Demon || pm.Race1 == Race1.Sukkub )
{
				pm.Hue = 27;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Demonem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

				case 18: 
				{
if ( pm.Race1 == Race1.Demon || pm.Race1 == Race1.P�Demon || pm.Race1 == Race1.Sukkub )
{
				pm.Hue = 0;
                                m.CloseGump( typeof ( KolorCia�aGump ) );
                                m.SendGump( new B�stwaG��wneGump());
}
else
{
m.SendMessage( 0x35, "Nie Jeste� Demonem!" );
m.SendGump( new KolorCia�aGump());
}
				break; 
				}

			        case 19:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 20:
			        {
				pm.Hue = 33770;
                                m.SendGump( new P�e�Gump());
                                break;
                                } 
      }
    }
  }
}