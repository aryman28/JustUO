using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class SzamanGump : Gump
	{
		public SzamanGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYB�R UMIEJ�TNO�CI KLASOWYCH:");
		                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
		
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"SZAMANIZM (+50%)");
                        this.AddLabel(210, 130, 917, @"MEDYTACJA (+50%)");
     			
     			this.AddLabel(150, 160, 137, @"Szaman mo�e awansowa� do klasy presti�owej - Druid -> Mag Natury");
     
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
                                pm.Skills[SkillName.Druidyzm].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
         		        break; 
				}

				case 2:
				{						            
                                pm.Skills[SkillName.Medytacja].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
         		        break; 
				}

			        case 3:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 4:
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