using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wielkiego wilka" )]
	public class WielkiWilk : BaseMount
	{
		[Constructable]
		public WielkiWilk() : this( "wielki wilk" )
		{
		}

		[Constructable]
		public WielkiWilk( string name ) : base( name, 0x117, 0x3F0D, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
                        Body = 279;
			BaseSoundID = 0x275;

			SetStr( 94, 170 );
			SetDex( 96, 115 );
			SetInt( 6, 10 );

			SetHits( 71, 110 );
			SetMana( 0 );

			SetDamage( 11, 17 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 10, 15 );
			SetResistance( ResistanceType.Poison, 20, 25 );
			SetResistance( ResistanceType.Energy, 20, 25 );

			SetSkill( SkillName.ObronaPrzedMagia, 75.1, 80.0 );
			SetSkill( SkillName.Taktyka, 79.3, 94.0 );
			SetSkill( SkillName.Boks, 79.3, 94.0 );

			Fame = 1500;
			Karma = -1500;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 77.1;
		}

		public override int Meat{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.FruitsAndVegies; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Ostard; } }

		public WielkiWilk( Serial serial ) : base( serial )
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