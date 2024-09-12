using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class B�stwaG��wneGump : Gump
	{
		public B�stwaG��wneGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

//////////////////////                OS-X WYS KOLOR
                        this.AddLabel(290, 63, 137, @"WYB�R B�STWA (G��WNE)");

//////////////////////                OS-X  WYS                                                   
////PORZADEK
			this.AddButton(120, 93, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(120, 123, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(120, 153, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(120, 183, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
////NEUTRAL
			this.AddButton(120, 243, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
			this.AddButton(120, 273, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
			this.AddButton(120, 303, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			this.AddButton(120, 333, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
////CHAOS
			this.AddButton(120, 408, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			this.AddButton(120, 438, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
			this.AddButton(120, 468, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
			this.AddButton(120, 498, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);
////WYJDZ/WR�C
			this.AddButton(140, 533, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);
			this.AddButton(400, 533, 4502, 4502, (int)Buttons.Button14, GumpButtonType.Reply, 0);
//////////////////////
                        this.AddLabel(160, 80, 567, @"PORZ�DEK:");
			this.AddLabel(170, 110, 192, @"TYR");
this.AddLabel(250, 110, 3, @"- Dobry b�g sprawiedliwo�ci, honoru, prawdy,");
this.AddLabel(250, 125, 3, @" sprawiedliwego s�du.");
			this.AddLabel(170, 140, 192, @"KELEMVOR");
this.AddLabel(250, 140, 7, @"- Dobre b�stwo spokojnej i naturalnej �mierci,");
this.AddLabel(250, 155, 7, @" prowadzi dusze zmar�ych do miejsca przeznaczenia.");
			this.AddLabel(170, 170, 192, @"LATHANDER");
this.AddLabel(250, 170, 3, @"- Dobry b�g �wiat�a, �witu, narodzin, m�odo�ci, zdrowia");
this.AddLabel(250, 185, 3, @" i oczyszczenia.");
			this.AddLabel(170, 200, 192, @"MYSTRA");
this.AddLabel(250, 200, 7, @"- Dobra bogini magii, czar�w oraz losu.");
                        this.AddLabel(160, 230, 567, @"NEUTRALNE:");
			this.AddLabel(170, 260, 917, @"SILVANUS");
this.AddLabel(250, 260, 256, @"- Neutralny b�g dzikiej natury oraz druid�w i szaman�w.");
                        this.AddLabel(170, 290, 917, @"OGHMA");
this.AddLabel(250, 290, 252, @"- Neutralny b�g wiedzy, wynalazk�w, inspiracji,");
this.AddLabel(250, 305, 252, @" trunk�w alkoholowych.");
                        this.AddLabel(170, 320, 917, @"GRUMBAR");
this.AddLabel(250, 320, 256, @"- Neutralny b�g �ywio�u ziemi, stali, solidno�ci, si�y.");
                        this.AddLabel(170, 350, 917, @"SUNE");
this.AddLabel(250, 350, 252, @"- Neutralna bogini pi�kna, mi�o�ci, muzyki, poezji");
this.AddLabel(250, 365, 252, @" oraz nami�tno�ci.");
                        this.AddLabel(160, 395, 567, @"CHAOS:");
                        this.AddLabel(170, 425, 137, @"CYRIC");
this.AddLabel(250, 425, 32, @"- Chaotyczny b�g morderstw, k�amstw, intryg, oszustw,");
this.AddLabel(250, 440, 32, @" spor�w oraz iluzji.");
                        this.AddLabel(170, 455, 137, @"SHAR");
this.AddLabel(250, 455, 36, @"- Chaotyczna bogini ciemno�ci, nocy, tortur, sekret�w,");
this.AddLabel(250, 470, 36, @" loch�w, zakazanych eksperyment�w.");
                        this.AddLabel(170, 485, 137, @"TALOS");
this.AddLabel(250, 485, 32, @"- Chaotyczny b�g burz, zniszczenia, buntu, podpale�");
this.AddLabel(250, 500, 32, @" oraz trz�sie� ziemi.");
                        this.AddLabel(170, 515, 137, @"BESHABA");
this.AddLabel(250, 515, 36, @"- Chaotyczna z�a bogini nieszcz�cia, pecha");
this.AddLabel(250, 530, 36, @" oraz wypadk�w.");

                        this.AddLabel(210, 550, 137, @"WYJDZ");
                        this.AddLabel(470, 550, 137, @"WR��");
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
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Tyr;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());     
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}
				
				case 2: 
				{						            
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Kelemvor;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

				case 3: 
				{						            
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Lathander;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

				case 4: 
				{						            
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Mystra;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

				case 5: 
				{						            
                                  pm.B�stwo = B�stwo.Silvanus;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 6: 
				{						            
                                  pm.B�stwo = B�stwo.Oghma;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 7: 
				{						            
                                  pm.B�stwo = B�stwo.Grumbar;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 8: 
				{						            
                                  pm.B�stwo = B�stwo.Sune;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 9: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Cyric;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Praworz�dnej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

				case 10: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Shar;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Praworz�dnej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

				case 11: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Talos;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Praworz�dnej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

				case 12: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.B�stwo = B�stwo.Beshaba;
                                  m.CloseGump( typeof ( B�stwaG��wneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale�ysz Do Praworz�dnej Frakji!" );
                                  m.SendGump( new B�stwaG��wneGump());
                                  }
         		          break; 
				}

			        case 13:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }	
			
			        case 14:
			        {
				pm.B�stwo = B�stwo.None;
                                m.SendGump( new KolorCia�aGump());
                                break;
                                }		
					
	     }
        }
    }
}