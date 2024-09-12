using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki wielkiej ropuchy" )]
	[TypeAlias( "Server.Mobiles.Sandfrog" )]
	public class SandFrog : BaseCreature
	{
		[Constructable]
		public SandFrog() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "pustynna ropucha";
			Body = 81;
			Hue = Utility.RandomList( 1452,1448,1446,1169,1145 );
			BaseSoundID = 0x266;

			SetStr( 46, 70 );
			SetDex( 6, 25 );
			SetInt( 11, 20 );

			SetHits( 28, 42 );
			SetMana( 0 );

			SetDamage( 2, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 5, 10 );

			SetSkill( SkillName.ObronaPrzedMagia, 25.1, 40.0 );
			SetSkill( SkillName.Taktyka, 40.1, 60.0 );
			SetSkill( SkillName.Boks, 40.1, 60.0 );

			Fame = 350;
			Karma = 100;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 50.1;
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Poor );
		}
                public override Poison PoisonImmune{ get{ return Poison.Greater; } }
		public override Poison HitPoison{ get{ return (0.8 >= Utility.RandomDouble() ? Poison.Lesser : Poison.Greater); } }
		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 1; } }
		public override FoodType FavoriteFood{ get{ return FoodType.Fish | FoodType.Meat; } }

		public SandFrog(Serial serial) : base(serial)
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
	}
}