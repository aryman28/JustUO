using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "puddle of spring water" )]
	public class WaterSpiritFamiliar : BaseFamiliar
	{
		public override double DispelDifficulty{ get{ return 75.0; } }
		public override double DispelFocus{ get{ return 45.0; } }

		public WaterSpiritFamiliar()
		{
			Name = "a water spirit";
			Body = 16;
			BaseSoundID = 278;

			SetStr( 120 );
			SetDex( 100 );
			SetInt( 25 );

			SetHits( 120 );

			SetDamage( 10, 14 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Poison, 25 );

			SetResistance( ResistanceType.Physical, 45, 55 );
			SetResistance( ResistanceType.Fire, 30, 40 );
			SetResistance( ResistanceType.Cold, 40, 50 );
			SetResistance( ResistanceType.Poison, 60, 70 );
			SetResistance( ResistanceType.Energy, 70, 80 );

			SetSkill( SkillName.Intelekt, 90.1, 100.0 );
			SetSkill( SkillName.Medytacja, 90.1, 100.0 );
			SetSkill( SkillName.ObronaPrzedMagia, 80.1, 90.0 );
			SetSkill( SkillName.Taktyka, 100.0 );
			SetSkill( SkillName.Boks, 98.1, 99.0 );

			VirtualArmor = 20;
			ControlSlots = 3;
		}

		public override Poison PoisonImmune{ get{ return Poison.Deadly; } } // TODO: Immune to poison?

		public WaterSpiritFamiliar( Serial serial ) : base( serial )
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