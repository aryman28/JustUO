using System;
using System.Collections.Generic;
using Server;

namespace Server.Mobiles
{
	public class Druid : BaseVendor
	{
        private List<SBInfo> m_SBInfos = new List<SBInfo>();
        protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

		

		[Constructable]
		public Druid() : base( "the druid initiate" )
		{
			SetSkill(SkillName.WiedzaOBestiach, 95.0, 120.0);
            SetSkill(SkillName.Medytacja, 95.0, 100.0);
            SetSkill(SkillName.ObronaPrzedMagia, 100.0, 120.0);
            SetSkill(SkillName.Taktyka, 95.0, 120.0);
            SetSkill(SkillName.Boks, 95.0, 120.0);
			
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add( new SBDruid() );
		}

		


		public override void InitOutfit()
		{
			AddItem( new Server.Items.Robe( 0xB0 ) );
		        AddItem( new Server.Items.WildStaff() );
                  	AddItem( new Server.Items.Sandals() );
            }

		public Druid( Serial serial ) : base( serial )
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