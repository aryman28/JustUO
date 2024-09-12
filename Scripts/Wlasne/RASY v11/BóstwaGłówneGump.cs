using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class BóstwaG³ówneGump : Gump
	{
		public BóstwaG³ówneGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

//////////////////////                OS-X WYS KOLOR
                        this.AddLabel(290, 63, 137, @"WYBÓR BÓSTWA (G£ÓWNE)");

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
////WYJDZ/WRÓC
			this.AddButton(140, 533, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);
			this.AddButton(400, 533, 4502, 4502, (int)Buttons.Button14, GumpButtonType.Reply, 0);
//////////////////////
                        this.AddLabel(160, 80, 567, @"PORZ¥DEK:");
			this.AddLabel(170, 110, 192, @"TYR");
this.AddLabel(250, 110, 3, @"- Dobry bóg sprawiedliwoœci, honoru, prawdy,");
this.AddLabel(250, 125, 3, @" sprawiedliwego s¹du.");
			this.AddLabel(170, 140, 192, @"KELEMVOR");
this.AddLabel(250, 140, 7, @"- Dobre bóstwo spokojnej i naturalnej œmierci,");
this.AddLabel(250, 155, 7, @" prowadzi dusze zmar³ych do miejsca przeznaczenia.");
			this.AddLabel(170, 170, 192, @"LATHANDER");
this.AddLabel(250, 170, 3, @"- Dobry bóg œwiat³a, œwitu, narodzin, m³odoœci, zdrowia");
this.AddLabel(250, 185, 3, @" i oczyszczenia.");
			this.AddLabel(170, 200, 192, @"MYSTRA");
this.AddLabel(250, 200, 7, @"- Dobra bogini magii, czarów oraz losu.");
                        this.AddLabel(160, 230, 567, @"NEUTRALNE:");
			this.AddLabel(170, 260, 917, @"SILVANUS");
this.AddLabel(250, 260, 256, @"- Neutralny bóg dzikiej natury oraz druidów i szamanów.");
                        this.AddLabel(170, 290, 917, @"OGHMA");
this.AddLabel(250, 290, 252, @"- Neutralny bóg wiedzy, wynalazków, inspiracji,");
this.AddLabel(250, 305, 252, @" trunków alkoholowych.");
                        this.AddLabel(170, 320, 917, @"GRUMBAR");
this.AddLabel(250, 320, 256, @"- Neutralny bóg ¿ywio³u ziemi, stali, solidnoœci, si³y.");
                        this.AddLabel(170, 350, 917, @"SUNE");
this.AddLabel(250, 350, 252, @"- Neutralna bogini piêkna, mi³oœci, muzyki, poezji");
this.AddLabel(250, 365, 252, @" oraz namiêtnoœci.");
                        this.AddLabel(160, 395, 567, @"CHAOS:");
                        this.AddLabel(170, 425, 137, @"CYRIC");
this.AddLabel(250, 425, 32, @"- Chaotyczny bóg morderstw, k³amstw, intryg, oszustw,");
this.AddLabel(250, 440, 32, @" sporów oraz iluzji.");
                        this.AddLabel(170, 455, 137, @"SHAR");
this.AddLabel(250, 455, 36, @"- Chaotyczna bogini ciemnoœci, nocy, tortur, sekretów,");
this.AddLabel(250, 470, 36, @" lochów, zakazanych eksperymentów.");
                        this.AddLabel(170, 485, 137, @"TALOS");
this.AddLabel(250, 485, 32, @"- Chaotyczny bóg burz, zniszczenia, buntu, podpaleñ");
this.AddLabel(250, 500, 32, @" oraz trzêsieñ ziemi.");
                        this.AddLabel(170, 515, 137, @"BESHABA");
this.AddLabel(250, 515, 36, @"- Chaotyczna z³a bogini nieszczêœcia, pecha");
this.AddLabel(250, 530, 36, @" oraz wypadków.");

                        this.AddLabel(210, 550, 137, @"WYJDZ");
                        this.AddLabel(470, 550, 137, @"WRÓÆ");
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
                                  pm.Bóstwo = Bóstwo.Tyr;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());     
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}
				
				case 2: 
				{						            
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Kelemvor;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

				case 3: 
				{						            
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Lathander;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

				case 4: 
				{						            
				  if ( pm.Frakcja == Frakcja.Porzadek || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Mystra;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Chaotycznej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

				case 5: 
				{						            
                                  pm.Bóstwo = Bóstwo.Silvanus;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 6: 
				{						            
                                  pm.Bóstwo = Bóstwo.Oghma;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 7: 
				{						            
                                  pm.Bóstwo = Bóstwo.Grumbar;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 8: 
				{						            
                                  pm.Bóstwo = Bóstwo.Sune;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
         		          break; 
				}

				case 9: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Cyric;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Praworz¹dnej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

				case 10: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Shar;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Praworz¹dnej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

				case 11: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Talos;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Praworz¹dnej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

				case 12: 
				{						            
				  if ( pm.Frakcja == Frakcja.Chaos || pm.Frakcja == Frakcja.Neutralna )
                                  { 
                                  pm.Bóstwo = Bóstwo.Beshaba;
                                  m.CloseGump( typeof ( BóstwaG³ówneGump ) );
                                  m.SendGump( new SkillBojowyGump());
                                  }
                                  else
                                  {
                                  m.SendMessage( 0x35, "Nale¿ysz Do Praworz¹dnej Frakji!" );
                                  m.SendGump( new BóstwaG³ówneGump());
                                  }
         		          break; 
				}

			        case 13:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }	
			
			        case 14:
			        {
				pm.Bóstwo = Bóstwo.None;
                                m.SendGump( new KolorCia³aGump());
                                break;
                                }		
					
	     }
        }
    }
}