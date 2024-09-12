using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class BarkSkinScroll : SpellScroll
   {
      [Constructable]
      public BarkSkinScroll() : this( 1 )
      {
      }

      [Constructable]
      public BarkSkinScroll( int amount ) : base( 316, 0xE39 )
      {
         Name = "Bark Skin";
         Hue = 0x58B;
      }

      public BarkSkinScroll( Serial serial ) : base( serial )
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
     // {
        // return base.Dupe( new BarkSkinScroll( amount ), amount );
      //}
   }
}
