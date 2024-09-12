using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	//[FlipableAttribute( 0x13B2, 0x13B1 )]
	public class OrcLongBow : BaseRanged
	{
		public override int EffectID{ get{ return 0xF42; } }
		public override Type AmmoType{ get{ return typeof( Arrow ); } }
		public override Item Ammo{ get{ return new Arrow(); } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.MovingShot; } }
		
		public override int AosMinDamage{ get{ return 25; } }
		public override int AosMaxDamage{ get{ return 35; } }
		public override int AosSpeed{ get{ return 40; } }

		public override int OldMinDamage{ get{ return 9; } }
		public override int OldMaxDamage{ get{ return 41; } }
		public override int OldSpeed{ get{ return 20; } }

		public override int DefMaxRange{ get{ return 13; } }

		public override int InitMinHits{ get{ return 31; } }
		public override int InitMaxHits{ get{ return 60; } }

		public override WeaponAnimation DefAnimation{ get{ return WeaponAnimation.ShootBow; } }

		[Constructable]
		public OrcLongBow() : base( 0x26C2 )
		{
			Weight = 6.0;
			Layer = Layer.TwoHanded;
			Name = "an orcish long bow";
			StrRequirement = 80;
			Attributes.BonusDex = 20;
			Attributes.WeaponDamage = 35;
			Hue = 0x33;
                  Slayer=SlayerName.Repond;
		}

		public override bool OnFired( Mobile attacker, Mobile defender )
		{
			Container pack = attacker.Backpack;

			if ( attacker.Player && (pack == null || !pack.ConsumeTotal( AmmoType, 2 )) )
				return false;

			attacker.MovingEffect( defender, EffectID, 18, 1, false, false );

			return true;
		}

            public OrcLongBow( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			if ( Weight == 7.0 )
				Weight = 6.0;
		}
	}
}