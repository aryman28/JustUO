using System;

namespace Server.Items
{
    public class DemoniaSkóra : Item
    {
        [Constructable]
        public DemoniaSkóra()
            : base(0x1081)
        {
            this.Name = "Demonia Skóra";
            this.Hue = 38;
            this.Weight = 1.0;
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public DemoniaSkóra(Serial serial)
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