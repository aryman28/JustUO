using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "zwloki dromadera" )]
	public class DzikiDromader : BaseCreature
	{
		[Constructable]
		public DzikiDromader() : base( AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			Name = "dziki dromader";
			Body = 0xDC;
			BaseSoundID = 0x3F3;

			SetStr( 21, 49 );
			SetDex( 56, 75 );
			SetInt( 16, 30 );

			SetHits( 15, 27 );
			SetMana( 0 );

			SetDamage( 3, 5 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10, 15 );
			SetResistance( ResistanceType.Fire, 5, 10 );
			SetResistance( ResistanceType.Cold, 5, 10 );
			SetResistance( ResistanceType.Poison, 5, 10 );
			SetResistance( ResistanceType.Energy, 5, 10 );

			SetSkill( SkillName.ObronaPrzedMagia, 15.1, 20.0 );
			SetSkill( SkillName.Taktyka, 19.2, 29.0 );
			SetSkill( SkillName.Boks, 19.2, 29.0 );

			Fame = 300;
			Karma = 0;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 35.1;
		        Hue = Utility.RandomList( 348,448,1356,1256 );
		}

		public override int Meat{ get{ return 1; } }
		public override int Hides{ get{ return 5; } }
		public override FoodType FavoriteFood{ get{ return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public DzikiDromader(Serial serial) : base(serial)
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