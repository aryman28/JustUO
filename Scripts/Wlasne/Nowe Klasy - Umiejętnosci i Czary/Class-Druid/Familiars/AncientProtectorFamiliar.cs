using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName( "a treefellow corpse" )]
	public class SummonedTreefellow : BaseFamiliar
	{
		public SummonedTreefellow()
		{
			Name = "an ancient protector";
			Body = 301;
			BaseSoundID = 442;
			
			SetStr( 95 );
			SetDex( 55 );
			SetInt( 90 );

			SetMana( 20 );
			SetHits( 1000 );
			SetStam( 100 );
			
			SetDamage( 18, 30 );
			
			SetDamageType( ResistanceType.Physical, 100 );
			
			SetResistance( ResistanceType.Physical, 70, 85 );
			SetResistance( ResistanceType.Cold, 50, 60 );
			SetResistance( ResistanceType.Poison, 78, 90 );
			SetResistance( ResistanceType.Energy, 20, 30 );
			
			SetSkill( SkillName.ObronaPrzedMagia, 65.0 );
			SetSkill( SkillName.Taktyka, 100.0 );
			SetSkill( SkillName.Boks, 90.0 );
			
			VirtualArmor = 80;
			ControlSlots = 2;
		}
		
		public SummonedTreefellow( Serial serial ) : base( serial )
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
