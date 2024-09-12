using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{
	public class P�e�Gump : Gump
	{
		public P�e�Gump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);
			this.AddLabel(290, 63, 137, @"WYB�R P�CI");

			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 183, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 567, @"KOBIETA");
			this.AddLabel(210, 200, 567, @"MʯCZYZNA");
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
                                  pm.Female = true;
                                  pm.P�e� = P�e�.Kobieta;
                                  pm.FacialHairItemID = 0x0;

				  if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.P�Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.Le�nyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf || pm.Race1 == Race1.MrocznyElf || pm.Race1 == Race1.Drow || pm.Race1 == Race1.KrwawyElf || pm.Race1 == Race1.Driada )
                                  { 
                                   pm.BodyValue = 606;
                                   m.CloseGump( typeof ( P�e�Gump ) );
                                   m.SendGump( new FryzuraDamskaElfiaGump());
          		           break; 
				  }

  				  if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Krasnolud || pm.Race1 == Race1.SkalnyKrasnolud || pm.Race1 == Race1.G�rskiKrasnolud || pm.Race1 == Race1.Duergar || pm.Race1 == Race1.P�Krasnolud || pm.Race1 == Race1.P�Cz�owiek || pm.Race1 == Race1.Wampir || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.Dampir || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Cie� || pm.Race1 == Race1.Nieumar�y )
                                  { 
                                  pm.BodyValue = 401; 
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new FryzuraDamskaGump());
                                  break;
                                  }
                                  if ( pm.Race1 == Race1.Ork || pm.Race1 == Race1.P�Ork )
                                  {     
                                  pm.BodyValue = 401; 
                                  pm.FacialHairItemID = 0x0;
				  pm.HairItemID = 0x0;
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new KolorCia�aGump());  ///// Kolor Cia�a 558
                                  break;
                                  }

  				  if ( pm.Race1 == Race1.Demon || pm.Race1 == Race1.Sukkub || pm.Race1 == Race1.P�Demon )
                                  { 
                                  pm.BodyValue = 667;
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new RogiDamskieDemonGump());
                                  break;
                                  }
     
         		        break; 
				}
				

				case 2: 
				{
                                  pm.Female = false;
                                  pm.P�e� = P�e�.M�czyzna;

				  if ( pm.Race1 == Race1.Elf || pm.Race1 == Race1.P�Elf || pm.Race1 == Race1.WysokiElf || pm.Race1 == Race1.Le�nyElf || pm.Race1 == Race1.SzaryElf || pm.Race1 == Race1.NocnyElf || pm.Race1 == Race1.MrocznyElf || pm.Race1 == Race1.Drow || pm.Race1 == Race1.KrwawyElf || pm.Race1 == Race1.Driada )
                                  { 
                                  pm.BodyValue = 605;
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new FryzuraM�skaElfiaGump());
                                  break;
                                  }
  
  				  if ( pm.Race1 == Race1.Cz�owiek || pm.Race1 == Race1.Krasnolud || pm.Race1 == Race1.SkalnyKrasnolud || pm.Race1 == Race1.G�rskiKrasnolud || pm.Race1 == Race1.Duergar || pm.Race1 == Race1.P�Krasnolud || pm.Race1 == Race1.P�Cz�owiek || pm.Race1 == Race1.Wampir || pm.Race1 == Race1.Wilko�ak || pm.Race1 == Race1.Dampir || pm.Race1 == Race1.Gnom || pm.Race1 == Race1.Hobbita || pm.Race1 == Race1.Nizio�ek || pm.Race1 == Race1.Cie� || pm.Race1 == Race1.Nieumar�y )
                                  { 
                                  pm.BodyValue = 400; 
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new FryzuraM�skaGump());
                                  break;
                                  }
                                  if ( pm.Race1 == Race1.Ork || pm.Race1 == Race1.P�Ork )
                                  {     
                                  pm.BodyValue = 400; 
                                  pm.FacialHairItemID = 0x0;
				  pm.HairItemID = 0x0;
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new KolorCia�aGump());  ////// Kolor Cia�a 558
                                  break;
                                  }
     
				  if ( pm.Race1 == Race1.Demon || pm.Race1 == Race1.Sukkub || pm.Race1 == Race1.P�Demon )
                                  { 
                                  pm.BodyValue = 666;
                                  m.CloseGump( typeof ( P�e�Gump ) );
                                  m.SendGump( new RogiM�skieDemonGump());
                                  break;
                                  }

				break; 
				}

			        case 3:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 4:
			        {
                          pm.Race1 = Race1.None;
                          pm.P�e� = P�e�.None;

                          if ( pm.Frakcja == Frakcja.Porzadek )
                          {
                          m.SendGump( new RasaPorzadekGump());
                          break;
                          } 
                          
                          if ( pm.Frakcja == Frakcja.Neutralna )
                          {
                          m.SendGump( new RasaNeutralnaGump());
                          break;
                          } 
                          
                          if ( pm.Frakcja == Frakcja.Chaos )
                          {
                          m.SendGump( new RasaChaosGump());
                          break;
                          } 

         		      break; 
	                      }
        }
    }
  }
}