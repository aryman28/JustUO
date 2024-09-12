using System;
using Server;
using Server.Items;

namespace Server.Items
{
   public class FertileEarth : BaseReagent, ICommodity
   {
        int ICommodity.DescriptionNumber { get { return LabelNumber; } }
        bool ICommodity.IsDeedable { get { return (Core.ML); } }

      [Constructable]
      public FertileEarth() : this( 1 )
      {
      }

      [Constructable]
      public FertileEarth( int amount ) : base( 0xF81, amount )
      {
       
      }

      public FertileEarth(Serial serial)
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
   }
}
