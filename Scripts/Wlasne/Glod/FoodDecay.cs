// Modified by Alari for the hunger messages and hit point penalties.
// Change the: "public static bool KeepAlive = true;" part if you want
//  players to die from dehydration or starvation.
//

using System;
using Server.Network;
using Server;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Misc
{
	public class FoodDecayTimer : Timer
	{
		// new: keep player from dying from hunger?
		public static bool KeepAlive = true;
		
		public static void Initialize()
		{
			new FoodDecayTimer().Start();
		}
		
		public FoodDecayTimer() : base( TimeSpan.FromMinutes( 10 ), TimeSpan.FromMinutes( 10 ) )
		{
			Priority = TimerPriority.OneMinute;
		}
		
		protected override void OnTick()
		{
			FoodDecay();
		}
		
		public static void FoodDecay()
		{
			foreach ( NetState state in NetState.Instances )
			{
				HungerDecay( state.Mobile );
				ThirstDecay( state.Mobile );
			}
		}
		
		public static void HungerDecay( Mobile m )
		{
			if ( m != null && m.Hunger >= 1 )
				m.Hunger -= 1;
			
			// new additions
			
			bool keepAlive = KeepAlive;	// keep player from dying from this
			
			if ( !( m is PlayerMobile ) )
				return;

			if (!(m.AccessLevel == AccessLevel.Player))
			    return;  
			
			if (!(m.Alive))
			    return; 

			if ( m.Hunger < 6 )
			{
				try
				{
					m.CloseGump( typeof( gumpfaim ) );
				        m.SendGump( new Server.Gumps.gumpfaim ( m ) ); // popup hunger gump.
				}
				catch
				{}
			}			

			switch ( m.Hunger )
			{
				case 5: m.SendMessage( "Jestes odrobine glodny." ); break;
				case 4: m.SendMessage( "Jestes glodny." ); break;
				case 3: m.SendMessage( "Jestes naprawde glodny." ); break;
				case 2: m.SendMessage( "Jestes bardzo glodny!" ); break;

				case 1:
				{
					m.SendMessage( "Potrzebujesz jesc juz!" );

					if ( m.Hits < ( m.HitsMax / 20 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 20 );
					m.SendMessage(33, "" + -( m.HitsMax / 20 ) + " hp!" );
					break;
				}

				case 0:
				{
					m.SendMessage( "Umierasz z glodu!" );
					if ( m.Hits < ( m.HitsMax / 10 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 10 );
					m.SendMessage(33, "" + -( m.HitsMax / 10 ) + " hp!" );
					break;
				}
			}
		}
		
		public static void ThirstDecay( Mobile m )
		{
			if ( m != null && m.Thirst >= 1 )
				m.Thirst -= 1;
			
			// new additions:
				bool keepAlive = KeepAlive;	// keep player from dying from this
				
				if ( !( m is PlayerMobile ) )
					return;

			        if (!(m.AccessLevel == AccessLevel.Player))
			                return;  
			 			
			        if (!(m.Alive))
			                return; 
			
			if ( m.Thirst < 6 )
			{
				try
				{
					m.CloseGump( typeof( gumpfaim ) );
				        m.SendGump( new Server.Gumps.gumpfaim ( m ) );
				}
				catch
				{}
			}


			switch ( m.Thirst )
			{
				case 5: m.SendMessage( "Jestes odrobine spragniony." ); break;
				case 4: m.SendMessage( "Jestes spragniony." ); break;
				case 3: m.SendMessage( "Jestes naprawde spragniony." ); break;
				case 2: m.SendMessage( "Jestes bardzo spragniony!" ); break;

				case 1:
				{
					m.SendMessage( "Potrzebujesz sie napic juz!" );

					if ( m.Hits < ( m.HitsMax / 10 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 10);
					m.SendMessage(33, "" + -( m.HitsMax / 10 ) + " hp!" );
					break;
				}

				case 0:
				{
					m.SendMessage( "Umierasz z pragnienia!" );

					if ( m.Hits < ( m.HitsMax / 5 ) && ( keepAlive ) )
						return;

					m.Hits = m.Hits - ( m.HitsMax / 5 );
					m.SendMessage(33, "" + -( m.HitsMax / 5 ) + " hp!" );
					break;
				}
			}
		}
	}
}
