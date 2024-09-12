using System; 
using System.Collections; 
using Server; 
using Server.Items; 
using Server.Network; 
using Server.Spells; 
using Server.Spells.Song; 
using Server.Prompts;
using Server.Targeting; 
 using Server.Gumps;

namespace Server.Gumps 
{ 
   public class SongBookGump : Gump 
   { 
      private SongBook m_Book; 
       
      int gth = 0x903; 
      private void AddBackground() 
      { 
         AddPage( 0 ); 
         AddImage( 100, 10, 0x89B, 0 ); 
      } 
       
      public static bool HasSpell( Mobile from, int spellID ) 
      { 
         Spellbook book = Spellbook.Find( from, spellID ); 
         return ( book != null && book.HasSpell( spellID ) ); 
      } 
       
      public SongBookGump( Mobile from, SongBook book ) : base( 150, 200 ) 
      { 
          
         m_Book = book; 
         AddBackground(); 
         AddPage( 1 ); 
         AddLabel( 150, 17, gth, "Pieœni Barda" ); 
         int sbtn = 0x93A; 
         int sbtn1 = 0x7585; 
         int dby = 40; 
         int dbpy = 40;; 
         AddButton( 396, 14, 0x89E, 0x89E, 17, GumpButtonType.Page, 2 ); 

	      AddLabel(306, 17, 0, @"Wskarz Instr.");
	      AddButton(291, 21, sbtn, sbtn, 20, GumpButtonType.Reply, 0 );
         //AddButton(291, 121, sbtn, sbtn, 2, GumpButtonType.Reply, 0 );
         
         AddButton(283, 179, sbtn1, sbtn1, 19, GumpButtonType.Reply, 0 );
         AddLabel(306, 181, 0, @"Poka¿ pasek zaklêæ");
         //AddLabel(306, 69, 0, @"uaktywnia");
         //AddLabel(306, 81, 0, @"pasek zaklêæ");
	      
         
         if (HasSpell( from, 351) ) 
         { 
            AddLabel( 145, dbpy, gth, "Ballada o Magu" ); 
            AddButton( 125, dbpy + 3, sbtn, sbtn, 1, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 352) ) 
         { 
            AddLabel( 145, dby, gth, "Bohaterska Etiuda" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 2, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 353) ) 
         { 
            AddLabel( 145, dby, gth, "Magiczny Fina³" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 3, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 354) ) 
         { 
            AddLabel( 145, dby, gth, "Ognista Zemsta" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 4, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 355) ) 
         { 
            AddLabel( 145, dby, gth, "Pieœn ¯ywio³ów" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 5, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 356) ) 
         { 
            AddLabel( 145, dby, gth, "Ponury ¿ywio³" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 6, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 357) ) 
         { 
            AddLabel( 145, dby, gth, "Rzeka ¿ycia" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 7, GumpButtonType.Reply, 1 ); 
            dby = dby + 20; 
         } 
         if (HasSpell( from, 358) ) 
         { 
            AddLabel( 145, dby, gth, "Tarcza Odwagi" ); 
            AddButton( 125, dby + 3, sbtn, sbtn, 8, GumpButtonType.Reply, 1 ); 
         } 
         
          
         int i = 2; 
          
         if (HasSpell( from, 351) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Ballada o Magu" ); 
            AddHtml( 130, 59, 123, 132, "Regeneruje manÄ™. [Area Effect]", false, false ); 
//            AddLabel( 295, 37, gth, "Reagents:" ); 
//            AddLabel( 295, 57, gth, "Sulfurous Ash" ); 
//            AddLabel( 295, 77, gth, "Destroying Angel" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 65" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 16" ); 
            i++; 
         } 
          
         if (HasSpell( from, 352) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Bohaterska Etiuda" ); 
            AddHtml( 130, 59, 123, 132, "Zwiêksza si³ê, zrêcznoœæ oraz inteligencjê wybranemu celowi.", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            //            AddLabel( 295, 77, gth, "Mandrake Root" ); 
            //            AddLabel( 295, 97, gth, "Nightshade" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 70" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 12" ); 
            i++; 
         } 
          
         if (HasSpell( from, 353) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Magiczne odesÅ‚anie" ); 
            AddHtml( 130, 59, 123, 132, "Odsy³a wszystkie przywane stwory. [Area Effect]", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            //            AddLabel( 295, 77, gth, "Black Pearl" ); 
            //            AddLabel( 295, 97, gth, "Petrified Wood" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 80" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 15" ); 
            i++; 
         } 
         if (HasSpell( from, 354) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Ognista Zemsta" ); 
            AddHtml( 130, 59, 123, 132, "Atakujesz wybrany cel ognistym p³omieniem.", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Spring Water" ); 
            //            AddLabel( 295, 77, gth, "Spring Water" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 55" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 18" ); 
            i++; 
         } 
         if (HasSpell( from, 355) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Pieœñ ¿ywio³ów" ); 
            AddHtml( 130, 59, 123, 132, "Zwiêksza odpornoœæ na ogieñ, zmino, truciznê oraz energiê. [Area Effect]", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            //            AddLabel( 295, 77, gth, "Spring Water" ); 
            //            AddLabel( 295, 97, gth, "Spiders Silk" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 30" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 12" ); 
            i++; 
         } 
         if (HasSpell( from, 356) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Ponury Å»ywioÅ‚" ); 
            AddHtml( 130, 59, 123, 132, "Zmniejsza odpornoœci wybranego celu", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            //            AddLabel( 295, 77, gth, "Spring Water" ); 
            //            AddLabel( 295, 97, gth, "Spiders Silk" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 50" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 12" ); 
            i++; 
         } 
         if (HasSpell( from, 357) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Rzeka Å¼ycia" ); 
            AddHtml( 130, 59, 123, 132, "Powoli odnawia ¿ycie. [Area Effect]", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            //            AddLabel( 295, 77, gth, "Spring Water" ); 
            //            AddLabel( 295, 97, gth, "Spiders Silk" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 60" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 15" ); 
            i++; 
         } 
         if (HasSpell( from, 358) ) 
         { 
            AddPage( i ); 
            AddButton( 396, 14, 0x89E, 0x89E, 18, GumpButtonType.Page, i+1 ); 
            AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
            AddLabel( 150, 37, gth, "Tarcza Odwagi" ); 
            AddHtml( 130, 59, 123, 132, "Zwiêksza odpornoœæ fizyczn¹ wybranemu celowi.", false, false ); 
            //            AddLabel( 295, 37, gth, "Reagents:" ); 
            //            AddLabel( 295, 57, gth, "Bloodmoss" ); 
            //            AddLabel( 295, 77, gth, "Nightshade" ); 
            AddLabel( 295, 60, gth, "Min. Umiej. : 45" ); 
            AddLabel( 295, 80, gth, "Min. Mana: 12" ); 
            i++; 
         } 
          
          
        AddPage( i ); 
        AddButton( 123, 15, 0x89D, 0x89D, 19, GumpButtonType.Page, i-1 ); 
      } 
       
      public override void OnResponse( NetState state, RelayInfo info )
      {
         Mobile from = state.Mobile; 

   
		//if book has a nearby instrument assigned then play that tune, else target an instrument
		 if ( m_Book.Instrument == null || !(from.InRange( m_Book.Instrument.GetWorldLocation(), 1 )) )
			{
				from.SendMessage( "Potrzebujesz instrumentu aby wykonaæ t¹ pieœñ..." );
				from.SendMessage( "Wskarz instrument na którym chcesz zagraæ" );
            //from.CloseGump( typeof( SpiewnikBardaScrollGump ) );
				//from.SendGump( new SpiewnikBardaScrollGump( from, this ) );
                from.Target = new InternalTarget( m_Book );
			}
		else
		  {
         switch ( info.ButtonID ) 
         { 
            case 0:
            {
               break;
            }
            case 1:
            {
               new BalladaMaga( from, null).Cast();
               break;
            }
            case 2:
            {
               new BohaterskaEtiuda( from, null).Cast();
               break;
            }
            case 3:
            {
               new MagicznyFinal( from, null).Cast();
               break;
            }
            case 4:
            {
               new OgnistaZemsta( from, null).Cast();
               break;
            }
            case 5:
            {
               new PiesnZywiolow( from, null).Cast();
               break;
            }
            case 6:
            {
               new PonuryZywiol( from, null).Cast();
               break;
            }
            case 7:
            {
               new RzekaZycia( from, null).Cast();
               break;
            }
            case 8:
            {
               new TarczaOdwagi( from, null).Cast();
               break;
            }
            case 19:
            {
               from.CloseGump( typeof( SpiewnikBardaScrollGump ) );
			      from.SendGump( new SpiewnikBardaScrollGump( from, new SpiewnikBardaScroll() ) );
               break;
            }
           
            
            case 20:
            {
               from.Target = new InternalTarget( m_Book );
	            break;
            }

          }
         } 
      } 

      private class InternalTarget : Target
      {
	      private SongBook Book;
			
	      public InternalTarget( SongBook book ) : base( 1, false, TargetFlags.None ) 
	        {
		        Book = book;
	        }
			
		  protected override void OnTarget( Mobile from, object target )
		  {
			 if ( target is BaseInstrument )
			 {
			Book.Instrument = (BaseInstrument)target;
			 }
			 else
			from.SendMessage( "To nie jest instrument!" );
		  }
      }

   } 
}
