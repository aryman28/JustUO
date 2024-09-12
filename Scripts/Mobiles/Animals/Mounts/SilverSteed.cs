using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName( "a silver steed corpse" )]
	public class SilverSteed : BaseMount
	{
		[Constructable]
		public SilverSteed() : this( "a silver steed" )
		{
		}

		[Constructable]
		public SilverSteed( string name ) : base( name, 228, 0x3F12, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4 )
		{
			InitStats( Utility.Random( 50, 30 ), Utility.Random( 50, 30 ), 10 );
			Skills[SkillName.ObronaPrzedMagia].Base = 25.0 + (Utility.RandomDouble() * 5.0);
			Skills[SkillName.Boks].Base = 35.0 + (Utility.RandomDouble() * 10.0);
			Skills[SkillName.Taktyka].Base = 30.0 + (Utility.RandomDouble() * 15.0);

			Hue = 1901;
			ControlSlots = 1;
			Tamable = true;
			MinTameSkill = 103.1;
		}

		public SilverSteed( Serial serial ) : base( serial )
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