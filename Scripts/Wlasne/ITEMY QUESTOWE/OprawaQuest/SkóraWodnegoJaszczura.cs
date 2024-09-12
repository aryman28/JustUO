using System;

namespace Server.Items
{
    public class Sk�raWodnegoJaszczura : Item
    {
        [Constructable]
        public Sk�raWodnegoJaszczura()
            : base(0x1081)
        {
            this.Name = "Sk�ra Wodnego Jaszczura";
            this.Hue = 283;
            this.Weight = 1.0;
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public Sk�raWodnegoJaszczura(Serial serial)
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