using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;


namespace Server.Mobiles
{
	[CorpseName( "zwloki zbrojmistrza" )]
	public class Zbrojmistrz : BaseCreature
	{
		
	public override WeaponAbility GetWeaponAbility()	
	{
	//return WeaponAbility.ParalyzingBlow;
	return WeaponAbility.CrushingBlow;
        } 	
		
		[Constructable]
		public Zbrojmistrz() : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.015, 0.01 )
		{
			SetStr( 100 );
			SetDex( 80 );
			SetInt( 50, 80 );

			SetHits( 200 );

			SetDamage( 20, 30 );
                        TithingPoints = 1000;
                        Fame = 9000;
			Karma = 9000;

			SpeechHue = Utility.RandomDyedHue();

			SetResistance( ResistanceType.Physical, 70, 80 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 50, 65 );


			Hue = Utility.RandomSkinHue();


				Body = 0x190;
				Name = NameList.RandomName( "male" );
                                Kills = 0;
				Title = "- Zbrojmistrz";

                                Cloak cloak = new Cloak(); 
				cloak.Movable = false;
				cloak.Hue = 1282;
				cloak.LootType = LootType.Blessed;
				AddItem ( cloak );
								
				PlateChest chest = new PlateChest(); 
				chest.Hue = 1150; 
				chest.Movable = false;
				AddItem( chest ); 

				PlateGloves gloves = new PlateGloves(); 
				gloves.Hue = 1150; 
				gloves.Movable = false;
				AddItem( gloves ); 

				CloseHelm helm = new CloseHelm(); 
				helm.Hue = 1150; 
				helm.Movable = false;
				AddItem( helm ); 

				PlateGorget gorget = new PlateGorget(); 
				gorget.Hue = 1150; 
				gorget.Movable = false;
				AddItem( gorget ); 

				PlateArms arms = new PlateArms(); 
				arms.Hue = 1150; 
				arms.Movable = false;
				AddItem( arms ); 

				PlateLegs legs = new PlateLegs(); 
				legs.Hue = 1150;
				legs.Movable = false; 
				AddItem( legs ); 

				WarMace weapon = new WarMace();
				weapon.Movable = false;
				weapon.Hue = 1150;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Dobr; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );

				HeaterShield shield = new HeaterShield();
				shield.Movable = false;
				shield.Hue = 1150;
				shield.LootType = LootType.Blessed;
				AddItem( shield );


			Container pack = new Backpack();

			pack.Movable = false;
			pack.Hue = 1282;

			pack.DropItem( new Gold( 100, 400 ) );

			AddItem( pack );

			Skills[SkillName.Magia].Base = 100.0;
                        Skills[SkillName.Rycerstwo].Base = 100.0;
			Skills[SkillName.Taktyka].Base = 100.0;
			Skills[SkillName.WalkaObuchami].Base = 100.0;
			Skills[SkillName.ObronaPrzedMagia].Base = 80.0;
                        Skills[SkillName.Parowanie].Base = 100.0;
			Skills[SkillName.Logistyka].Base = 100.0;
                        
                }

		public override bool AutoDispel{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		//public override bool NoHouseRestrictions { get { return true; } }
                public override bool ReacquireOnMovement{ get{ return true; } }

public override void OnMovement( Mobile m, Point3D oldLocation ) 
{ 

if( m.InRange( this, 1 )) 
{
this.Say( "Witaj nieznajomy!" ); 
} 

}
		public Zbrojmistrz(Serial serial) : base(serial)
		{
		}



		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
