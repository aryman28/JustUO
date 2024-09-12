using System;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "zwloki szakala" )]
	public class Szakal : BaseCreature
	{
		public TimeSpan Duration{ get{ return TimeSpan.FromMinutes( 1.0 ); } }
                public override TimeSpan ReacquireDelay{ get{ return TimeSpan.FromSeconds( 1.0 ); } }

		public static int skala( Mobile m, int v )
		{
			if ( !Core.AOS )
				return v;

			return AOS.Scale( v, 100  );
		}

		[Constructable]
		public Szakal() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.4, 0.6 ) //Szybszy niz zwykly pies (0.2 , 0.4)
		{
			Name = "szakal";
			Body = 97;
			Hue = Utility.RandomList( 653, 654, 655 );
			BaseSoundID = 0x85;

			SetStr( 130, 150 );
			SetDex( 90, 120 );
			SetInt( 50, 50 );

			SetHits( 150, 200 );
			SetMana( 0 );

			SetDamage( 10, 15 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 30, 40 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 30, 40 );
			SetResistance( ResistanceType.Poison, 30, 40 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.ObronaPrzedMagia, 45.1, 60.0 );
			SetSkill( SkillName.Taktyka, 45.1, 70.0 );
			SetSkill( SkillName.Boks, 55.1, 75.0 );

			Fame = 0;
			Karma = 500;

			VirtualArmor = 30;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 80.0;
			
			PackItem( new Bone(1) );
		}

		public override int Meat{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Meat; } }
		public override PackInstinct PackInstinct{ get{ return PackInstinct.Canine; } }

		public Szakal(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int) 0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
		
		
		public bool DoStrength( Mobile from )
		{
			// TODO: Verify scaled; is it offset, duration, or both?
			if ( Spells.SpellHelper.AddStatOffset( from, StatType.Str, skala( from, 100 ), Duration ) )
			{
				PublicOverheadMessage( MessageType.Regular, 0x3B2, true, "* szakal robi sie agresywniejszy *" );
				from.FixedEffect( 0x375A, 10, 15 );
				from.PlaySound( 0x1E7 );
				return true;
			}

			//from.SendLocalizedMessage( 502173 ); // You are already under a similar effect.
			return false;
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( !willKill )
			{
	
	
					if ( Hits < 150 )
					{
						DoStrength(this);
					}

				
			}
		}

	}
}