using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki jaszczura" )]
	public class UjezdzalnyJaszczurOgnisty : BaseMount
	{
		[Constructable]
		public UjezdzalnyJaszczurOgnisty() : this( "ognisty jaszczur" )
		{
		}

		[Constructable]
		public UjezdzalnyJaszczurOgnisty( string name ) : base( name, 45, 0x3F18, AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
                        Body = 45;
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
			ControlSlots = 3;
			MinTameSkill = 77.1;
		}

		public override int Meat{ get{ return 3; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat | FoodType.Fish | FoodType.Eggs | FoodType.FruitsAndVegies; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Ostard; } }

		public UjezdzalnyJaszczurOgnisty( Serial serial ) : base( serial )
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