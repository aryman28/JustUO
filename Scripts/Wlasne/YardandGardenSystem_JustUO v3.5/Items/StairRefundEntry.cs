using System;
using Server.Items;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;

namespace Server.ACC.YS
{
	public class StairRefundEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private YardStair m_Stair;
		private int value = 0;

		public StairRefundEntry( Mobile from, YardStair stair, int price) : base( 6104, 9 )
		{
			m_From = from;
			m_Stair = stair;
			value = price;
		}
		public override void OnClick()
		{
			Container c = m_From.Backpack;
			Gold t = new Gold( value );
			if( c.TryDropItem( m_From, t, true ) )
			{
				m_Stair.Delete();
				m_From.SendMessage( "Ten przedmiot zostal usuniêty a monety zwrócono tobie" );
			}
			else
			{
				t.Delete();
				m_From.SendMessage("z jakiegoœ powodu zwrot monet nie dzia³a! skontaktuj siê z ekip¹");
			}
		}
	}
}