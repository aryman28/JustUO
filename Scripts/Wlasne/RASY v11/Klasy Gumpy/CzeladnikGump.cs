using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class CzeladnikGump : Gump
	{
		public CzeladnikGump()
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
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
			this.AddButton(140, 203, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
			this.AddButton(140, 233, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
			this.AddButton(140, 263, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			this.AddButton(140, 293, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);
			this.AddButton(140, 323, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			this.AddButton(140, 353, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
		
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"G�RNICTWO (+50%)");
			this.AddLabel(210, 130, 917, @"DRWALSTWO (+50%)");
			this.AddLabel(210, 160, 917, @"KOWALSTWO (+50%)");
			this.AddLabel(210, 190, 917, @"GOTOWANIE(+50%)");
			this.AddLabel(210, 220, 917, @"KRAWIECTWO (+50%)");
                        this.AddLabel(210, 250, 917, @"MAJSTERKOWANIE (+50%)");
                        this.AddLabel(210, 280, 917, @"STOLARKA (+50%)");
                        this.AddLabel(210, 310, 917, @"ALCHEMIA (+50%)");
                        this.AddLabel(210, 340, 917, @"WYTWARZANIE �UK�W(+50%)");
                        this.AddLabel(210, 370, 917, @"ZNAJOMO�� ORʯA (+50%)");
 
     			this.AddLabel(150, 400, 137, @"Czeladnik mo�e awansowa� do klasy presti�owej - Rzemie�lnik");
     			this.AddLabel(150, 430, 137, @"-> Mistrz Rzemios�a");
     
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
                                pm.Skills[SkillName.Gornictwo].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
         		        break; 
				}
				
				case 2: 
				{
                                pm.Skills[SkillName.Drwalnictwo].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 3: 
				{
                                pm.Skills[SkillName.Kowalstwo].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 4: 
				{
                                pm.Skills[SkillName.Gotowanie].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 5: 
				{
                                pm.Skills[SkillName.Krawiectwo].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 6: 
				{
                                pm.Skills[SkillName.Majsterkowanie].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 7: 
				{
                                pm.Skills[SkillName.Stolarstwo].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 8: 
				{
                                pm.Skills[SkillName.Alchemia].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 9: 
				{
                                pm.Skills[SkillName.Lukmistrzostwo].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

				case 10: 
				{
                                pm.Skills[SkillName.WiedzaOUzbrojeniu].Base += 50;
                                m.SendGump( new BudowaCia�aGump());
				break; 
				}

			        case 11:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 12:
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