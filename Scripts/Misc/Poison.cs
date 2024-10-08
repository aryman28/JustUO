#region References
using System;
using System.Globalization;

using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Nekromancja;
using Server.Spells.Skrytobojstwo;
#endregion

namespace Server
{
	public class PoisonImpl : Poison
	{
		[CallPriority(10)]
		public static void Configure()
		{
			if (Core.AOS)
			{
				Register(new PoisonImpl("Lesser", 0, 4, 16, 7.5, 3.0, 2.25, 10, 4));
				Register(new PoisonImpl("Regular", 1, 8, 18, 10.0, 3.0, 3.25, 10, 3));
				Register(new PoisonImpl("Greater", 2, 12, 20, 15.0, 3.0, 4.25, 10, 2));
				Register(new PoisonImpl("Deadly", 3, 16, 30, 30.0, 3.0, 5.25, 15, 2));
				Register(new PoisonImpl("Lethal", 4, 20, 50, 35.0, 3.0, 5.25, 20, 2));
			}
			else
			{
				Register(new PoisonImpl("Lesser", 0, 4, 26, 2.500, 3.5, 3.0, 10, 2));
				Register(new PoisonImpl("Regular", 1, 5, 26, 3.125, 3.5, 3.0, 10, 2));
				Register(new PoisonImpl("Greater", 2, 6, 26, 6.250, 3.5, 3.0, 10, 2));
				Register(new PoisonImpl("Deadly", 3, 7, 26, 12.500, 3.5, 4.0, 10, 2));
				Register(new PoisonImpl("Lethal", 4, 9, 26, 25.000, 3.5, 5.0, 10, 2));
			}

			#region Mondain's Legacy
			if (Core.ML)
			{
				Register(new PoisonImpl("LesserDarkglow", 10, 4, 16, 7.5, 3.0, 2.25, 10, 4));
				Register(new PoisonImpl("RegularDarkglow", 11, 8, 18, 10.0, 3.0, 3.25, 10, 3));
				Register(new PoisonImpl("GreaterDarkglow", 12, 12, 20, 15.0, 3.0, 4.25, 10, 2));
				Register(new PoisonImpl("DeadlyDarkglow", 13, 16, 30, 30.0, 3.0, 5.25, 15, 2));

				Register(new PoisonImpl("LesserParasitic", 14, 4, 16, 7.5, 3.0, 2.25, 10, 4));
				Register(new PoisonImpl("RegularParasitic", 15, 8, 18, 10.0, 3.0, 3.25, 10, 3));
				Register(new PoisonImpl("GreaterParasitic", 16, 12, 20, 15.0, 3.0, 4.25, 10, 2));
				Register(new PoisonImpl("DeadlyParasitic", 17, 16, 30, 30.0, 3.0, 5.25, 15, 2));
				Register(new PoisonImpl("LethalParasitic", 18, 20, 50, 35.0, 3.0, 5.25, 20, 2));
			}
			#endregion
		}

		public static Poison IncreaseLevel(Poison oldPoison)
		{
			Poison newPoison = oldPoison == null ? null : GetPoison(oldPoison.Level + 1);

			return newPoison ?? oldPoison;
		}

		// Info
		private readonly string m_Name;
		private readonly int m_Level;

		// Damage
		private readonly int m_Minimum;
		private readonly int m_Maximum;
		private readonly double m_Scalar;

		// Timers
		private readonly TimeSpan m_Delay;
		private readonly TimeSpan m_Interval;
		private readonly int m_Count;

		private readonly int m_MessageInterval;

		public override string Name { get { return m_Name; } }
		public override int Level { get { return m_Level; } }

		#region Mondain's Legacy
		public override int RealLevel
		{
			get
			{
				if (m_Level >= 14)
				{
					return m_Level - 14;
				}
				
				if (m_Level >= 10)
				{
					return m_Level - 10;
				}

				return m_Level;
			}
		}

		public override int LabelNumber
		{
			get
			{
				if (m_Level >= 14)
				{
					return 1072852; // parasitic poison charges: ~1_val~
				}
				
				if (m_Level >= 10)
				{
					return 1072853; // darkglow poison charges: ~1_val~
				}

				return 1062412 + m_Level; // ~poison~ poison charges: ~1_val~
			}
		}
		#endregion

		public PoisonImpl(
			string name,
			int level,
			int min,
			int max,
			double percent,
			double delay,
			double interval,
			int count,
			int messageInterval)
		{
			m_Name = name;
			m_Level = level;
			m_Minimum = min;
			m_Maximum = max;
			m_Scalar = percent * 0.01;
			m_Delay = TimeSpan.FromSeconds(delay);
			m_Interval = TimeSpan.FromSeconds(interval);
			m_Count = count;
			m_MessageInterval = messageInterval;
		}

		public override Timer ConstructTimer(Mobile m)
		{
			return new PoisonTimer(m, this);
		}

		public class PoisonTimer : Timer
		{
			private readonly PoisonImpl m_Poison;
			private readonly Mobile m_Mobile;
			private Mobile m_From;
			private int m_LastDamage;
			private int m_Index;

			public Mobile From { get { return m_From; } set { m_From = value; } }

			public PoisonTimer(Mobile m, PoisonImpl p)
				: base(p.m_Delay, p.m_Interval)
			{
				m_From = m;
				m_Mobile = m;
				m_Poison = p;
			}

			protected override void OnTick()
			{
				#region Mondain's Legacy
				if ((Core.AOS && m_Poison.RealLevel < 4 &&
					 TransformationSpellHelper.UnderTransformation(m_Mobile, typeof(VampiricEmbraceSpell))) ||
					(m_Poison.RealLevel < 3 && OrangePetals.UnderEffect(m_Mobile)) ||
					AnimalForm.UnderTransformation(m_Mobile, typeof(Unicorn)))
				{
					if (m_Mobile.CurePoison(m_Mobile))
					{
						m_Mobile.LocalOverheadMessage(
							MessageType.Emote, 0x3F, true, "* You feel yourself resisting the effects of the poison *");

						m_Mobile.NonlocalOverheadMessage(
							MessageType.Emote, 0x3F, true, String.Format("* {0} seems resistant to the poison *", m_Mobile.Name));

						Stop();
						return;
					}
				}
				#endregion

				if (m_Index++ == m_Poison.m_Count)
				{
					m_Mobile.SendLocalizedMessage(502136); // The poison seems to have worn off.
					m_Mobile.Poison = null;

					Stop();
					return;
				}

				int damage;

				if (!Core.AOS && m_LastDamage != 0 && Utility.RandomBool())
				{
					damage = m_LastDamage;
				}
				else
				{
					damage = 1 + (int)(m_Mobile.Hits * m_Poison.m_Scalar);

					if (damage < m_Poison.m_Minimum)
					{
						damage = m_Poison.m_Minimum;
					}
					else if (damage > m_Poison.m_Maximum)
					{
						damage = m_Poison.m_Maximum;
					}

					m_LastDamage = damage;
				}

				if (m_From != null)
				{
					m_From.DoHarmful(m_Mobile, true);
				}

				IHonorTarget honorTarget = m_Mobile as IHonorTarget;
				if (honorTarget != null && honorTarget.ReceivedHonorContext != null)
				{
					honorTarget.ReceivedHonorContext.OnTargetPoisoned();
				}

				#region Mondain's Legacy
				if (Core.ML)
				{
					if (m_From != null && m_Mobile != m_From && !m_From.InRange(m_Mobile.Location, 1) && m_Poison.m_Level >= 10 &&
						m_Poison.m_Level <= 13) // darkglow
					{
						m_From.SendLocalizedMessage(1072850); // Darkglow poison increases your damage!
						damage = (int)Math.Floor(damage * 1.1);
					}

					if (m_From != null && m_Mobile != m_From && m_From.InRange(m_Mobile.Location, 1) && m_Poison.m_Level >= 14 &&
						m_Poison.m_Level <= 18) // parasitic
					{
						int toHeal = Math.Min(m_From.HitsMax - m_From.Hits, damage);

						if (toHeal > 0)
						{
							m_From.SendLocalizedMessage(1060203, toHeal.ToString(CultureInfo.InvariantCulture));
								// You have had ~1_HEALED_AMOUNT~ hit points of damage healed.
							m_From.Heal(toHeal, m_Mobile, false);
						}
					}
				}
				#endregion

				AOS.Damage(m_Mobile, m_From, damage, 0, 0, 0, 100, 0);

				if (0.60 <= Utility.RandomDouble())
					// OSI: randomly revealed between first and third damage tick, guessing 60% chance
				{
					m_Mobile.RevealingAction();
				}

				if ((m_Index % m_Poison.m_MessageInterval) == 0)
				{
					m_Mobile.OnPoisoned(m_From, m_Poison, m_Poison);
				}
			}
		}
	}
}