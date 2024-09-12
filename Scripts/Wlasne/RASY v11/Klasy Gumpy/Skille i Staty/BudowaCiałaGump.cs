using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class BudowaCia³aGump : Gump
	{
		public BudowaCia³aGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYBÓR BUDOWY CIA£A:");
		                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
			this.AddButton(140, 203, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(140, 233, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
                        this.AddButton(140, 263, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
		
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"MIÊŒNIAK     +60 SI£A, +10 ZRECZNOŒÆ, +10 INTELEKT");
			this.AddLabel(210, 130, 917, @"GIBKI        +20 SI£A, +50 ZRECZNOŒÆ, +10 INTELEKT");
			this.AddLabel(210, 160, 917, @"SPRAWNY      +35 SI£A, +35 ZRECZNOŒÆ, +10 INTELEKT");
                        this.AddLabel(210, 190, 917, @"POŒREDNIA    +35 SI£A, +25 ZRECZNOŒÆ, +20 INTELEKT");
                        this.AddLabel(210, 220, 917, @"PRAWID£OWA   +30 SI£A, +20 ZRECZNOŒÆ, +30 INTELEKT");
                        this.AddLabel(210, 250, 917, @"INTELIGENT   +20 SI£A, +10 ZRECZNOŒÆ, +50 INTELEKT");
                        this.AddLabel(210, 280, 917, @"CHOROWITY    +10 SI£A, +10 ZRECZNOŒÆ, +60 INTELEKT");
                        
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
                                pm.Str = 60;
                                pm.Dex = 10;
                                pm.Int = 10;
                                m.SendGump( new ZakonczenieGump());

         		        break; 
				}
				
				case 2: 
				{
                                pm.Str = 20;
                                pm.Dex = 50;
                                pm.Int = 10;
                                m.SendGump( new ZakonczenieGump());
				break; 
				}

				case 3: 
				{
                                pm.Str = 35;
                                pm.Dex = 35;
                                pm.Int = 10;
                                m.SendGump( new ZakonczenieGump());
                                break; 
				}
			
			        case 4:
			        {
                                pm.Str = 35;
                                pm.Dex = 25;
                                pm.Int = 20;
                                m.SendGump( new ZakonczenieGump());
                                break; 
                                }		

			        case 5:
			        {
                                pm.Str = 30;
                                pm.Dex = 20;
                                pm.Int = 30;
                                m.SendGump( new ZakonczenieGump());
                                break;
                                }

			        case 6:
			        {
                                pm.Str = 20;
                                pm.Dex = 10;
                                pm.Int = 50;
                                m.SendGump( new ZakonczenieGump());
                                break;
                                }

			        case 7:
			        {
                                pm.Str = 10;
                                pm.Dex = 10;
                                pm.Int = 60;
                                m.SendGump( new ZakonczenieGump());
                                break;
                                }

			        case 8:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 9:
			        {
                                pm.Str = 10;
                                pm.Dex = 10;
                                pm.Int = 10;
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