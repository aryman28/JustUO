using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class SmakoszGump : Gump
	{
		public SmakoszGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYBÓR UMIEJÊTNOŒCI KLASOWYCH:");
		                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
		
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"ROLNICTWO (+50%)");
                        this.AddLabel(210, 130, 917, @"GOTOWANIE (+50%)");
                        this.AddLabel(210, 160, 917, @"£OWIENIE (+50%)");
                        this.AddLabel(210, 190, 917, @"SMAKOWANIE (+50%)");
     			
     			this.AddLabel(150, 220, 137, @"Smakosz mo¿e awansowaæ do klasy presti¿owej - Kucharz");
     			this.AddLabel(150, 250, 137, @"-> Mistrz Kuchni");
     
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
                                pm.Skills[SkillName.Rolnictwo].Base += 50;
                                m.SendGump( new BudowaCia³aGump());
         		        break; 
				}

				case 2:
				{						            
                                pm.Skills[SkillName.Gotowanie].Base += 50;
                                m.SendGump( new BudowaCia³aGump());
         		        break; 
				}

				case 3:
				{						            
                                pm.Skills[SkillName.Rybactwo].Base += 50;
                                m.SendGump( new BudowaCia³aGump());
         		        break; 
				}

				case 4:
				{						            
                                pm.Skills[SkillName.OcenaSmaku].Base += 50;
                                m.SendGump( new BudowaCia³aGump());
         		        break; 
				}
				
			        case 5:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 6:
			        {
////WSZYSTKIE SKILLE NA 0
			Skills skills = pm.Skills;
			for ( int i = 0; i < ( skills.Length ); ++i )
			{
				Skill skill = skills[i];
				skill.Base = 0;
			}
////
                                      pm.Klasa = Klasa.None;
                                      m.SendGump( new SkillBojowyGump());
                                      break;

                                }
				
	     }
        }
    }
}