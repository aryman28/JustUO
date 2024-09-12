using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class HurricaneScroll : SpellScroll
   {
      [Constructable]
      public HurricaneScroll() : this( 1 )
      {
      }

      [Constructable]
      public HurricaneScroll( int amount ) : base( 314, 0xE39 )
      {
         Name = "Mushroom Gateway";
         Hue = 0x58B;
      }

      public HurricaneScroll( Serial serial ) : base( serial )
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

     // public override Item Dupe( int amount )
     // {
        // return base.Dupe( new HurricaneScroll( amount ), amount );
      //}
   }
}
