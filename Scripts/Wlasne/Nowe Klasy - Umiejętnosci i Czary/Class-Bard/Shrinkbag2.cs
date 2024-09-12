using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class Shrinkbag2 : Bag
   {
      [Constructable]
      public Shrinkbag2() : this( 1 )
      {
      }

      [Constructable]
      public Shrinkbag2( int amount )
      {
         DropItem( new SongBook( (UInt64)0xFFFF ));



      }

      public Shrinkbag2( Serial serial ) : base( serial )
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