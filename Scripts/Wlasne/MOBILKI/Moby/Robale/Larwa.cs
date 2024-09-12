using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki robala" )]
	public class Larwa : BaseCreature
	{
		[Constructable]
		public Larwa() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "larwa";
			Body = 768;
			BaseSoundID = 0x388;

			SetStr( 76, 100 );
			SetDex( 126, 145 );
			SetInt( 36, 60 );

			SetHits( 46, 60 );
			SetMana( 0 );

			SetDamage( 6, 16 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 80 );

			SetResistance( ResistanceType.Physical, 25, 30 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 10, 20 );

			SetSkill( SkillName.ObronaPrzedMagia, 25.1, 40.0 );
			SetSkill( SkillName.Taktyka, 35.1, 50.0 );
			SetSkill( SkillName.Boks, 50.1, 65.0 );

			Fame = 775;
			Karma = -775;

			VirtualArmor = 28; 

			Tamable = false;
			ControlSlots = 1;
			MinTameSkill = 74.7;

			PackItem( new SpidersSilk( 2 ) );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
			AddLoot( LootPack.Poor );
		}

		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Arachnid; } }

		public Larwa( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			if ( BaseSoundID == 387 )
				BaseSoundID = 0x388;
		}
	}
}