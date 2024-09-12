using System;
using Server.Network;

using System.Collections; 
using Server.Items; 
using Server.Targeting;
using Server.Mobiles;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Login += new LoginEventHandler( EventSink_Login ); 
		}

		private static void EventSink_Login( LoginEventArgs args )
		{                        
                        
                        int userCount = NetState.Instances.Count + 8; 
			int itemCount = World.Items.Count;
			int mobileCount = World.Mobiles.Count;

                        if ( NetState.Instances.Count <= 30 )
                        {
                            userCount = NetState.Instances.Count + 7;
                        }

                        if ( NetState.Instances.Count <= 25 )
                        {
                            userCount = NetState.Instances.Count + 6;
                        }

                        if ( NetState.Instances.Count <= 20 )
                        {
                            userCount = NetState.Instances.Count + 5;
                        }

                        if ( NetState.Instances.Count <= 15 )
                        {
                            userCount = NetState.Instances.Count + 4;
                        }

                        if ( NetState.Instances.Count <= 5 )
                        {
                            userCount = NetState.Instances.Count + 1;
                        }                       

                        if ( NetState.Instances.Count <= 1 )
                        {
                            userCount = NetState.Instances.Count;
                        }



			Mobile m = args.Mobile;

			m.SendMessage( "Witaj, {0}! Jest {1} obecnie {2} gracz{3} online, oraz {4} przedmioto{5} i {6} mobilko{7} na swiecie.",
				args.Mobile.Name,
				userCount == 1 ? "" : "",
				userCount, userCount == 1 ? "" : "y",
				itemCount, itemCount == 1 ? "" : "w",
				mobileCount, mobileCount == 1 ? "" : "w" );
		}
        }
}