using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki drzewca" )]
	public class ZlotolistnyDrzwiec : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		[Constructable]
		public ZlotolistnyDrzwiec() : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
		{
			Name = "zlotolistny drzewiec";
			Body = 152;

			SetStr( 196, 220 );
			SetDex( 31, 55 );
			SetInt( 66, 90 );

			SetHits( 118, 132 );

			SetDamage( 12, 16 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 20, 25 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 30, 35 );
			SetResistance( ResistanceType.Energy, 20, 30 );

			SetSkill( SkillName.ObronaPrzedMagia, 40.1, 55.0 );
			SetSkill( SkillName.Taktyka, 65.1, 90.0 );
			SetSkill( SkillName.Boks, 65.1, 85.0 );

			Fame = 500;
			Karma = 1500;

			VirtualArmor = 24;
			PackItem( new Log( Utility.RandomMinMax( 23, 34 ) ) );
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
		}

		public override int GetIdleSound()
		{
			return 443;
		}

		public override int GetDeathSound()
		{
			return 31;
		}

		public override int GetAttackSound()
		{
			return 672;
		}

		public override bool BleedImmune{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
		}

		public ZlotolistnyDrzwiec( Serial serial ) : base( serial )
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

			if ( BaseSoundID == 442 )
				BaseSoundID = -1;
		}
	}
}