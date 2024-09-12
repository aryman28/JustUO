using System;
using System.Collections;

namespace Server.Items
{
    public abstract class BaseRunicTool : BaseTool
    {
        private static readonly SkillName[] m_PossibleBonusSkills = new SkillName[]
        {
            SkillName.WalkaMieczami,
            SkillName.WalkaSzpadami,
            SkillName.WalkaObuchami,
            SkillName.Lucznictwo,
            SkillName.Boks,
            SkillName.Parowanie,
            SkillName.Taktyka,
            SkillName.Anatomia,
            SkillName.Leczenie,
            SkillName.Magia,
            SkillName.Medytacja,
            SkillName.Intelekt,
            SkillName.ObronaPrzedMagia,
            SkillName.Oswajanie,
            SkillName.WiedzaOBestiach,
            SkillName.Weterynaria,
            SkillName.Muzykowanie,
            SkillName.Prowokacja,
            SkillName.Manipulacja,
            SkillName.Uspokajanie,
            SkillName.Rycerstwo,
            SkillName.Logistyka,
            SkillName.Nekromancja,
            SkillName.Okradanie,
            SkillName.Zakradanie,
            SkillName.MowaDuchow,
            SkillName.Fanatyzm,
            SkillName.Skrytobojstwo,
////Dodane Skille
            SkillName.Alchemia,
            SkillName.Identyfikacja,
            SkillName.WiedzaOUzbrojeniu,
            SkillName.Rolnictwo,
            SkillName.Kowalstwo,
            SkillName.Lukmistrzostwo,
            SkillName.Obozowanie,
            SkillName.Stolarstwo,
            SkillName.Kartografia,
            SkillName.Gotowanie,
            SkillName.Wykrywanie,
            SkillName.Rybactwo,
            SkillName.Kryminalistyka,
            SkillName.Zielarstwo,
            SkillName.Ukrywanie,
            SkillName.Inskrypcja,
            SkillName.Wlamywanie,
            SkillName.Zagladanie,
            SkillName.Zatruwanie,
            SkillName.Krawiectwo,
            SkillName.OcenaSmaku,
            SkillName.Majsterkowanie,
            SkillName.Tropienie,
            SkillName.Drwalnictwo,
            SkillName.Gornictwo,
            SkillName.UsuwaniePulapek,
            SkillName.Druidyzm,
            SkillName.Mistycyzm,
            SkillName.Umagicznianie,
            SkillName.Rzucanie
////
        };
        private static readonly SkillName[] m_PossibleSpellbookSkills = new SkillName[]
        {
            SkillName.Magia,
            SkillName.Medytacja,
            SkillName.Intelekt,
            SkillName.ObronaPrzedMagia
        };
        private static readonly BitArray m_Props = new BitArray(MaxProperties);
        private static readonly int[] m_Possible = new int[MaxProperties];
        private static bool m_IsRunicTool;
        private static int m_LuckChance;
        private const int MaxProperties = 32;
        private CraftResource m_Resource;
        public BaseRunicTool(CraftResource resource, int itemID)
            : base(itemID)
        {
            this.m_Resource = resource;
        }

        public BaseRunicTool(CraftResource resource, int uses, int itemID)
            : base(uses, itemID)
        {
            this.m_Resource = resource;
        }

        public BaseRunicTool(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public CraftResource Resource
        {
            get
            {
                return this.m_Resource;
            }
            set
            {
                this.m_Resource = value;
                this.Hue = CraftResources.GetHue(this.m_Resource);
                this.InvalidateProperties();
            }
        }
        public static int GetUniqueRandom(int count)
        {
            int avail = 0;

            for (int i = 0; i < count; ++i)
            {
                if (!m_Props[i])
                    m_Possible[avail++] = i;
            }

            if (avail == 0)
                return -1;

            int v = m_Possible[Utility.Random(avail)];

            m_Props.Set(v, true);

            return v;
        }

        public static void ApplyAttributesTo(BaseWeapon weapon, int attributeCount, int min, int max)
        {
            ApplyAttributesTo(weapon, false, 0, attributeCount, min, max);
        }

        public static void ApplyAttributesTo(BaseWeapon weapon, bool isRunicTool, int luckChance, int attributeCount, int min, int max)
        {
            m_IsRunicTool = isRunicTool;
            m_LuckChance = luckChance;

            AosAttributes primary = weapon.Attributes;
            AosWeaponAttributes secondary = weapon.WeaponAttributes;

            m_Props.SetAll(false);

            if (weapon is BaseRanged)
            {
                m_Props.Set(2, true); // ranged weapons cannot be ubws or mageweapon
            }
            else
            {
            	m_Props.Set(25, true); // Only bows can be Balanced
            	m_Props.Set(26, true); // Only bows have Velocity
            }

            for (int i = 0; i < attributeCount; ++i)
            {
                int random = GetUniqueRandom(27);

                if (random == -1)
                    break;

                switch ( random )
                {
                    case 0:
                        {
                            switch ( Utility.Random(5) )
                            {
                                case 0:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitPhysicalArea, 2, 50, 2);
                                    break;
                                case 1:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitFireArea, 2, 50, 2);
                                    break;
                                case 2:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitColdArea, 2, 50, 2);
                                    break;
                                case 3:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitPoisonArea, 2, 50, 2);
                                    break;
                                case 4:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitEnergyArea, 2, 50, 2);
                                    break;
                            }

                            break;
                        }
                    case 1:
                        {
                            switch ( Utility.Random(4) )
                            {
                                case 0:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitMagicArrow, 2, 50, 2);
                                    break;
                                case 1:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitHarm, 2, 50, 2);
                                    break;
                                case 2:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitFireball, 2, 50, 2);
                                    break;
                                case 3:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitLightning, 2, 50, 2);
                                    break;
                            }

                            break;
                        }
                    case 2:
                        {
                            switch ( Utility.Random(2) )
                            {
                                case 0:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.UseBestSkill, 1, 1);
                                    break;
                                case 1:
                                    ApplyAttribute(secondary, min, max, AosWeaponAttribute.MageWeapon, 1, 10);
                                    break;
                            }

                            break;
                        }
                    case 3:
                        ApplyAttribute(primary, min, max, AosAttribute.WeaponDamage, 1, 50);
                        break;
                    case 4:
                        ApplyAttribute(primary, min, max, AosAttribute.DefendChance, 1, 15);
                        break;
                    case 5:
                        ApplyAttribute(primary, min, max, AosAttribute.CastSpeed, 1, 1);
                        break;
                    case 6:
                        ApplyAttribute(primary, min, max, AosAttribute.AttackChance, 1, 15);
                        break;
                    case 7:
                        ApplyAttribute(primary, min, max, AosAttribute.Luck, 1, 100);
                        break;
                    case 8:
                        ApplyAttribute(primary, min, max, AosAttribute.WeaponSpeed, 5, 30, 5);
                        break;
                    case 9:
                        ApplyAttribute(primary, min, max, AosAttribute.SpellChanneling, 1, 1);
                        break;
                    case 10:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitDispel, 2, 50, 2);
                        break;
                    case 11:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitLeechHits, 2, 50, 2);
                        break;
                    case 12:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitLowerAttack, 2, 50, 2);
                        break;
                    case 13:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitLowerDefend, 2, 50, 2);
                        break;
                    case 14:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitLeechMana, 2, 50, 2);
                        break;
                    case 15:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.HitLeechStam, 2, 50, 2);
                        break;
                    case 16:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.LowerStatReq, 10, 100, 10);
                        break;
                    case 17:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.ResistPhysicalBonus, 1, 15);
                        break;
                    case 18:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.ResistFireBonus, 1, 15);
                        break;
                    case 19:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.ResistColdBonus, 1, 15);
                        break;
                    case 20:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.ResistPoisonBonus, 1, 15);
                        break;
                    case 21:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.ResistEnergyBonus, 1, 15);
                        break;
                    case 22:
                        ApplyAttribute(secondary, min, max, AosWeaponAttribute.DurabilityBonus, 10, 100, 10);
                        break;
                    case 23:
                        weapon.Slayer = GetRandomSlayer();
                        break;
                    case 24:
                        GetElementalDamages(weapon);
                        break;
                    case 25:
                        BaseRanged brb = weapon as BaseRanged;
                        brb.Balanced = true;
                        break;
                    case 26:
                        BaseRanged brv = weapon as BaseRanged;
                    	brv.Velocity = (Utility.RandomMinMax(2,50));
                   		break;
                }
            }
        }

        public static void GetElementalDamages(BaseWeapon weapon)
        {
            GetElementalDamages(weapon, true);
        }

        public static void GetElementalDamages(BaseWeapon weapon, bool randomizeOrder)
        {
            int fire, phys, cold, nrgy, pois, chaos, direct;

            weapon.GetDamageTypes(null, out phys, out fire, out cold, out pois, out nrgy, out chaos, out direct);

            int totalDamage = phys;

            AosElementAttribute[] attrs = new AosElementAttribute[]
            {
                AosElementAttribute.Cold,
                AosElementAttribute.Energy,
                AosElementAttribute.Fire,
                AosElementAttribute.Poison
            };

            if (randomizeOrder)
            {
                for (int i = 0; i < attrs.Length; i++)
                {
                    int rand = Utility.Random(attrs.Length);
                    AosElementAttribute temp = attrs[i];

                    attrs[i] = attrs[rand];
                    attrs[rand] = temp;
                }
            }

            /*
            totalDamage = AssignElementalDamage( weapon, AosElementAttribute.Cold,		totalDamage );
            totalDamage = AssignElementalDamage( weapon, AosElementAttribute.Energy,	totalDamage );
            totalDamage = AssignElementalDamage( weapon, AosElementAttribute.Fire,		totalDamage );
            totalDamage = AssignElementalDamage( weapon, AosElementAttribute.Poison,	totalDamage );

            weapon.AosElementDamages[AosElementAttribute.Physical] = 100 - totalDamage;
            * */

            for (int i = 0; i < attrs.Length; i++)
                totalDamage = AssignElementalDamage(weapon, attrs[i], totalDamage);

            //Order is Cold, Energy, Fire, Poison -> Physical left
            //Cannot be looped, AoselementAttribute is 'out of order'

            weapon.Hue = weapon.GetElementalDamageHue();
        }

        public static SlayerName GetRandomSlayer()
        {
            // TODO: Check random algorithm on OSI
            SlayerGroup[] groups = SlayerGroup.Groups;

            if (groups.Length == 0)
                return SlayerName.None;

            SlayerGroup group = groups[Utility.Random(groups.Length - 1)]; //-1 To Exclude the Fey Slayer which appears ONLY on a certain artifact.
            SlayerEntry entry;

            if (group.Entries.Length == 0 || 10 > Utility.Random(100)) // 10% chance to do super slayer
            {
                entry = group.Super;
            }
            else
            {
                SlayerEntry[] entries = group.Entries;
                entry = entries[Utility.Random(entries.Length)];
            }

            return entry.Name;
        }

        public static void ApplyAttributesTo(BaseArmor armor, int attributeCount, int min, int max)
        {
            ApplyAttributesTo(armor, false, 0, attributeCount, min, max);
        }

        public static void ApplyAttributesTo(BaseArmor armor, bool isRunicTool, int luckChance, int attributeCount, int min, int max)
        {
            m_IsRunicTool = isRunicTool;
            m_LuckChance = luckChance;

            AosAttributes primary = armor.Attributes;
            AosArmorAttributes secondary = armor.ArmorAttributes;

            m_Props.SetAll(false);

            bool isShield = (armor is BaseShield);
            int baseCount = (isShield ? 6 : 20);
            int baseOffset = (isShield ? 0 : 3);

            if (!isShield && armor.MeditationAllowance == ArmorMeditationAllowance.All)
                m_Props.Set(3, true); // remove mage armor from possible properties
            if (armor.Resource >= CraftResource.RegularLeather && armor.Resource <= CraftResource.BarbedLeather)
            {
                m_Props.Set(0, true); // remove lower requirements from possible properties for leather armor
                m_Props.Set(2, true); // remove durability bonus from possible properties
            }
            if (armor.RequiredRace == Race.Elf)
                m_Props.Set(7, true); // elves inherently have night sight and elf only armor doesn't get night sight as a mod

            for (int i = 0; i < attributeCount; ++i)
            {
                int random = GetUniqueRandom(baseCount);

                if (random == -1)
                    break;

                random += baseOffset;

                switch ( random )
                {
                    /* Begin Sheilds */
                    case 0:
                        ApplyAttribute(primary, min, max, AosAttribute.SpellChanneling, 1, 1);
                        break;
                    case 1:

                       switch ( Utility.Random(2) )
                       {
                             case 0:
                                ApplyAttribute(primary, min, max, AosAttribute.DefendChance, 1, 20);
                                break;
                             case 1:
                              if (Core.ML)
                              {
                                ApplyAttribute(primary, min, max, AosAttribute.ReflectPhysical, 1, 20);
                                break;
                              }
                              else
                              {
                                ApplyAttribute(primary, min, max, AosAttribute.AttackChance, 1, 20);
                              }
                              break;
                       }

                           break;
                    case 2:
                        ApplyAttribute(primary, min, max, AosAttribute.CastSpeed, 1, 1);
                        break;

                        /* Begin Armor */
                    case 3:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.LowerStatReq, 10, 100, 10);
                        break;
                    case 4:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.SelfRepair, 1, 5);
                        break;
                    case 5:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.DurabilityBonus, 10, 100, 10);
                        break;
                        /* End Shields */
                    case 6:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.MageArmor, 1, 1);
                        break;
                    case 8:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenHits, 1, 2);
                        break;
                    case 9:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenStam, 1, 3);
                        break;
                    case 10:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenMana, 1, 2);
                        break;
                    case 11:
                         ApplyAttribute(primary, min, max, AosAttribute.BonusHits, 1, 11);
                        break;
                    case 12:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusStam, 1, 11);
                        break;
                    case 13:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusMana, 1, 11);
                        break;
                    case 14:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerManaCost, 1, 8);
                        break;
                    case 15:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerRegCost, 1, 8);
                        break;
                    case 16:
                        ApplyAttribute(primary, min, max, AosAttribute.Luck, 1, 100);
                        break;
                    case 17:
                        ApplyAttribute(primary, min, max, AosAttribute.ReflectPhysical, 1, 15);
                        break;
                    case 18:
switch ( Utility.Random(5) )
{
                             case 0:
                               ApplyResistance(armor, min, max, ResistanceType.Physical, 1, 18);
                               break;
                             case 1:
                               ApplyResistance(armor, min, max, ResistanceType.Fire, 1, 18);
                               break;
                             case 2:
                               ApplyResistance(armor, min, max, ResistanceType.Cold, 1, 18);
                               break;
                             case 3:
                               ApplyResistance(armor, min, max, ResistanceType.Poison, 1, 18);
                               break;
                             case 4:
                               ApplyResistance(armor, min, max, ResistanceType.Energy, 1, 18);
                               break;
}    //Koniec Switch
break;
                  /* End Armor */
                } //Koniec ApplyAttributes 

if (armor.ArmorAttributes.DurabilityBonus > armor.Attributes.DefendChance || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.ReflectPhysical || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.AttackChance || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.BonusStam || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.BonusMana || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.BonusHits
|| armor.ArmorAttributes.DurabilityBonus > armor.Attributes.LowerManaCost || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.LowerRegCost || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.RegenHits || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.RegenStam || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.RegenMana || armor.ArmorAttributes.DurabilityBonus > armor.Attributes.RegenHits
|| armor.ArmorAttributes.DurabilityBonus > armor.Attributes.Luck || armor.ArmorAttributes.DurabilityBonus > armor.PhysicalBonus || armor.ArmorAttributes.DurabilityBonus > armor.FireBonus || armor.ArmorAttributes.DurabilityBonus > armor.ColdBonus || armor.ArmorAttributes.DurabilityBonus > armor.PoisonBonus || armor.ArmorAttributes.DurabilityBonus > armor.EnergyBonus || armor.ArmorAttributes.DurabilityBonus > armor.ArmorAttributes.SelfRepair )
{
                        if ( armor.ArmorAttributes.DurabilityBonus >= 45 && armor.ArmorAttributes.DurabilityBonus < 50 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 50 && armor.ArmorAttributes.DurabilityBonus < 55 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 55 && armor.ArmorAttributes.DurabilityBonus < 60 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 60 && armor.ArmorAttributes.DurabilityBonus < 65  )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 65 && armor.ArmorAttributes.DurabilityBonus < 70 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 70 && armor.ArmorAttributes.DurabilityBonus < 75 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 75 && armor.ArmorAttributes.DurabilityBonus < 80 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 80 && armor.ArmorAttributes.DurabilityBonus < 85 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 85 && armor.ArmorAttributes.DurabilityBonus < 90 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 90 && armor.ArmorAttributes.DurabilityBonus < 95 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.ArmorAttributes.DurabilityBonus >= 95 && armor.ArmorAttributes.DurabilityBonus <= 100 )
                        {
                        armor.Hue = 447;
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.Attributes.DefendChance > armor.ArmorAttributes.SelfRepair || armor.Attributes.DefendChance > armor.Attributes.ReflectPhysical || armor.Attributes.DefendChance > armor.Attributes.AttackChance || armor.Attributes.DefendChance > armor.Attributes.BonusStam || armor.Attributes.DefendChance > armor.Attributes.BonusMana || armor.Attributes.DefendChance > armor.Attributes.BonusHits
|| armor.Attributes.DefendChance > armor.Attributes.LowerManaCost || armor.Attributes.DefendChance > armor.Attributes.LowerRegCost || armor.Attributes.DefendChance > armor.Attributes.RegenHits || armor.Attributes.DefendChance > armor.Attributes.RegenStam || armor.Attributes.DefendChance > armor.Attributes.RegenMana || armor.Attributes.DefendChance > armor.Attributes.RegenHits
|| armor.Attributes.DefendChance > armor.Attributes.Luck || armor.Attributes.DefendChance > armor.PhysicalBonus || armor.Attributes.DefendChance > armor.FireBonus || armor.Attributes.DefendChance > armor.ColdBonus || armor.Attributes.DefendChance > armor.PoisonBonus || armor.Attributes.DefendChance > armor.EnergyBonus || armor.Attributes.DefendChance > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.Attributes.DefendChance >= 1 && armor.Attributes.DefendChance <= 5 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.DefendChance == 6 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.DefendChance == 7 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.DefendChance == 8 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.DefendChance == 9 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.DefendChance == 10 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.DefendChance == 11 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.DefendChance == 12 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.DefendChance == 13 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.DefendChance == 14 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.DefendChance == 15 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.DefendChance == 16 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.DefendChance == 17 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.DefendChance == 18 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.DefendChance == 19 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.DefendChance >= 20 )
                        {
                        armor.Hue = 45;
                        armor.Cechy = ArmorCechy.Obronn;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.Attributes.ReflectPhysical > armor.Attributes.DefendChance || armor.Attributes.ReflectPhysical > armor.ArmorAttributes.SelfRepair || armor.Attributes.ReflectPhysical > armor.Attributes.AttackChance || armor.Attributes.ReflectPhysical > armor.Attributes.BonusStam || armor.Attributes.ReflectPhysical > armor.Attributes.BonusMana || armor.Attributes.ReflectPhysical > armor.Attributes.BonusHits
|| armor.Attributes.ReflectPhysical > armor.Attributes.LowerManaCost || armor.Attributes.ReflectPhysical > armor.Attributes.LowerRegCost || armor.Attributes.ReflectPhysical > armor.Attributes.RegenHits || armor.Attributes.ReflectPhysical > armor.Attributes.RegenStam || armor.Attributes.ReflectPhysical > armor.Attributes.RegenMana || armor.Attributes.ReflectPhysical > armor.Attributes.RegenHits
|| armor.Attributes.ReflectPhysical > armor.Attributes.Luck || armor.Attributes.ReflectPhysical > armor.PhysicalBonus || armor.Attributes.ReflectPhysical > armor.FireBonus || armor.Attributes.ReflectPhysical > armor.ColdBonus || armor.Attributes.ReflectPhysical > armor.PoisonBonus || armor.Attributes.ReflectPhysical > armor.EnergyBonus || armor.Attributes.ReflectPhysical > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.Attributes.ReflectPhysical >= 1 && armor.Attributes.ReflectPhysical <= 5 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 6 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 7 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 8 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 9 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 10 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 11 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.ReflectPhysical == 12 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 13 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.ReflectPhysical == 14 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 15 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 16 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.ReflectPhysical == 17 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 18 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.ReflectPhysical == 19 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.ReflectPhysical == 20 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.ArmorAttributes.SelfRepair > armor.Attributes.DefendChance || armor.ArmorAttributes.SelfRepair > armor.Attributes.ReflectPhysical || armor.ArmorAttributes.SelfRepair > armor.Attributes.AttackChance || armor.ArmorAttributes.SelfRepair > armor.Attributes.BonusStam || armor.ArmorAttributes.SelfRepair > armor.Attributes.BonusMana || armor.ArmorAttributes.SelfRepair > armor.Attributes.BonusHits
|| armor.ArmorAttributes.SelfRepair > armor.Attributes.LowerManaCost || armor.ArmorAttributes.SelfRepair > armor.Attributes.LowerRegCost || armor.ArmorAttributes.SelfRepair > armor.Attributes.RegenHits || armor.ArmorAttributes.SelfRepair > armor.Attributes.RegenStam || armor.ArmorAttributes.SelfRepair > armor.Attributes.RegenMana || armor.ArmorAttributes.SelfRepair > armor.Attributes.RegenHits
|| armor.ArmorAttributes.SelfRepair > armor.Attributes.Luck || armor.ArmorAttributes.SelfRepair > armor.PhysicalBonus || armor.ArmorAttributes.SelfRepair > armor.FireBonus || armor.ArmorAttributes.SelfRepair > armor.ColdBonus || armor.ArmorAttributes.SelfRepair > armor.PoisonBonus || armor.ArmorAttributes.SelfRepair > armor.EnergyBonus || armor.ArmorAttributes.SelfRepair > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.ArmorAttributes.SelfRepair == 1 )
                        {
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.ArmorAttributes.SelfRepair == 2 )
                        {
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.ArmorAttributes.SelfRepair == 3 )
                        {
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.ArmorAttributes.SelfRepair == 4 )
                        {
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.ArmorAttributes.SelfRepair == 5 )
                        {
                        armor.Cechy = ArmorCechy.Wytrzyma³;
                        armor.Quality = ArmorQuality.Dobr;
                        }  
}
if (armor.Attributes.RegenHits > armor.ArmorAttributes.SelfRepair || armor.Attributes.RegenHits > armor.Attributes.ReflectPhysical || armor.Attributes.RegenHits > armor.Attributes.DefendChance || armor.Attributes.RegenHits > armor.Attributes.AttackChance || armor.Attributes.RegenHits > armor.Attributes.BonusStam || armor.Attributes.RegenHits > armor.Attributes.BonusMana
|| armor.Attributes.RegenHits > armor.Attributes.BonusHits || armor.Attributes.RegenHits > armor.Attributes.LowerManaCost || armor.Attributes.RegenHits > armor.Attributes.LowerRegCost || armor.Attributes.RegenHits > armor.Attributes.RegenStam || armor.Attributes.RegenHits > armor.Attributes.RegenMana
|| armor.Attributes.RegenHits > armor.Attributes.Luck || armor.Attributes.RegenHits > armor.PhysicalBonus || armor.Attributes.RegenHits > armor.FireBonus || armor.Attributes.RegenHits > armor.ColdBonus || armor.Attributes.RegenHits > armor.PoisonBonus || armor.Attributes.RegenHits > armor.EnergyBonus || armor.Attributes.RegenHits > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.RegenHits == 1 )
                        {
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.RegenHits == 2 )
                        {
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
}
if (armor.Attributes.RegenStam > armor.ArmorAttributes.SelfRepair || armor.Attributes.RegenStam > armor.Attributes.ReflectPhysical || armor.Attributes.RegenStam > armor.Attributes.DefendChance || armor.Attributes.RegenStam > armor.Attributes.AttackChance || armor.Attributes.RegenStam > armor.Attributes.BonusStam || armor.Attributes.RegenStam > armor.Attributes.BonusMana
|| armor.Attributes.RegenStam > armor.Attributes.BonusHits || armor.Attributes.RegenStam > armor.Attributes.LowerManaCost || armor.Attributes.RegenStam > armor.Attributes.LowerRegCost || armor.Attributes.RegenStam > armor.Attributes.RegenHits || armor.Attributes.RegenStam > armor.Attributes.RegenMana
|| armor.Attributes.RegenStam > armor.Attributes.Luck || armor.Attributes.RegenStam > armor.PhysicalBonus || armor.Attributes.RegenStam > armor.FireBonus || armor.Attributes.RegenStam > armor.ColdBonus || armor.Attributes.RegenStam > armor.PoisonBonus || armor.Attributes.RegenStam > armor.EnergyBonus || armor.Attributes.RegenStam > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.RegenStam == 1 )
                        {
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.RegenStam == 2 )
                        {
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.RegenStam == 3 )
                        {
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Dobr;
                        }
}
if (armor.Attributes.RegenMana > armor.ArmorAttributes.SelfRepair || armor.Attributes.RegenMana > armor.Attributes.ReflectPhysical || armor.Attributes.RegenMana > armor.Attributes.DefendChance || armor.Attributes.RegenMana > armor.Attributes.AttackChance || armor.Attributes.RegenMana > armor.Attributes.BonusStam || armor.Attributes.RegenMana > armor.Attributes.BonusMana
|| armor.Attributes.RegenMana > armor.Attributes.BonusHits || armor.Attributes.RegenMana > armor.Attributes.LowerManaCost || armor.Attributes.RegenMana > armor.Attributes.LowerRegCost || armor.Attributes.RegenMana > armor.Attributes.RegenHits || armor.Attributes.RegenMana > armor.Attributes.RegenStam
|| armor.Attributes.RegenMana > armor.Attributes.Luck || armor.Attributes.RegenMana > armor.PhysicalBonus || armor.Attributes.RegenMana > armor.FireBonus || armor.Attributes.RegenMana > armor.ColdBonus || armor.Attributes.RegenMana > armor.PoisonBonus || armor.Attributes.RegenMana > armor.EnergyBonus || armor.Attributes.RegenMana > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.RegenMana == 1 )
                        {
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.RegenMana == 2 )
                        {
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
}
if (armor.Attributes.BonusHits > armor.ArmorAttributes.SelfRepair || armor.Attributes.BonusHits > armor.Attributes.ReflectPhysical || armor.Attributes.BonusHits > armor.Attributes.AttackChance || armor.Attributes.BonusHits > armor.Attributes.BonusStam || armor.Attributes.BonusHits > armor.Attributes.BonusMana || armor.Attributes.BonusHits > armor.Attributes.DefendChance
|| armor.Attributes.BonusHits > armor.Attributes.LowerManaCost || armor.Attributes.BonusHits > armor.Attributes.LowerRegCost || armor.Attributes.BonusHits > armor.Attributes.RegenHits || armor.Attributes.BonusHits > armor.Attributes.RegenStam || armor.Attributes.BonusHits > armor.Attributes.RegenMana || armor.Attributes.BonusHits > armor.Attributes.RegenHits
|| armor.Attributes.BonusHits > armor.Attributes.Luck || armor.Attributes.BonusHits > armor.PhysicalBonus || armor.Attributes.BonusHits > armor.FireBonus || armor.Attributes.BonusHits > armor.ColdBonus || armor.Attributes.BonusHits > armor.PoisonBonus || armor.Attributes.BonusHits > armor.EnergyBonus || armor.Attributes.BonusHits > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.BonusHits == 1 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.BonusHits == 2 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.BonusHits == 3 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.BonusHits == 4 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.BonusHits == 5 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.BonusHits == 6 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.BonusHits == 7 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.BonusHits == 8 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.BonusHits == 9 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.BonusHits == 10 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.BonusHits == 11 )
                        {
                        armor.Hue = 33;
                        armor.Cechy = ArmorCechy.Witaln;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.Attributes.BonusStam > armor.Attributes.DefendChance || armor.Attributes.BonusStam > armor.Attributes.ReflectPhysical || armor.Attributes.BonusStam > armor.Attributes.AttackChance || armor.Attributes.BonusStam > armor.Attributes.BonusHits || armor.Attributes.BonusStam > armor.Attributes.BonusMana
|| armor.Attributes.BonusStam > armor.Attributes.Luck || armor.Attributes.BonusStam > armor.Attributes.LowerManaCost || armor.Attributes.BonusStam > armor.Attributes.LowerRegCost || armor.Attributes.BonusStam > armor.PhysicalResistance || armor.Attributes.BonusStam > armor.FireResistance || armor.Attributes.BonusStam > armor.ColdResistance || armor.Attributes.BonusStam > armor.PoisonResistance || armor.Attributes.BonusStam > armor.EnergyResistance
|| armor.Attributes.BonusStam > armor.PhysicalBonus || armor.Attributes.BonusStam > armor.FireBonus || armor.Attributes.BonusStam > armor.ColdBonus || armor.Attributes.BonusStam > armor.PoisonBonus || armor.Attributes.BonusStam > armor.EnergyBonus)
{
                       if ( armor.Attributes.BonusStam == 1 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.BonusStam == 2 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.BonusStam == 3 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.BonusStam == 4 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.BonusStam == 5 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.BonusStam == 6 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.BonusStam == 7 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.BonusStam == 8 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.BonusStam == 9 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.BonusStam == 10 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.BonusStam == 11 )
                        {
                        armor.Hue = 900;
                        armor.Cechy = ArmorCechy.Stabiln;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.Attributes.BonusMana > armor.ArmorAttributes.SelfRepair || armor.Attributes.BonusMana > armor.Attributes.ReflectPhysical || armor.Attributes.BonusMana > armor.Attributes.AttackChance || armor.Attributes.BonusMana > armor.Attributes.BonusStam || armor.Attributes.BonusMana > armor.Attributes.DefendChance || armor.Attributes.BonusMana > armor.Attributes.BonusHits
|| armor.Attributes.BonusMana > armor.Attributes.LowerManaCost || armor.Attributes.BonusMana > armor.Attributes.LowerRegCost || armor.Attributes.BonusMana > armor.Attributes.RegenHits || armor.Attributes.BonusMana > armor.Attributes.RegenStam || armor.Attributes.BonusMana > armor.Attributes.RegenMana || armor.Attributes.BonusMana > armor.Attributes.RegenHits
|| armor.Attributes.BonusMana > armor.Attributes.Luck || armor.Attributes.BonusMana > armor.PhysicalBonus || armor.Attributes.BonusMana > armor.FireBonus || armor.Attributes.BonusMana > armor.ColdBonus || armor.Attributes.BonusMana > armor.PoisonBonus || armor.Attributes.BonusMana > armor.EnergyBonus || armor.Attributes.BonusMana > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.BonusMana == 1 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.BonusMana == 2 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.BonusMana == 3 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.BonusMana == 4 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.BonusMana == 5 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.BonusMana == 6 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.BonusMana == 7 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.BonusMana == 8 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.BonusMana == 9 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.BonusMana == 10 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.BonusMana == 11 )
                        {
                        armor.Hue = 97;
                        armor.Cechy = ArmorCechy.M¹dr;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.Attributes.LowerManaCost > armor.ArmorAttributes.SelfRepair || armor.Attributes.LowerManaCost > armor.Attributes.ReflectPhysical || armor.Attributes.LowerManaCost > armor.Attributes.AttackChance || armor.Attributes.LowerManaCost > armor.Attributes.BonusStam || armor.Attributes.LowerManaCost > armor.Attributes.BonusMana || armor.Attributes.LowerManaCost > armor.Attributes.BonusHits
|| armor.Attributes.LowerManaCost > armor.Attributes.DefendChance || armor.Attributes.LowerManaCost > armor.Attributes.LowerRegCost || armor.Attributes.LowerManaCost > armor.Attributes.RegenHits || armor.Attributes.LowerManaCost > armor.Attributes.RegenStam || armor.Attributes.LowerManaCost > armor.Attributes.RegenMana || armor.Attributes.LowerManaCost > armor.Attributes.RegenHits
|| armor.Attributes.LowerManaCost > armor.Attributes.Luck || armor.Attributes.LowerManaCost > armor.PhysicalBonus || armor.Attributes.LowerManaCost > armor.FireBonus || armor.Attributes.LowerManaCost > armor.ColdBonus || armor.Attributes.LowerManaCost > armor.PoisonBonus || armor.Attributes.LowerManaCost > armor.EnergyBonus || armor.Attributes.LowerManaCost > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.LowerManaCost == 1 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.LowerManaCost == 2 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.LowerManaCost == 3 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.LowerManaCost == 4 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.LowerManaCost == 5 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.LowerManaCost == 6 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.LowerManaCost == 7 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.LowerManaCost == 8 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
}
if (armor.Attributes.LowerRegCost > armor.ArmorAttributes.SelfRepair || armor.Attributes.LowerRegCost > armor.Attributes.ReflectPhysical || armor.Attributes.LowerRegCost > armor.Attributes.AttackChance || armor.Attributes.LowerRegCost > armor.Attributes.BonusStam || armor.Attributes.LowerRegCost > armor.Attributes.BonusMana || armor.Attributes.LowerRegCost > armor.Attributes.BonusHits
|| armor.Attributes.LowerRegCost > armor.Attributes.LowerManaCost || armor.Attributes.LowerRegCost > armor.Attributes.DefendChance || armor.Attributes.LowerRegCost > armor.Attributes.RegenHits || armor.Attributes.LowerRegCost > armor.Attributes.RegenStam || armor.Attributes.LowerRegCost > armor.Attributes.RegenMana || armor.Attributes.LowerRegCost > armor.Attributes.RegenHits
|| armor.Attributes.LowerRegCost > armor.Attributes.Luck || armor.Attributes.LowerRegCost > armor.PhysicalBonus || armor.Attributes.LowerRegCost > armor.FireBonus || armor.Attributes.LowerRegCost > armor.ColdBonus || armor.Attributes.LowerRegCost > armor.PoisonBonus || armor.Attributes.LowerRegCost > armor.EnergyBonus || armor.Attributes.LowerRegCost > armor.ArmorAttributes.DurabilityBonus )
{
                       if ( armor.Attributes.LowerRegCost == 1 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.LowerRegCost == 2 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.LowerRegCost == 3 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.LowerRegCost == 4 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.LowerRegCost == 5 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.LowerRegCost == 6 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.LowerRegCost == 7 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.LowerRegCost == 8 )
                        {
                        armor.Hue = 557;
                        armor.Cechy = ArmorCechy.Oszczêdn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
}
if (armor.Attributes.Luck > armor.ArmorAttributes.SelfRepair || armor.Attributes.Luck > armor.Attributes.ReflectPhysical || armor.Attributes.Luck > armor.Attributes.AttackChance || armor.Attributes.Luck > armor.Attributes.BonusStam || armor.Attributes.Luck > armor.Attributes.BonusMana || armor.Attributes.Luck > armor.Attributes.BonusHits
|| armor.Attributes.Luck > armor.Attributes.LowerManaCost || armor.Attributes.Luck > armor.Attributes.LowerRegCost || armor.Attributes.Luck > armor.Attributes.RegenHits || armor.Attributes.Luck > armor.Attributes.RegenStam || armor.Attributes.Luck > armor.Attributes.RegenMana || armor.Attributes.Luck > armor.Attributes.RegenHits
|| armor.Attributes.Luck > armor.Attributes.DefendChance || armor.Attributes.Luck > armor.PhysicalBonus || armor.Attributes.Luck > armor.FireBonus || armor.Attributes.Luck > armor.ColdBonus || armor.Attributes.Luck > armor.PoisonBonus || armor.Attributes.Luck > armor.EnergyBonus || armor.Attributes.Luck > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.Attributes.Luck >= 45 && armor.Attributes.Luck < 50 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.Luck >= 50 && armor.Attributes.Luck < 55 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.Luck >= 55 && armor.Attributes.Luck < 60 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.Luck >= 60 && armor.Attributes.Luck < 65  )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.Luck >= 65 && armor.Attributes.Luck < 70 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.Luck >= 70 && armor.Attributes.Luck < 75 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.Luck >= 75 && armor.Attributes.Luck < 80 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.Luck >= 80 && armor.Attributes.Luck < 85 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.Luck >= 85 && armor.Attributes.Luck < 90 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.Luck >= 90 && armor.Attributes.Luck < 95 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.Luck >= 95 && armor.Attributes.Luck <= 100 )
                        {
                        armor.Hue = 253;
                        armor.Cechy = ArmorCechy.Szczêœliw;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.Attributes.ReflectPhysical > armor.ArmorAttributes.SelfRepair || armor.Attributes.ReflectPhysical > armor.Attributes.DefendChance || armor.Attributes.ReflectPhysical > armor.Attributes.AttackChance || armor.Attributes.ReflectPhysical > armor.Attributes.BonusStam || armor.Attributes.ReflectPhysical > armor.Attributes.BonusMana || armor.Attributes.ReflectPhysical > armor.Attributes.BonusHits
|| armor.Attributes.ReflectPhysical > armor.Attributes.ReflectPhysical || armor.Attributes.ReflectPhysical > armor.Attributes.LowerRegCost || armor.Attributes.ReflectPhysical > armor.Attributes.RegenHits || armor.Attributes.ReflectPhysical > armor.Attributes.RegenStam || armor.Attributes.ReflectPhysical > armor.Attributes.RegenMana || armor.Attributes.ReflectPhysical > armor.Attributes.RegenHits
|| armor.Attributes.ReflectPhysical > armor.Attributes.Luck || armor.Attributes.ReflectPhysical > armor.PhysicalBonus || armor.Attributes.ReflectPhysical > armor.FireBonus || armor.Attributes.ReflectPhysical > armor.ColdBonus || armor.Attributes.ReflectPhysical > armor.PoisonBonus || armor.Attributes.ReflectPhysical > armor.EnergyBonus || armor.Attributes.ReflectPhysical > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.Attributes.ReflectPhysical == 1 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 2 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 3 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 4 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 5 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.Attributes.ReflectPhysical == 6 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.Attributes.ReflectPhysical == 7 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 8 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.Attributes.ReflectPhysical == 9 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 10 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 11 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.Attributes.ReflectPhysical == 12 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.Attributes.ReflectPhysical == 13 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.Attributes.ReflectPhysical == 14 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.Attributes.ReflectPhysical == 15 )
                        {
                        armor.Hue = 43;
                        armor.Cechy = ArmorCechy.Odbijaj¹c;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.PhysicalBonus > armor.ArmorAttributes.SelfRepair || armor.PhysicalBonus > armor.Attributes.ReflectPhysical || armor.PhysicalBonus > armor.Attributes.AttackChance || armor.PhysicalBonus > armor.Attributes.BonusStam || armor.PhysicalBonus > armor.Attributes.BonusMana || armor.PhysicalBonus > armor.Attributes.BonusHits
|| armor.PhysicalBonus > armor.Attributes.LowerManaCost || armor.PhysicalBonus > armor.Attributes.LowerRegCost || armor.PhysicalBonus > armor.Attributes.RegenHits || armor.PhysicalBonus > armor.Attributes.RegenStam || armor.PhysicalBonus > armor.Attributes.RegenMana || armor.PhysicalBonus > armor.Attributes.RegenHits
|| armor.PhysicalBonus > armor.Attributes.Luck || armor.PhysicalBonus > armor.Attributes.DefendChance || armor.PhysicalBonus > armor.FireBonus || armor.PhysicalBonus > armor.ColdBonus || armor.PhysicalBonus > armor.PoisonBonus || armor.PhysicalBonus > armor.EnergyBonus || armor.PhysicalBonus > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.PhysicalBonus == 3 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.PhysicalBonus == 4 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.PhysicalBonus == 5 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.PhysicalBonus == 6 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.PhysicalBonus == 7 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.PhysicalBonus == 8 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.PhysicalBonus == 9 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.PhysicalBonus == 10 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.PhysicalBonus == 11 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.PhysicalBonus == 12 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.PhysicalBonus == 13 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.PhysicalBonus == 14 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.PhysicalBonus == 15 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.PhysicalBonus == 16 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.PhysicalBonus == 17 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.PhysicalBonus >= 18 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.FireBonus > armor.ArmorAttributes.SelfRepair || armor.FireBonus > armor.Attributes.ReflectPhysical || armor.FireBonus > armor.Attributes.AttackChance || armor.FireBonus > armor.Attributes.BonusStam || armor.FireBonus > armor.Attributes.BonusMana || armor.FireBonus > armor.Attributes.BonusHits
|| armor.FireBonus > armor.Attributes.LowerManaCost || armor.FireBonus > armor.Attributes.LowerRegCost || armor.FireBonus > armor.Attributes.RegenHits || armor.FireBonus > armor.Attributes.RegenStam || armor.FireBonus > armor.Attributes.RegenMana || armor.FireBonus > armor.Attributes.RegenHits
|| armor.FireBonus > armor.Attributes.Luck || armor.FireBonus > armor.Attributes.DefendChance || armor.FireBonus > armor.PhysicalBonus || armor.FireBonus > armor.ColdBonus || armor.FireBonus > armor.PoisonBonus || armor.FireBonus > armor.EnergyBonus || armor.FireBonus > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.FireBonus == 3 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.FireBonus == 4 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.FireBonus == 5 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.FireBonus == 6 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.FireBonus == 7 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.FireBonus == 8 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.FireBonus == 9 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.FireBonus == 10 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.FireBonus == 11 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.FireBonus == 12 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.FireBonus == 13 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.FireBonus == 14 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.FireBonus == 15 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.FireBonus == 16 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.FireBonus == 17 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.FireBonus >= 18 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.ColdBonus > armor.ArmorAttributes.SelfRepair || armor.ColdBonus > armor.Attributes.ReflectPhysical || armor.ColdBonus > armor.Attributes.AttackChance || armor.ColdBonus > armor.Attributes.BonusStam || armor.ColdBonus > armor.Attributes.BonusMana || armor.ColdBonus > armor.Attributes.BonusHits
|| armor.ColdBonus > armor.Attributes.LowerManaCost || armor.ColdBonus > armor.Attributes.LowerRegCost || armor.ColdBonus > armor.Attributes.RegenHits || armor.ColdBonus > armor.Attributes.RegenStam || armor.ColdBonus > armor.Attributes.RegenMana || armor.ColdBonus > armor.Attributes.RegenHits
|| armor.ColdBonus > armor.Attributes.Luck || armor.ColdBonus > armor.Attributes.DefendChance || armor.ColdBonus > armor.PhysicalBonus || armor.ColdBonus > armor.FireBonus || armor.ColdBonus > armor.PoisonBonus || armor.ColdBonus > armor.EnergyBonus || armor.ColdBonus > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.ColdBonus == 3 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.ColdBonus == 4 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.ColdBonus == 5 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.ColdBonus == 6 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.ColdBonus == 7 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.ColdBonus == 8 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.ColdBonus == 9 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.ColdBonus == 10 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.ColdBonus == 11 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.ColdBonus == 12 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.ColdBonus == 13 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.ColdBonus == 14 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.ColdBonus == 15 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.ColdBonus == 16 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.ColdBonus == 17 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.ColdBonus >= 18 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.PoisonBonus > armor.ArmorAttributes.SelfRepair || armor.PoisonBonus > armor.Attributes.ReflectPhysical || armor.PoisonBonus > armor.Attributes.AttackChance || armor.PoisonBonus > armor.Attributes.BonusStam || armor.PoisonBonus > armor.Attributes.BonusMana || armor.PoisonBonus > armor.Attributes.BonusHits
|| armor.PoisonBonus > armor.Attributes.LowerManaCost || armor.PoisonBonus > armor.Attributes.LowerRegCost || armor.PoisonBonus > armor.Attributes.RegenHits || armor.PoisonBonus > armor.Attributes.RegenStam || armor.PoisonBonus > armor.Attributes.RegenMana || armor.PoisonBonus > armor.Attributes.RegenHits
|| armor.PoisonBonus > armor.Attributes.Luck || armor.PoisonBonus > armor.Attributes.DefendChance || armor.PoisonBonus > armor.PhysicalBonus || armor.PoisonBonus > armor.FireBonus || armor.PoisonBonus > armor.ColdBonus || armor.PoisonBonus > armor.EnergyBonus || armor.PoisonBonus > armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.PoisonBonus == 3 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.PoisonBonus == 4 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.PoisonBonus == 5 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.PoisonBonus == 6 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.PoisonBonus == 7 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.PoisonBonus == 8 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.PoisonBonus == 9 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.PoisonBonus == 10 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.PoisonBonus == 11 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.PoisonBonus == 12 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.PoisonBonus == 13 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.PoisonBonus == 14 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.PoisonBonus == 15 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.PoisonBonus == 16 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.PoisonBonus == 17 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.PoisonBonus >= 18 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
if (armor.EnergyBonus > armor.ArmorAttributes.SelfRepair || armor.EnergyBonus > armor.Attributes.ReflectPhysical || armor.EnergyBonus > armor.Attributes.AttackChance || armor.EnergyBonus > armor.Attributes.BonusStam || armor.EnergyBonus > armor.Attributes.BonusMana || armor.EnergyBonus > armor.Attributes.BonusHits
|| armor.EnergyBonus > armor.Attributes.LowerManaCost || armor.EnergyBonus > armor.Attributes.LowerRegCost || armor.EnergyBonus > armor.Attributes.RegenHits || armor.EnergyBonus > armor.Attributes.RegenStam || armor.EnergyBonus > armor.Attributes.RegenMana || armor.EnergyBonus > armor.Attributes.RegenHits
|| armor.EnergyBonus > armor.Attributes.Luck || armor.EnergyBonus > armor.Attributes.DefendChance || armor.EnergyBonus > armor.PhysicalBonus || armor.EnergyBonus > armor.FireBonus || armor.EnergyBonus > armor.ColdBonus || armor.EnergyBonus > armor.PoisonBonus || armor.EnergyBonus> armor.ArmorAttributes.DurabilityBonus )
{
                        if ( armor.EnergyBonus == 3 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.EnergyBonus == 4 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.EnergyBonus == 5 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.S³ab;
                        }
                        if ( armor.EnergyBonus == 6 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.EnergyBonus == 7 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( armor.EnergyBonus == 8 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.EnergyBonus == 9 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( armor.EnergyBonus == 10 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.EnergyBonus == 11 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Dobr;
                        }
                        if ( armor.EnergyBonus == 12 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Doskona³;
                        }
                        if ( armor.EnergyBonus == 13 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wspania³;
                        }
                        if ( armor.EnergyBonus == 14 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( armor.EnergyBonus == 15 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( armor.EnergyBonus == 16 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Cudown;
                        }
                        if ( armor.EnergyBonus == 17 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( armor.EnergyBonus >= 18 )
                        {
                        armor.Hue = 17;
                        armor.Cechy = ArmorCechy.Ochronn;
                        armor.Quality = ArmorQuality.Legendarn;
                        }
}
            } //Koniec ApplyAttributes
        } //Koniec ApplyAttributes

        public static void ApplyAttributesTo(BaseHat hat, int attributeCount, int min, int max)
        {
            ApplyAttributesTo(hat, false, 0, attributeCount, min, max);
        }

        public static void ApplyAttributesTo(BaseHat hat, bool isRunicTool, int luckChance, int attributeCount, int min, int max)
        {
            m_IsRunicTool = isRunicTool;
            m_LuckChance = luckChance;

            AosAttributes primary = hat.Attributes;
            AosArmorAttributes secondary = hat.ClothingAttributes;
            AosElementAttributes resists = hat.Resistances;

            m_Props.SetAll(false);

            for (int i = 0; i < attributeCount; ++i)
            {
                int random = GetUniqueRandom(19);

                if (random == -1)
                    break;

                switch ( random )
                {
                    case 0:
                        ApplyAttribute(primary, min, max, AosAttribute.ReflectPhysical, 1, 15);
                        break;
                    case 1:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenHits, 1, 2);
                        break;
                    case 2:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenStam, 1, 3);
                        break;
                    case 3:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenMana, 1, 2);
                        break;
                    case 4:
                        ApplyAttribute(primary, min, max, AosAttribute.NightSight, 1, 1);
                        break;
                    case 5:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusHits, 1, 5);
                        break;
                    case 6:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusStam, 1, 8);
                        break;
                    case 7:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusMana, 1, 8);
                        break;
                    case 8:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerManaCost, 1, 8);
                        break;
                    case 9:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerRegCost, 1, 20);
                        break;
                    case 10:
                        ApplyAttribute(primary, min, max, AosAttribute.Luck, 1, 100);
                        break;
                    case 11:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.LowerStatReq, 10, 100, 10);
                        break;
                    case 12:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.SelfRepair, 1, 5);
                        break;
                    case 13:
                        ApplyAttribute(secondary, min, max, AosArmorAttribute.DurabilityBonus, 10, 100, 10);
                        break;
                    case 14:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Physical, 1, 15);
                        break;
                    case 15:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Fire, 1, 15);
                        break;
                    case 16:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Cold, 1, 15);
                        break;
                    case 17:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Poison, 1, 15);
                        break;
                    case 18:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Energy, 1, 15);
                        break;
                }
            }
        }

        public static void ApplyAttributesTo(BaseJewel jewelry, int attributeCount, int min, int max)
        {
            ApplyAttributesTo(jewelry, false, 0, attributeCount, min, max);
        }

        public static void ApplyAttributesTo(BaseJewel jewelry, bool isRunicTool, int luckChance, int attributeCount, int min, int max)
        {
            m_IsRunicTool = isRunicTool;
            m_LuckChance = luckChance;

            AosAttributes primary = jewelry.Attributes;
            AosElementAttributes resists = jewelry.Resistances;
            AosSkillBonuses skills = jewelry.SkillBonuses;

            m_Props.SetAll(false);

            for (int i = 0; i < attributeCount; ++i)
            {
                int random = GetUniqueRandom(24);

                if (random == -1)
                    break;

                switch ( random )
                {
                    case 0:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Physical, 1, 15);
                        break;
                    case 1:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Fire, 1, 15);
                        break;
                    case 2:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Cold, 1, 15);
                        break;
                    case 3:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Poison, 1, 15);
                        break;
                    case 4:
                        ApplyAttribute(resists, min, max, AosElementAttribute.Energy, 1, 15);
                        break;
                    case 5:
                        ApplyAttribute(primary, min, max, AosAttribute.WeaponDamage, 1, 25);
                        break;
                    case 6:
                        ApplyAttribute(primary, min, max, AosAttribute.DefendChance, 1, 16);
                        break;
                    case 7:
                        ApplyAttribute(primary, min, max, AosAttribute.AttackChance, 1, 15);
                        break;
                    case 8:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusStr, 1, 8);
                        break;
                    case 9:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusDex, 1, 8);
                        break;
                    case 10:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusInt, 1, 8);
                        break;
                    case 11:
                        ApplyAttribute(primary, min, max, AosAttribute.EnhancePotions, 5, 25, 5);
                        break;
                    case 12:
                        ApplyAttribute(primary, min, max, AosAttribute.CastSpeed, 1, 1);
                        break;
                    case 13:
                        ApplyAttribute(primary, min, max, AosAttribute.CastRecovery, 1, 3);
                        break;
                    case 14:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerManaCost, 1, 8);
                        break;
                    case 15:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerRegCost, 1, 20);
                        break;
                    case 16:
                        ApplyAttribute(primary, min, max, AosAttribute.Luck, 1, 100);
                        break;
                    case 17:
                        ApplyAttribute(primary, min, max, AosAttribute.SpellDamage, 1, 12);
                        break;
                    case 18:
                        ApplyAttribute(primary, min, max, AosAttribute.NightSight, 1, 1);
                        break;
                    case 19:
                        ApplySkillBonus(skills, min, max, 0, 1, 15);
                        break;
                    case 20:
                        ApplySkillBonus(skills, min, max, 1, 1, 15);
                        break;
                    case 21:
                        ApplySkillBonus(skills, min, max, 2, 1, 15);
                        break;
                    case 22:
                        ApplySkillBonus(skills, min, max, 3, 1, 15);
                        break;
                    case 23:
                        ApplySkillBonus(skills, min, max, 4, 1, 15);
                        break;
                }

                        if ( jewelry.Resistances.Physical == 1 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Resistances.Physical == 2 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Resistances.Physical == 3 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Physical == 4 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Physical == 5 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Physical == 6 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Physical == 7 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Physical == 8 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Physical == 9 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Physical == 10 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Resistances.Physical == 11 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Resistances.Physical == 12 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Resistances.Physical == 13 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Resistances.Physical == 14 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Resistances.Physical == 15 )
                        {
                        jewelry.SetHue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }





                        if ( jewelry.Resistances.Fire == 1 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Resistances.Fire == 2 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Resistances.Fire == 3 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Fire == 4 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Fire == 5 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Fire == 6 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Fire == 7 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Fire == 8 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Fire == 9 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Fire == 10 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Resistances.Fire == 11 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Resistances.Fire == 12 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Resistances.Fire == 13 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Resistances.Fire == 14 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Resistances.Fire == 15 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }





                        if ( jewelry.Resistances.Cold == 1 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Resistances.Cold == 2 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Resistances.Cold == 3 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Cold == 4 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Cold == 5 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Cold == 6 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Cold == 7 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Cold == 8 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Cold == 9 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Cold == 10 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Resistances.Cold == 11 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Resistances.Cold == 12 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Resistances.Cold == 13 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Resistances.Cold == 14 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Resistances.Cold == 15 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }





                        if ( jewelry.Resistances.Poison == 1 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Resistances.Poison == 2 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Resistances.Poison == 3 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Poison == 4 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Poison == 5 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Poison == 6 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Poison == 7 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Poison == 8 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Poison == 9 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Poison == 10 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Resistances.Poison == 11 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Resistances.Poison == 12 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Resistances.Poison == 13 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Resistances.Poison == 14 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Resistances.Poison == 15 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }





                        if ( jewelry.Resistances.Energy == 1 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Resistances.Energy == 2 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Resistances.Energy == 3 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Energy == 4 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Resistances.Energy == 5 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Energy == 6 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Energy == 7 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Resistances.Energy == 8 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Energy == 9 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Resistances.Energy == 10 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Resistances.Energy == 11 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Resistances.Energy == 12 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Resistances.Energy == 13 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Resistances.Energy == 14 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Resistances.Energy == 15 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Ochronn;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }





                        if ( jewelry.Attributes.WeaponDamage == 1 && jewelry.Attributes.WeaponDamage < 2)
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 2 && jewelry.Attributes.WeaponDamage < 4 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 4 && jewelry.Attributes.WeaponDamage < 6 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 6 && jewelry.Attributes.WeaponDamage < 8 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 8 && jewelry.Attributes.WeaponDamage < 10 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 10 && jewelry.Attributes.WeaponDamage < 12 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 12 && jewelry.Attributes.WeaponDamage < 14 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 14 && jewelry.Attributes.WeaponDamage < 16 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 16 && jewelry.Attributes.WeaponDamage < 18 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 18 && jewelry.Attributes.WeaponDamage < 20 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Attributes.WeaponDamage >= 20 && jewelry.Attributes.WeaponDamage < 25 )
                        {
                        jewelry.Hue = 17;
                        jewelry.Cechy = ArmorCechy.Mia¿dz¹c;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }





                        if ( jewelry.Attributes.DefendChance == 1 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Attributes.DefendChance == 2 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Attributes.DefendChance == 3 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.S³ab;
                        }
                        if ( jewelry.Attributes.DefendChance == 4 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Attributes.DefendChance == 5 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Przeciêtn;
                        }
                        if ( jewelry.Attributes.DefendChance == 6 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Attributes.DefendChance == 7 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Zwyk³;
                        }
                        if ( jewelry.Attributes.DefendChance == 8 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Attributes.DefendChance == 9 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Dobr;
                        }
                        if ( jewelry.Attributes.DefendChance == 10 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Doskona³;
                        }
                        if ( jewelry.Attributes.DefendChance == 11 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Wspania³;
                        }
                        if ( jewelry.Attributes.DefendChance == 12 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Wyj¹tkow;
                        }
                        if ( jewelry.Attributes.DefendChance == 13 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Niezwyk³;
                        }
                        if ( jewelry.Attributes.DefendChance == 14 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Cudown;
                        }
                        if ( jewelry.Attributes.DefendChance == 15 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Mistyczn;
                        }
                        if ( jewelry.Attributes.DefendChance >= 16 )
                        {
                        jewelry.Hue = 45;
                        jewelry.Cechy = ArmorCechy.Obronn;
                        jewelry.Quality = ArmorQuality.Legendarn;
                        }

            }
        }

        public static void ApplyAttributesTo(Spellbook spellbook, int attributeCount, int min, int max)
        {
            ApplyAttributesTo(spellbook, false, 0, attributeCount, min, max);
        }

        public static void ApplyAttributesTo(Spellbook spellbook, bool isRunicTool, int luckChance, int attributeCount, int min, int max)
        {
            m_IsRunicTool = isRunicTool;
            m_LuckChance = luckChance;

            AosAttributes primary = spellbook.Attributes;
            AosSkillBonuses skills = spellbook.SkillBonuses;

            m_Props.SetAll(false);

            for (int i = 0; i < attributeCount; ++i)
            {
                int random = GetUniqueRandom(16);

                if (random == -1)
                    break;

                switch ( random )
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        {
                            ApplyAttribute(primary, min, max, AosAttribute.BonusInt, 1, 8);

                            for (int j = 0; j < 4; ++j)
                                m_Props.Set(j, true);

                            break;
                        }
                    case 4:
                        ApplyAttribute(primary, min, max, AosAttribute.BonusMana, 1, 8);
                        break;
                    case 5:
                        ApplyAttribute(primary, min, max, AosAttribute.CastSpeed, 1, 1);
                        break;
                    case 6:
                        ApplyAttribute(primary, min, max, AosAttribute.CastRecovery, 1, 3);
                        break;
                    case 7:
                        ApplyAttribute(primary, min, max, AosAttribute.SpellDamage, 1, 12);
                        break;
                    case 8:
                        ApplySkillBonus(skills, min, max, 0, 1, 15);
                        break;
                    case 9:
                        ApplySkillBonus(skills, min, max, 1, 1, 15);
                        break;
                    case 10:
                        ApplySkillBonus(skills, min, max, 2, 1, 15);
                        break;
                    case 11:
                        ApplySkillBonus(skills, min, max, 3, 1, 15);
                        break;
                    case 12:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerRegCost, 1, 20);
                        break;
                    case 13:
                        ApplyAttribute(primary, min, max, AosAttribute.LowerManaCost, 1, 8);
                        break;
                    case 14:
                        ApplyAttribute(primary, min, max, AosAttribute.RegenMana, 1, 2);
                        break;
                    case 15:
                        spellbook.Slayer = GetRandomSlayer();
                        break;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
            writer.Write((int)this.m_Resource);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 0:
                    {
                        this.m_Resource = (CraftResource)reader.ReadInt();
                        break;
                    }
            }
        }

        public void ApplyAttributesTo(BaseWeapon weapon)
        {
            CraftResourceInfo resInfo = CraftResources.GetInfo(this.m_Resource);

            if (resInfo == null)
                return;

            CraftAttributeInfo attrs = resInfo.AttributeInfo;

            if (attrs == null)
                return;

            int attributeCount = Utility.RandomMinMax(attrs.RunicMinAttributes, attrs.RunicMaxAttributes);
            int min = attrs.RunicMinIntensity;
            int max = attrs.RunicMaxIntensity;

            ApplyAttributesTo(weapon, true, 0, attributeCount, min, max);
        }

        public void ApplyAttributesTo(BaseArmor armor)
        {
            CraftResourceInfo resInfo = CraftResources.GetInfo(this.m_Resource);

            if (resInfo == null)
                return;

            CraftAttributeInfo attrs = resInfo.AttributeInfo;

            if (attrs == null)
                return;

            int attributeCount = Utility.RandomMinMax(attrs.RunicMinAttributes, attrs.RunicMaxAttributes);
            int min = attrs.RunicMinIntensity;
            int max = attrs.RunicMaxIntensity;

            ApplyAttributesTo(armor, true, 0, attributeCount, min, max);
        }

        private static int Scale(int min, int max, int low, int high)
        {
            int percent;

            if (m_IsRunicTool)
            {
                percent = Utility.RandomMinMax(min, max);
            }
            else
            {
                // Behold, the worst system ever!
                int v = Utility.RandomMinMax(0, 10000);

                v = (int)Math.Sqrt(v);
                v = 100 - v;

                if (LootPack.CheckLuck(m_LuckChance))
                    v += 10;

                if (v < min)
                    v = min;
                else if (v > max)
                    v = max;

                percent = v;
            }

            int scaledBy = Math.Abs(high - low) + 1;

            if (scaledBy != 0)
                scaledBy = 10000 / scaledBy;

            percent *= (10000 + scaledBy);

            return low + (((high - low) * percent) / 1000001);
        }

        private static void ApplyAttribute(AosAttributes attrs, int min, int max, AosAttribute attr, int low, int high)
        {
            ApplyAttribute(attrs, min, max, attr, low, high, 1);
        }

        private static void ApplyAttribute(AosAttributes attrs, int min, int max, AosAttribute attr, int low, int high, int scale)
        {
            if (attr == AosAttribute.CastSpeed)
                attrs[attr] += Scale(min, max, low / scale, high / scale) * scale;
            else
                attrs[attr] = Scale(min, max, low / scale, high / scale) * scale;

            if (attr == AosAttribute.SpellChanneling)
                attrs[AosAttribute.CastSpeed] -= 1;
        }

        private static void ApplyAttribute(AosArmorAttributes attrs, int min, int max, AosArmorAttribute attr, int low, int high)
        {
            attrs[attr] = Scale(min, max, low, high);
        }

        private static void ApplyAttribute(AosArmorAttributes attrs, int min, int max, AosArmorAttribute attr, int low, int high, int scale)
        {
            attrs[attr] = Scale(min, max, low / scale, high / scale) * scale;
        }

        private static void ApplyAttribute(AosWeaponAttributes attrs, int min, int max, AosWeaponAttribute attr, int low, int high)
        {
            attrs[attr] = Scale(min, max, low, high);
        }

        private static void ApplyAttribute(AosWeaponAttributes attrs, int min, int max, AosWeaponAttribute attr, int low, int high, int scale)
        {
            attrs[attr] = Scale(min, max, low / scale, high / scale) * scale;
        }

        private static void ApplyAttribute(AosElementAttributes attrs, int min, int max, AosElementAttribute attr, int low, int high)
        {
            attrs[attr] = Scale(min, max, low, high);
        }

        private static void ApplyAttribute(AosElementAttributes attrs, int min, int max, AosElementAttribute attr, int low, int high, int scale)
        {
            attrs[attr] = Scale(min, max, low / scale, high / scale) * scale;
        }

        private static void ApplySkillBonus(AosSkillBonuses attrs, int min, int max, int index, int low, int high)
        {
            SkillName[] possibleSkills = (attrs.Owner is Spellbook ? m_PossibleSpellbookSkills : m_PossibleBonusSkills);
            int count = (Core.SE ? possibleSkills.Length : possibleSkills.Length - 2);

            SkillName sk, check;
            double bonus;
            bool found;

            do
            {
                found = false;
                sk = possibleSkills[Utility.Random(count)];

                for (int i = 0; !found && i < 5; ++i)
                    found = (attrs.GetValues(i, out check, out bonus) && check == sk);
            }
            while (found);

            attrs.SetValues(index, sk, Scale(min, max, low, high));
        }

        private static void ApplyResistance(BaseArmor ar, int min, int max, ResistanceType res, int low, int high)
        {
            switch ( res )
            {
                case ResistanceType.Physical:
                    ar.PhysicalBonus += Scale(min, max, low, high);
                    break;
                case ResistanceType.Fire:
                    ar.FireBonus += Scale(min, max, low, high);
                    break;
                case ResistanceType.Cold:
                    ar.ColdBonus += Scale(min, max, low, high);
                    break;
                case ResistanceType.Poison:
                    ar.PoisonBonus += Scale(min, max, low, high);
                    break;
                case ResistanceType.Energy:
                    ar.EnergyBonus += Scale(min, max, low, high);
                    break;
            }
        }

        private static int AssignElementalDamage(BaseWeapon weapon, AosElementAttribute attr, int totalDamage)
        {
            if (totalDamage <= 0)
                return 0;

            int random = Utility.Random((int)(totalDamage / 10) + 1) * 10;
            weapon.AosElementDamages[attr] = random;

            return (totalDamage - random);
        }
    }
}