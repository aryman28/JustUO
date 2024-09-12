using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
	public class RycerzDeed : Item
	{
		public override string DefaultName
		{
			get { return "List polecaj¹cy"; }
		}
		public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>Otwórz list aby zostaæ pasowanym na Rycerza(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
        }

		[Constructable]
		public RycerzDeed() : base( 0x14F0 )
		{
			base.Weight = 1.0;
		        Movable = false;
                        Hue = 1150;
		}

		public RycerzDeed( Serial serial ) : base( serial )
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

		public override void OnDoubleClick( Mobile from )
		{
			 PlayerMobile pm = (PlayerMobile)from;
			                                
			//pm.Title = "";
			//pm.Race1 = Race1.Czlowiek;
			//pm.Race = Race.Human;

			pm.Klasa = Klasa.Rycerz;
			pm.DisplayKlasaTitle = true;
         		pm.SendMessage( 0x35, "Zostales Rycerzem!" );

			Effects.SendLocationParticles( EffectItem.Create( from.Location, from.Map, EffectItem.DefaultDuration ), 0, 0, 0, 0, 0, 5060, 0 );
			Effects.PlaySound( from.Location, from.Map, 0x243 );
			Effects.SendMovingParticles( new Entity( Serial.Zero, new Point3D( from.X - 6, from.Y - 6, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100 );
			Effects.SendMovingParticles( new Entity( Serial.Zero, new Point3D( from.X - 4, from.Y - 6, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100 );
			Effects.SendMovingParticles( new Entity( Serial.Zero, new Point3D( from.X - 6, from.Y - 4, from.Z + 15 ), from.Map ), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100 );
			Effects.SendTargetParticles( from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100 );

                        this.Delete();
		}
	}
}


