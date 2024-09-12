using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class HibernateScroll : SpellScroll
   {
      [Constructable]
      public HibernateScroll() : this( 1 )
      {
      }

      [Constructable]
      public HibernateScroll( int amount ) : base( 313, 0xE39 )
      {
         Name = "Hibernate";
         Hue = 0x58B;
      }

      public HibernateScroll( Serial serial ) : base( serial )
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

     /* public override Item Dupe( int amount )
      {
         return base.Dupe( new HibernateScroll( amount ), amount );
      }*/
   }
}
