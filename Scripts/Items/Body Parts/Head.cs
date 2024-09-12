using System;
using Server;

using Server.Mobiles;
using Server.Accounting;
using Server.Misc;
using Server.Network;

namespace Server.Items
{
	public enum HeadType
	{
		Regular,
		Duel,
		Tournament
	}

	public class Head : Item
	{

//bount system here
		private DateTime m_CreationTime;
		private Mobile m_Owner;
		private Mobile m_Killer;
		private bool m_Player;

		[CommandProperty( AccessLevel.GameMaster )]
		public DateTime CreationTime
		{
			get{ return m_CreationTime; }
			set{ m_CreationTime = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Owner
		{
			get{ return m_Owner; }
			set{ m_Owner = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Killer
		{
			get{ return m_Killer; }
			set{ m_Killer = value; }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsPlayer
		{
			get{ return m_Player; }
			set{ m_Player = value; }
		}
//end bounty system

		[Constructable]
		public Head()
			: this( null )
		{
		}

		[Constructable]
		public Head( string playerName )
			: this( HeadType.Regular, playerName )
		{
		}

		[Constructable]
		public Head( HeadType headType, string playerName )
			: base( 0x1DA0 )
		{
			//m_HeadType = headType;
			//m_PlayerName = playerName;
		}

		public Head( Serial serial )
			: base( serial )
		{
		}

public override void GetProperties( ObjectPropertyList list )
{
base.GetProperties( list );

  if ( m_Killer != null )
  {
         //list.Add( "Zabity przez {0}", m_Killer );
         if ( m_Killer is PlayerMobile )
         {
         list.Add( "Zamordowany" );
         }

         if ( m_Killer is BaseCreature )
         {
         list.Add( "Rozszarpany przez jakas bestie" );
         }
  }
}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 1 ); // version
			
			//writer.Write( (string) m_PlayerName );
			//writer.WriteEncodedInt( (int) m_HeadType );
//bounty system

			writer.Write( m_Player );
			writer.Write( m_CreationTime );

			//if( m_Player )
			//{
				writer.Write(  m_Owner );
				writer.Write(  m_Killer );
			//}
//end bounty system

//bounty system
			m_Player = false;
			m_Owner = null;
			m_Killer = null;
			m_CreationTime = DateTime.Now;
//end bounty system
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

//bounty system
			switch( version )
			{
				case 1:
				{
					m_Player = reader.ReadBool();
					m_CreationTime = reader.ReadDateTime();
					//if( m_Player )
					//{
						m_Owner = reader.ReadMobile();
						m_Killer = reader.ReadMobile();
					//}

					goto case 0;
				}
				case 0:
				{
					if( version == 0 )
					{
						m_Owner = null;
						m_Killer = null;
						m_Player = false;
						m_CreationTime = DateTime.Now;
					}

					break;
				}
			}
//end bounty system
		}
	}
}