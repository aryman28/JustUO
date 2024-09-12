using System;
using Server.Network;
using Server.Gumps;
using Server.Spells;

namespace Server.Items
{
   public class DruidSpellbook : Spellbook
   {
      public override SpellbookType SpellbookType{ get{ return SpellbookType.Druid; } }
      public override int BookOffset{ get{ return 301; } }
      public override int BookCount{ get{ return 14; } }

      
      [Constructable]
      public DruidSpellbook() : this( (ulong)0 )
      {
      }

      [Constructable]
      public DruidSpellbook( ulong content ) : base( content, 0xEFA )
      {
         Hue = 0x8A2;
         Name = "Grimmur Magii Druidycznej";
         Content = 0x3FFF;
      }

      public override void OnDoubleClick( Mobile from )
      {
          Container pack = from.Backpack;

          if (Parent == from || (pack != null && Parent == pack))
              //DisplayTo(from);
          
         if ( from.InRange( GetWorldLocation(), 1 ) )
         {
            from.CloseGump( typeof( DruidSpellbookGump ) );
            from.SendGump( new DruidSpellbookGump( from, this ) );
         }
         if( Wlasciciel == null )
			{
				Wlasciciel = from;
				from.SendMessage( 32, "Zosta³eœ w³aœcicielem tej ksiêgi" );
				//base.OnDoubleClick( from );
			}
          
         else from.SendLocalizedMessage(500207); // The spellbook must be in your backpack (and not in a container within) to open.
	

      }

      public DruidSpellbook( Serial serial ) : base( serial )
      {
      }

      public override void Serialize( GenericWriter writer )
      {
         base.Serialize( writer );

         writer.Write( (int) 0 ); // version
      }

      public override void Deserialize( GenericReader reader )
      {
         base.Deserialize( reader );

         int version = reader.ReadInt();
      }
   }
}
