using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "zwloki szkieletu" )]
	public class FireSkeleton : BaseCreature
	{
		[Constructable]
		public FireSkeleton() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "planacy szkielet";
			this.Body = 147;
            
			this.BaseSoundID = 0x48D;
                        Hue = 1259;

			SetStr( 56, 80 );
			SetDex( 56, 75 );
			SetInt( 16, 40 );

			SetHits( 60, 70 );

			SetDamage( 5, 7 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 15, 20 );
			SetResistance( ResistanceType.Fire, 90, 100 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 25, 35 );
			SetResistance( ResistanceType.Energy, 5, 15 );

			SetSkill( SkillName.ObronaPrzedMagia, 65.1, 80.0 );
			SetSkill( SkillName.Taktyka, 45.1, 50.0 );
			SetSkill( SkillName.Boks, 25.1, 35.0 );
			Fame = 450;
			Karma = -450;

			VirtualArmor = 16;
                        AddItem( new LightSource() );

			switch ( Utility.Random( 5 ))
			{
				case 0: PackItem( new BoneArms() ); break;
				case 1: PackItem( new BoneChest() ); break;
				case 2: PackItem( new BoneGloves() ); break;
				case 3: PackItem( new BoneLegs() ); break;
				case 4: PackItem( new BoneHelm() ); break;
			}
		}

                public override void OnGotMeleeAttack( Mobile attacker )
		{

if ( InRange( attacker, 2 ) )
{
                  if ( attacker.Skills[SkillName.ObronaPrzedMagia].Value >= 50 )
		  {
                           attacker.Hits -=2;

                       Effects.SendLocationEffect( new Point3D( X + 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y + 1, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X - 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y - 1, Z + 6 ), Map, 0x3709, 15 );

                       Effects.PlaySound( new Point3D( X, Y, Z ), Map, 0x208 );
		  }

		       else			
                       {
                       attacker.Hits -=8;

                       Effects.SendLocationEffect( new Point3D( X + 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y + 1, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X - 1, Y, Z + 6 ), Map, 0x3709, 15 );
                       Effects.SendLocationEffect( new Point3D( X, Y - 1, Z + 6 ), Map, 0x3709, 15 );

                       Effects.PlaySound( new Point3D( X, Y, Z ), Map, 0x208 );

			base.OnGotMeleeAttack( attacker );
                        }	
		  }
}		
		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}

		public override bool BleedImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lesser; } }

		public FireSkeleton( Serial serial ) : base( serial )
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
