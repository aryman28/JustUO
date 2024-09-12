using System;
using Server.Items;

namespace Server.Items
{
  public class Bardbookstone : Item
  {
    [Constructable]
    public Bardbookstone() : base( 0xED4 )
    {
      Movable = false;
      Hue = 0x26;
      Name = "Bard book for 5k vy Vertigo";
    }

    public override void OnDoubleClick( Mobile from )
    {
      Container pack = from.Backpack;

      if ( pack != null && pack.ConsumeTotal( typeof( Gold ), 5000) )
      {
        Shrinkbag2 sbag = new Shrinkbag2( 1 );


        if ( !from.AddToBackpack( sbag ) )
          sbag.Delete();
      }
      else
      {
        from.SendMessage( 0XAD, "You need 5,000 gold to buy this item." );
      }
    }

    public Bardbookstone( Serial serial ) : base( serial )
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