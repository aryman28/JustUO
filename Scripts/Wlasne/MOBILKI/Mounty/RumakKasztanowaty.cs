using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki rumaka" )]
	public class RumakKasztanowaty : BaseMount
	{
		[Constructable]
		public RumakKasztanowaty() : this( "rumak kasztanowaty" )
		{
		}

		[Constructable]
		public RumakKasztanowaty( string name ) : base( name, 228, 0x3F12, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			
			//Body = 0xE4;
			BaseSoundID = 0xA8;

			SetStr( 22, 98 );
			SetDex( 56, 75 );
			SetInt( 6, 10 );

			SetHits( 28, 45 );
			SetMana( 0 );

			SetDamage( 3, 4 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );

			SetSkill( SkillName.ObronaPrzedMagia, 25.1, 30.0 );
			SetSkill( SkillName.Taktyka, 29.3, 44.0 );
			SetSkill( SkillName.Boks, 29.3, 44.0 );

			Fame = 300;
			Karma = 300;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		public override int Meat{ get{ return 3; } }
		public override int Hides{ get{ return 10; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public RumakKasztanowaty( Serial serial ) : base( serial )
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