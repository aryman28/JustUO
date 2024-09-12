using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki pandy" )]
	public class Panda : BaseMount
	{
		[Constructable]
		public Panda() : this( "panda" )
		{
		}

		[Constructable]
		public Panda( string name ) : base( name, 252, 0x3F14, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			
			Body = 0xFC;

			SetStr( 62, 98 );
			SetDex( 56, 75 );
			SetInt( 20, 80 );

			SetHits( 110, 140 );
			SetMana( 0 );

			SetDamage( 6, 10 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 50, 70 );

			SetSkill( SkillName.ObronaPrzedMagia, 55.1, 90.0 );
			SetSkill( SkillName.Taktyka, 69.3, 94.0 );
			SetSkill( SkillName.Boks, 59.3, 94.0 );

			Fame = 300;
			Karma = 300;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		public override int Meat{ get{ return 3; } }
		public override int Hides{ get{ return 10; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Panda( Serial serial ) : base( serial )
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