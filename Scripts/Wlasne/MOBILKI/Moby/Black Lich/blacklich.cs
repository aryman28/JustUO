using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fifth;

namespace Server.Mobiles
{
	public class BlackLich : BaseCreature
	{
		[Constructable]
		public BlackLich() : base( AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = 0x2C3;
			Female = false;
			BodyValue = 24;
			BaseSoundID = 1001;
			Name = "czarny licz";

			SetStr( 416, 505 );
			SetDex( 146, 165 );
			SetInt( 566, 655 );

			SetHits( 2500 );
			SetMana( 2000 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 70 );
			SetDamageType( ResistanceType.Cold, 15 );
			SetDamageType( ResistanceType.Energy, 15 );

			SetResistance( ResistanceType.Physical, 40, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 50, 60 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Intelekt, 77.6, 87.5 );
			SetSkill( SkillName.Nekromancja, 100.6, 120.5 );
			SetSkill( SkillName.MowaDuchow, 110.1, 120.5 );
			SetSkill( SkillName.Magia, 90.1, 100.1);
			SetSkill( SkillName.Zatruwanie, 80.5 );
			SetSkill( SkillName.Medytacja, 110.0 );
			SetSkill( SkillName.ObronaPrzedMagia, 80.1, 85.0 );
			SetSkill( SkillName.Parowanie, 90.1, 95.1 );
			SetSkill( SkillName.Taktyka, 120.0 );
			SetSkill( SkillName.Boks, 70.1, 80.0 );

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 65;

			PackNecroReg( 50, 100 );
			PackGold( 5500, 7000 );
			PackItem( new LichStaff() );
			
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.MedScrolls, 4 );
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override int TreasureMapLevel{ get{ return 5; } }

		public void Polymorph( Mobile m )
		{
			if ( !m.CanBeginAction( typeof( PolymorphSpell ) ) || !m.CanBeginAction( typeof( IncognitoSpell ) ) || m.IsBodyMod )
				return;

			IMount mount = m.Mount;

			if ( mount != null )
				mount.Rider = null;

			if ( m.Mounted )
				return;

			if ( m.BeginAction( typeof( PolymorphSpell ) ) )
			{
				Item disarm = m.FindItemOnLayer( Layer.OneHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				disarm = m.FindItemOnLayer( Layer.TwoHanded );

				if ( disarm != null && disarm.Movable )
					m.AddToBackpack( disarm );

				m.BodyMod = 50;
				m.HueMod = 0;

				new ExpirePolymorphTimer( m ).Start();
			}
		}

		private class ExpirePolymorphTimer : Timer
		{
			private Mobile m_Owner;

			public ExpirePolymorphTimer( Mobile owner ) : base( TimeSpan.FromMinutes( 3.0 ) )
			{
				m_Owner = owner;

				Priority = TimerPriority.OneSecond;
			}

			protected override void OnTick()
			{
				if ( !m_Owner.CanBeginAction( typeof( PolymorphSpell ) ) )
				{
					m_Owner.BodyMod = 0;
					m_Owner.HueMod = -1;
					m_Owner.EndAction( typeof( PolymorphSpell ) );
				}
			}
		}

		public void SpawnWraiths( Mobile target )
		{

		BaseCreature spawn1 = new WraithsSpawn( this );
		BaseCreature spawn2 = new WraithsSpawn( this );
		BaseCreature spawn3 = new WraithsSpawn( this );
		BaseCreature spawn4 = new WraithsSpawn( this );
		BaseCreature spawn5 = new WraithsSpawn( this );
		BaseCreature spawn6 = new WraithsSpawn( this );
		spawn1.Team = this.Team;
		spawn1.MoveToWorld( this.Location, this.Map );
		spawn2.Team = this.Team;
		spawn2.MoveToWorld( this.Location, this.Map );
		spawn3.Team = this.Team;
		spawn3.MoveToWorld( this.Location, this.Map );
		spawn4.Team = this.Team;
		spawn4.MoveToWorld( this.Location, this.Map );
		spawn5.Team = this.Team;
		spawn5.MoveToWorld( this.Location, this.Map );
		spawn6.Team = this.Team;
		spawn6.MoveToWorld( this.Location, this.Map );

		}

		public void DoSpecialAbility( Mobile target )
		{
			if ( 0.6 >= Utility.RandomDouble() )
				Polymorph( target );

			if ( 0.2 >= Utility.RandomDouble() )
				SpawnWraiths( target );
		}

		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			DoSpecialAbility( defender );

			defender.Damage( Utility.Random( 20, 10 ), this );
			defender.Stam -= Utility.Random( 20, 10 );
			defender.Mana -= Utility.Random( 20, 10 );
		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			DoSpecialAbility( attacker );

		}

		public BlackLich( Serial serial ) : base( serial )
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