using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki drakona" )]
	public class KolczastyDrakon : BaseCreature
	{
		[Constructable]
		public KolczastyDrakon() : base( AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "kolczasty drakon";
			Body = 56;
			BaseSoundID = 362;

			SetStr( 898, 1030 );
			SetDex( 68, 200 );
			SetInt( 488, 620 );

			SetHits( 558, 599 );

			SetDamage( 29, 35 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Cold, 25 );

			SetResistance( ResistanceType.Physical, 65, 75 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 45, 55 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 50, 60 );

			SetSkill( SkillName.Intelekt, 80.1, 100.0 );
			SetSkill( SkillName.Magia, 80.1, 100.0 );
			SetSkill( SkillName.Medytacja, 52.5, 75.0 );
			SetSkill( SkillName.ObronaPrzedMagia, 100.3, 130.0 );
			SetSkill( SkillName.Taktyka, 97.6, 100.0 );
			SetSkill( SkillName.Boks, 97.6, 100.0 );

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.FilthyRich, 3 );
			AddLoot( LootPack.Gems, 5 );
		}

		public override int GetIdleSound()
		{
			return 0x2D5;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool ReacquireOnMovement{ get{ return true; } }
		public override bool HasBreath{ get{ return true; } } // fire breath enabled
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Deadly; } }
		public override Poison HitPoison{ get{ return Poison.Deadly; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public override int Meat{ get{ return 19; } }
		public override int Hides{ get{ return 20; } }
		public override int Scales{ get{ return 10; } }
		public override ScaleType ScaleType{ get{ return ScaleType.Black; } }
		public override HideType HideType{ get{ return HideType.Barbed; } }
		public override bool CanFly { get { return true; } }

		public KolczastyDrakon( Serial serial ) : base( serial )
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