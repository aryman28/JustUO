using System;

namespace Server.Items
{
    [FlipableAttribute(0xe43, 0xe42)] 
    public class KuferElada : BaseKuferElada 
    { 
        [Constructable] 
        public KuferElada()
            : base(0xE43)
        { 
        }

        public KuferElada(Serial serial)
            : base(serial)
        { 
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public override void Serialize(GenericWriter writer) 
        { 
            base.Serialize(writer); 

            writer.Write((int)0); // version 
        }

        public override void Deserialize(GenericReader reader) 
        { 
            base.Deserialize(reader); 

            int version = reader.ReadInt(); 
        }
    }
}