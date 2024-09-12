using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13F8, 0x13F9 )]
	public class DruidStaff : BaseStaff
	{
		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ConcussionBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }

		public override int AosStrengthReq{ get{ return 20; } }
		public override int AosMinDamage{ get{ return 25; } }
		public override int AosMaxDamage{ get{ return 37; } }
		public override int AosSpeed{ get{ return 33; } }
		public override float MlSpeed{ get{ return 3.25f; } }

		public override int OldStrengthReq{ get{ return 20; } }
		public override int OldMinDamage{ get{ return 10; } }
		public override int OldMaxDamage{ get{ return 30; } }
		public override int OldSpeed{ get{ return 33; } }

	//	public override int InitMinHits{ get{ return 31; } }
	//	public override int InitMaxHits{ get{ return 50; } }

		[Constructable]
		public DruidStaff() : base( 0x13F8 )
		{
            Name = "Druid Staff";
			Weight = 3.0;
            WeaponAttributes.HitLeechStam = Utility.RandomMinMax(7, 20);
            Attributes.SpellChanneling = 1;
            Attributes.WeaponDamage = Utility.RandomMinMax(5, 15);
            Attributes.WeaponSpeed = Utility.RandomMinMax(7, 16);
            Attributes.ReflectPhysical = Utility.RandomMinMax(12, 19);
            WeaponAttributes.HitHarm = Utility.RandomMinMax(9, 14);
            WeaponAttributes.HitLightning = Utility.RandomMinMax(8, 16);
		}

		public DruidStaff( Serial serial ) : base( serial )
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
		}
	}
}