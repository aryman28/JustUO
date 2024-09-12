using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0x13F8, 0x13F9 )]
	public class ArchDruidStaff : BaseStaff
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

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public ArchDruidStaff() : base( 0x13F8 )
		{
            Name = "Staff of the ArchDruid";
			Weight = 3.0;
            LootType = LootType.Blessed;
            WeaponAttributes.HitLeechStam = Utility.RandomMinMax(7, 20);
            Attributes.SpellChanneling = 1;
            Attributes.WeaponDamage = Utility.RandomMinMax(5, 15);
            Attributes.WeaponSpeed = Utility.RandomMinMax(7, 16);
            Attributes.ReflectPhysical = Utility.RandomMinMax(12, 19);
            WeaponAttributes.HitHarm = Utility.RandomMinMax(9, 14);
            WeaponAttributes.HitLightning = Utility.RandomMinMax(8, 16);
            WeaponAttributes.UseBestSkill = 1;
            WeaponAttributes.HitLeechStam = Utility.RandomMinMax(8, 16);
            WeaponAttributes.HitPhysicalArea = 10;
            WeaponAttributes.ResistPhysicalBonus = 20;
            WeaponAttributes.SelfRepair = 5;

            Attributes.NightSight = 1;
            Attributes.RegenHits = 10;
            Attributes.RegenStam = 15;

            Attributes.AttackChance = Utility.RandomMinMax(6, 23);
            Attributes.DefendChance = Utility.RandomMinMax(8, 20);
            Attributes.Luck = 200;
            Attributes.CastSpeed = 10;
            Attributes.CastRecovery = 8;
            Attributes.LowerManaCost = 20;
            Attributes.LowerRegCost = 15;
		}

        public ArchDruidStaff(Serial serial)
            : base(serial)
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