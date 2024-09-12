//   ___|========================|___
//   \  |  Written by Felladrin  |  /
//    > |      February 2010     | <
//   /__|========================|__\

using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;

namespace Server.Items
{
    public class IdentificationScroll : Item
    {
        [Constructable]
        public IdentificationScroll() : this(1)
        {
        }

        [Constructable]
        public IdentificationScroll( int amount ) : base( 0x1F67 )
        {
            Name = "Zwój identyfikacji";
            //list.Add("<BASEFONT COLOR=YELLOW><B>[U¿yj aby zidentyfikowaæ przedmiot]</B><BASEFONT COLOR=WHITE>");
            Hue = 0xC2;
            Stackable = true;
            Weight = 0.1;
            Amount = amount;
        }
        public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>U¯YJ ABY ZIDENTYFIKOWAÆ PRZEDMIOT.<BASEFONT COLOR=#FFFFFF>" );	
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(from.Backpack))
            {
                from.BeginTarget(2, false, TargetFlags.Beneficial, new TargetCallback(OnTarget));
                from.SendMessage("Który przedmiot chcesz zidentyfikowaæ?");
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }
        }

        public virtual void OnTarget(Mobile from, object o)
        {
            if (Deleted)
                return;

            if (o is Item)
            {
                if (o is BaseWeapon && ((BaseWeapon)o).Identified == false)
                {
                    ((BaseWeapon)o).Identified = true;
                    from.SendMessage( "Zidentyfikowa³eœ przedmiot." );
                    from.Emote( "*Czyta zaklêcie identyfikacji*" );
                    this.Consume();
                }
                else if (o is BaseArmor && ((BaseArmor)o).Identified == false )
                {
                    ((BaseArmor)o).Identified = true;
                    ((BaseArmor)o).Ukrycie = false;
                    ((BaseArmor)o).Sprawdzony = true;
                    from.SendMessage( "Zidentyfikowa³eœ przedmiot." );
                    from.Emote( "*Czyta zaklêcie identyfikacji*" );
                    this.Consume();
                }
                /*else if (o is BaseClothing && ((BaseClothing)o).Identified == false)
                {
                    ((BaseClothing)o).Identified = true;
                    from.SendMessage( "Zidentyfikowa³eœ przedmiot." );
                    from.Emote( "*Czyta zaklêcie identyfikacji*" );
                    this.Consume();
                }*/
                else if (o is BaseJewel && ((BaseJewel)o).Identified == false)
                {
                    ((BaseJewel)o).Identified = true;
                    ((BaseJewel)o).Ukrycie = false;
                    //((BaseJewel)o).Sprawdzony = true;
                    from.SendMessage( "Zidentyfikowa³eœ przedmiot." );
                    from.Emote( "*Czyta zaklêcie identyfikacji*" );
                    this.Consume();
                }
                else
                    from.SendMessage("Jesteœ pewien ¿e to jest przedmiot który chcesz zidentyfikowaæ?");
            }
            else if (o == null)
            {
                from.SendMessage("To nie jest przedmiot który mo¿esz zidentyfikowaæ!");
            }
            else
            {
                from.SendMessage("Po co próbujesz to identyfikowaæ!?");
            }
        }
        
        public IdentificationScroll(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
