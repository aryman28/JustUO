// created on 2003-06-23 at 15:48
using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;


namespace Server.Mobiles
{
	[CorpseName( "zwloki straznika" )]
	public class StrazPieszaZwykla : BaseCreature
	{
		
	public override WeaponAbility GetWeaponAbility()	
	{
	return WeaponAbility.ConcussionBlow;
	return WeaponAbility.WhirlwindAttack;
        } 	
		
		[Constructable]
		public StrazPieszaZwykla() : base( AIType.AI_Guard, FightMode.Red, 10, 1, 0.015, 0.01 )
		{
			SetStr( 100 );
			SetDex( 100 );
			SetInt( 100, 110 );

			SetHits( 200 );

			SetDamage( 25, 30 );

			SpeechHue = Utility.RandomDyedHue();

			SetResistance( ResistanceType.Physical, 65, 70 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 65 );
			SetResistance( ResistanceType.Energy, 50, 65 );


			Hue = Utility.RandomSkinHue();

			if ( Female = Utility.RandomBool() )
			{
				Body = 0x191;
				Name = NameList.RandomName( "female" );

				Title = "- Strazniczka";

				Cloak cloak = new Cloak(); 
				cloak.Movable = false;
				cloak.Hue = 1175;
				cloak.LootType = LootType.Blessed;
				AddItem ( cloak );
								
				FemalePlateChest chest = new FemalePlateChest(); 
				//chest.Hue = 1150; 
				chest.Movable = false;
				AddItem( chest ); 

				PlateGloves gloves = new PlateGloves(); 
				//gloves.Hue = 1150; 
				gloves.Movable = false;
				AddItem( gloves ); 

				CloseHelm helm = new CloseHelm(); 
				//helm.Hue = 1150; 
				helm.Movable = false;
				AddItem( helm ); 

				PlateGorget gorget = new PlateGorget(); 
				//gorget.Hue = 1150; 
				gorget.Movable = false;
				AddItem( gorget ); 

				PlateArms arms = new PlateArms(); 
				//arms.Hue = 1150; 
				arms.Movable = false;
				AddItem( arms ); 

				PlateLegs legs = new PlateLegs(); 
				//legs.Hue = 1150;
				legs.Movable = false; 
				AddItem( legs ); 

				Halberd weapon = new Halberd();
				weapon.Movable = false;
				//weapon.Hue = 1150;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Doskona³; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );

				//HeaterShield shield = new HeaterShield();
				//shield.Movable = false;
				//shield.Hue = 1150;
				//shield.LootType = LootType.Blessed;
				//AddItem( shield );
				
				
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );

				Title = "- Straznik";

				Cloak cloak = new Cloak(); 
				cloak.Movable = false;
				cloak.Hue = 1175;
				cloak.LootType = LootType.Blessed;
				AddItem ( cloak );
								
				PlateChest chest = new PlateChest(); 
				//chest.Hue = 1150; 
				chest.Movable = false;
				AddItem( chest ); 

				PlateGloves gloves = new PlateGloves(); 
				//gloves.Hue = 1150; 
				gloves.Movable = false;
				AddItem( gloves ); 

				CloseHelm helm = new CloseHelm(); 
				//helm.Hue = 1150; 
				helm.Movable = false;
				AddItem( helm ); 

				PlateGorget gorget = new PlateGorget(); 
				//gorget.Hue = 1150; 
				gorget.Movable = false;
				AddItem( gorget ); 

				PlateArms arms = new PlateArms(); 
				//arms.Hue = 1150; 
				arms.Movable = false;
				AddItem( arms ); 

				PlateLegs legs = new PlateLegs(); 
				//legs.Hue = 1150;
				legs.Movable = false; 
				AddItem( legs ); 

				Halberd weapon = new Halberd();
				weapon.Movable = false;
				//weapon.Hue = 1150;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Doskona³; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );

				//HeaterShield shield = new HeaterShield();
				//shield.Movable = false;
				//shield.Hue = 1150;
				//shield.LootType = LootType.Blessed;
				//AddItem( shield );
			}

			Container pack = new Backpack();

			pack.Movable = false;
			pack.Hue = 1150;

			pack.DropItem( new Gold( 100, 400 ) );

			AddItem( pack );

			Skills[SkillName.Anatomia].Base = 120.0;
			Skills[SkillName.Taktyka].Base = 100.0;
			Skills[SkillName.WalkaMieczami].Base = 120.0;
			Skills[SkillName.ObronaPrzedMagia].Base = 60.0;
      Skills[SkillName.Parowanie].Base = 100.0;
			Skills[SkillName.Logistyka].Base = 100.0;
                }

		public override void CriminalAction( bool message ) { }

		public override bool AutoDispel{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		//public override bool NoHouseRestrictions { get { return true; } }
                public override bool ReacquireOnMovement{ get{ return true; } }
               	
		public StrazPieszaZwykla(Serial serial) : base(serial)
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
