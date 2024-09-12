using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Song;
using Server.Commands;



namespace Server.Scripts.Commands
{
	public class CastSongSpells
	{
		public static void Initialize()
		{
            Properties.Initialize();

			Register( "BalladaMaga", AccessLevel.Player, new CommandEventHandler( BalladaMaga_OnCommand ) );

			Register( "BohaterskaEtiuda", AccessLevel.Player, new CommandEventHandler( BohaterskaEtiuda_OnCommand ) );

			Register( "MagicznyFinal", AccessLevel.Player, new CommandEventHandler( MagicznyFinal_OnCommand ) );

			Register( "OgnistaZemsta", AccessLevel.Player, new CommandEventHandler( OgnistaZemsta_OnCommand ) );

			Register( "PiesnZywiolow", AccessLevel.Player, new CommandEventHandler( PiesnZywiolow_OnCommand ) );

			Register( "PonuryZywiol", AccessLevel.Player, new CommandEventHandler( PonuryZywiol_OnCommand ) );

			Register( "RzekaZycia", AccessLevel.Player, new CommandEventHandler( RzekaZycia_OnCommand ) );

			Register( "TarczaOdwagi", AccessLevel.Player, new CommandEventHandler( TarczaOdwagi_OnCommand ) );

			
		}

	    public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
            CommandSystem.Register(command, access, handler);
		}

		public static bool HasSpell( Mobile from, int spellID )
		{
			Spellbook book = Spellbook.Find( from, spellID );

			return ( book != null && book.HasSpell( spellID ) );
		}

		[Usage( "BalladaMaga" )]
		[Description( "casts BalladaMaga" )]
		public static void BalladaMaga_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 351 ) )
					{
					new BalladaMaga( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
                    }
        } 			

		
		[Usage( "BohaterskaEtiuda" )]
		[Description( "Casts BohaterskaEtiuda" )]
		public static void BohaterskaEtiuda_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;
			
         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 352 ) )
					{
					new BohaterskaEtiuda( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		[Usage( "MagicznyFinal" )]
		[Description( "Casts MagicznyFinal" )]
		public static void MagicznyFinal_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 353 ) )
					{
					new MagicznyFinal( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		[Usage( "OgnistaZemsta" )]
		[Description( "Casts OgnistaZemsta" )]
		public static void OgnistaZemsta_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 354 ) )
					{
					new OgnistaZemsta( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		[Usage( "PiesnZywiolow" )]
		[Description( "Casts PiesnZywiolow" )]
		public static void PiesnZywiolow_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 355 ) )
					{
					new PiesnZywiolow( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		[Usage( "PonuryZywiol" )]
		[Description( "Casts PonuryZywiol" )]
		public static void PonuryZywiol_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 356 ) )
					{
					new  PonuryZywiol( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		[Usage( "RzekaZycia" )]
		[Description( "Casts RzekaZycia" )]
		public static void RzekaZycia_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 357 ) )
					{
					new RzekaZycia( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		[Usage( "TarczaOdwagi" )]
		[Description( "Casts TarczaOdwagi" )]
		public static void TarczaOdwagi_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				if ( HasSpell( from, 358 ) )
					{
					new TarczaOdwagi( e.Mobile, null ).Cast(); 
					}
				else
					{
									from.SendLocalizedMessage( 500015 ); // You do not have that spell!
					} 			

		}

		
	}
}
