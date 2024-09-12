//Ridable/Tamable Polar Bear 
//re-wrote by EvilFreak
// Version 2.0





using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki niedzwiedzia polarnego" )]
	public class UjezdzalnyNiedzwiedzPolarny : BaseMount
	{
		[Constructable]
		public UjezdzalnyNiedzwiedzPolarny() : this( "ujezdzalny niedzwiedz polarny" )
		{
		}

		[Constructable]
		public UjezdzalnyNiedzwiedzPolarny( string name ) : base( name, 0xD5 , 0x3F1E , AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			BaseSoundID = 0xA3;

	//stats are high for my server adjust at will

			SetStr( 210, 300 );
			SetDex( 75, 120 );
			SetInt( 20, 40 );

			SetHits( 400, 470 );
			SetMana( 0 );

			SetDamage( 20, 50 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10, 15 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.ObronaPrzedMagia, 15.1, 20.0 );
			SetSkill( SkillName.Taktyka, 19.2, 29.0 );
			SetSkill( SkillName.Boks, 19.2, 29.0 );

			Fame = 300;
			Karma = 0;

	//might wanna adjust these for your server

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 95.6;
		}

	//this polar does not eat fish you can adjust this as well

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 12; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public UjezdzalnyNiedzwiedzPolarny( Serial serial ) : base( serial )
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