using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class ManaSpringScroll : SpellScroll
   {
      [Constructable]
      public ManaSpringScroll() : this( 1 )
      {
      }

      [Constructable]
      public ManaSpringScroll( int amount ) : base( 304, 0xE39 )
      {
         Name = "Mana Spring";
         Hue = 0x58B;
      }

      public ManaSpringScroll( Serial serial ) : base( serial )
      {
      		ItemID=0xE39;
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

     /* public override Item Dupe( int amount )
      {
         return base.Dupe( new ManaSpringScroll( amount ), amount );
      }*/
   }
}

