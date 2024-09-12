using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;


namespace Server.Mobiles
{
	[CorpseName( "zwloki nekromanty" )]
	public class StrazNecroAzzan : BaseCreature
	{
		
	public override WeaponAbility GetWeaponAbility()	
	{
	//return WeaponAbility.ParalyzingBlow;
	return WeaponAbility.MortalStrike;
        } 	
		
		[Constructable]
		public StrazNecroAzzan() : base( AIType.AI_Necro, FightMode.Blue, 10, 1, 0.015, 0.01 )
		{
			SetStr( 70 );
			SetDex( 50 );
			SetInt( 100, 200 );

			SetHits( 150 );

			SetDamage( 20, 25 );

                        Fame = 9000;
			Karma = -9000;

			SpeechHue = Utility.RandomDyedHue();

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 65, 70 );
			SetResistance( ResistanceType.Energy, 50, 65 );


			Hue = Utility.RandomSkinHue();


				Body = 0x190;
				Name = NameList.RandomName( "impaler" ); //male ,Imiona demonów
                                Kills = 50;
				Title = "- Nekromanta";

				Cloak cloak = new Cloak(); 
				cloak.Movable = false;
				cloak.Hue = 1194;
				cloak.LootType = LootType.Blessed;
				AddItem ( cloak );
								
				BoneChest chest = new BoneChest(); 
				chest.Hue = 1194; 
				chest.Movable = false;
				AddItem( chest ); 

				BoneGloves gloves = new BoneGloves(); 
				gloves.Hue = 1194; 
				gloves.Movable = false;
				AddItem( gloves ); 

				BoneHelm helm = new BoneHelm(); 
				helm.Hue = 1194; 
				helm.Movable = false;
				AddItem( helm ); 

				BoneArms arms = new BoneArms(); 
				arms.Hue = 1194; 
				arms.Movable = false;
				AddItem( arms ); 

				BoneLegs legs = new BoneLegs(); 
				legs.Hue = 1194;
				legs.Movable = false; 
				AddItem( legs ); 

				BoneHarvester weapon = new BoneHarvester();
				weapon.Movable = false;
				weapon.Hue = 1194;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Doskona³; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );



			Container pack = new Backpack();

			pack.Movable = false;
			pack.Hue = 1190;

			pack.DropItem( new Gold( 100, 400 ) );

			AddItem( pack );

			Skills[SkillName.Anatomia].Base = 120.0;
			Skills[SkillName.Taktyka].Base = 100.0;
			Skills[SkillName.WalkaMieczami].Base = 120.0;
			Skills[SkillName.ObronaPrzedMagia].Base = 60.0;
      Skills[SkillName.Parowanie].Base = 100.0;
			Skills[SkillName.Logistyka].Base = 100.0;
			//Skills[SkillName.Nekromancja].Base = 100.0;
			Skills[SkillName.MowaDuchow].Base = 100.0;
                }

		public override bool AutoDispel{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		//public override bool NoHouseRestrictions { get { return true; } }
                public override bool ReacquireOnMovement{ get{ return true; } }
                public override bool AlwaysMurderer{ get{ return true; } }

		public StrazNecroAzzan(Serial serial) : base(serial)
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
