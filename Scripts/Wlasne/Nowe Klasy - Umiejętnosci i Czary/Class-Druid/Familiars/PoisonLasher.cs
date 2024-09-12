using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName( "a poison lasher corpse" )]
	public class PoisonLasher : BaseFamiliar
	{
		public PoisonLasher()
		{
			Name = "a poison lasher";
			Body = 8;
			Hue = 0x851;
			BaseSoundID = 352;

			SetStr( 70 );
			SetDex( 150 );
			SetInt( 100 );

			SetHits( 50 );
			SetStam( 128 );
			SetMana( 0 );

			SetDamage( 1, 6 );
			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 10 );
			SetResistance( ResistanceType.Poison, 100 );

			SetSkill( SkillName.Boks, 90.0 );
			SetSkill( SkillName.Taktyka, 50.0 );
			SetSkill( SkillName.ObronaPrzedMagia, 100.0 );
			SetSkill( SkillName.Zatruwanie, 150.0 );

			ControlSlots = 1;
		}

		public override Poison HitPoison{ get{ return (0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }

		public PoisonLasher( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}