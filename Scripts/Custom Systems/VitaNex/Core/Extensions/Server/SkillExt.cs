#region Header
//   Vorspire    _,-'/-'/  SkillExt.cs
//   .      __,-; ,'( '/
//    \.    `-.__`-._`:_,-._       _ , . ``
//     `:-._,------' ` _,`--` -: `_ , ` ,' :
//        `---..__,,--'  (C) 2014  ` -'. -'
//        #  Vita-Nex [http://core.vita-nex.com]  #
//  {o)xxx|===============-   #   -===============|xxx(o}
//        #        The MIT License (MIT)          #
#endregion

#region References
using System;
#endregion

namespace Server
{
	public static class SkillExtUtility
	{
		public static bool IsLocked(this Skill skill, SkillLock locked)
		{
			return skill.Lock == locked;
		}

		public static bool IsCapped(this Skill skill)
		{
			return skill.Base >= skill.Cap;
		}

		public static bool IsZero(this Skill skill)
		{
			return skill.Base <= 0;
		}

		public static bool IsZeroOrCapped(this Skill skill)
		{
			return IsZero(skill) || IsCapped(skill);
		}

		public static bool WillCap(this Skill skill, double value, bool isEqual = true)
		{
			return isEqual ? (skill.Base + value >= skill.Cap) : (skill.Base + value > skill.Cap);
		}

		public static bool WillZero(this Skill skill, double value, bool isEqual = true)
		{
			return isEqual ? (skill.Base - value <= 0) : (skill.Base - value < 0);
		}

		public static bool DecreaseBase(this Skill skill, double value, bool ignoreZero = false, bool trim = true)
		{
			if (trim)
			{
				value = Math.Min(skill.Base, value);
			}

			if (ignoreZero || (!IsZero(skill) && !WillZero(skill, value, false)))
			{
				skill.Base -= value;
				return true;
			}

			return false;
		}

		public static bool IncreaseBase(this Skill skill, double value, bool ignoreCap = false, bool trim = true)
		{
			if (trim)
			{
				value = Math.Min(skill.Cap - skill.Base, value);
			}

			if (ignoreCap || (!IsCapped(skill) && !WillCap(skill, value, false)))
			{
				skill.Base += value;
				return true;
			}

			return false;
		}

		public static bool SetBase(this Skill skill, double value, bool ignoreLimits = false, bool trim = true)
		{
			if (trim)
			{
				value = Math.Max(0, Math.Min(skill.Cap, value));
			}

			if (ignoreLimits || (value < skill.Base && !IsZero(skill) && !WillZero(skill, skill.Base - value, false)) ||
				(value > skill.Base && !IsCapped(skill) && !WillCap(skill, value - skill.Base, false)))
			{
				skill.Base = value;
				return true;
			}

			return false;
		}

		public static void DecreaseCap(this Skill skill, double value)
		{
			SetCap(skill, skill.Cap - value);
		}

		public static void IncreaseCap(this Skill skill, double value)
		{
			SetCap(skill, skill.Cap + value);
		}

		public static void SetCap(this Skill skill, double value)
		{
			skill.Cap = Math.Max(0, value);
			Normalize(skill);
		}

		public static void Normalize(this Skill skill)
		{
			if (IsCapped(skill))
			{
				skill.BaseFixedPoint = skill.CapFixedPoint;
			}

			if (IsZero(skill))
			{
				skill.BaseFixedPoint = 0;
			}
		}

		public static readonly SkillName[] CombatSkills = new[]
		{
			SkillName.Lucznictwo, SkillName.Rycerstwo, SkillName.WalkaSzpadami, SkillName.Logistyka, SkillName.WalkaObuchami, SkillName.Parowanie,
			SkillName.WalkaMieczami, SkillName.Taktyka, SkillName.Boks, SkillName.Fanatyzm
		};

		public static readonly SkillName[] HealingSkills = new[] {SkillName.Leczenie, SkillName.Weterynaria};

		public static readonly SkillName[] MagicSkills = new[]
		{
			SkillName.Alchemia, SkillName.Intelekt, SkillName.Inskrypcja, SkillName.Magia, SkillName.Medytacja,
			SkillName.Nekromancja, SkillName.ObronaPrzedMagia, SkillName.Druidyzm, SkillName.MowaDuchow
		};

		public static readonly SkillName[] BardicSkills = new[]
		{SkillName.Manipulacja, SkillName.Muzykowanie, SkillName.Uspokajanie, SkillName.Prowokacja};

		public static readonly SkillName[] RogueSkills = new[]
		{
			SkillName.Rolnictwo, SkillName.Wykrywanie, SkillName.Ukrywanie, SkillName.Wlamywanie, SkillName.Zatruwanie,
			SkillName.UsuwaniePulapek, SkillName.Zagladanie, SkillName.Okradanie, SkillName.Zakradanie, SkillName.Skrytobojstwo
		};

		public static readonly SkillName[] KnowledgeSkills = new[]
		{
			SkillName.Anatomia, SkillName.WiedzaOBestiach, SkillName.Oswajanie, SkillName.WiedzaOUzbrojeniu, SkillName.Obozowanie,
			SkillName.Kryminalistyka, SkillName.Zielarstwo, SkillName.Identyfikacja, SkillName.OcenaSmaku, SkillName.Tropienie
		};

		public static readonly SkillName[] CraftSkills = new[]
		{
			SkillName.Kowalstwo, SkillName.Lukmistrzostwo, SkillName.Stolarstwo, SkillName.Gotowanie, SkillName.Kartografia,
			SkillName.Krawiectwo, SkillName.Majsterkowanie, SkillName.Umagicznianie
		};

		public static readonly SkillName[] HarvestSkills = new[]
		{SkillName.Rybactwo, SkillName.Gornictwo, SkillName.Drwalnictwo};

		public static bool IsCombat(this SkillName skill)
		{
			return CombatSkills.Contains(skill);
		}

		public static bool IsHealing(this SkillName skill)
		{
			return HealingSkills.Contains(skill);
		}

		public static bool IsMagic(this SkillName skill)
		{
			return MagicSkills.Contains(skill);
		}

		public static bool IsBardic(this SkillName skill)
		{
			return BardicSkills.Contains(skill);
		}

		public static bool IsRogue(this SkillName skill)
		{
			return RogueSkills.Contains(skill);
		}

		public static bool IsKnowledge(this SkillName skill)
		{
			return KnowledgeSkills.Contains(skill);
		}

		public static bool IsCraft(this SkillName skill)
		{
			return CraftSkills.Contains(skill);
		}

		public static bool IsHarvest(this SkillName skill)
		{
			return HarvestSkills.Contains(skill);
		}
	}
}