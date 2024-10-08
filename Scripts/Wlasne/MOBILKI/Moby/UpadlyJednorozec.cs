using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki jednorozca" )]
	public class UpadlyJednorozec: BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		[Constructable]
		public UpadlyJednorozec() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Upadly Jednorozec";
			Body = 257;
			BaseSoundID = 0x482;

			SetStr( 126, 150 );
			SetDex( 176, 200 );
			SetInt( 86, 110 );

			SetHits( 760, 900 );

			SetDamage( 20, 24 );

			SetDamageType( ResistanceType.Physical, 20 );
			SetDamageType( ResistanceType.Cold, 60 );
			SetDamageType( ResistanceType.Poison, 20 );

			SetResistance( ResistanceType.Physical, 50, 60 );
			SetResistance( ResistanceType.Fire, 25, 30 );
			SetResistance( ResistanceType.Cold, 70, 80 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.ObronaPrzedMagia, 65.1, 80.0 );
			SetSkill( SkillName.Taktyka, 85.1, 100.0 );
			SetSkill( SkillName.Boks, 85.1, 95.0 );

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 19;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Meager );
		}

		public override bool BleedImmune{ get{ return true; } }

		public UpadlyJednorozec( Serial serial ) : base( serial )
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
		}
	}
}