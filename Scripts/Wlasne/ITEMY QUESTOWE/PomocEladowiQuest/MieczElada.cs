using System;

namespace Server.Items
{
    public class MieczElada : Item
    {
        [Constructable]
        public MieczElada()
            : base(0xF61)
        {
            this.Name = "Stary Miecz Elada";
            this.Weight = 7.0;
            this.Hue = 0;
        }

        public override void AddNameProperty( ObjectPropertyList list )
		{ 
		    base.AddNameProperty( list );
		    list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
        }

        public MieczElada(Serial serial)
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