using System;
using Server.Network;
using Server.Gumps;
using Server.Spells;
using Server.Mobiles;

namespace Server.Items
{
   public class SongBook : Spellbook
   {
      public override SpellbookType SpellbookType{ get{ return SpellbookType.Song; } }
      public override int BookOffset{ get{ return 351; } }
      public override int BookCount{ get{ return 8; } }
      
      public BaseInstrument Instrument;
   	
   	
      [Constructable]
      public SongBook() : this( (ulong)0 )
      {
      }

      [Constructable]
      public SongBook( ulong content ) : base( content, 0xEFA )
      {
        Name = "�piewnik Barda";
        Hue = 0x2F;
        Content = 0xFF;
        
      }

      public override void OnDoubleClick( Mobile from )
      {
         if ( from.InRange( GetWorldLocation(), 1 ) )
         {
            from.CloseGump( typeof( SongBookGump ) );
            from.SendGump( new SongBookGump( from, this ) );
            
         }
         if( Wlasciciel == null )
			{
				Wlasciciel = from;
				from.SendMessage( 32, "Zosta�e� w�a�cicielem tej ksi�gi" );
				
			}
         else from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
                          
      }

      public SongBook( Serial serial ) : base( serial )
      {
      }

      public override void Serialize( GenericWriter writer )
      {
         base.Serialize( writer );

         writer.Write( (int) 0 ); // version
         
         writer.Write( (Item)Instrument );
      		
      }

      public override void Deserialize( GenericReader reader )
      {
         base.Deserialize( reader );

         int version = reader.ReadInt();
      	
      	switch( version )
      	{
      		case 0:
      		{
      			Instrument = reader.ReadItem() as BaseInstrument;
      			break;
      		}
      	}
      }
   }
}
