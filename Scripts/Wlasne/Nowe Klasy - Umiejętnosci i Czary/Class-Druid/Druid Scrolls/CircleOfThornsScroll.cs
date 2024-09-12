using System;	
using Server;
using Server.Items;

namespace Server.Items
{
   public class CircleOfThornsScroll : SpellScroll
   {
      [Constructable]
      public CircleOfThornsScroll() : this( 1 )
      {
      }

      [Constructable]
      public CircleOfThornsScroll( int amount ) : base( 312, 0xE39 )
      {
          Name = "Circle Of Thorns";
         Hue = 0x58B;
      }

      public CircleOfThornsScroll(Serial serial)
          : base(serial)
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

      //public override Item Dupe( int amount )
      //{
         //return base.Dupe( new LureStoneScroll( amount ), amount );
      //}
   }
}
