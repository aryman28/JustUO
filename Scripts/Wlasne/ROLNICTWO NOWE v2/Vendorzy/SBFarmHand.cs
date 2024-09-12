using System;
using System.Collections.Generic;
using Server.Items;
using Server.Items.Crops;
using Server.ACC.YS;

namespace Server.Mobiles
{
    public class SBFarmHand : SBInfo
    {
        private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();
        private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();
        public SBFarmHand()
        {
        }

        public override IShopSellInfo SellInfo
        {
            get
            {
                return this.m_SellInfo;
            }
        }
        public override List<GenericBuyInfo> BuyInfo
        {
            get
            {
                return this.m_BuyInfo;
            }
        }

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            { 
                                //Add( new GenericBuyInfo( typeof( Apple ), 20, 20, 0x9D0, 0 ) );
				//Add( new GenericBuyInfo( typeof( Grapes ), 20, 20, 0x9D1, 0 ) );
				//Add( new GenericBuyInfo( typeof( Watermelon ), 25, 20, 0xC5C, 0 ) );
				//Add( new GenericBuyInfo( typeof( YellowGourd ), 30, 20, 0xC64, 0 ) );
				//Add( new GenericBuyInfo( typeof( Pumpkin ), 40, 20, 0xC6A, 0 ) );
				//Add( new GenericBuyInfo( typeof( Onion ), 20, 20, 0xC6D, 0 ) );
				//Add( new GenericBuyInfo( typeof( Lettuce ), 15, 20, 0xC70, 0 ) );
				//Add( new GenericBuyInfo( typeof( Squash ), 30, 20, 0xC72, 0 ) );
				//Add( new GenericBuyInfo( typeof( HoneydewMelon ), 50, 20, 0xC74, 0 ) );
				//Add( new GenericBuyInfo( typeof( Carrot ), 30, 20, 0xC77, 0 ) );
				//Add( new GenericBuyInfo( typeof( Cantaloupe ), 25, 20, 0xC79, 0 ) );
				//Add( new GenericBuyInfo( typeof( Cabbage ), 20, 20, 0xC7B, 0 ) );
				//Add( new GenericBuyInfo( typeof( EarOfCorn ), 3, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( Turnip ), 6, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( SheafOfHay ), 2, 20, XXXXXX, 0 ) );
				//Add( new GenericBuyInfo( typeof( Lemon ), 20, 20, 0x1728, 0 ) );
				//Add( new GenericBuyInfo( typeof( Lime ), 20, 20, 0x172A, 0 ) );
				//Add( new GenericBuyInfo( typeof( Peach ), 20, 20, 0x9D2, 0 ) );
				//Add( new GenericBuyInfo( typeof( Pear ), 20, 20, 0x994, 0 ) );

				this.Add( new GenericBuyInfo( "Nasiona marchwi", typeof( CarrotSeed ), 150, 10, 0xF27, 0x5E2 ) );				
				this.Add( new GenericBuyInfo( "Nasiona kapusty", typeof( CabbageSeed ), 200, 10, 0xF27, 0x5E2 ) );
    				this.Add( new GenericBuyInfo( "Nasiona salaty", typeof( LettuceSeed ), 250, 10, 0xF27, 0x5E2 ) );
                                this.Add( new GenericBuyInfo( "Nasiona cebuli", typeof( OnionSeed ), 300, 10, 0xF27, 0x5E2 ) );
				this.Add( new GenericBuyInfo( "Nasiona kukurydzy", typeof( CornSeed ), 350, 10, 0xF27, 0x5E2 ) );
				this.Add( new GenericBuyInfo( "Nasiona pszenicy", typeof( WheatSeed ), 400, 10, 0xF27, 0x5E2 ) );
				this.Add( new GenericBuyInfo( "Nasiona lnu", typeof( FlaxSeed ), 450, 10, 0xF27, 0x5E2 ) );
                                this.Add( new GenericBuyInfo( "Nasiona bawelny", typeof( CottonSeed ), 500, 10, 0xF27, 0x5E2 ) );
                                this.Add( new GenericBuyInfo( "Nasiona rzepy", typeof( TurnipSeed ), 550, 10, 0xF27, 0x5E2 ) );
                                //YardSystem
                                Add( new GenericBuyInfo( "Narzêdzia Ogrodowe", typeof( YardWand ), 20000, 10, 9569, 0xE88 ) );
                                Add( new GenericBuyInfo( "Pojemnik na Wodê", typeof( WaterBucket ), 5000, 10, 0x0FFA, 0xE88 ) );
                                ////Drzewa////
                                //Add( new GenericBuyInfo( "Apple Sapling", typeof( AppleSapling ), 5000, 10, 0xCE9, 0x5E2 ) );
				//Add( new GenericBuyInfo( "Pear Sapling", typeof( PearSapling ), 5000, 10, 0xCE9, 0x5E2 ) );
				//Add( new GenericBuyInfo( "Peach Sapling", typeof( PeachSapling ), 5000, 10, 0xCE9, 0x5E2 ) );
				////Drzewa////
                                
                                ////Ziola////
                                //Add( new GenericBuyInfo( "Ginseng Seed", typeof( GinsengPlant ), 25, 10, 0xF27, 0x5E2 ) );
			        //Add( new GenericBuyInfo( "Mandrake Seed", typeof( MandrakePlant ), 25, 10, 0xF27, 0x5E2 ) );
			        //Add( new GenericBuyInfo( "Nightshade Seed", typeof( NightshadePlant ), 25, 10, 0xF27, 0x5E2 ) );		
                                ////Ziola////
                                
                                //Add( new GenericBuyInfo( "Ziemia Uprawna", typeof( GardenGroundAddonDeed ), 20000, 10, 0xE87, 0xE88 ) );
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {

Add( typeof( Carrot ), 5 );
Add( typeof( Cabbage ), 10 );
Add( typeof( Lettuce ), 10 );
Add( typeof( Onion ), 6 );
Add( typeof( Corn ), 7 );
Add( typeof( Wheat ), 7 );
Add( typeof( Flax ), 10 );
Add( typeof( Cotton ), 10 );
Add( typeof( Turnip ), 15 );
                                Add( typeof( Apple ), 1 );
				Add( typeof( Grapes ), 1 );
				Add( typeof( Watermelon ), 3 );
				Add( typeof( YellowGourd ), 1 );
				Add( typeof( Pumpkin ), 5 );
				Add( typeof( Squash ), 1 );
				Add( typeof( HoneydewMelon ), 3 );
				Add( typeof( Cantaloupe ), 3 );
				Add( typeof( Lemon ), 1 );
				Add( typeof( Lime ), 1 );
				Add( typeof( Peach ), 1 );
				Add( typeof( Pear ), 1 );                
            }
        }
    }
}