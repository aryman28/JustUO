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
	public class StrazLucznikVal : BaseCreature
	{
	
	public override WeaponAbility GetWeaponAbility()	
	{
		return WeaponAbility.MortalStrike;
        } 	
		
		[Constructable]
		public StrazLucznikVal() : base( AIType.AI_Guard, FightMode.Red, 10, 1, 0.015, 0.01 )
		{
			SetStr( 80 );
			SetDex( 90 );
			SetInt( 70, 100 );

			SetHits( 150 );

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

		Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2045, 0x204A, 0x2046 , 0x2049 ) );
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem( hair );

                                Sandals sand = new Sandals(); 
				sand.Hue = 1109;
				sand.LootType = LootType.Blessed;
				AddItem ( sand );

				ElvenQuiver Quiver = new ElvenQuiver(); 
				Quiver.Movable = false;
				Quiver.Hue = 1109;
				Quiver.LootType = LootType.Blessed;
				AddItem ( Quiver );
								
				FemaleStuddedChest chest = new FemaleStuddedChest(); 
				chest.Hue = 1150; 
				chest.Movable = false;
				AddItem( chest ); 

				StuddedGloves gloves = new StuddedGloves(); 
				gloves.Hue = 1150; 
				gloves.Movable = false;
				AddItem( gloves ); 

				StuddedGorget gorget = new StuddedGorget(); 
				gorget.Hue = 1150; 
				gorget.Movable = false;
				AddItem( gorget ); 

				StuddedArms arms = new StuddedArms(); 
				arms.Hue = 1150; 
				arms.Movable = false;
				AddItem( arms ); 

				StuddedLegs legs = new StuddedLegs(); 
				legs.Hue = 1150;
				legs.Movable = false; 
				AddItem( legs ); 

				GuardCrossbow weapon = new GuardCrossbow();
				weapon.Movable = false;
				weapon.Hue = 1150;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Doskona³; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );

			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName( "male" );

				Title = "- Straznik";

		Item hair = new Item( Utility.RandomList( 0x203B, 0x203C, 0x203D, 0x2045, 0x204A, 0x2046 , 0x2049 ) );
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem( hair );

                                Sandals sand = new Sandals(); 
				sand.Hue = 1109;
				sand.LootType = LootType.Blessed;
				AddItem ( sand );

				ElvenQuiver Quiver = new ElvenQuiver(); 
				Quiver.Movable = false;
				Quiver.Hue = 1109;
				Quiver.LootType = LootType.Blessed;
				AddItem ( Quiver );
								
				StuddedChest chest = new StuddedChest(); 
				chest.Hue = 1150; 
				chest.Movable = false;
				AddItem( chest ); 

				StuddedGloves gloves = new StuddedGloves(); 
				gloves.Hue = 1150; 
				gloves.Movable = false;
				AddItem( gloves ); 

				StuddedGorget gorget = new StuddedGorget(); 
				gorget.Hue = 1150; 
				gorget.Movable = false;
				AddItem( gorget ); 

				StuddedArms arms = new StuddedArms(); 
				arms.Hue = 1150; 
				arms.Movable = false;
				AddItem( arms ); 

				StuddedLegs legs = new StuddedLegs(); 
				legs.Hue = 1150;
				legs.Movable = false; 
				AddItem( legs ); 

				GuardCrossbow weapon = new GuardCrossbow();
				weapon.Movable = false;
				weapon.Hue = 1150;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Doskona³; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );

			}

			Container pack = new Backpack();
                        Bolt bolts = new Bolt( 250 );

                        CantWalk = true;
			pack.Movable = false;
			pack.Hue = 1150;

			pack.DropItem( new Gold( 100, 400 ) );
                        pack.DropItem( new Bolt(100) );

			AddItem( pack );

			Skills[SkillName.Anatomia].Base = 100.0;
			Skills[SkillName.Taktyka].Base = 100.0;
			Skills[SkillName.Lucznictwo].Base = 100.0;
			Skills[SkillName.ObronaPrzedMagia].Base = 60.0;
			Skills[SkillName.Logistyka].Base = 100.0;
                }

		public override void CriminalAction( bool message ) { }

		public override bool AutoDispel{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		//public override bool NoHouseRestrictions { get { return true; } }
                public override bool ReacquireOnMovement{ get{ return true; } }

		public StrazLucznikVal(Serial serial) : base(serial)
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
