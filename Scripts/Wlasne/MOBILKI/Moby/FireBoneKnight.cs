using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "zwloki koscianego rycerza" )]
	public class FireBoneKnight : BaseCreature
	{
		[Constructable]
		public FireBoneKnight() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "plonacy kosciany rycerz";
			Body = 57;
			BaseSoundID = 451;
                        Hue = 1259;

			SetStr( 196, 250 );
			SetDex( 76, 95 );
			SetInt( 36, 60 );

			SetHits( 130, 170 );

			SetDamage( 8, 18 );

			SetDamageType( ResistanceType.Physical, 40 );
			SetDamageType( ResistanceType.Cold, 60 );

			SetResistance( ResistanceType.Physical, 35, 45 );
			SetResistance( ResistanceType.Fire, 20, 30 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 20, 30 );
			SetResistance( ResistanceType.Energy, 30, 40 );

			SetSkill( SkillName.ObronaPrzedMagia, 65.1, 80.0 );
			SetSkill( SkillName.Taktyka, 85.1, 100.0 );
			SetSkill( SkillName.Boks, 85.1, 95.0 );

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;
			AddItem( new LightSource() );
			
			switch ( Utility.Random( 6 ) )
			{
				case 0: PackItem( new PlateArms() ); break;
				case 1: PackItem( new PlateChest() ); break;
				case 2: PackItem( new PlateGloves() ); break;
				case 3: PackItem( new PlateGorget() ); break;
				case 4: PackItem( new PlateLegs() ); break;
				case 5: PackItem( new PlateHelm() ); break;
			}

			PackSlayer();
			PackItem( new Scimitar() );
			PackItem( new WoodenShield() );
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Average );
			AddLoot( LootPack.Meager );
		}

                public override void OnGotMeleeAttack( Mobile attacker )
		{

if ( InRange( attacker, 2 ) )
{
                  if ( attacker.Skills[SkillName.ObronaPrzedMagia].Value >= 50 )
		  {
                           attacker.Hits -=3;

                       Effects.SendLocationEffect( new Point3D( X + 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y + 1, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X - 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y - 1, Z + 6 ), Map, 0x3709, 15 );

                       Effects.PlaySound( new Point3D( X, Y, Z ), Map, 0x208 );
		  }

		       else			
                       {
                       attacker.Hits -=9;

                       Effects.SendLocationEffect( new Point3D( X + 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y + 1, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X - 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y - 1, Z + 6 ), Map, 0x3709, 15 );

                       Effects.PlaySound( new Point3D( X, Y, Z ), Map, 0x208 );

			base.OnGotMeleeAttack( attacker );
                        }	
		  }
}		

		public override bool BleedImmune{ get{ return true; } }

		public FireBoneKnight( Serial serial ) : base( serial )
		{
		}

		public override OppositionGroup OppositionGroup
		{
			get{ return OppositionGroup.FeyAndUndead; }
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