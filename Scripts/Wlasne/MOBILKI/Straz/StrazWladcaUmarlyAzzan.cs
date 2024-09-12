using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;


namespace Server.Mobiles
{
	[CorpseName( "zwloki nekromanty" )]
	[TypeAlias( "Server.Mobiles.nekrus" )]
	public class StrazWladcaUmarlyAzzan : BaseCreature
	{
		private ArrayList m_pups;
		int pupCount = Utility.RandomMinMax( 1, 4 );

                public override bool AutoDispel{ get{ return true; } }
		public override bool Unprovokable{ get{ return true; } }
		public override bool Uncalmable{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		//public override bool NoHouseRestrictions { get { return true; } }
                public override bool ReacquireOnMovement{ get{ return true; } }
                public override bool AlwaysMurderer{ get{ return true; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool RespawnPups
		{
			get{ return false; }
			set{ if( value ) SpawnBabies(); }
		}

		[Constructable]
		public StrazWladcaUmarlyAzzan() : base( AIType.AI_Necro, FightMode.Blue, 10, 1, 0.015, 0.01 )
		{

			SetStr( 70 );
			SetDex( 50 );
			SetInt( 100, 200 );

			SetHits( 200 );

			SetDamage( 20, 25 );

                        Fame = 10000;
			Karma = -10000;

			SpeechHue = Utility.RandomDyedHue();

			SetResistance( ResistanceType.Physical, 60, 80 );
			SetResistance( ResistanceType.Fire, 50, 60 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 65, 70 );
			SetResistance( ResistanceType.Energy, 50, 65 );


			Hue = Utility.RandomSkinHue();


				Body = 0x190;
				Name = NameList.RandomName( "shadow knight" ); //male ,Imiona Rycerzy cienia
                                Kills = 50;
				Title = "- Wladca Umarlych";

				Cloak cloak = new Cloak(); 
				cloak.Movable = false;
				cloak.Hue = 1109;
				cloak.LootType = LootType.Blessed;
				AddItem ( cloak );
								
				BoneChest chest = new BoneChest(); 
				chest.Hue = 1150; 
				chest.Movable = false;
				AddItem( chest ); 

				BoneGloves gloves = new BoneGloves(); 
				gloves.Hue = 1150; 
				gloves.Movable = false;
				AddItem( gloves ); 

				BoneHelm helm = new BoneHelm(); 
				helm.Hue = 1150; 
				helm.Movable = false;
				AddItem( helm ); 

				BoneArms arms = new BoneArms(); 
				arms.Hue = 1150; 
				arms.Movable = false;
				AddItem( arms ); 

				BoneLegs legs = new BoneLegs(); 
				legs.Hue = 1150;
				legs.Movable = false; 
				AddItem( legs ); 

				Scythe weapon = new Scythe();
				weapon.Movable = false;
				weapon.Hue = 1109;
				weapon.WeaponAttributes.HitDispel = 100;
				weapon.Quality = WeaponQuality.Doskona³; 
				weapon.LootType = LootType.Blessed;
				AddItem( weapon );



			Container pack = new Backpack();

			pack.Movable = false;
			pack.Hue = 1190;

			pack.DropItem( new Gold( 100, 400 ) );

			AddItem( pack );

			Skills[SkillName.Anatomia].Base = 120.0;
			Skills[SkillName.Taktyka].Base = 100.0;
			Skills[SkillName.WalkaMieczami].Base = 120.0;
			Skills[SkillName.ObronaPrzedMagia].Base = 60.0;
      Skills[SkillName.Parowanie].Base = 100.0;
			Skills[SkillName.Logistyka].Base = 100.0;
			Skills[SkillName.Nekromancja].Base = 100.0;
			Skills[SkillName.MowaDuchow].Base = 100.0;
             
                        m_pups = new ArrayList();
			Timer m_timer = new UndeadTimer( this );
			m_timer.Start();
		}

		public override bool OnBeforeDeath()
		{	
			foreach( Mobile m in m_pups )
			{	
				if( m is DeadPup && m.Alive && ((DeadPup)m).ControlMaster != null )
					m.Kill();
			}
			
			return base.OnBeforeDeath();
		}
		
		public void SpawnBabies()
		{

			Defrag();
			int family = m_pups.Count;

			if( family >= pupCount )
				return;

			//Say( "family {0}, should be {1}", family, pupCount );

			Map map = this.Map;

			if ( map == null )
				return;

			int hr = (int)((this.RangeHome + 1) / 2);

			for ( int i = family; i < pupCount; ++i )
			{
				DeadPup pup = new DeadPup();

				bool validLocation = false;
				Point3D loc = this.Location;

				for ( int j = 0; !validLocation && j < 10; ++j )
				{
					int x = X + Utility.Random( 5 ) - 1;
					int y = Y + Utility.Random( 5 ) - 1;
					int z = map.GetAverageZ( x, y );

					if ( validLocation = map.CanFit( x, y, this.Z, 16, false, false ) )
						loc = new Point3D( x, y, Z );
					else if ( validLocation = map.CanFit( x, y, z, 16, false, false ) )
						loc = new Point3D( x, y, z );
				}

				pup.Mother = this;
				pup.Team = this.Team;
				pup.Home = this.Location;
				pup.RangeHome = ( hr > 6 ? 6 : hr );
				
				pup.MoveToWorld( loc, map );
				m_pups.Add( pup );
			}
		}

		protected override void OnLocationChange( Point3D oldLocation )
		{

			try
			{
				foreach( Mobile m in m_pups )
				{	
					if( m is DeadPup && m.Alive && ((DeadPup)m).ControlMaster == null )
					{
						((DeadPup)m).Home = this.Location;
					}
				}
			}
			catch{}

			base.OnLocationChange( oldLocation );
		}
		
		public void Defrag()
		{
			for ( int i = 0; i < m_pups.Count; ++i )
			{
				try
				{
					object o = m_pups[i];

					DeadPup pup = o as DeadPup;

					if ( pup == null || !pup.Alive )
					{
						m_pups.RemoveAt( i );
						--i;
					}

					else if ( pup.Controlled || pup.IsStabled )
					{
						pup.Mother = null;
						m_pups.RemoveAt( i );
						--i;
					}
				}
				catch{}
			}
		}

		public override void OnDelete()
		{
			Defrag();

			foreach( Mobile m in m_pups )
			{	
				if( m.Alive && ((DeadPup)m).ControlMaster == null )
					m.Delete();
			}

			base.OnDelete();
		}

		public StrazWladcaUmarlyAzzan(Serial serial) : base(serial)
		{
			m_pups = new ArrayList();
			Timer m_timer = new UndeadTimer( this );
			m_timer.Start();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
			writer.WriteMobileList( m_pups, true );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_pups = reader.ReadMobileList();
		}

	[CorpseName( "smierdzace flaki" )]
	public class DeadPup : BaseCreature
	{
		public override bool BleedImmune{ get{ return true; } }
                public override Poison PoisonImmune{ get{ return Poison.Greater; } }

		private StrazWladcaUmarlyAzzan m_mommy;

		[CommandProperty( AccessLevel.GameMaster )]
		public StrazWladcaUmarlyAzzan Mother
		{
			get{ return m_mommy; }
			set{ m_mommy = value; }
		}

		[Constructable]
		public DeadPup() : base( AIType.AI_Mage, FightMode.Blue, 10, 1, 0.2, 0.4 )
		{

                        Name = "licz";
			Body = 24;
			BaseSoundID = 0x3E9;
                        Kills = 50;

			SetStr( 171, 200 );
			SetDex( 126, 145 );
			SetInt( 276, 305 );

			SetHits( 103, 120 );

			SetDamage( 24, 26 );

			SetDamageType( ResistanceType.Physical, 10 );
			SetDamageType( ResistanceType.Cold, 40 );
			SetDamageType( ResistanceType.Energy, 50 );

			SetResistance( ResistanceType.Physical, 40, 60 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 55, 65 );
			SetResistance( ResistanceType.Energy, 40, 50 );

			SetSkill( SkillName.Intelekt, 100.0 );
			SetSkill( SkillName.Magia, 70.1, 80.0 );
			SetSkill( SkillName.Medytacja, 85.1, 95.0 );
			SetSkill( SkillName.ObronaPrzedMagia, 80.1, 100.0 );
			SetSkill( SkillName.Taktyka, 70.1, 90.0 );

			Fame = 8000;
			Karma = -8000;
                }
		public override void OnCombatantChange()
		{
			if( Combatant != null && Combatant.Alive && m_mommy != null && m_mommy.Combatant == null )
				m_mommy.Combatant = Combatant;
		}

		public DeadPup(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
			writer.Write( m_mommy );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_mommy = (StrazWladcaUmarlyAzzan)reader.ReadMobile();
		}
	}

	public class UndeadTimer : Timer
	{
		private StrazWladcaUmarlyAzzan m_from;

		public UndeadTimer( StrazWladcaUmarlyAzzan from  ) : base( TimeSpan.FromMinutes( 1 ), TimeSpan.FromMinutes( 20 ) )
		{
			Priority = TimerPriority.OneMinute; 
			m_from = from;
		}

		protected override void OnTick()
		{
               
               m_from.PlaySound( 0x1FB );

			if ( m_from != null && m_from.Alive )
				m_from.SpawnBabies();
			else
				Stop();
		}
	}
}
}