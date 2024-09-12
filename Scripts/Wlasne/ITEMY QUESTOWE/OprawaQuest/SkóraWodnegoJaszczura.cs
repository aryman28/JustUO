using System;

namespace Server.Items
{
    public class SkóraWodnegoJaszczura : Item
    {
        [Constructable]
        public SkóraWodnegoJaszczura()
            : base(0x1081)
        {
            this.Name = "Skóra Wodnego Jaszczura";
            this.Hue = 283;
            this.Weight = 1.0;
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public SkóraWodnegoJaszczura(Serial serial)
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