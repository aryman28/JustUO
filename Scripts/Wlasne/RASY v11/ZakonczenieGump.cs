using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class ZakonczenieGump : Gump
	{
		public ZakonczenieGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(260, 63, 137, @"ZAKOÑCZENIE TWORZENIA POSTACI:");
		                                            
			this.AddButton(210, 400, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(210, 450, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
		
                        this.AddLabel(280, 417, 137, @"POTWIERDZ");
                        this.AddLabel(280, 467, 137, @"OD NOWA");
     			
     			this.AddLabel(150, 250, 55, @"Potwierdzenie oznacza ,¿e twoich wyborów nie da siê ju¿ cofnaæ!");
     			this.AddLabel(150, 280, 137, @"CHCESZ POTWIERDZIÆ CZY SPRÓBOWAÆ OD NOWA?");     
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
                                      pm.TworzeniePostaci = true;
                                      pm.DisplayRaceTitle = true;
                                      pm.DisplayKlasaTitle = true;
                                      pm.CloseGump( typeof ( ZakonczenieGump ) );
                                      break;
                                }			

			        case 2:
			        {
////WSZYSTKIE SKILLE NA 0
			Skills skills = pm.Skills;
			for ( int i = 0; i < ( skills.Length ); ++i )
			{
				Skill skill = skills[i];
				skill.Base = 0;
			}
////
                                      pm.Frakcja = Frakcja.None;
                                      pm.Klasa = Klasa.None;
                                      pm.Race1 = Race1.None;                                      
                                      pm.Str = 10;
                                      pm.Dex = 10;
                                      pm.Int = 10;
                                      pm.CloseGump( typeof ( ZakonczenieGump ) );
                                      break;

                                }
			
	     }
        }
    }
}