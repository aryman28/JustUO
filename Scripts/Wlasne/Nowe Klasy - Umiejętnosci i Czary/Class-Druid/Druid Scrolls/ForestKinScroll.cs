using System;	
using Server;
using Server.Items;

namespace Server.Items
{
   public class ForestKinScroll : SpellScroll
   {
      [Constructable]
      public ForestKinScroll() : this( 1 )
      {
      }

      [Constructable]
      public ForestKinScroll( int amount ) : base( 309, 0xE39 )
      {
         Name = "Forest kin";
         Hue = 0x58B;
      }

      public ForestKinScroll( Serial serial ) : base( serial )
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

    /*  public override Item Dupe( int amount )
      {
         return base.Dupe( new SpiritsOfTheWildScroll( amount ), amount );
      }*/
   }
}
