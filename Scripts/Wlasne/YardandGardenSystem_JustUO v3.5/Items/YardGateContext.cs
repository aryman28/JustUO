using System;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.ContextMenus
{
	public class YardSecurityGump : Gump
	{
		BaseDoor m_Gate;
		Mobile m_From;
		public YardSecurityGump( Mobile from, BaseDoor gate ) : base( 50, 50 )
		{
			m_Gate = gate;
			m_From = from;
			this.Closable=true;
			this.Disposable=true;
			this.Dragable=true;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(0, 0, 200, 100, 9250);
			this.AddLabel(58, 13, 0, @"USTAW DOSTÊP");
			this.AddButton(131, 38, 1150, 1152, (int)Buttons.Unlock, GumpButtonType.Reply, 0);
			this.AddButton(40, 38, 1153, 1155, (int)Buttons.Lock, GumpButtonType.Reply, 0);
			this.AddLabel(38, 58, 0, @"Zamknij");
			this.AddLabel(123, 58, 0, @"Otwórz");
		}

		public enum Buttons
		{
			Lock,
			Unlock,
		}
		public override void OnResponse( NetState state, RelayInfo info )
		{
			switch ( info.ButtonID )
			{
				case (int)Buttons.Lock:
				{
					m_Gate.Locked = true;
					m_From.SendMessage("Zamkne³eœ bramke");
					break;
				}
				case (int)Buttons.Unlock:
				{
					m_Gate.Locked = false;
					m_From.SendMessage("Otworzy³eœ bramke");
					break;
				}
			}
		}
	}

	public class YardSecurityEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private BaseDoor m_Gate;

		public YardSecurityEntry( Mobile from, BaseDoor gate) : base( 6203, 9 )
		{
			m_From = from;
			m_Gate = gate;
		}
		public override void OnClick()
		{
			m_From.SendGump( new YardSecurityGump(m_From, m_Gate));
		}
	}


	public class RefundEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private BaseDoor m_Gate;
		private int value = 0;

		public RefundEntry( Mobile from, BaseDoor gate, int price) : base( 6104, 9 )
		{
			m_From = from;
			m_Gate = gate;
			value = price;
		}
		public override void OnClick()
		{
			Container c = m_From.Backpack;
			Gold t = new Gold( value );
			if( c.TryDropItem( m_From, t, true ) )
			{
				m_Gate.Delete();
				m_From.SendMessage( "Ten przedmiot zostal usuniêty a monety zwrócono tobie" );
			}
			else
			{
				t.Delete();
				m_From.SendMessage("Z jakiegoœ powodu zwrot monet nie dzia³a! skontaktuj siê z ekip¹");
			}
		}
	}
}