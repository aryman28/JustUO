using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;


namespace Server.Items
{
    public class KamienPrzeznaczenia : Item
    {
        [Constructable]
        public KamienPrzeznaczenia()
            : base(0x2AEC)
        {
            this.Movable = false;
            this.Hue = 0x3AD;
        }

        public KamienPrzeznaczenia(Serial serial)
            : base(serial)
        {
        }

        public override string DefaultName
        {
            get
            {
                return "Kamie� Przeznaczenia";
            }
        }
        public override void OnDoubleClick(Mobile from)
        {
                      PlayerMobile pmobile = from as PlayerMobile;


if ( pmobile.TworzeniePostaci == false )
{
from.SendGump( new FrakcjaGump());

////WSZYSTKIE SKILLE NA 0
			Skills skills = from.Skills;
			for ( int i = 0; i < ( skills.Length ); ++i )// might need to adjust 1 to 0 can't remember - if 1 is skipped then remove -1
			{
				Skill skill = skills[i];
				skill.Base = 0;
			}
////

}
if ( pmobile.TworzeniePostaci == true )
{
from.SendMessage( "Ju� uko�czy�e� tworzenie postaci!" );
from.SendGump( new PodsumowanieGump(from));
}

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

	public class FrakcjaGump : Gump
	{
		public FrakcjaGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

//////////////////////                OS-X WYS KOLOR
                        this.AddLabel(290, 63, 137, @"WYB�R FRAKCJI");
			//this.AddHtml( 385, 230, 181, 279, @"Dokonaj wyboru frakcji, ka�da z nich daje ci inne mo�liwo�ci wyboru klasy postaci oraz drogi �ycia.", (bool)true, (bool)false);

//////////////////////                OS-X  WYS                                                   
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 223, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 363, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
//////////////////////                     WYS X txt Y txt
			this.AddHtml( 140, 125, 300, 100, @"Frakcja Porz�dku to doskona�y wyb�r je�li na swojej drodze kierujesz si� sprawiedliwo�ci� , honorem , prawd�. Miastem frakcyjnym jest Valaria.", (bool)true, (bool)true);
			this.AddHtml( 140, 265, 300, 100, @"Frakcja Neutralna to wyb�r dla tych kt�rzy od walki wol� rzemios�o lub nie poznali jeszcze swojej drogi. Miastem frakcyjnym jest Talvania.", (bool)true, (bool)true);
			this.AddHtml( 140, 405, 300, 100, @"Frakcja Chaosu to wyb�r dla tych kt�rzy szukaj� mordu , gwa�tu , pot�gi , w�adzy. Miastem frakcyjnym jest Azzan.", (bool)true, (bool)true);
			
                        this.AddLabel(210, 100, 567, @"PORZ�DEK");
			this.AddLabel(210, 240, 567, @"NEUTRALNA");
			this.AddLabel(210, 380, 567, @"CHAOS");
                        this.AddLabel(210, 540, 137, @"WYJDZ");
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
				  if ( pm.Frakcja == Frakcja.Porzadek )
                                  { 
                                  m.SendMessage( 0x35, "Wybrales Frakcje Porz�dku!" );
                                  m.SendGump( new FrakcjaGump());
                                  return;
                                  }
                                pm.Frakcja = Frakcja.Porzadek;
                                
                                m.SendGump( new RasaPorzadekGump());
         		        break; 
				}
				

				case 2: 
				{
				    if ( pm.Frakcja == Frakcja.Neutralna )
                                    { 
                                    m.SendMessage( 0x35, "Wybra�e� Frakcje Neutraln�!" );
                                    m.SendGump( new FrakcjaGump());
                                    return;
                                    }		
                                pm.Frakcja = Frakcja.Neutralna;

                                m.SendGump( new RasaNeutralnaGump());
				break; 

				}

				case 3: 
				{
				
                                  if ( pm.Frakcja == Frakcja.Chaos )
                                  { 
                                  m.SendMessage( 0x35, "Wybrales Frakcj� Chaos!" );
                                  m.SendGump( new FrakcjaGump());
                                  return;
                                  }
                                pm.Frakcja = Frakcja.Chaos;

                                m.SendGump( new RasaChaosGump());
                                break; 

				}
			
			        case 4:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }		
						
	     }
        }
    }

	public class RasaPorzadekGump : Gump
	{
		public RasaPorzadekGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYB�R RASY (PORZ�DEK)");
////RASY ZWYK�E			                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
			this.AddButton(140, 203, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
////RASY VIP
			//this.AddButton(140, 263, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
			//this.AddButton(140, 293, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			//this.AddButton(140, 323, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);			
			//this.AddButton(140, 353, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			//this.AddButton(140, 383, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 192, @"CZ�OWIEK");
			this.AddLabel(210, 130, 192, @"ELF");
			this.AddLabel(210, 160, 192, @"Pӣ ELF");
                        this.AddLabel(210, 190, 192, @"KRASNOLUD");
                        this.AddLabel(210, 220, 192, @"NIZIO�EK");

                        //this.AddLabel(160, 250, 137, @"RASY DOST�PNE TYLKO DLA POSIADACZY KONTA VIP");

                        //this.AddLabel(210, 280, 192, @"WYSOKI ELF");
                        //this.AddLabel(210, 310, 192, @"LE�NY ELF");
                        //this.AddLabel(210, 340, 192, @"NOCNY ELF");
                        //this.AddLabel(210, 370, 192, @"G�RSKI KRASNOLUD");
                        //this.AddLabel(210, 400, 192, @"HOBBITA");

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
            pm.HairItemID = 0x0;
            pm.Hue = 0;

			switch ( info.ButtonID )
			{
				case 0:
				{
				m.SendMessage( "" );
				break;
				}

				case 1: 
				{						            
				  if ( pm.Race1 == Race1.Cz�owiek )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Cz�owieka!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Cz�owiek;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
         		        break; 
				}
				
				case 2: 
				{
				    if ( pm.Race1 == Race1.Elf )
                                    { 
                                    m.SendMessage( 0x35, "Wybra�e� Elfa!" );
                                    m.SendGump( new RasaPorzadekGump());
                                    return;
                                    }		
                                pm.Race1 = Race1.Elf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
				break; 
				}

				case 3: 
				{
                                  if ( pm.Race1 == Race1.P�Elf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� P� Elfa!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.P�Elf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break; 
				}
			
			        case 4:
			        {
                                  if ( pm.Race1 == Race1.Krasnolud )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Krasnoluda!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Krasnolud;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break; 
                                }		

			        case 5:
			        {
                                  if ( pm.Race1 == Race1.Nizio�ek )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Nizio�ka!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Nizio�ek;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                                }

			        case 6:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.WysokiElf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Wysokiego Elfa!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.WysokiElf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaPorzadekGump());
                   return;
                                }

			        case 7:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Le�nyElf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Le�nego Elfa!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Le�nyElf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaPorzadekGump());
                   return;
                                }

			        case 8:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.NocnyElf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Nocnego Elfa!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.NocnyElf;
                                pm.Race = Race.Elf;                                
                                
                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaPorzadekGump());
                   return;
                                }

			        case 9:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.G�rskiKrasnolud )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� G�rskiego Krasnoluda!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.G�rskiKrasnolud;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaPorzadekGump());
                   return;
                                }

			        case 10:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Hobbita )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Hobbit�!" );
                                  m.SendGump( new RasaPorzadekGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Hobbita;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaPorzadekGump());
                   return;
                                }

			        case 11:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 12:
			        {
                                      pm.Frakcja = Frakcja.None;
                                      m.SendGump( new FrakcjaGump());
                                      break;
                                }
						
	     }
        }
    }

	public class RasaNeutralnaGump : Gump
	{
		public RasaNeutralnaGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYB�R RASY (NEUTRALNE)");
////RASY ZWYK�E			                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
			this.AddButton(140, 203, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
////RASY VIP
			//this.AddButton(140, 263, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
			//this.AddButton(140, 293, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			//this.AddButton(140, 323, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);			

			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"CZ�OWIEK");
			this.AddLabel(210, 130, 917, @"SKALNY KRASNOLUD");
			this.AddLabel(210, 160, 917, @"Pӣ ELF");
                        this.AddLabel(210, 190, 917, @"ELF");
                        this.AddLabel(210, 220, 917, @"GNOM");

                        //this.AddLabel(160, 250, 137, @"RASY DOST�PNE TYLKO DLA POSIADACZY KONTA VIP");

                        //this.AddLabel(210, 280, 917, @"DAMPIR");
                        //this.AddLabel(210, 310, 917, @"DRIADA");
                        //this.AddLabel(210, 340, 917, @"Pӣ KRASNOLUD");

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
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{                  
			   Mobile m = sender.Mobile;
               
			if (m == null)
				return;

            PlayerMobile pm = (PlayerMobile)m;
            pm.PlaySound(0x2DF);
            pm.HairItemID = 0x0;
            pm.Hue = 0;
 
			switch ( info.ButtonID )
			{
				case 0:
				{
				m.SendMessage( "" );
				break;
				}

				case 1: 
				{						            
				  if ( pm.Race1 == Race1.Cz�owiek )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Cz�owieka!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Cz�owiek;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
         		        break; 
				}
				
				case 2: 
				{
				    if ( pm.Race1 == Race1.SkalnyKrasnolud )
                                    { 
                                    m.SendMessage( 0x35, "Wybra�e� Skalnego Krasnoluda!" );
                                    m.SendGump( new RasaNeutralnaGump());
                                    return;
                                    }		
                                pm.Race1 = Race1.SkalnyKrasnolud;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
				break; 
				}

				case 3: 
				{
                                  if ( pm.Race1 == Race1.P�Elf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� P� Elfa!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.P�Elf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break; 
				}
			
			        case 4:
			        {
                                  if ( pm.Race1 == Race1.Elf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Elfa!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Elf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break; 
                                }		

			        case 5:
			        {
                                  if ( pm.Race1 == Race1.Gnom )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Nizio�ka!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Gnom;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                                }

			        case 6:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Dampir )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Dampira!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Dampir;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaNeutralnaGump());
                   return;
                                }

			        case 7:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Driada )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Driad�!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Driada;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaNeutralnaGump());
                   return;
                                }

			        case 8:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.P�Krasnolud )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� P� Krasnoluda!" );
                                  m.SendGump( new RasaNeutralnaGump());
                                  return;
                                  }
                                pm.Race1 = Race1.P�Krasnolud;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaNeutralnaGump());
                   return;
                                }


			        case 9:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 10:
			        {
                                      pm.Frakcja = Frakcja.None;
                                      m.SendGump( new FrakcjaGump());
                                      break;
                                }
					
	     }
        }
    }

	public class RasaChaosGump : Gump
	{
		public RasaChaosGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 192, @"WYB�R RASY (CHAOS)");
////RASY ZWYK�E			                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
///////////////
////RASY VIP
			//this.AddButton(140, 263, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
			//this.AddButton(140, 293, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
			//this.AddButton(140, 323, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			//this.AddButton(140, 353, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);			
			//this.AddButton(140, 383, 4502, 4502, (int)Buttons.Button9, GumpButtonType.Reply, 0);
			//this.AddButton(140, 413, 4502, 4502, (int)Buttons.Button10, GumpButtonType.Reply, 0);
			//this.AddButton(140, 443, 4502, 4502, (int)Buttons.Button11, GumpButtonType.Reply, 0);
			//this.AddButton(140, 473, 4502, 4502, (int)Buttons.Button12, GumpButtonType.Reply, 0);
			//this.AddButton(380, 263, 4502, 4502, (int)Buttons.Button13, GumpButtonType.Reply, 0);
			//this.AddButton(380, 293, 4502, 4502, (int)Buttons.Button14, GumpButtonType.Reply, 0);
///////////////
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button15, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button16, GumpButtonType.Reply, 0);
////RASY ZWYK�E
                        this.AddLabel(210, 100, 137, @"CZ�OWIEK");
			this.AddLabel(210, 130, 137, @"MROCZNY ELF");
			this.AddLabel(210, 160, 137, @"DUERGAR");
                        this.AddLabel(210, 190, 137, @"Pӣ ORK");
//////////////                       
                        //this.AddLabel(160, 250, 192, @"RASY DOST�PNE TYLKO DLA POSIADACZY KONTA VIP");
////RASY VIP
                        //this.AddLabel(210, 280, 137, @"ORK");
                        //this.AddLabel(210, 310, 137, @"DROW");
                        //this.AddLabel(210, 340, 137, @"WILKO�AK");
                        //this.AddLabel(210, 370, 137, @"WAMPIR");
                        //this.AddLabel(210, 400, 137, @"KRWAWY ELF");
                        //this.AddLabel(210, 430, 137, @"DEMON");
                        //this.AddLabel(210, 460, 137, @"SUKKUB");
                        //this.AddLabel(210, 490, 137, @"Pӣ DEMON");
////////////////
////PRAWA STRONA
                        //this.AddLabel(450, 280, 137, @"CIE�");
                        //this.AddLabel(450, 310, 137, @"NIEUMAR�Y");
///////////////
                        this.AddLabel(210, 540, 192, @"WYJDZ");
                        this.AddLabel(410, 540, 192, @"WR��");
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
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{                  
			   Mobile m = sender.Mobile;
               
			if (m == null)
				return;

            PlayerMobile pm = (PlayerMobile)m;
            pm.PlaySound(0x2DF);
            pm.HairItemID = 0x0;
            pm.Hue = 0;
             
			switch ( info.ButtonID )
			{
				case 0:
				{
				m.SendMessage( "" );
				break;
				}

				case 1: 
				{						            
				  if ( pm.Race1 == Race1.Cz�owiek )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Cz�owieka!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Cz�owiek;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
         		        break; 
				}
				
				case 2: 
				{
				    if ( pm.Race1 == Race1.MrocznyElf )
                                    { 
                                    m.SendMessage( 0x35, "Wybra�e� Mrocznego Elfa!" );
                                    m.SendGump( new RasaChaosGump());
                                    return;
                                    }		
                                pm.Race1 = Race1.MrocznyElf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
				break; 
				}

				case 3: 
				{
                                  if ( pm.Race1 == Race1.Duergar )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Duergara!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Duergar;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break; 
				}
			
			        case 4:
			        {
                                  if ( pm.Race1 == Race1.P�Ork )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� P� Orka!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.P�Ork;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break; 
                                }		

			        case 5:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Ork )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Orka!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Ork;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 6:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Drow )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Drowa!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Drow;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 7:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Wilko�ak )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Wilko�aka!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Wilko�ak;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 8:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Wampir )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Wampira!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Wampir;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 9:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.KrwawyElf )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Krwawego Elfa!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.KrwawyElf;
                                pm.Race = Race.Elf;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 10:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Demon )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Demona!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Demon;
                                pm.Race = Race.Gargoyle;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 11:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Sukkub )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Sukkuba!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Sukkub;
                                pm.Race = Race.Gargoyle;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 12:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.P�Demon )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� P� Demona!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.P�Demon;
                                pm.Race = Race.Gargoyle;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 13:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Cie� )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Cienia!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Cie�;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 14:
			        {

                   if (pm.AccessLevel >= AccessLevel.VIP)
                   {
                                  if ( pm.Race1 == Race1.Nieumar�y )
                                  { 
                                  m.SendMessage( 0x35, "Wybra�e� Nieumar�ego!" );
                                  m.SendGump( new RasaChaosGump());
                                  return;
                                  }
                                pm.Race1 = Race1.Nieumar�y;
                                pm.Race = Race.Human;

                                m.SendGump( new P�e�Gump());
                                break;
                   }
                   else
                   {
                   m.SendMessage( 0x35, "Nie posiadasz statusu VIP!" );
                   }
                   m.SendGump( new RasaChaosGump());
                   return;
                                }

			        case 15:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa�e� zaczeka� z wyborem!" );
                                      break;
                                }			

			        case 16:
			        {
                                      pm.Frakcja = Frakcja.None;
                                      m.SendGump( new FrakcjaGump());
                                      break;
                                }
						
	     }
        }
    }

	public class PodsumowanieGump : Gump
	{
		private Mobile from;
		private int ta;
		private int tb;
		private int tc;

		public PodsumowanieGump(Mobile from)
			: base( 0, 0 )
		{
PlayerMobile pmobile = from as PlayerMobile;

			int ta = from.Str;
			int tb = from.Dex;
			int tc = from.Int;

			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);


                        this.AddLabel(250, 63, 137, @"PODSUMOWANIE TWOICH WYBOR�W:");

			this.AddLabel(160, 100, 55, "FRAKCJA:");
			this.AddLabel(170, 130, 137, pmobile.Frakcja.ToString() );

			this.AddLabel(160, 160, 55, "RASA:");
			this.AddLabel(170, 190, 137, pmobile.Race1.ToString() );

			this.AddLabel(160, 220, 55, "KLASA:");
			this.AddLabel(170, 250, 137, pmobile.Klasa.ToString() );

			this.AddLabel(160, 280, 55, "B�STWO:");
			this.AddLabel(170, 310, 137, pmobile.B�stwo.ToString() );

			this.AddLabel(160, 340, 55, "P�E�:");
			this.AddLabel(170, 370, 137, pmobile.P�e�.ToString() );

			this.AddLabel(400, 100, 55, "SI�A:");
			this.AddLabel(430, 130, 137, ta.ToString() );
			this.AddLabel(400, 160, 55, "ZR�CZNO��:");
			this.AddLabel(430, 190, 137, tb.ToString() );
			this.AddLabel(400, 220, 55, "INTELEKTL:");
			this.AddLabel(430, 250, 137, tc.ToString() );
			                         
                        this.AddLabel(400, 280, 55, "IMI�:");
                        this.AddLabel(430, 310, 137, pmobile.Name.ToString() );

////WYJDZ/WR�C
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
//////////////////////
this.AddLabel(130, 430, 137, @"KAMIE� EMANUJE DZIWNYM �WIAT�EM" );
this.AddLabel(130, 460, 137, @"TWORZ�CYM TUNEL W CZASOPRZESTRZENI," );
this.AddLabel(130, 490, 137, @"CZY CHCESZ DO NIEGO WEJ��?" );

                        this.AddLabel(210, 540, 137, @"TAK");
                        this.AddLabel(410, 540, 137, @"NIE");
		}
		
		public enum Buttons
		{
			Close,
			Button1,
			Button2,
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
				pm.SendMessage( "" );
				break;
				}

			        case 1:
			        {
                                ////przenoszenie(Brit)
	
				pm.Combatant = null;
				pm.Warmode = false;
				pm.Hidden = false;
								
				pm.MoveToWorld( new Point3D( 194, 666, 1 ), Map.Ilshenar );
				pm.SendMessage( 0x35, "Zostale� przeniesiony." );
                                pm.PlaySound(0x1FE);
                               /////koniec przenoszenia    

				   if (pm.AccessLevel == AccessLevel.VIP)
				   {
					pm.AccessLevel = AccessLevel.Player;
				   }

                                      pm.CloseGump( typeof ( PodsumowanieGump ) );
                                      break;
                                }	
			
			        case 2:
			        {
				      pm.SendMessage( 0x35, "Zdecydowa�e� zaczeka�." );
                                      pm.CloseGump( typeof ( PodsumowanieGump ) );
                                      break;
                                }		
					
	                }
        }
    }
}