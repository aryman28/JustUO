using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki bagiennego ostarda" )]
	public class SwampOstard : BaseMount
	{
		[Constructable]
		public SwampOstard() : this( "bagienny ostard" )
		{
		}

		[Constructable]
		public SwampOstard( string name ) : base( name, 0xDB, 0x3EA5, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Hue = Utility.RandomList( 1268,1269,1270 );
                        //Hue = Utility.RandomList( Utility.RandomRedHue(), Utility.RandomYellowHue(),Utility.RandomBlueHue(),Utility.RandomGreenHue(), Utility.RandomAnimalHue(), Utility.RandomBirdHue(), Utility.RandomDyedHue(), Utility.RandomHairHue(), Utility.RandomMetalHue(), Utility.RandomNeutralHue(), Utility.RandomNondyedHue(), Utility.RandomOrangeHue(), Utility.RandomPinkHue(), Utility.RandomSkinHue(), Utility.RandomSlimeHue(), Utility.RandomSnakeHue() );

			BaseSoundID = 0x270;

			SetStr( 94, 170 );
			SetDex( 56, 75 );
			SetInt( 6, 10 );

			SetHits( 71, 88 );
			SetMana( 0 );

			SetDamage( 8, 14 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );

			SetSkill( SkillName.ObronaPrzedMagia, 15.1, 50.0 );
			SetSkill( SkillName.Taktyka, 19.2, 60.0 );
			SetSkill( SkillName.Boks, 19.2, 60.0 );

			Fame = 450;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 45.1;
		}

		public override int Meat{ get{ return 2; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Ostard; } }

		public SwampOstard( Serial serial ) : base( serial )
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
	}
}