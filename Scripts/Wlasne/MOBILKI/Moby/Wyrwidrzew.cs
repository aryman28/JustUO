using System;
using System.Collections;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wyrwidrzewa" )]
	public class Wyrwidrzew : BaseCreature
	{
		[Constructable]
		public Wyrwidrzew() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Wyrwidrzew";
			Body = 76;
			BaseSoundID = 604;
			Hue = 541;

			SetStr( 336, 385 );
			SetDex( 96, 115 );
			SetInt( 31, 55 );

			SetHits( 202, 231 );
			SetMana( 0 );

			SetDamage( 7, 23 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 45, 50 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 25, 35 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.ObronaPrzedMagia, 65.1, 80.0 );
			SetSkill( SkillName.Taktyka, 85.1, 100.0 );
			SetSkill( SkillName.Boks, 85.1, 95.0 );

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 48;
			PackItem( new Log( Utility.RandomMinMax( 1, 5 ) ) );

if ( Utility.RandomDouble() < 0.1 )
{
		PackItem( new OakLog( Utility.RandomMinMax( 1, 4 ) ) );
}
else if ( Utility.RandomDouble() < 0.05 )
{
		PackItem( new FrostwoodLog( Utility.RandomMinMax( 1, 3 ) ) );
}
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override void OnDamagedBySpell( Mobile from )
		{
			if( from != null && from.Alive && 0.4 > Utility.RandomDouble() )
			{
				SendEBolt( from );
			}

		}

		public override void OnGotMeleeAttack( Mobile attacker )
		{
			base.OnGotMeleeAttack( attacker );

			if( attacker != null && attacker.Alive && attacker.Weapon is BaseRanged && 0.4 > Utility.RandomDouble() )
			{
				SendEBolt( attacker );
			}
		}

		public void SendEBolt( Mobile to )
		{
			this.DoHarmful( to );
			AOS.Damage( to, this, 50, 0, 0, 0, 0, 100 );
		}

		private DateTime m_NextBomb;
		private int m_Thrown;

		public override void OnActionCombat()
		{
			Mobile combatant = Combatant;

			if ( combatant == null || combatant.Deleted || combatant.Map != Map || !InRange( combatant, 12 ) || !CanBeHarmful( combatant ) || !InLOS( combatant ) )
				return;

			if ( DateTime.Now >= m_NextBomb )
			{
				ThrowBomb( combatant );

				m_Thrown++;

				if ( 0.75 >= Utility.RandomDouble() && (m_Thrown % 2) == 1 ) // 75% chance to quickly throw another bomb
					m_NextBomb = DateTime.Now + TimeSpan.FromSeconds( 3.0 );
				else
					m_NextBomb = DateTime.Now + TimeSpan.FromSeconds( 5.0 + (10.0 * Utility.RandomDouble()) ); // 5-15 seconds
			}
		}

		public void ThrowBomb( Mobile m )
		{

			DoHarmful( m );

			this.MovingParticles( m, 0x0CCB, 10, 3, false, false, 0, 0, 0, 0, 0, EffectLayer.Waist, 0 ); //m, grafika rzucanego obiektu, 1, 0, true, true, 0, 0, ilosc urzucanych obiektow na ture, 6014, 0x4D, EffectLayer.Waist, 0
			//MovingParticles( IEntity to, int itemID, int speed, int duration, bool fixedDirection, bool explodes, int hue, int renderMode, int effect, int explodeEffect, int explodeSound, EffectLayer layer, int unknown )

			if ( m.Player && m.Mounted && m.AccessLevel == AccessLevel.Player )
			{
			IMount mount = (IMount)m.Mount;
			mount.Rider = null;
			}

			new InternalTimer( m, this ).Start();
		}

		private class InternalTimer : Timer
		{
			private Mobile m_Mobile, m_From;

			public InternalTimer( Mobile m, Mobile from ) : base( TimeSpan.FromSeconds( 1.0 ) )
			{
				m_Mobile = m;
				m_From = from;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{

				m_Mobile.Emote ("*uderzenie drzewem*");
				m_Mobile.PlaySound( 0x149 ); //dzwiek trafienia
				AOS.Damage( m_Mobile, m_From, Utility.RandomMinMax( 10, 20 ), 0, 100, 0, 0, 0 );
			}
		}

		public override int Meat{ get{ return 4; } }
		public override int TreasureMapLevel{ get{ return 3; } }

		public Wyrwidrzew( Serial serial ) : base( serial )
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