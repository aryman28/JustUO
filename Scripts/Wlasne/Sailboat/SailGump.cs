using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
	public class SailGump : Gump
	{
		private const int FieldsPerPage = 14;

		private Mobile m_From;
		private Mobile m_Mobile;
		private Point3D m_StartPoint;
		private Point3D m_SailTo;

		public SailGump ( Mobile from, Point3D startPoint) : base ( 20, 30 )
		{
			m_From = from;
			m_StartPoint = startPoint;

			AddPage ( 0 );
			AddBackground( 0, 0, 410, 260, 5054 );

			AddImageTiled( 10, 10, 390, 23, 0x52 );
			AddImageTiled( 11, 11, 388, 21, 0xBBC );

			AddLabel( 110, 11, 0, "Gdzie chcesz p³yn¹æ?" );

			AddButton( 11, 35, 0x15E3, 0x15E7, 1, GumpButtonType.Reply, 1 );
			AddLabel( 30, 34, 0, "P³yn do Valery" );
			AddButton( 11, 57, 0x15E3, 0x15E7, 2, GumpButtonType.Reply, 1 );
			AddLabel( 30, 56, 0, "P³yn do Gusulii" );
			AddButton( 11, 79, 0x15E3, 0x15E7, 3, GumpButtonType.Reply, 1 );
			AddLabel( 30, 78, 0, "P³yn do Talvanii" );
			

		}

		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			int xd;
			int yd;
			int Distance;
			Container pack = from.Backpack;

			switch ( info.ButtonID )
			{
				case 1: 
				{
					xd = 278 - m_StartPoint.X;
					yd = 673 - m_StartPoint.Y;
					Distance = (int)Math.Sqrt(xd*xd + yd*yd);
					m_SailTo = new Point3D(280,672,4);
					from.SendGump(new InternalSailGump(from,Distance,new Point3D(416,32,0), m_SailTo));
					
					
					break;
				}
				case 2: 
				{
					xd = 807 - m_StartPoint.X;
					yd = 262 - m_StartPoint.Y;
					Distance = (int)Math.Sqrt(xd*xd + yd*yd);
					m_SailTo = new Point3D(809,263,-1);
					from.SendGump(new InternalSailGump(from,Distance,new Point3D(416,32,0), m_SailTo));
					
					
					break;
				}
				case 3: 
				{
					xd = 1432 - m_StartPoint.X;
					yd = 1405- m_StartPoint.Y;
					Distance = (int)Math.Sqrt(xd*xd + yd*yd);
					m_SailTo = new Point3D(1432,1407,0);
					from.SendGump(new InternalSailGump(from,Distance,new Point3D(416,32,0), m_SailTo));
					
					
					break;
				}
				
				default:
				{
					break;
				}
			}
		}
	}
	public class InternalSailGump : Gump
	{
		int m_cost;
		Point3D m_sendTo;
		Point3D m_SailTo;
		
		public InternalSailGump ( Mobile from, int cost, Point3D sendTo, Point3D sailTo) : base ( 20, 30 )
		{
			m_cost = cost;
			m_sendTo = sendTo;
			m_SailTo = sailTo;
			
			AddPage ( 0 );
			AddBackground( 0, 0, 410, 107, 5054 );

			AddImageTiled( 10, 10, 390, 23, 0x52 );
			AddImageTiled( 11, 11, 388, 21, 0xBBC );

			AddLabel( 100, 11, 0, "Rejs w t¹ strone bêdzie kosztowa³ ciê "+ m_cost/4 + " Gold"  );

			AddButton( 11, 35, 0x15E3, 0x15E7, 1, GumpButtonType.Reply, 1 );
			AddLabel( 30, 34, 0, "Zap³aæ" );
			AddButton( 11, 57, 0x15E3, 0x15E7, 2, GumpButtonType.Reply, 1 );
			AddLabel( 30, 56, 0, "Poka¿ kartê podro¿nika" );
			AddButton( 11, 79, 0x15E3, 0x15E7, 3, GumpButtonType.Reply, 1 );
			AddLabel( 30, 78, 0, "Uhm, nie dziêkujê" );

		}
		
		public override void OnResponse( NetState sender, RelayInfo info )
		{
			Mobile from = sender.Mobile;
			Container pack = from.Backpack;
			SailTimer waitTime;

			switch ( info.ButtonID )
			{
				case 1: 
				{
					if ( pack.ConsumeTotal( typeof( Gold ), m_cost/4 ) )
					{
						from.Location = m_sendTo;
						if (m_cost > 60)
							m_cost = 60;
						waitTime = new SailTimer(from,m_SailTo,TimeSpan.FromSeconds(m_cost/10 ));
						waitTime.Start();
						//Give items
					}
					else
					{
						from.SendMessage("HA, Nie masz odpowiedni du¿o z³ota aby wyruszyæ w rejs!");
					}
					break;
				}
				case 2: 
				{
					if ( pack.ConsumeTotal( typeof( SailboatMembershipcard ), 0 ) )
					{
						from.Location = m_sendTo;
						if (m_cost > 60)
							m_cost = 60;
						waitTime = new SailTimer(from,m_SailTo,TimeSpan.FromSeconds(m_cost/10 ));
						waitTime.Start();
					}
					else
					{
						from.SendMessage("It might be usefull if you actualy have a membership card...");
					}
					break;
				}
				case 3: 
				{
					from.SendMessage("Perhaps later then");
					break;
				}
				
				default:
				{
					from.SendMessage("Perhaps later then");
					break;
				}
			}
		}
	}
	
	public class SailTimer : Timer
        {
		Mobile from;
		Point3D finalSpot;

		public SailTimer( Mobile m_from, Point3D m_finalSpot, TimeSpan duration ) : base( duration )
		{
			from = m_from;
			finalSpot = m_finalSpot;
		}

		protected override void OnTick()
		{
			from.Location = finalSpot;
			from.SendMessage("¯yczê udanego rejsu!");
			Stop();
		}
	}
	
}

