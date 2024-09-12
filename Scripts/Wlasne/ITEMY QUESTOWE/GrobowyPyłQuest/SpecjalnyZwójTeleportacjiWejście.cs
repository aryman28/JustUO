using System;

namespace Server.Items
{
    public class SpecjalnyZwójTeleportacjiWejœcie : Item
    {
        [Constructable]
        public SpecjalnyZwójTeleportacjiWejœcie()
            : base(0x2260)
        {
            this.Name = "Specjalny Zwój Teleportacji";
            this.LootType = LootType.Blessed;
            this.Weight = 1.0;
            this.Hue = 33;
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public override void OnDoubleClick(Mobile from)
        {
                                ////przenoszenie(wie¿a necro)
				from.Combatant = null;
				from.Warmode = false;
				from.Hidden = false;
								
				from.MoveToWorld( new Point3D( 614, 823, 0 ), Map.Ilshenar );
				from.SendMessage( 0x35, "Zostaleœ przeniesiony." );
                                from.PlaySound(0x1FE);
                               /////koniec przenoszenia
        }

        public SpecjalnyZwójTeleportacjiWejœcie(Serial serial)
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