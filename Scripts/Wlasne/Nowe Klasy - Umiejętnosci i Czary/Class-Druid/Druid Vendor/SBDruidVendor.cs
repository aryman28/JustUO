using System;
using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
public class SBDruidVendor : SBInfo
{
        private List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBDruidVendor()
		{
		}

        public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
        public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }

        public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{ 
				Add( new GenericBuyInfo( "Forest Kin", typeof( ForestKinScroll ), 1500, 999, 0xE39, 1419 ) );
				Add( new GenericBuyInfo( "Lure Stone", typeof( LureStoneScroll ), 1500, 999, 0xE39, 1419 ) );
				Add( new GenericBuyInfo( "Shield of Earth", typeof( ShieldOfEarthScroll ), 1500, 999, 0xE39, 1419 ) );
				//Add( new GenericBuyInfo( "Swarm of Insects", typeof( SwarmOfInsectsScroll ), 1500, 999, 0xE39, 1419 ) );

                Add(new GenericBuyInfo("Petrified Wood", typeof(PetrifiedWood), 10, 999, 0x97A, 0x46C));
				Add( new GenericBuyInfo( "Spring Water", typeof( SpringWater ), 10, 999, 0xE24, 0x47F ) );
				Add( new GenericBuyInfo( "Pumice", typeof( Pumice ), 10, 999, 0xF8B, 0x290 ) );
				
				Add( new GenericBuyInfo( "Tome of Nature", typeof( DruidSpellbook ), 60000, 999, 0xEFA, 1164 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
                Add(typeof(PetrifiedWood), 3); 
				Add( typeof( SpringWater ), 3 ); 
				Add( typeof( Pumice ), 3 ); 
				Add( typeof( FertileDirt ), 3 ); 
				Add( typeof( FenMoss ), 3 ); 
				
				//Add( typeof( BagOfReagents ), 1150 );
				//Add( typeof( BagOfNecroReagents ), 1150 );
				//Add( typeof( BagOfDruidReagents ), 1150 );

			}
		}
	}
}
