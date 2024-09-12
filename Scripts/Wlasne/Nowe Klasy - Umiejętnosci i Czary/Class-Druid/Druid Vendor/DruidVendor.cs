using System;
using System.Collections.Generic;
using Server;

namespace Server.Mobiles
{
	public class DruidVendor : BaseVendor
	{
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		public override NpcGuild NpcGuild{ get{ return NpcGuild.MagesGuild; } }

		[Constructable]
		public DruidVendor() : base( "the druid" )
		{
			SetSkill( SkillName.WiedzaOBestiach, 75.0, 100.0 );
			SetSkill( SkillName.Oswajanie, 75.0, 100.0 );
			SetSkill( SkillName.Intelekt, 65.0, 88.0 );
			SetSkill( SkillName.Magia, 64.0, 100.0 );
			SetSkill( SkillName.Medytacja, 60.0, 83.0 );
			SetSkill( SkillName.Boks, 36.0, 68.0 );
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBDruidVendor() );
		}

		public override VendorShoeType ShoeType
		{
			get{ return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals; }
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			AddItem( new Server.Items.Robe( Utility.RandomGreenHue() ) );
			AddItem( new Server.Items.ShepherdsCrook() );
		}

		public DruidVendor( Serial serial ) : base( serial )
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
