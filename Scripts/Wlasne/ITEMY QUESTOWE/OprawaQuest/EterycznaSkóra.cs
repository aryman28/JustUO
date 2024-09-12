using System;

namespace Server.Items
{
    public class EterycznaSk�ra : Item
    {
        [Constructable]
        public EterycznaSk�ra()
            : base(0x1081)
        {
            this.Name = "Eteryczna Sk�ra";
            this.Hue = 932;
            this.Weight = 1.0;
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public EterycznaSk�ra(Serial serial)
            : base(serial)
        {
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