using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class KlasyGump : Gump
	{
		public KlasyGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYBÓR KLASY POSTACI:");
		                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
			this.AddButton(140, 203, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(140, 233, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
                        this.AddButton(140, 263, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
                        this.AddButton(140, 293, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
                        this.AddButton(140, 323, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);                        
                        this.AddButton(140, 353, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
                        this.AddButton(140, 383, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
                        this.AddButton(140, 413, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);
		
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button14, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"WOJOWNIK");
			this.AddLabel(210, 130, 917, @"MYŒLIWY");
			this.AddLabel(210, 160, 917, @"ADEPT MAGII");
                        this.AddLabel(210, 190, 917, @"£OTR");
                        this.AddLabel(210, 220, 917, @"KULTYSTA");
                        this.AddLabel(210, 250, 917, @"CZELADNIK");
                        this.AddLabel(210, 280, 917, @"ZBIERACZ");
                        this.AddLabel(210, 310, 917, @"DZIKUS");
                        this.AddLabel(210, 340, 917, @"ZNACHOR");
                        this.AddLabel(210, 370, 917, @"SZAMAN");
                        this.AddLabel(210, 400, 917, @"GRAJEK");
                        this.AddLabel(210, 430, 917, @"SMAKOSZ");
                        
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
                                pm.Klasa = Klasa.Wojownik;
                                m.SendGump( new WojownikGump());
         		        break; 
				}
				
				case 2: 
				{
                                pm.Klasa = Klasa.Myœliwy;
                                m.SendGump( new MyœliwyGump());
				break; 
				}

				case 3: 
				{
                                pm.Klasa = Klasa.Adept;
                                m.SendGump( new AdeptGump());
                                break; 
				}
			
			        case 4:
			        {
                                pm.Klasa = Klasa.£otr;
                                m.SendGump( new £otrGump());
                                break; 
                                }		

			        case 5:
			        {
                                pm.Klasa = Klasa.Kultysta;
                                m.SendGump( new KultystaGump());
                                break;
                                }

			        case 6:
			        {
                                pm.Klasa = Klasa.Czeladnik;
                                m.SendGump( new CzeladnikGump());
                                break;
                                }

			        case 7:
			        {
                                pm.Klasa = Klasa.Zbieracz;
                                m.SendGump( new ZbieraczGump());
                                break;
                                }

			        case 8:
			        {
                                pm.Klasa = Klasa.Dzikus;
                                m.SendGump( new DzikusGump());
                                break;
                                }

			        case 9:
			        {
                                pm.Klasa = Klasa.Znachor;
                                m.SendGump( new ZnachorGump());
                                break;
                                }

			        case 10:
			        {
                                pm.Klasa = Klasa.Szaman;
                                m.SendGump( new SzamanGump());
                                break;
                                }

			        case 11:
			        {
                                pm.Klasa = Klasa.Grajek;
                                m.SendGump( new GrajekGump());
                                break;
                                }

			        case 12:
			        {
                                pm.Klasa = Klasa.Smakosz;
                                m.SendGump( new SmakoszGump());
                                break;
                                }

			        case 13:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 14:
			        {
////WSZYSTKIE SKILLE NA 0
			Skills skills = pm.Skills;
			for ( int i = 0; i < ( skills.Length ); ++i )
			{
				Skill skill = skills[i];
				skill.Base = 0;
			}
////
                                      m.SendGump( new SkillBojowyGump());
                                      break;
                                }
					
	     }
        }
    }
}