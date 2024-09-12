/*----------------*/
/*--- Scripted ---*/
/*--- By: JBob ---*/
/*----------------*/
using System;
using Server;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using System.Timers;

namespace Server.Items
{
  public abstract class BaseStaminaRefreshPotion : BasePotion
	{
		public abstract double StaminaRefresh{ get; }

		public BaseStaminaRefreshPotion( PotionEffect effect ) : base( 0xF0B, effect )
		{
		}
		public BaseStaminaRefreshPotion( Serial serial ) : base( serial )
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

#region Timer		
		private class StaminaRegenTimer : Timer
		{
			private readonly Mobile m_From;
			private readonly int m_SoundID;
			private int m_Count;

			public StaminaRegenTimer(Mobile from, int soundID) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
			{
				m_From = from;
				m_SoundID = soundID;

				Priority = TimerPriority.TenMS;
			}
			protected override void OnTick()
			{

				m_Count++;
				if (m_Count == 1)     //co 5 sekund
				{ m_From.Stam += 30; }//+30 Stam
				if (m_Count == 5)
				{ m_From.Stam += 30; }//+30 Stam
				if (m_Count == 10)
				{ m_From.Stam += 30; }//+30 Stam
				if (m_Count == 15)
				{ m_From.Stam += 30; }//+30 Stam
				if (m_Count == 20)
				{ m_From.Stam += 30; }//+30 Stam
                                if (m_Count == 25)
				{ m_From.Stam += 30; }//+30 Stam
				if (m_Count == 30)
				{
					m_From.PlaySound(m_SoundID);
					m_From.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "*mikstura regeneracji traci moc*", m_From.NetState);
					Stop();
				}
				//if ((m_From.Stam == m_From.StamMax) || (m_From.Stam > m_From.StamMax))
				//{
				//	m_From.PrivateOverheadMessage(MessageType.Regular, 0x3B2, false, "*mikstura regeneracji traci moc*", m_From.NetState);
				//	m_From.PlaySound(m_SoundID);
				//	Stop();
				//}
			}
                }

#endregion
		public override void Drink( Mobile from )
		{
			Mobile player = from as PlayerMobile;

			if ( player.Stam < player.StamMax )
			{
				if ( player.BeginAction( typeof( BaseStaminaRefreshPotion ) ) ) 
				{ 
					player.PlaySound(0xF9);
					new StaminaRegenTimer(player, 0xFB).Start();
					BasePotion.PlayDrinkEffect(player);
					Timer.DelayCall( TimeSpan.FromSeconds( 30.0 ), new TimerStateCallback( ReleaseStaminaLock ), player );
					this.Consume(1);
				}
				else 
				{ 
					player.SendMessage( 0x22, "Musisz odczekaæ 30s przed wypiciem kolejnej mikstury." ); 
				} 
			}
			else
			{
				from.SendMessage( "Jesteœ w pe³ni swojej wytrzyma³oœci." );
			}
		}

		private static void ReleaseStaminaLock( object state ) 
		{ 
			((Mobile)state).EndAction( typeof( BaseStaminaRefreshPotion ) ); 
		}
	}
}
