#region Header
// **************************************\
//     _  _   _   __  ___  _   _   ___   |
//    |# |#  |#  |## |### |#  |#  |###   |
//    |# |#  |# |#    |#  |#  |# |#  |#  |
//    |# |#  |#  |#   |#  |#  |# |#  |#  |
//   _|# |#__|#  _|#  |#  |#__|# |#__|#  |
//  |##   |##   |##   |#   |##    |###   |
//        [http://www.playuo.org]        |
// **************************************/
//  [2014] Skills.cs
// ************************************/
#endregion

#region References
using System;
using System.Collections;
using System.Collections.Generic;

using Server.Network;
#endregion

namespace Server
{
	public delegate TimeSpan SkillUseCallback(Mobile user);

	public enum SkillLock : byte
	{
		Up = 0,
		Down = 1,
		Locked = 2
	}

	public enum SkillName
	{
		Alchemia = 0,
		Anatomia = 1,
		WiedzaOBestiach  = 2,
		Identyfikacja = 3,
		WiedzaOUzbrojeniu = 4,
		Parowanie = 5,
		Rolnictwo = 6,
		Kowalstwo = 7,
		Lukmistrzostwo = 8,
		Uspokajanie = 9,
		Obozowanie = 10,
		Stolarstwo = 11,
		Kartografia = 12,
		Gotowanie = 13,
		Wykrywanie = 14,
		Manipulacja = 15,
		Intelekt = 16,
		Leczenie = 17,
		Rybactwo = 18,
		Kryminalistyka = 19,
		Zielarstwo = 20,
		Ukrywanie = 21,
		Prowokacja = 22,
		Inskrypcja = 23,
		Wlamywanie = 24,
		Magia = 25,
		ObronaPrzedMagia = 26,
		Taktyka = 27,
		Zagladanie = 28,
		Muzykowanie = 29,
		Zatruwanie = 30,
		Lucznictwo = 31,
		MowaDuchow = 32,
		Okradanie = 33,
		Krawiectwo = 34,
		Oswajanie = 35,
		OcenaSmaku = 36,
		Majsterkowanie = 37,
		Tropienie = 38,
		Weterynaria = 39,
		WalkaMieczami = 40,
		WalkaObuchami = 41,
		WalkaSzpadami = 42,
		Boks = 43,
		Drwalnictwo = 44,
		Gornictwo = 45,
		Medytacja = 46,
		Zakradanie = 47,
		UsuwaniePulapek = 48,
		Nekromancja = 49,
		Logistyka = 50,
		Rycerstwo = 51,
		Fanatyzm = 52,
		Skrytobojstwo = 53,
		Druidyzm = 54,
		Mistycyzm = 55,
		Umagicznianie = 56,
		Rzucanie = 57
	}

	[PropertyObject]
	public class Skill
	{
		private readonly Skills m_Owner;
		private readonly SkillInfo m_Info;
		private ushort m_Base;
		private ushort m_Cap;
		private SkillLock m_Lock;

		public override string ToString()
		{
			return String.Format("[{0}: {1}]", Name, Base);
		}

		public Skill(Skills owner, SkillInfo info, GenericReader reader)
		{
			m_Owner = owner;
			m_Info = info;

			int version = reader.ReadByte();

			switch (version)
			{
				case 0:
					{
						m_Base = reader.ReadUShort();
						m_Cap = reader.ReadUShort();
						m_Lock = (SkillLock)reader.ReadByte();

						break;
					}
				case 0xFF:
					{
						m_Base = 0;
						m_Cap = 1000;
						m_Lock = SkillLock.Up;

						break;
					}
				default:
					{
						if ((version & 0xC0) == 0x00)
						{
							if ((version & 0x1) != 0)
							{
								m_Base = reader.ReadUShort();
							}

							if ((version & 0x2) != 0)
							{
								m_Cap = reader.ReadUShort();
							}
							else
							{
								m_Cap = 1000;
							}

							if ((version & 0x4) != 0)
							{
								m_Lock = (SkillLock)reader.ReadByte();
							}
						}

						break;
					}
			}

			if (m_Lock < SkillLock.Up || m_Lock > SkillLock.Locked)
			{
				Console.WriteLine("Bad skill lock -> {0}.{1}", owner.Owner, m_Lock);
				m_Lock = SkillLock.Up;
			}
		}

		public Skill(Skills owner, SkillInfo info, int baseValue, int cap, SkillLock skillLock)
		{
			m_Owner = owner;
			m_Info = info;
			m_Base = (ushort)baseValue;
			m_Cap = (ushort)cap;
			m_Lock = skillLock;
		}

		public void SetLockNoRelay(SkillLock skillLock)
		{
			if (skillLock < SkillLock.Up || skillLock > SkillLock.Locked)
			{
				return;
			}

			m_Lock = skillLock;
		}

		public void Serialize(GenericWriter writer)
		{
			if (m_Base == 0 && m_Cap == 1000 && m_Lock == SkillLock.Up)
			{
				writer.Write((byte)0xFF); // default
			}
			else
			{
				int flags = 0x0;

				if (m_Base != 0)
				{
					flags |= 0x1;
				}

				if (m_Cap != 1000)
				{
					flags |= 0x2;
				}

				if (m_Lock != SkillLock.Up)
				{
					flags |= 0x4;
				}

				writer.Write((byte)flags); // version

				if (m_Base != 0)
				{
					writer.Write((short)m_Base);
				}

				if (m_Cap != 1000)
				{
					writer.Write((short)m_Cap);
				}

				if (m_Lock != SkillLock.Up)
				{
					writer.Write((byte)m_Lock);
				}
			}
		}

		public Skills Owner { get { return m_Owner; } }

		public SkillName SkillName { get { return (SkillName)m_Info.SkillID; } }

		public int SkillID { get { return m_Info.SkillID; } }

		[CommandProperty(AccessLevel.Counselor)]
		public string Name { get { return m_Info.Name; } }

		public SkillInfo Info { get { return m_Info; } }

		[CommandProperty(AccessLevel.Counselor)]
		public SkillLock Lock { get { return m_Lock; } }

		public int BaseFixedPoint
		{
			get { return m_Base; }
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value >= 0x10000)
				{
					value = 0xFFFF;
				}

				var sv = (ushort)value;

				int oldBase = m_Base;

				if (m_Base != sv)
				{
					m_Owner.Total = (m_Owner.Total - m_Base) + sv;

					m_Base = sv;

					m_Owner.OnSkillChange(this);

					Mobile m = m_Owner.Owner;

					if (m != null)
					{
						m.OnSkillChange(SkillName, (double)oldBase / 10);
					}
				}
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double Base { get { return (m_Base / 10.0); } set { BaseFixedPoint = (int)(value * 10.0); } }

		public int CapFixedPoint
		{
			get { return m_Cap; }
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value >= 0x10000)
				{
					value = 0xFFFF;
				}

				var sv = (ushort)value;

				if (m_Cap != sv)
				{
					m_Cap = sv;

					m_Owner.OnSkillChange(this);
				}
			}
		}

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public double Cap { get { return (m_Cap / 10.0); } set { CapFixedPoint = (int)(value * 10.0); } }

		private static bool m_UseStatMods;

		public static bool UseStatMods { get { return m_UseStatMods; } set { m_UseStatMods = value; } }

		public int Fixed { get { return (int)(Value * 10); } }

		[CommandProperty(AccessLevel.Counselor)]
		public double Value
		{
			get
			{
				//There has to be this distinction between the racial values and not to account for gaining skills and these skills aren't displayed nor Totaled up.
				double value = NonRacialValue;

				double raceBonus = m_Owner.Owner.RacialSkillBonus;

				if (raceBonus > value)
				{
					value = raceBonus;
				}

				return value;
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public double NonRacialValue
		{
			get
			{
				double baseValue = Base;
				double inv = 100.0 - baseValue;

				if (inv < 0.0)
				{
					inv = 0.0;
				}

				inv /= 100.0;

				double statsOffset = ((m_UseStatMods ? m_Owner.Owner.Str : m_Owner.Owner.RawStr) * m_Info.StrScale) +
									 ((m_UseStatMods ? m_Owner.Owner.Dex : m_Owner.Owner.RawDex) * m_Info.DexScale) +
									 ((m_UseStatMods ? m_Owner.Owner.Int : m_Owner.Owner.RawInt) * m_Info.IntScale);
				double statTotal = m_Info.StatTotal * inv;

				statsOffset *= inv;

				if (statsOffset > statTotal)
				{
					statsOffset = statTotal;
				}

				double value = baseValue + statsOffset;

				m_Owner.Owner.ValidateSkillMods();

				List<SkillMod> mods = m_Owner.Owner.SkillMods;

				double bonusObey = 0.0, bonusNotObey = 0.0;

				for (int i = 0; i < mods.Count; ++i)
				{
					SkillMod mod = mods[i];

					if (mod.Skill == (SkillName)m_Info.SkillID)
					{
						if (mod.Relative)
						{
							if (mod.ObeyCap)
							{
								bonusObey += mod.Value;
							}
							else
							{
								bonusNotObey += mod.Value;
							}
						}
						else
						{
							bonusObey = 0.0;
							bonusNotObey = 0.0;
							value = mod.Value;
						}
					}
				}

				value += bonusNotObey;

				if (value < Cap)
				{
					value += bonusObey;

					if (value > Cap)
					{
						value = Cap;
					}
				}

				return value;
			}
		}

		public void Update()
		{
			m_Owner.OnSkillChange(this);
		}
	}

	public class SkillInfo
	{
		private readonly int m_SkillID;

		public SkillInfo(
			int skillID,
			string name,
			double strScale,
			double dexScale,
			double intScale,
			string title,
			SkillUseCallback callback,
			double strGain,
			double dexGain,
			double intGain,
			double gainFactor)
		{
			Name = name;
			Title = title;
			m_SkillID = skillID;
			StrScale = strScale / 100.0;
			DexScale = dexScale / 100.0;
			IntScale = intScale / 100.0;
			Callback = callback;
			StrGain = strGain;
			DexGain = dexGain;
			IntGain = intGain;
			GainFactor = gainFactor;

			StatTotal = strScale + dexScale + intScale;
		}

		public SkillUseCallback Callback { get; set; }

		public int SkillID { get { return m_SkillID; } }

		public string Name { get; set; }

		public string Title { get; set; }

		public double StrScale { get; set; }

		public double DexScale { get; set; }

		public double IntScale { get; set; }

		public double StatTotal { get; set; }

		public double StrGain { get; set; }

		public double DexGain { get; set; }

		public double IntGain { get; set; }

		public double GainFactor { get; set; }

		private static SkillInfo[] m_Table = new SkillInfo[58]
		{
			new SkillInfo(0, "Alchemia", 0.0, 5.0, 5.0, "Alchemik", null, 0.0, 0.5, 0.5, 1.0),
			new SkillInfo(1, "Anatomia", 0.0, 0.0, 0.0, "Medyk", null, 0.15, 0.15, 0.7, 1.0),
			new SkillInfo(2, "Wiedza O Bestiach", 0.0, 0.0, 0.0, "Znawca Bestii", null, 0.0, 0.0, 1.0, 1.0),
			new SkillInfo(3, "Identyfikacja", 0.0, 0.0, 0.0, "Znawca", null, 0.0, 0.0, 1.0, 1.0),
			new SkillInfo(4, "Wiedza O Uzbrojeniu", 0.0, 0.0, 0.0, "Znawca Uzbrojenia", null, 0.75, 0.15, 0.1, 1.0),
			new SkillInfo(5, "Parowanie", 7.5, 2.5, 0.0, "Tarczownik", null, 0.75, 0.25, 0.0, 1.0),
			new SkillInfo(6, "Rolnictwo", 0.0, 0.0, 0.0, "Rolnik", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(7, "Kowalstwo", 10.0, 0.0, 0.0, "Kowal", null, 1.0, 0.0, 0.0, 1.0),
			new SkillInfo(8, "Lukmistrzostwo", 6.0, 16.0, 0.0, "Lukmistrz", null, 0.6, 1.6, 0.0, 1.0),
			new SkillInfo(9, "Uspokajanie", 0.0, 0.0, 0.0, "Bard", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(10, "Obozowanie", 20.0, 15.0, 15.0, "Traper", null, 2.0, 1.5, 1.5, 1.0),
			new SkillInfo(11, "Stolarstwo", 20.0, 5.0, 0.0, "Stolarz", null, 2.0, 0.5, 0.0, 1.0),
			new SkillInfo(12, "Kartografia", 0.0, 7.5, 7.5, "Kartograf", null, 0.0, 0.75, 0.75, 1.0),
			new SkillInfo(13, "Gotowanie", 0.0, 20.0, 30.0, "Kucharz", null, 0.0, 2.0, 3.0, 1.0),
			new SkillInfo(14, "Wykrywanie", 0.0, 0.0, 0.0, "Zwiadowca", null, 0.0, 0.4, 0.6, 1.0),
			new SkillInfo(15, "Manipulacja", 0.0, 2.5, 2.5, "Bard", null, 0.0, 0.25, 0.25, 1.0),
			new SkillInfo(16, "Intelekt", 0.0, 0.0, 0.0, "Uczony", null, 0.0, 0.0, 1.0, 1.0),
			new SkillInfo(17, "Leczenie", 6.0, 6.0, 8.0, "Uzdrowiciel", null, 0.6, 0.6, 0.8, 1.0),
			new SkillInfo(18, "Rybactwo", 0.0, 0.0, 0.0, "Rybak", null, 0.5, 0.5, 0.0, 1.0),
			new SkillInfo(19, "Kryminalistyka", 0.0, 0.0, 0.0, "Detektyw", null, 0.0, 0.2, 0.8, 1.0),
			new SkillInfo(20, "Zielarstwo", 16.25, 6.25, 2.5, "Zielarz", null, 1.625, 0.625, 0.25, 1.0),
			new SkillInfo(21, "Ukrywanie", 0.0, 0.0, 0.0, "Cien", null, 0.0, 0.8, 0.2, 1.0),
			new SkillInfo(22, "Prowokacja", 0.0, 4.5, 0.5, "Bard", null, 0.0, 0.45, 0.05, 1.0),
			new SkillInfo(23, "Inskrypcja", 0.0, 2.0, 8.0, "Skryba", null, 0.0, 0.2, 0.8, 1.0),
			new SkillInfo(24, "Wlamywanie", 0.0, 25.0, 0.0, "Wlamywacz", null, 0.0, 2.0, 0.0, 1.0),
			new SkillInfo(25, "Magia", 0.0, 0.0, 15.0, "Mag", null, 0.0, 0.0, 1.5, 1.0),
			new SkillInfo(26, "Obrona Przed Magia", 0.0, 0.0, 0.0, "Lamacz Magi", null, 0.25, 0.25, 0.5, 1.0),
			new SkillInfo(27, "Taktyka", 0.0, 0.0, 0.0, "Taktyk", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(28, "Zagladanie", 0.0, 25.0, 0.0, "Szpieg", null, 0.0, 2.5, 0.0, 1.0),
			new SkillInfo(29, "Muzykowanie", 0.0, 0.0, 0.0, "Muzyk", null, 0.0, 0.8, 0.2, 1.0),
			new SkillInfo(30, "Zatruwanie", 0.0, 4.0, 16.0, "Zatruwacz", null, 0.0, 0.4, 1.6, 1.0),
			new SkillInfo(31, "Lucznictwo", 2.5, 7.5, 0.0, "Lucznik", null, 0.25, 0.75, 0.0, 1.0),
			new SkillInfo(32, "Mowa Duchow", 0.0, 0.0, 0.0, "Medium", null, 0.0, 0.0, 1.0, 1.0),
			new SkillInfo(33, "Okradanie", 0.0, 10.0, 0.0, "Zlodziej", null, 0.0, 1.0, 0.0, 1.0),
			new SkillInfo(34, "Krawiectwo", 3.75, 16.25, 5.0, "Krawiec", null, 0.38, 1.63, 0.5, 1.0),
			new SkillInfo(35, "Oswajanie", 14.0, 2.0, 4.0, "Oswajacz", null, 1.4, 0.2, 0.4, 1.0),
			new SkillInfo(36, "Ocena Smaku", 0.0, 0.0, 0.0, "Degustator", null, 0.2, 0.0, 0.8, 1.0),
			new SkillInfo(37, "Majsterkowanie", 5.0, 2.0, 3.0, "Majsterkowicz", null, 0.5, 0.2, 0.3, 1.0),
			new SkillInfo(38, "Tropienie", 0.0, 12.5, 12.5, "Tropiciel", null, 0.0, 1.25, 1.25, 1.0),
			new SkillInfo(39, "Weterynaria", 8.0, 4.0, 8.0, "Weterynarz", null, 0.8, 0.4, 0.8, 1.0),
			new SkillInfo(40, "Walka Mieczami", 7.5, 2.5, 0.0, "Mieczownik", null, 0.75, 0.25, 0.0, 1.0),
			new SkillInfo(41, "Walka Obuchami", 9.0, 1.0, 0.0, "Obuchomistrz", null, 0.9, 0.1, 0.0, 1.0),
			new SkillInfo(42, "Walka Szpadami", 4.5, 5.5, 0.0, "Szermierz", null, 0.45, 0.55, 0.0, 1.0),
			new SkillInfo(43, "Boks", 9.0, 1.0, 0.0, "Bokser", null, 0.9, 0.1, 0.0, 1.0),
			new SkillInfo(44, "Drwalnictwo", 20.0, 0.0, 0.0, "Drwal", null, 2.0, 0.0, 0.0, 1.0),
			new SkillInfo(45, "Gornictwo", 20.0, 0.0, 0.0, "Gornik", null, 2.0, 0.0, 0.0, 1.0),
			new SkillInfo(46, "Medytacja", 0.0, 0.0, 0.0, "Mnich", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(47, "Zakradanie", 0.0, 0.0, 0.0, "Zakradacz", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(48, "Usuwanie Pulapek", 0.0, 0.0, 0.0, "Znawca Pulapek", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(49, "Nekromancja", 0.0, 0.0, 0.0, "Nekromanta", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(50, "Logistyka", 0.0, 0.0, 0.0, "Jezdziec", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(51, "Rycerstwo", 0.0, 0.0, 0.0, "Paladyn", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(52, "Fanatyzm", 0.0, 0.0, 0.0, "Fanatyk", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(53, "Skrytobojstwo", 0.0, 0.0, 0.0, "Ninja", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(54, "Druidyzm", 0.0, 0.0, 0.0, "Druid", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(55, "Mistycyzm", 0.0, 0.0, 0.0, "Mistyk", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(56, "Umagicznianie", 0.0, 0.0, 0.0, "Umagiczniacz", null, 0.0, 0.0, 0.0, 1.0),
			new SkillInfo(57, "Rzucanie", 0.0, 0.0, 0.0, "Miotacz", null, 0.0, 0.0, 0.0, 1.0),
		};

		public static SkillInfo[] Table { get { return m_Table; } set { m_Table = value; } }
	}

	[PropertyObject]
	public class Skills : IEnumerable
	{
		private readonly Mobile m_Owner;
		private readonly Skill[] m_Skills;
		private int m_Total, m_Cap;
		private Skill m_Highest;

		#region Skill Getters & Setters
		[CommandProperty(AccessLevel.Counselor)]
		public Skill Alchemia { get { return this[SkillName.Alchemia]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Anatomia { get { return this[SkillName.Anatomia]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill WiedzaOBestiach { get { return this[SkillName.WiedzaOBestiach]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Identyfikacja { get { return this[SkillName.Identyfikacja]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill WiedzaOUzbrojeniu { get { return this[SkillName.WiedzaOUzbrojeniu]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Parowanie { get { return this[SkillName.Parowanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Rolnictwo { get { return this[SkillName.Rolnictwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Kowalstwo { get { return this[SkillName.Kowalstwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Lukmistrzostwo { get { return this[SkillName.Lukmistrzostwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Uspokajanie { get { return this[SkillName.Uspokajanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Obozowanie { get { return this[SkillName.Obozowanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Stolarstwo { get { return this[SkillName.Stolarstwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Kartografia { get { return this[SkillName.Kartografia]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Gotowanie { get { return this[SkillName.Gotowanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Wykrywanie { get { return this[SkillName.Wykrywanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Manipulacja { get { return this[SkillName.Manipulacja]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Intelekt { get { return this[SkillName.Intelekt]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Leczenie { get { return this[SkillName.Leczenie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Rybactwo { get { return this[SkillName.Rybactwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Kryminalistyka { get { return this[SkillName.Kryminalistyka]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Zielarstwo { get { return this[SkillName.Zielarstwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Ukrywanie { get { return this[SkillName.Ukrywanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Prowokacja { get { return this[SkillName.Prowokacja]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Inskrypcja { get { return this[SkillName.Inskrypcja]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Wlamywanie { get { return this[SkillName.Wlamywanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Magia { get { return this[SkillName.Magia]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill ObronaPrzedMagia { get { return this[SkillName.ObronaPrzedMagia]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Taktyka { get { return this[SkillName.Taktyka]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Zagladanie { get { return this[SkillName.Zagladanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Muzykowanie { get { return this[SkillName.Muzykowanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Zatruwanie { get { return this[SkillName.Zatruwanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Lucznictwo { get { return this[SkillName.Lucznictwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill MowaDuchow { get { return this[SkillName.MowaDuchow]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Okradanie { get { return this[SkillName.Okradanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Krawiectwo { get { return this[SkillName.Krawiectwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Oswajanie { get { return this[SkillName.Oswajanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill OcenaSmaku { get { return this[SkillName.OcenaSmaku]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Majsterkowanie { get { return this[SkillName.Majsterkowanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Tropienie { get { return this[SkillName.Tropienie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Weterynaria { get { return this[SkillName.Weterynaria]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill WalkaMieczami { get { return this[SkillName.WalkaMieczami]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill WalkaObuchami { get { return this[SkillName.WalkaObuchami]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill WalkaSzpadami { get { return this[SkillName.WalkaSzpadami]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Boks { get { return this[SkillName.Boks]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Drwalnictwo { get { return this[SkillName.Drwalnictwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Gornictwo { get { return this[SkillName.Gornictwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Medytacja { get { return this[SkillName.Medytacja]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Zakradanie { get { return this[SkillName.Zakradanie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill UsuwaniePulapek { get { return this[SkillName.UsuwaniePulapek]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Nekromancja { get { return this[SkillName.Nekromancja]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Logistyka { get { return this[SkillName.Logistyka]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Rycerstwo { get { return this[SkillName.Rycerstwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Fanatyzm { get { return this[SkillName.Fanatyzm]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Skrytobojstwo { get { return this[SkillName.Skrytobojstwo]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Druidyzm { get { return this[SkillName.Druidyzm]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Mistycyzm { get { return this[SkillName.Mistycyzm]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Umagicznianie { get { return this[SkillName.Umagicznianie]; } set { } }

		[CommandProperty(AccessLevel.Counselor)]
		public Skill Rzucanie { get { return this[SkillName.Rzucanie]; } set { } }
		#endregion

		[CommandProperty(AccessLevel.Counselor, AccessLevel.GameMaster)]
		public int Cap { get { return m_Cap; } set { m_Cap = value; } }

		public int Total { get { return m_Total; } set { m_Total = value; } }

		public Mobile Owner { get { return m_Owner; } }

		public int Length { get { return m_Skills.Length; } }

		public Skill this[SkillName name] { get { return this[(int)name]; } }

		public Skill this[int skillID]
		{
			get
			{
				if (skillID < 0 || skillID >= m_Skills.Length)
				{
					return null;
				}

				Skill sk = m_Skills[skillID];

				if (sk == null)
				{
					m_Skills[skillID] = sk = new Skill(this, SkillInfo.Table[skillID], 0, 1000, SkillLock.Up);
				}

				return sk;
			}
		}

		public override string ToString()
		{
			return "...";
		}

		public static bool UseSkill(Mobile from, SkillName name)
		{
			return UseSkill(from, (int)name);
		}

		public static bool UseSkill(Mobile from, int skillID)
		{
			if (!from.CheckAlive())
			{
				return false;
			}
			else if (!from.Region.OnSkillUse(from, skillID))
			{
				return false;
			}
			else if (!from.AllowSkillUse((SkillName)skillID))
			{
				return false;
			}

			if (skillID >= 0 && skillID < SkillInfo.Table.Length)
			{
				SkillInfo info = SkillInfo.Table[skillID];

				if (info.Callback != null)
				{
					if (Core.TickCount - from.NextSkillTime >= 0 && from.Spell == null)
					{
						from.DisruptiveAction();

						from.NextSkillTime = Core.TickCount + (int)(info.Callback(from)).TotalMilliseconds;

						return true;
					}
					else
					{
						from.SendSkillMessage();
					}
				}
				else
				{
					from.SendLocalizedMessage(500014); // That skill cannot be used directly.
				}
			}

			return false;
		}

		public Skill Highest
		{
			get
			{
				if (m_Highest == null)
				{
					Skill highest = null;
					int value = int.MinValue;

					for (int i = 0; i < m_Skills.Length; ++i)
					{
						Skill sk = m_Skills[i];

						if (sk != null && sk.BaseFixedPoint > value)
						{
							value = sk.BaseFixedPoint;
							highest = sk;
						}
					}

					if (highest == null && m_Skills.Length > 0)
					{
						highest = this[0];
					}

					m_Highest = highest;
				}

				return m_Highest;
			}
		}

		public void Serialize(GenericWriter writer)
		{
			m_Total = 0;

			writer.Write(3); // version

			writer.Write(m_Cap);
			writer.Write(m_Skills.Length);

			for (int i = 0; i < m_Skills.Length; ++i)
			{
				Skill sk = m_Skills[i];

				if (sk == null)
				{
					writer.Write((byte)0xFF);
				}
				else
				{
					sk.Serialize(writer);
					m_Total += sk.BaseFixedPoint;
				}
			}
		}

		public Skills(Mobile owner)
		{
			m_Owner = owner;
			m_Cap = 7000;

			SkillInfo[] info = SkillInfo.Table;

			m_Skills = new Skill[info.Length];

			//for ( int i = 0; i < info.Length; ++i )
			//	m_Skills[i] = new Skill( this, info[i], 0, 1000, SkillLock.Up );
		}

		public Skills(Mobile owner, GenericReader reader)
		{
			m_Owner = owner;

			int version = reader.ReadInt();

			switch (version)
			{
				case 3:
				case 2:
					{
						m_Cap = reader.ReadInt();

						goto case 1;
					}
				case 1:
					{
						if (version < 2)
						{
							m_Cap = 7000;
						}

						if (version < 3)
						{
							/*m_Total =*/
							reader.ReadInt();
						}

						SkillInfo[] info = SkillInfo.Table;

						m_Skills = new Skill[info.Length];

						int count = reader.ReadInt();

						for (int i = 0; i < count; ++i)
						{
							if (i < info.Length)
							{
								var sk = new Skill(this, info[i], reader);

								if (sk.BaseFixedPoint != 0 || sk.CapFixedPoint != 1000 || sk.Lock != SkillLock.Up)
								{
									m_Skills[i] = sk;
									m_Total += sk.BaseFixedPoint;
								}
							}
							else
							{
								new Skill(this, null, reader);
							}
						}

						//for ( int i = count; i < info.Length; ++i )
						//	m_Skills[i] = new Skill( this, info[i], 0, 1000, SkillLock.Up );

						break;
					}
				case 0:
					{
						reader.ReadInt();

						goto case 1;
					}
			}
		}

		public void OnSkillChange(Skill skill)
		{
			if (skill == m_Highest) // could be downgrading the skill, force a recalc
			{
				m_Highest = null;
			}
			else if (m_Highest != null && skill.BaseFixedPoint > m_Highest.BaseFixedPoint)
			{
				m_Highest = skill;
			}

			m_Owner.OnSkillInvalidated(skill);

			NetState ns = m_Owner.NetState;

			if (ns != null)
			{
				ns.Send(new SkillChange(skill));
			}
		}

		public IEnumerator GetEnumerator()
		{
			return m_Skills.GetEnumerator();
		}
	}
}