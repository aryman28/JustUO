using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Craft;
using Server.Engines.XmlSpawner2;
using Server.Factions;
using Server.Network;
using Server.XMLConfiguration;

using AMA = Server.Items.ArmorMeditationAllowance;
using AMT = Server.Items.ArmorMaterialType;

namespace Server.Items
{
    public abstract class BaseArmor : Item, IScissorable, IFactionItem, ICraftable, IWearableDurability, ISetItem
    {
        #region Factions
        private FactionItem m_FactionState;

        public FactionItem FactionItemState
        {
            get
            {
                return this.m_FactionState;
            }
            set
            {
                this.m_FactionState = value;

                if (this.m_FactionState == null)
                    this.Hue = CraftResources.GetHue(this.Resource);

                this.LootType = (this.m_FactionState == null ? LootType.Regular : LootType.Blessed);
            }
        }
        #endregion

        /* Armor internals work differently now (Jun 19 2003)
        * 
        * The attributes defined below default to -1.
        * If the value is -1, the corresponding virtual 'Aos/Old' property is used.
        * If not, the attribute value itself is used. Here's the list:
        *  - ArmorBase
        *  - StrBonus
        *  - DexBonus
        *  - IntBonus
        *  - StrReq
        *  - DexReq
        *  - IntReq
        *  - MeditationAllowance
        */

        // Instance values. These values must are unique to each armor piece.
        private int m_MaxHitPoints;
        private int m_HitPoints;
        private Mobile m_Crafter;
        private ArmorQuality m_Quality;
        ///
        private ArmorCechy m_Cechy;
        ///
        private ArmorDurabilityLevel m_Durability;
        private ArmorProtectionLevel m_Protection;
        private CraftResource m_Resource;
        private bool m_Identified, m_PlayerConstructed, m_Ukrycie, m_Sprawdzony;
        private int m_PhysicalBonus, m_FireBonus, m_ColdBonus, m_PoisonBonus, m_EnergyBonus;
        private int m_TimesImbued;

        private AosAttributes m_AosAttributes;
        private AosArmorAttributes m_AosArmorAttributes;
        private AosSkillBonuses m_AosSkillBonuses;
        private SAAbsorptionAttributes m_SAAbsorptionAttributes;

        // Overridable values. These values are provided to override the defaults which get defined in the individual armor scripts.
        private int m_ArmorBase = -1;
        private int m_StrBonus = -1, m_DexBonus = -1, m_IntBonus = -1;
        private int m_StrReq = -1, m_DexReq = -1, m_IntReq = -1;
        private AMA m_Meditate = (AMA)(-1);

        public virtual bool AllowMaleWearer
        {
            get
            {
                return true;
            }
        }
        public virtual bool AllowFemaleWearer
        {
            get
            {
                return true;
            }
        }

        public abstract AMT MaterialType { get; }

        public virtual int RevertArmorBase
        {
            get
            {
                return this.ArmorBase;
            }
        }
        public virtual int ArmorBase
        {
            get
            {
                return 0;
            }
        }

        public virtual AMA DefMedAllowance
        {
            get
            {
                return AMA.None;
            }
        }
        public virtual AMA AosMedAllowance
        {
            get
            {
                return this.DefMedAllowance;
            }
        }
        public virtual AMA OldMedAllowance
        {
            get
            {
                return this.DefMedAllowance;
            }
        }

        public virtual int AosStrBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosDexBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosIntBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosStrReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosDexReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int AosIntReq
        {
            get
            {
                return 0;
            }
        }

        public virtual int OldStrBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldDexBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldIntBonus
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldStrReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldDexReq
        {
            get
            {
                return 0;
            }
        }
        public virtual int OldIntReq
        {
            get
            {
                return 0;
            }
        }

        public virtual bool CanFortify
        {
            get
            {
                return true;
            }
        }

        public virtual bool UseIntOrDexProperty
        {
            get
            {
                return false;
            }
        }
        public virtual int IntOrDexPropertyValue
        {
            get
            {
                return 0;
            }
        }

        public override void OnAfterDuped(Item newItem)
        {
            BaseArmor armor = newItem as BaseArmor;

            if (armor == null)
                return;

            armor.m_AosAttributes = new AosAttributes(newItem, this.m_AosAttributes);
            armor.m_AosArmorAttributes = new AosArmorAttributes(newItem, this.m_AosArmorAttributes);
            armor.m_AosSkillBonuses = new AosSkillBonuses(newItem, this.m_AosSkillBonuses);
            armor.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(newItem, this.m_SAAbsorptionAttributes);
            armor.m_SetAttributes = new AosAttributes(newItem, this.m_SetAttributes);
            armor.m_SetSkillBonuses = new AosSkillBonuses(newItem, this.m_SetSkillBonuses);
        }

        #region Personal Bless Deed
        private Mobile m_BlessedBy;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile BlessedBy
        {
            get
            {
                return this.m_BlessedBy;
            }
            set
            {
                this.m_BlessedBy = value;
                this.InvalidateProperties();
            }
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (this.BlessedFor == from && this.BlessedBy == from && this.RootParent == from)
            {
                list.Add(new UnBlessEntry(from, this));
            }
        }

        private class UnBlessEntry : ContextMenuEntry
        {
            private readonly Mobile m_From;
            private readonly BaseArmor m_Item;

            public UnBlessEntry(Mobile from, BaseArmor item)
                : base(6208, -1)
            {
                this.m_From = from;
                this.m_Item = item;
            }

            public override void OnClick()
            {
                this.m_Item.BlessedFor = null;
                this.m_Item.BlessedBy = null;

                Container pack = this.m_From.Backpack;

                if (pack != null)
                {
                    pack.DropItem(new PersonalBlessDeed(this.m_From));
                    this.m_From.SendLocalizedMessage(1062200); // A personal bless deed has been placed in your backpack.
                }
            }
        }
        #endregion

        [CommandProperty(AccessLevel.GameMaster)]
        public AMA MeditationAllowance
        {
            get
            {
                return (this.m_Meditate == (AMA)(-1) ? Core.AOS ? this.AosMedAllowance : this.OldMedAllowance : this.m_Meditate);
            }
            set
            {
                this.m_Meditate = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int BaseArmorRating
        {
            get
            {
                if (this.m_ArmorBase == -1)
                    return this.ArmorBase;
                else
                    return this.m_ArmorBase;
            }
            set
            {
                this.m_ArmorBase = value;
                this.Invalidate();
            }
        }

        public double BaseArmorRatingScaled
        {
            get
            {
                return (this.BaseArmorRating * this.ArmorScalar);
            }
        }

        public virtual double ArmorRating
        {
            get
            {
                int ar = this.BaseArmorRating;

                if (this.m_Protection != ArmorProtectionLevel.Regular)
                    ar += 10 + (5 * (int)this.m_Protection);

                switch (this.m_Resource)
                {
                    case CraftResource.DullCopper:
                        ar += 2;
                        break;
                    case CraftResource.ShadowIron:
                        ar += 4;
                        break;
                    case CraftResource.Copper:
                        ar += 6;
                        break;
                    case CraftResource.Bronze:
                        ar += 8;
                        break;
                    case CraftResource.Gold:
                        ar += 10;
                        break;
                    case CraftResource.Agapite:
                        ar += 12;
                        break;
                    case CraftResource.Verite:
                        ar += 14;
                        break;
                    case CraftResource.Valorite:
                        ar += 16;
                        break;
                    case CraftResource.SpinedLeather:
                        ar += 10;
                        break;
                    case CraftResource.HornedLeather:
                        ar += 13;
                        break;
                    case CraftResource.BarbedLeather:
                        ar += 16;
                        break;
                }

                //ar += -8 + (8 * (int)this.m_Quality);
                ar += -3 + (3 * (int)this.m_Quality);
                return this.ScaleArmorByDurability(ar);
            }
        }

        public double ArmorRatingScaled
        {
            get
            {
                return (this.ArmorRating * this.ArmorScalar);
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TimesImbued
        {
            get
            {
                return this.m_TimesImbued;
            }
            set
            {
                this.m_TimesImbued = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrBonus
        {
            get
            {
                return (this.m_StrBonus == -1 ? Core.AOS ? this.AosStrBonus : this.OldStrBonus : this.m_StrBonus);
            }
            set
            {
                this.m_StrBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexBonus
        {
            get
            {
                return (this.m_DexBonus == -1 ? Core.AOS ? this.AosDexBonus : this.OldDexBonus : this.m_DexBonus);
            }
            set
            {
                this.m_DexBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntBonus
        {
            get
            {
                return (this.m_IntBonus == -1 ? Core.AOS ? this.AosIntBonus : this.OldIntBonus : this.m_IntBonus);
            }
            set
            {
                this.m_IntBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int StrRequirement
        {
            get
            {
                return (this.m_StrReq == -1 ? Core.AOS ? this.AosStrReq : this.OldStrReq : this.m_StrReq);
            }
            set
            {
                this.m_StrReq = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int DexRequirement
        {
            get
            {
                return (this.m_DexReq == -1 ? Core.AOS ? this.AosDexReq : this.OldDexReq : this.m_DexReq);
            }
            set
            {
                this.m_DexReq = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int IntRequirement
        {
            get
            {
                return (this.m_IntReq == -1 ? Core.AOS ? this.AosIntReq : this.OldIntReq : this.m_IntReq);
            }
            set
            {
                this.m_IntReq = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Identified
        {
            get
            {
                return this.m_Identified;
            }
            set
            {
                this.m_Identified = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Ukrycie
        {
            get
            {
                return this.m_Ukrycie;
            }
            set
            {
                this.m_Ukrycie = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Sprawdzony
        {
            get
            {
                return this.m_Sprawdzony;
            }
            set
            {
                this.m_Sprawdzony = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool PlayerConstructed
        {
            get
            {
                return this.m_PlayerConstructed;
            }
            set
            {
                this.m_PlayerConstructed = value;
            }
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
                if (this.m_Resource != value)
                {
                    this.UnscaleDurability();

                    this.m_Resource = value;

                    if (CraftItem.RetainsColor(this.GetType()))
                    {
                        this.Hue = CraftResources.GetHue(this.m_Resource);
                    }

                    this.Invalidate();
                    this.InvalidateProperties();

                    if (this.Parent is Mobile)
                        ((Mobile)this.Parent).UpdateResistances();

                    this.ScaleDurability();
                }
            }
        }

        public virtual double ArmorScalar
        {
            get
            {
                int pos = (int)this.BodyPosition;

                if (pos >= 0 && pos < m_ArmorScalars.Length)
                    return m_ArmorScalars[pos];

                return 1.0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxHitPoints
        {
            get
            {
                return this.m_MaxHitPoints;
            }
            set
            {
                this.m_MaxHitPoints = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int HitPoints
        {
            get
            {
                return this.m_HitPoints;
            }
            set
            {
                if (value != this.m_HitPoints && this.MaxHitPoints > 0)
                {
                    this.m_HitPoints = value;

                    if (this.m_HitPoints < 0)
                        this.Delete();
                    else if (this.m_HitPoints > this.MaxHitPoints)
                        this.m_HitPoints = this.MaxHitPoints;

                    this.InvalidateProperties();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get
            {
                return this.m_Crafter;
                m_Identified = true;
                //m_Ukrycie = false;
            }
            set
            {
                this.m_Crafter = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorQuality Quality
        {
            get
            {
                return this.m_Quality;
            }
            set
            {
                this.UnscaleDurability();
                this.m_Quality = value;
                this.Invalidate();
                this.InvalidateProperties();
                this.ScaleDurability();
            }
        }
        ///
        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorCechy Cechy
        {
            get
            {
                return this.m_Cechy;
            }
            set
            {
                this.m_Cechy = value;
                this.Invalidate();
                this.InvalidateProperties();
            }
        }
        ///
        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorDurabilityLevel Durability
        {
            get
            {
                return this.m_Durability;
            }
            set
            {
                this.UnscaleDurability();
                this.m_Durability = value;
                this.ScaleDurability();
                this.InvalidateProperties();
            }
        }

        public virtual int ArtifactRarity
        {
            get
            {
                return 0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorProtectionLevel ProtectionLevel
        {
            get
            {
                return this.m_Protection;
            }
            set
            {
                if (this.m_Protection != value)
                {
                    this.m_Protection = value;

                    this.Invalidate();
                    this.InvalidateProperties();

                    if (this.Parent is Mobile)
                        ((Mobile)this.Parent).UpdateResistances();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes Attributes
        {
            get
            {
                return this.m_AosAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosArmorAttributes ArmorAttributes
        {
            get
            {
                return this.m_AosArmorAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SkillBonuses
        {
            get
            {
                return this.m_AosSkillBonuses;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public SAAbsorptionAttributes AbsorptionAttributes
        {
            get
            {
                return this.m_SAAbsorptionAttributes;
            }
            set
            {
            }
        }

        public int ComputeStatReq(StatType type)
        {
            int v;

            if (type == StatType.Str)
                v = this.StrRequirement;
            else if (type == StatType.Dex)
                v = this.DexRequirement;
            else
                v = this.IntRequirement;

            return AOS.Scale(v, 100 - this.GetLowerStatReq());
        }

        public int ComputeStatBonus(StatType type)
        {
            if (type == StatType.Str)
                return this.StrBonus + this.Attributes.BonusStr;
            else if (type == StatType.Dex)
                return this.DexBonus + this.Attributes.BonusDex;
            else
                return this.IntBonus + this.Attributes.BonusInt;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PhysicalBonus
        {
            get
            {
                return this.m_PhysicalBonus;
            }
            set
            {
                this.m_PhysicalBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int FireBonus
        {
            get
            {
                return this.m_FireBonus;
            }
            set
            {
                this.m_FireBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int ColdBonus
        {
            get
            {
                return this.m_ColdBonus;
            }
            set
            {
                this.m_ColdBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PoisonBonus
        {
            get
            {
                return this.m_PoisonBonus;
            }
            set
            {
                this.m_PoisonBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int EnergyBonus
        {
            get
            {
                return this.m_EnergyBonus;
            }
            set
            {
                this.m_EnergyBonus = value;
                this.InvalidateProperties();
            }
        }

        public virtual int BasePhysicalResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BaseFireResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BaseColdResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BasePoisonResistance
        {
            get
            {
                return 0;
            }
        }
        public virtual int BaseEnergyResistance
        {
            get
            {
                return 0;
            }
        }

        public override int PhysicalResistance
        {
            get
            {
                return this.BasePhysicalResistance + this.GetProtOffset() + this.GetResourceAttrs().ArmorPhysicalResist + this.m_PhysicalBonus + (this.m_SetEquipped ? this.m_SetPhysicalBonus : 0);
            }
        }

        public override int FireResistance
        {
            get
            {
                return this.BaseFireResistance + this.GetProtOffset() + this.GetResourceAttrs().ArmorFireResist + this.m_FireBonus + (this.m_SetEquipped ? this.m_SetFireBonus : 0);
            }
        }

        public override int ColdResistance
        {
            get
            {
                return this.BaseColdResistance + this.GetProtOffset() + this.GetResourceAttrs().ArmorColdResist + this.m_ColdBonus + (this.m_SetEquipped ? this.m_SetColdBonus : 0);
            }
        }

        public override int PoisonResistance
        {
            get
            {
                return this.BasePoisonResistance + this.GetProtOffset() + this.GetResourceAttrs().ArmorPoisonResist + this.m_PoisonBonus + (this.m_SetEquipped ? this.m_SetPoisonBonus : 0);
            }
        }

        public override int EnergyResistance
        {
            get
            {
                return this.BaseEnergyResistance + this.GetProtOffset() + this.GetResourceAttrs().ArmorEnergyResist + this.m_EnergyBonus + (this.m_SetEquipped ? this.m_SetEnergyBonus : 0);
            }
        }

        public virtual int InitMinHits
        {
            get
            {
                return 0;
            }
        }
        public virtual int InitMaxHits
        {
            get
            {
                return 0;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public ArmorBodyType BodyPosition
        {
            get
            {
                switch (this.Layer)
                {
                    default:
                    case Layer.Neck:
                        return ArmorBodyType.Gorget;
                    case Layer.TwoHanded:
                        return ArmorBodyType.Shield;
                    case Layer.Gloves:
                        return ArmorBodyType.Gloves;
                    case Layer.Helm:
                        return ArmorBodyType.Helmet;
                    case Layer.Arms:
                        return ArmorBodyType.Arms;

                    case Layer.InnerLegs:
                    case Layer.OuterLegs:
                    case Layer.Pants:
                        return ArmorBodyType.Legs;

                    case Layer.InnerTorso:
                    case Layer.OuterTorso:
                    case Layer.Shirt:
                        return ArmorBodyType.Chest;
                }
            }
        }

        public void DistributeBonuses(int amount)
        {
            for (int i = 0; i < amount; ++i)
            {
                switch (Utility.Random(5))
                {
                    case 0:
                        ++this.m_PhysicalBonus;
                        break;
                    case 1:
                        ++this.m_FireBonus;
                        break;
                    case 2:
                        ++this.m_ColdBonus;
                        break;
                    case 3:
                        ++this.m_PoisonBonus;
                        break;
                    case 4:
                        ++this.m_EnergyBonus;
                        break;
                }
            }

            this.InvalidateProperties();
        }

        public CraftAttributeInfo GetResourceAttrs()
        {
            CraftResourceInfo info = CraftResources.GetInfo(this.m_Resource);

            if (info == null)
                return CraftAttributeInfo.Blank;

            return info.AttributeInfo;
        }

        public int GetProtOffset()
        {
            switch (this.m_Protection)
            {
                case ArmorProtectionLevel.Guarding:
                    return 1;
                case ArmorProtectionLevel.Hardening:
                    return 2;
                case ArmorProtectionLevel.Fortification:
                    return 3;
                case ArmorProtectionLevel.Invulnerability:
                    return 4;
            }

            return 0;
        }

        public void UnscaleDurability()
        {
            int scale = 100 + this.GetDurabilityBonus();

            this.m_HitPoints = ((this.m_HitPoints * 100) + (scale - 1)) / scale;
            this.m_MaxHitPoints = ((this.m_MaxHitPoints * 100) + (scale - 1)) / scale;
            this.InvalidateProperties();
        }

        public void ScaleDurability()
        {
            int scale = 100 + this.GetDurabilityBonus();

            this.m_HitPoints = ((this.m_HitPoints * scale) + 99) / 100;
            this.m_MaxHitPoints = ((this.m_MaxHitPoints * scale) + 99) / 100;
            this.InvalidateProperties();
        }

        public int GetDurabilityBonus()
        {
            int bonus = 0;

            if (this.m_Quality == ArmorQuality.Doskona�)
                bonus += 5;
            if (this.m_Quality == ArmorQuality.Wspania�)
                bonus += 10;
            if (this.m_Quality == ArmorQuality.Wyj�tkow)
                bonus += 15;
            if (this.m_Quality == ArmorQuality.Niezwyk�)
                bonus += 20;
            if (this.m_Quality == ArmorQuality.Cudown)
                bonus += 25;
            if (this.m_Quality == ArmorQuality.Mistyczn)
                bonus += 30;
            if (this.m_Quality == ArmorQuality.Legendarn)
                bonus += 35;

            switch (this.m_Durability)
            {
                case ArmorDurabilityLevel.Durable:
                    bonus += 20;
                    break;
                case ArmorDurabilityLevel.Substantial:
                    bonus += 50;
                    break;
                case ArmorDurabilityLevel.Massive:
                    bonus += 70;
                    break;
                case ArmorDurabilityLevel.Fortified:
                    bonus += 100;
                    break;
                case ArmorDurabilityLevel.Indestructible:
                    bonus += 120;
                    break;
            }

            if (Core.AOS)
            {
                bonus += this.m_AosArmorAttributes.DurabilityBonus;

                if (this.m_Resource == CraftResource.Heartwood)
                    return bonus;

                CraftResourceInfo resInfo = CraftResources.GetInfo(this.m_Resource);
                CraftAttributeInfo attrInfo = null;

                if (resInfo != null)
                    attrInfo = resInfo.AttributeInfo;

                if (attrInfo != null)
                    bonus += attrInfo.ArmorDurability;
            }

            return bonus;
        }

        public bool Scissor(Mobile from, Scissors scissors)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(502437); // Items you wish to cut must be in your backpack.
                return false;
            }

            if (Ethics.Ethic.IsImbued(this))
            {
                from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
                return false;
            }

            CraftSystem system = DefTailoring.CraftSystem;

            CraftItem item = system.CraftItems.SearchFor(this.GetType());

            if (item != null && item.Resources.Count == 1 && item.Resources.GetAt(0).Amount >= 2)
            {
                try
                {
                    Item res = (Item)Activator.CreateInstance(CraftResources.GetInfo(this.m_Resource).ResourceTypes[0]);

                    this.ScissorHelper(from, res, this.m_PlayerConstructed ? (item.Resources.GetAt(0).Amount / 2) : 1);
                    return true;
                }
                catch
                {
                }
            }

            from.SendLocalizedMessage(502440); // Scissors can not be used on that to produce anything.
            return false;
        }

        private static double[] m_ArmorScalars = { 0.07, 0.07, 0.14, 0.15, 0.22, 0.35 };

        public static double[] ArmorScalars
        {
            get
            {
                return m_ArmorScalars;
            }
            set
            {
                m_ArmorScalars = value;
            }
        }

        public static void ValidateMobile(Mobile m)
        {
            for (int i = m.Items.Count - 1; i >= 0; --i)
            {
                if (i >= m.Items.Count)
                    continue;

                Item item = m.Items[i];

                if (item is BaseArmor)
                {
                    BaseArmor armor = (BaseArmor)item;

                    if (m.Race == Race.Gargoyle && !armor.CanBeWornByGargoyles)
                    {
                        m.SendLocalizedMessage(1111708); // Gargoyles can't wear this.
                        m.AddToBackpack(armor);
                    }
                    if (armor.RequiredRace != null && m.Race != armor.RequiredRace)
                    {
                        if (armor.RequiredRace == Race.Elf)
                            m.SendLocalizedMessage(1072203); // Only Elves may use this.
                        else if (armor.RequiredRace == Race.Gargoyle)
                            m.SendLocalizedMessage(1111707); // Only gargoyles can wear this.
                        else
                            m.SendMessage("Only {0} may use this.", armor.RequiredRace.PluralName);

                        m.AddToBackpack(armor);
                    }
                    else if (!armor.AllowMaleWearer && !m.Female && m.AccessLevel < AccessLevel.GameMaster)
                    {
                        if (armor.AllowFemaleWearer)
                            m.SendLocalizedMessage(1010388); // Only females can wear this.
                        else
                            m.SendMessage("You may not wear this.");

                        m.AddToBackpack(armor);
                    }
                    else if (!armor.AllowFemaleWearer && m.Female && m.AccessLevel < AccessLevel.GameMaster)
                    {
                        if (armor.AllowMaleWearer)
                            m.SendLocalizedMessage(1063343); // Only males can wear this.
                        else
                            m.SendMessage("You may not wear this.");

                        m.AddToBackpack(armor);
                    }
                }
            }
        }

        public int GetLowerStatReq()
        {
            if (!Core.AOS)
                return 0;

            int v = this.m_AosArmorAttributes.LowerStatReq;

            if (this.m_Resource == CraftResource.Heartwood)
                return v;

            CraftResourceInfo info = CraftResources.GetInfo(this.m_Resource);

            if (info != null)
            {
                CraftAttributeInfo attrInfo = info.AttributeInfo;

                if (attrInfo != null)
                    v += attrInfo.ArmorLowerRequirements;
            }

            if (v > 100)
                v = 100;

            return v;
        }

        public override void OnAdded(object parent)
        {
            if (parent is Mobile)
            {
                Mobile from = (Mobile)parent;

                if (Core.AOS)
                    this.m_AosSkillBonuses.AddTo(from);

                #region Mondain's Legacy Sets
                if (this.IsSetItem)
                {
                    this.m_SetEquipped = SetHelper.FullSetEquipped(from, this.SetID, this.Pieces);

                    if (this.m_SetEquipped)
                    {
                        this.m_LastEquipped = true;
                        SetHelper.AddSetBonus(from, this.SetID);
                    }
                }
                #endregion

                from.Delta(MobileDelta.Armor); // Tell them armor rating has changed
            }
        }

        public virtual double ScaleArmorByDurability(double armor)
        {
            int scale = 100;

            if (this.m_MaxHitPoints > 0 && this.m_HitPoints < this.m_MaxHitPoints)
                scale = 50 + ((50 * this.m_HitPoints) / this.m_MaxHitPoints);

            return (armor * scale) / 100;
        }

        protected void Invalidate()
        {
            if (this.Parent is Mobile)
                ((Mobile)this.Parent).Delta(MobileDelta.Armor); // Tell them armor rating has changed
        }

        public BaseArmor(Serial serial)
            : base(serial)
        {
        }

        private static void SetSaveFlag(ref SaveFlag flags, SaveFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SaveFlag flags, SaveFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SaveFlag
        {
            None = 0x00000000,
            Attributes = 0x00000001,
            ArmorAttributes = 0x00000002,
            PhysicalBonus = 0x00000004,
            FireBonus = 0x00000008,
            ColdBonus = 0x00000010,
            PoisonBonus = 0x00000020,
            EnergyBonus = 0x00000040,
            Identified = 0x00000080,
            MaxHitPoints = 0x00000100,
            HitPoints = 0x00000200,
            Crafter = 0x00000400,
            Quality = 0x00000800,
            Durability = 0x00001000,
            Protection = 0x00002000,
            Resource = 0x00004000,
            BaseArmor = 0x00008000,
            StrBonus = 0x00010000,
            DexBonus = 0x00020000,
            IntBonus = 0x00040000,
            StrReq = 0x00080000,
            DexReq = 0x00100000,
            IntReq = 0x00200000,
            MedAllowance = 0x00400000,
            SkillBonuses = 0x00800000,
            PlayerConstructed = 0x01000000,
            xAbsorptionAttributes = 0x02000000,
            TimesImbued = 0x04000000,
            Cechy = 0x08000000,
            Ukrycie = 0x10000000,
            Sprawdzony = 0x20000000
        }

        #region Mondain's Legacy Sets
        private static void SetSaveFlag(ref SetFlag flags, SetFlag toSet, bool setIf)
        {
            if (setIf)
                flags |= toSet;
        }

        private static bool GetSaveFlag(SetFlag flags, SetFlag toGet)
        {
            return ((flags & toGet) != 0);
        }

        [Flags]
        private enum SetFlag
        {
            None = 0x00000000,
            Attributes = 0x00000001,
            ArmorAttributes = 0x00000002,
            SkillBonuses = 0x00000004,
            PhysicalBonus = 0x00000008,
            FireBonus = 0x00000010,
            ColdBonus = 0x00000020,
            PoisonBonus = 0x00000040,
            EnergyBonus = 0x00000080,
            Hue = 0x00000100,
            LastEquipped = 0x00000200,
            SetEquipped = 0x00000400,
            SetSelfRepair = 0x00000800,
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)8); // version

            // Version 8
            writer.Write((int)this.m_TimesImbued);
            writer.Write((Mobile)this.m_BlessedBy);

            SetFlag sflags = SetFlag.None;

            SetSaveFlag(ref sflags, SetFlag.Attributes, !this.m_SetAttributes.IsEmpty);
            SetSaveFlag(ref sflags, SetFlag.SkillBonuses, !this.m_SetSkillBonuses.IsEmpty);
            SetSaveFlag(ref sflags, SetFlag.PhysicalBonus, this.m_SetPhysicalBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.FireBonus, this.m_SetFireBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.ColdBonus, this.m_SetColdBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.PoisonBonus, this.m_SetPoisonBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.EnergyBonus, this.m_SetEnergyBonus != 0);
            SetSaveFlag(ref sflags, SetFlag.Hue, this.m_SetHue != 0);
            SetSaveFlag(ref sflags, SetFlag.LastEquipped, this.m_LastEquipped);
            SetSaveFlag(ref sflags, SetFlag.SetEquipped, this.m_SetEquipped);
            SetSaveFlag(ref sflags, SetFlag.SetSelfRepair, this.m_SetSelfRepair != 0);

            writer.WriteEncodedInt((int)sflags);

            if (GetSaveFlag(sflags, SetFlag.Attributes))
                this.m_SetAttributes.Serialize(writer);

            if (GetSaveFlag(sflags, SetFlag.SkillBonuses))
                this.m_SetSkillBonuses.Serialize(writer);

            if (GetSaveFlag(sflags, SetFlag.PhysicalBonus))
                writer.WriteEncodedInt((int)this.m_SetPhysicalBonus);

            if (GetSaveFlag(sflags, SetFlag.FireBonus))
                writer.WriteEncodedInt((int)this.m_SetFireBonus);

            if (GetSaveFlag(sflags, SetFlag.ColdBonus))
                writer.WriteEncodedInt((int)this.m_SetColdBonus);

            if (GetSaveFlag(sflags, SetFlag.PoisonBonus))
                writer.WriteEncodedInt((int)this.m_SetPoisonBonus);

            if (GetSaveFlag(sflags, SetFlag.EnergyBonus))
                writer.WriteEncodedInt((int)this.m_SetEnergyBonus);

            if (GetSaveFlag(sflags, SetFlag.Hue))
                writer.WriteEncodedInt((int)this.m_SetHue);

            if (GetSaveFlag(sflags, SetFlag.LastEquipped))
                writer.Write((bool)this.m_LastEquipped);

            if (GetSaveFlag(sflags, SetFlag.SetEquipped))
                writer.Write((bool)this.m_SetEquipped);

            if (GetSaveFlag(sflags, SetFlag.SetSelfRepair))
                writer.WriteEncodedInt((int)this.m_SetSelfRepair);

            // Version 7
            SaveFlag flags = SaveFlag.None;

            SetSaveFlag(ref flags, SaveFlag.Attributes, !this.m_AosAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.ArmorAttributes, !this.m_AosArmorAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.PhysicalBonus, this.m_PhysicalBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.FireBonus, this.m_FireBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.ColdBonus, this.m_ColdBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.PoisonBonus, this.m_PoisonBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.EnergyBonus, this.m_EnergyBonus != 0);
            SetSaveFlag(ref flags, SaveFlag.Identified, this.m_Identified != false);
            SetSaveFlag(ref flags, SaveFlag.Ukrycie, this.m_Ukrycie != false);
            SetSaveFlag(ref flags, SaveFlag.Sprawdzony, this.m_Sprawdzony != false);
            SetSaveFlag(ref flags, SaveFlag.MaxHitPoints, this.m_MaxHitPoints != 0);
            SetSaveFlag(ref flags, SaveFlag.HitPoints, this.m_HitPoints != 0);
            SetSaveFlag(ref flags, SaveFlag.Crafter, this.m_Crafter != null);
            SetSaveFlag(ref flags, SaveFlag.Quality, this.m_Quality != ArmorQuality.None);
            SetSaveFlag(ref flags, SaveFlag.Cechy, this.m_Cechy != ArmorCechy.None);
            SetSaveFlag(ref flags, SaveFlag.Durability, this.m_Durability != ArmorDurabilityLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Protection, this.m_Protection != ArmorProtectionLevel.Regular);
            SetSaveFlag(ref flags, SaveFlag.Resource, this.m_Resource != this.DefaultResource);
            SetSaveFlag(ref flags, SaveFlag.BaseArmor, this.m_ArmorBase != -1);
            SetSaveFlag(ref flags, SaveFlag.StrBonus, this.m_StrBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.DexBonus, this.m_DexBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.IntBonus, this.m_IntBonus != -1);
            SetSaveFlag(ref flags, SaveFlag.StrReq, this.m_StrReq != -1);
            SetSaveFlag(ref flags, SaveFlag.DexReq, this.m_DexReq != -1);
            SetSaveFlag(ref flags, SaveFlag.IntReq, this.m_IntReq != -1);
            SetSaveFlag(ref flags, SaveFlag.MedAllowance, this.m_Meditate != (AMA)(-1));
            SetSaveFlag(ref flags, SaveFlag.SkillBonuses, !this.m_AosSkillBonuses.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.PlayerConstructed, this.m_PlayerConstructed != false);
            SetSaveFlag(ref flags, SaveFlag.xAbsorptionAttributes, !this.m_SAAbsorptionAttributes.IsEmpty);
            SetSaveFlag(ref flags, SaveFlag.TimesImbued, this.m_TimesImbued != 0);

            writer.WriteEncodedInt((int)flags);

            if (GetSaveFlag(flags, SaveFlag.Attributes))
                this.m_AosAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.ArmorAttributes))
                this.m_AosArmorAttributes.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.PhysicalBonus))
                writer.WriteEncodedInt((int)this.m_PhysicalBonus);

            if (GetSaveFlag(flags, SaveFlag.FireBonus))
                writer.WriteEncodedInt((int)this.m_FireBonus);

            if (GetSaveFlag(flags, SaveFlag.ColdBonus))
                writer.WriteEncodedInt((int)this.m_ColdBonus);

            if (GetSaveFlag(flags, SaveFlag.PoisonBonus))
                writer.WriteEncodedInt((int)this.m_PoisonBonus);

            if (GetSaveFlag(flags, SaveFlag.EnergyBonus))
                writer.WriteEncodedInt((int)this.m_EnergyBonus);

            if (GetSaveFlag(flags, SaveFlag.MaxHitPoints))
                writer.WriteEncodedInt((int)this.m_MaxHitPoints);

            if (GetSaveFlag(flags, SaveFlag.HitPoints))
                writer.WriteEncodedInt((int)this.m_HitPoints);

            if (GetSaveFlag(flags, SaveFlag.Crafter))
                writer.Write((Mobile)this.m_Crafter);

            if (GetSaveFlag(flags, SaveFlag.Quality))
                writer.WriteEncodedInt((int)this.m_Quality);

            if (GetSaveFlag(flags, SaveFlag.Cechy))
                writer.WriteEncodedInt((int)this.m_Cechy);

            if (GetSaveFlag(flags, SaveFlag.Durability))
                writer.WriteEncodedInt((int)this.m_Durability);

            if (GetSaveFlag(flags, SaveFlag.Protection))
                writer.WriteEncodedInt((int)this.m_Protection);

            if (GetSaveFlag(flags, SaveFlag.Resource))
                writer.WriteEncodedInt((int)this.m_Resource);

            if (GetSaveFlag(flags, SaveFlag.BaseArmor))
                writer.WriteEncodedInt((int)this.m_ArmorBase);

            if (GetSaveFlag(flags, SaveFlag.StrBonus))
                writer.WriteEncodedInt((int)this.m_StrBonus);

            if (GetSaveFlag(flags, SaveFlag.DexBonus))
                writer.WriteEncodedInt((int)this.m_DexBonus);

            if (GetSaveFlag(flags, SaveFlag.IntBonus))
                writer.WriteEncodedInt((int)this.m_IntBonus);

            if (GetSaveFlag(flags, SaveFlag.StrReq))
                writer.WriteEncodedInt((int)this.m_StrReq);

            if (GetSaveFlag(flags, SaveFlag.DexReq))
                writer.WriteEncodedInt((int)this.m_DexReq);

            if (GetSaveFlag(flags, SaveFlag.IntReq))
                writer.WriteEncodedInt((int)this.m_IntReq);

            if (GetSaveFlag(flags, SaveFlag.MedAllowance))
                writer.WriteEncodedInt((int)this.m_Meditate);

            if (GetSaveFlag(flags, SaveFlag.SkillBonuses))
                this.m_AosSkillBonuses.Serialize(writer);

            if (GetSaveFlag(flags, SaveFlag.xAbsorptionAttributes))
                this.m_SAAbsorptionAttributes.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 8:
                    {
                        this.m_TimesImbued = reader.ReadInt();
                        this.m_BlessedBy = reader.ReadMobile();

                        SetFlag sflags = (SetFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.Attributes))
                            this.m_SetAttributes = new AosAttributes(this, reader);
                        else
                            this.m_SetAttributes = new AosAttributes(this);

                        if (GetSaveFlag(sflags, SetFlag.ArmorAttributes))
                            this.m_SetSelfRepair = (new AosArmorAttributes(this, reader)).SelfRepair;

                        if (GetSaveFlag(sflags, SetFlag.SkillBonuses))
                            this.m_SetSkillBonuses = new AosSkillBonuses(this, reader);
                        else
                            this.m_SetSkillBonuses = new AosSkillBonuses(this);

                        if (GetSaveFlag(sflags, SetFlag.PhysicalBonus))
                            this.m_SetPhysicalBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.FireBonus))
                            this.m_SetFireBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.ColdBonus))
                            this.m_SetColdBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.PoisonBonus))
                            this.m_SetPoisonBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.EnergyBonus))
                            this.m_SetEnergyBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.Hue))
                            this.m_SetHue = reader.ReadEncodedInt();

                        if (GetSaveFlag(sflags, SetFlag.LastEquipped))
                            this.m_LastEquipped = reader.ReadBool();

                        if (GetSaveFlag(sflags, SetFlag.SetEquipped))
                            this.m_SetEquipped = reader.ReadBool();

                        if (GetSaveFlag(sflags, SetFlag.SetSelfRepair))
                            this.m_SetSelfRepair = reader.ReadEncodedInt();

                        goto case 5;
                    }
                case 7:
                case 6:
                case 5:
                    {
                        SaveFlag flags = (SaveFlag)reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Attributes))
                            this.m_AosAttributes = new AosAttributes(this, reader);
                        else
                            this.m_AosAttributes = new AosAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.ArmorAttributes))
                            this.m_AosArmorAttributes = new AosArmorAttributes(this, reader);
                        else
                            this.m_AosArmorAttributes = new AosArmorAttributes(this);

                        if (GetSaveFlag(flags, SaveFlag.PhysicalBonus))
                            this.m_PhysicalBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.FireBonus))
                            this.m_FireBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.ColdBonus))
                            this.m_ColdBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.PoisonBonus))
                            this.m_PoisonBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.EnergyBonus))
                            this.m_EnergyBonus = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Identified))
                            this.m_Identified = (version >= 7 || reader.ReadBool());

                        if (GetSaveFlag(flags, SaveFlag.Ukrycie))
                            this.m_Ukrycie = (version >= 7 || reader.ReadBool());

                        if (GetSaveFlag(flags, SaveFlag.Sprawdzony))
                            this.m_Sprawdzony = (version >= 7 || reader.ReadBool());

                        if (GetSaveFlag(flags, SaveFlag.MaxHitPoints))
                            this.m_MaxHitPoints = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.HitPoints))
                            this.m_HitPoints = reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Crafter))
                            this.m_Crafter = reader.ReadMobile();

                        if (GetSaveFlag(flags, SaveFlag.Quality))
                            this.m_Quality = (ArmorQuality)reader.ReadEncodedInt();
                        else
                            this.m_Quality = ArmorQuality.None;

                        if (version == 5 && this.m_Quality == ArmorQuality.None)
                            this.m_Quality = ArmorQuality.None;

                        if (GetSaveFlag(flags, SaveFlag.Cechy))
                            this.m_Cechy = (ArmorCechy)reader.ReadEncodedInt();

                        if (GetSaveFlag(flags, SaveFlag.Durability))
                        {
                            this.m_Durability = (ArmorDurabilityLevel)reader.ReadEncodedInt();

                            if (this.m_Durability > ArmorDurabilityLevel.Indestructible)
                                this.m_Durability = ArmorDurabilityLevel.Durable;
                        }

                        if (GetSaveFlag(flags, SaveFlag.Protection))
                        {
                            this.m_Protection = (ArmorProtectionLevel)reader.ReadEncodedInt();

                            if (this.m_Protection > ArmorProtectionLevel.Invulnerability)
                                this.m_Protection = ArmorProtectionLevel.Defense;
                        }

                        if (GetSaveFlag(flags, SaveFlag.Resource))
                            this.m_Resource = (CraftResource)reader.ReadEncodedInt();
                        else
                            this.m_Resource = this.DefaultResource;

                        if (this.m_Resource == CraftResource.None)
                            this.m_Resource = this.DefaultResource;

                        if (GetSaveFlag(flags, SaveFlag.BaseArmor))
                            this.m_ArmorBase = reader.ReadEncodedInt();
                        else
                            this.m_ArmorBase = -1;

                        if (GetSaveFlag(flags, SaveFlag.StrBonus))
                            this.m_StrBonus = reader.ReadEncodedInt();
                        else
                            this.m_StrBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.DexBonus))
                            this.m_DexBonus = reader.ReadEncodedInt();
                        else
                            this.m_DexBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.IntBonus))
                            this.m_IntBonus = reader.ReadEncodedInt();
                        else
                            this.m_IntBonus = -1;

                        if (GetSaveFlag(flags, SaveFlag.StrReq))
                            this.m_StrReq = reader.ReadEncodedInt();
                        else
                            this.m_StrReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.DexReq))
                            this.m_DexReq = reader.ReadEncodedInt();
                        else
                            this.m_DexReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.IntReq))
                            this.m_IntReq = reader.ReadEncodedInt();
                        else
                            this.m_IntReq = -1;

                        if (GetSaveFlag(flags, SaveFlag.MedAllowance))
                            this.m_Meditate = (AMA)reader.ReadEncodedInt();
                        else
                            this.m_Meditate = (AMA)(-1);

                        if (GetSaveFlag(flags, SaveFlag.SkillBonuses))
                            this.m_AosSkillBonuses = new AosSkillBonuses(this, reader);

                        if (GetSaveFlag(flags, SaveFlag.PlayerConstructed))
                            this.m_PlayerConstructed = true;

                        if (version > 7 && GetSaveFlag(flags, SaveFlag.xAbsorptionAttributes))
                            this.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this, reader);
                        else
                            this.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this);

                        break;
                    }
                case 4:
                    {
                        this.m_AosAttributes = new AosAttributes(this, reader);
                        this.m_AosArmorAttributes = new AosArmorAttributes(this, reader);
                        goto case 3;
                    }
                case 3:
                    {
                        this.m_PhysicalBonus = reader.ReadInt();
                        this.m_FireBonus = reader.ReadInt();
                        this.m_ColdBonus = reader.ReadInt();
                        this.m_PoisonBonus = reader.ReadInt();
                        this.m_EnergyBonus = reader.ReadInt();
                        goto case 2;
                    }
                case 2:
                case 1:
                    {
                        this.m_Identified = reader.ReadBool();
                        goto case 0;
                    }
                case 0:
                    {
                        this.m_ArmorBase = reader.ReadInt();
                        this.m_MaxHitPoints = reader.ReadInt();
                        this.m_HitPoints = reader.ReadInt();
                        this.m_Crafter = reader.ReadMobile();
                        this.m_Quality = (ArmorQuality)reader.ReadInt();
                        ///
                        this.m_Cechy = (ArmorCechy)reader.ReadInt();
                        this.m_Ukrycie = reader.ReadBool();
                        this.m_Sprawdzony = reader.ReadBool();
                        ///
                        this.m_Durability = (ArmorDurabilityLevel)reader.ReadInt();
                        this.m_Protection = (ArmorProtectionLevel)reader.ReadInt();

                        AMT mat = (AMT)reader.ReadInt();

                        if (this.m_ArmorBase == this.RevertArmorBase)
                            this.m_ArmorBase = -1;

                        /*m_BodyPos = (ArmorBodyType)*/
                        reader.ReadInt();

                        if (version < 4)
                        {
                            this.m_AosAttributes = new AosAttributes(this);
                            this.m_AosArmorAttributes = new AosArmorAttributes(this);
                        }

                        if (version < 3 && this.m_Quality == ArmorQuality.Doskona�)
                            this.DistributeBonuses(6);

                        if (version < 3 && this.m_Quality == ArmorQuality.Wspania�)
                            this.DistributeBonuses(6);

                        if (version < 3 && this.m_Quality == ArmorQuality.Wyj�tkow)
                            this.DistributeBonuses(6);

                        if (version < 3 && this.m_Quality == ArmorQuality.Niezwyk�)
                            this.DistributeBonuses(6);

                        if (version < 3 && this.m_Quality == ArmorQuality.Cudown)
                            this.DistributeBonuses(6);

                        if (version < 3 && this.m_Quality == ArmorQuality.Mistyczn)
                            this.DistributeBonuses(6);

                        if (version < 3 && this.m_Quality == ArmorQuality.Legendarn)
                            this.DistributeBonuses(6);

                        if (version >= 2)
                        {
                            this.m_Resource = (CraftResource)reader.ReadInt();
                        }
                        else
                        {
                            OreInfo info;

                            switch (reader.ReadInt())
                            {
                                default:
                                case 0:
                                    info = OreInfo.Iron;
                                    break;
                                case 1:
                                    info = OreInfo.DullCopper;
                                    break;
                                case 2:
                                    info = OreInfo.ShadowIron;
                                    break;
                                case 3:
                                    info = OreInfo.Copper;
                                    break;
                                case 4:
                                    info = OreInfo.Bronze;
                                    break;
                                case 5:
                                    info = OreInfo.Gold;
                                    break;
                                case 6:
                                    info = OreInfo.Agapite;
                                    break;
                                case 7:
                                    info = OreInfo.Verite;
                                    break;
                                case 8:
                                    info = OreInfo.Valorite;
                                    break;
                            }

                            this.m_Resource = CraftResources.GetFromOreInfo(info, mat);
                        }

                        this.m_StrBonus = reader.ReadInt();
                        this.m_DexBonus = reader.ReadInt();
                        this.m_IntBonus = reader.ReadInt();
                        this.m_StrReq = reader.ReadInt();
                        this.m_DexReq = reader.ReadInt();
                        this.m_IntReq = reader.ReadInt();

                        if (this.m_StrBonus == this.OldStrBonus)
                            this.m_StrBonus = -1;

                        if (this.m_DexBonus == this.OldDexBonus)
                            this.m_DexBonus = -1;

                        if (this.m_IntBonus == this.OldIntBonus)
                            this.m_IntBonus = -1;

                        if (this.m_StrReq == this.OldStrReq)
                            this.m_StrReq = -1;

                        if (this.m_DexReq == this.OldDexReq)
                            this.m_DexReq = -1;

                        if (this.m_IntReq == this.OldIntReq)
                            this.m_IntReq = -1;

                        this.m_Meditate = (AMA)reader.ReadInt();

                        if (this.m_Meditate == this.OldMedAllowance)
                            this.m_Meditate = (AMA)(-1);

                        if (this.m_Resource == CraftResource.None)
                        {
                            if (mat == ArmorMaterialType.Studded || mat == ArmorMaterialType.Leather)
                                this.m_Resource = CraftResource.RegularLeather;
                            else if (mat == ArmorMaterialType.Spined)
                                this.m_Resource = CraftResource.SpinedLeather;
                            else if (mat == ArmorMaterialType.Horned)
                                this.m_Resource = CraftResource.HornedLeather;
                            else if (mat == ArmorMaterialType.Barbed)
                                this.m_Resource = CraftResource.BarbedLeather;
                            else
                                this.m_Resource = CraftResource.Iron;
                        }

                        if (this.m_MaxHitPoints == 0 && this.m_HitPoints == 0)
                            this.m_HitPoints = this.m_MaxHitPoints = Utility.RandomMinMax(this.InitMinHits, this.InitMaxHits);

                        break;
                    }
            }

            #region Mondain's Legacy Sets
            if (this.m_SetAttributes == null)
                this.m_SetAttributes = new AosAttributes(this);

            if (this.m_SetSkillBonuses == null)
                this.m_SetSkillBonuses = new AosSkillBonuses(this);
            #endregion

            if (this.m_AosSkillBonuses == null)
                this.m_AosSkillBonuses = new AosSkillBonuses(this);

            if (Core.AOS && this.Parent is Mobile)
                this.m_AosSkillBonuses.AddTo((Mobile)this.Parent);

            int strBonus = this.ComputeStatBonus(StatType.Str);
            int dexBonus = this.ComputeStatBonus(StatType.Dex);
            int intBonus = this.ComputeStatBonus(StatType.Int);

            if (this.Parent is Mobile && (strBonus != 0 || dexBonus != 0 || intBonus != 0))
            {
                Mobile m = (Mobile)this.Parent;

                string modName = this.Serial.ToString();

                if (strBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if (dexBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if (intBonus != 0)
                    m.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            if (this.Parent is Mobile)
                ((Mobile)this.Parent).CheckStatTimers();

            if (version < 7)
                this.m_PlayerConstructed = true; // we don't know, so, assume it's crafted
        }

        public virtual CraftResource DefaultResource
        {
            get
            {
                return CraftResource.Iron;
            }
        }

        public BaseArmor(int itemID)
            : base(itemID)
        {
            this.m_Quality = ArmorQuality.None;
            ///
            this.m_Cechy = ArmorCechy.None;
            this.m_Ukrycie = true;
            ///
            this.m_Durability = ArmorDurabilityLevel.Regular;
            this.m_Crafter = null;

            this.m_Resource = this.DefaultResource;
            this.Hue = CraftResources.GetHue(this.m_Resource);

            this.m_HitPoints = this.m_MaxHitPoints = Utility.RandomMinMax(this.InitMinHits, this.InitMaxHits);

            this.Layer = (Layer)this.ItemData.Quality;

            this.m_AosAttributes = new AosAttributes(this);
            this.m_AosArmorAttributes = new AosArmorAttributes(this);
            this.m_AosSkillBonuses = new AosSkillBonuses(this);

            this.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this);

            #region Mondain's Legacy Sets
            this.m_SetAttributes = new AosAttributes(this);
            this.m_SetSkillBonuses = new AosSkillBonuses(this);
            #endregion
            this.m_AosSkillBonuses = new AosSkillBonuses(this);

            // Mod to randomly add sockets and socketability features to armor. These settings will yield
            // 2% drop rate of socketed/socketable items
            // 0.1% chance of 5 sockets
            // 0.5% of 4 sockets
            // 3% chance of 3 sockets
            // 15% chance of 2 sockets
            // 50% chance of 1 socket
            // the remainder will be 0 socket (31.4% in this case)
            // uncomment the next line to prevent artifacts from being socketed
            // if(ArtifactRarity == 0)
            if (XmlConfig.XmlSocketsEnabled)
                XmlSockets.ConfigureRandom(this, 2.0, 0.1, 0.5, 3.0, 15.0, 50.0);
        }

        public override bool AllowSecureTrade(Mobile from, Mobile to, Mobile newOwner, bool accepted)
        {
            if (!Ethics.Ethic.CheckTrade(from, to, newOwner, this))
                return false;

            return base.AllowSecureTrade(from, to, newOwner, accepted);
        }

        public virtual Race RequiredRace
        {
            get
            {
                return null;
            }
        }

        public virtual bool CanBeWornByGargoyles
        {
            get
            {
                return false;
            }
        }

        public override bool CanEquip(Mobile from)
        {
            if (!Ethics.Ethic.CheckEquip(from, this))
                return false;

            if (from.IsPlayer())
            {
                if (from.Race == Race.Gargoyle && !this.CanBeWornByGargoyles)
                {
                    from.SendLocalizedMessage(1111708); // Gargoyles can't wear this.
                    return false;
                }
                #region ItemID_Mods
                if (m_Identified == false)
                {
                    from.SendMessage("Ten przedmiot jest niezidentyfikowany!");
                    return false;
                }
                #endregion

                if (this.RequiredRace != null && from.Race != this.RequiredRace)
                {
                    if (this.RequiredRace == Race.Elf)
                        from.SendLocalizedMessage(1072203); // Only Elves may use this.
                    else if (this.RequiredRace == Race.Gargoyle)
                        from.SendLocalizedMessage(1111707); // Only gargoyles can wear this.
                    else
                        from.SendMessage("Only {0} may use this.", this.RequiredRace.PluralName);

                    return false;
                }
                else if (!this.AllowMaleWearer && !from.Female)
                {
                    if (this.AllowFemaleWearer)
                        from.SendLocalizedMessage(1010388); // Only females can wear this.
                    else
                        from.SendMessage("You may not wear this.");

                    return false;
                }
                else if (!this.AllowFemaleWearer && from.Female)
                {
                    if (this.AllowMaleWearer)
                        from.SendLocalizedMessage(1063343); // Only males can wear this.
                    else
                        from.SendMessage("You may not wear this.");

                    return false;
                }
                #region Personal Bless Deed
                else if (this.BlessedBy != null && this.BlessedBy != from)
                {
                    from.SendLocalizedMessage(1075277); // That item is blessed by another player.

                    return false;
                }
                #endregion
                else
                {
                    int strBonus = this.ComputeStatBonus(StatType.Str), strReq = this.ComputeStatReq(StatType.Str);
                    int dexBonus = this.ComputeStatBonus(StatType.Dex), dexReq = this.ComputeStatReq(StatType.Dex);
                    int intBonus = this.ComputeStatBonus(StatType.Int), intReq = this.ComputeStatReq(StatType.Int);

                    if (from.Dex < dexReq || (from.Dex + dexBonus) < 1)
                    {
                        from.SendLocalizedMessage(502077); // You do not have enough dexterity to equip this item.
                        return false;
                    }
                    else if (from.Str < strReq || (from.Str + strBonus) < 1)
                    {
                        from.SendLocalizedMessage(500213); // You are not strong enough to equip that.
                        return false;
                    }
                    else if (from.Int < intReq || (from.Int + intBonus) < 1)
                    {
                        from.SendMessage("You are not smart enough to equip that.");
                        return false;
                    }
                }
            }

            if (!Server.Engines.XmlSpawner2.XmlAttach.CheckCanEquip(this, from))
                return false;
            else
                return base.CanEquip(from);
        }

        public override bool CheckPropertyConfliction(Mobile m)
        {
            if (base.CheckPropertyConfliction(m))
                return true;

            if (this.Layer == Layer.Pants)
                return (m.FindItemOnLayer(Layer.InnerLegs) != null);

            if (this.Layer == Layer.Shirt)
                return (m.FindItemOnLayer(Layer.InnerTorso) != null);

            return false;
        }

        public override bool OnEquip(Mobile from)
        {
            from.CheckStatTimers();

            int strBonus = this.ComputeStatBonus(StatType.Str);
            int dexBonus = this.ComputeStatBonus(StatType.Dex);
            int intBonus = this.ComputeStatBonus(StatType.Int);

            if (strBonus != 0 || dexBonus != 0 || intBonus != 0)
            {
                string modName = this.Serial.ToString();

                if (strBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Str, modName + "Str", strBonus, TimeSpan.Zero));

                if (dexBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Dex, modName + "Dex", dexBonus, TimeSpan.Zero));

                if (intBonus != 0)
                    from.AddStatMod(new StatMod(StatType.Int, modName + "Int", intBonus, TimeSpan.Zero));
            }

            Server.Engines.XmlSpawner2.XmlAttach.CheckOnEquip(this, from);

            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            if (parent is Mobile)
            {
                Mobile m = (Mobile)parent;
                string modName = this.Serial.ToString();

                m.RemoveStatMod(modName + "Str");
                m.RemoveStatMod(modName + "Dex");
                m.RemoveStatMod(modName + "Int");

                if (Core.AOS)
                    this.m_AosSkillBonuses.Remove();

                ((Mobile)parent).Delta(MobileDelta.Armor); // Tell them armor rating has changed
                m.CheckStatTimers();

                #region Mondain's Legacy Sets
                if (this.IsSetItem && this.m_SetEquipped)
                    SetHelper.RemoveSetBonus(m, this.SetID, this);
                #endregion
            }

            Server.Engines.XmlSpawner2.XmlAttach.CheckOnRemoved(this, parent);

            base.OnRemoved(parent);
        }

        public virtual int OnHit(BaseWeapon weapon, int damageTaken)
        {
            double HalfAr = this.ArmorRating / 2.0;
            int Absorbed = (int)(HalfAr + HalfAr * Utility.RandomDouble());

            damageTaken -= Absorbed;
            if (damageTaken < 0)
                damageTaken = 0;

            if (Absorbed < 2)
                Absorbed = 2;

            if (25 > Utility.Random(100)) // 25% chance to lower durability
            {
                if (Core.AOS && this.m_AosArmorAttributes.SelfRepair + (this.IsSetItem && this.m_SetEquipped ? this.m_SetSelfRepair : 0) > Utility.Random(10))
                {
                    this.HitPoints += 2;
                }
                else
                {
                    int wear;

                    if (weapon.Type == WeaponType.Bashing)
                        wear = Absorbed / 2;
                    else
                        wear = Utility.Random(2);

                    if (wear > 0 && this.m_MaxHitPoints > 0)
                    {
                        if (this.m_HitPoints >= wear)
                        {
                            this.HitPoints -= wear;
                            wear = 0;
                        }
                        else
                        {
                            wear -= this.HitPoints;
                            this.HitPoints = 0;
                        }

                        if (wear > 0)
                        {
                            if (this.m_MaxHitPoints > wear)
                            {
                                this.MaxHitPoints -= wear;

                                if (this.Parent is Mobile)
                                    ((Mobile)this.Parent).LocalOverheadMessage(MessageType.Regular, 0x3B2, 1061121); // Your equipment is severely damaged.
                            }
                            else
                            {
                                this.Delete();
                            }
                        }
                    }
                }
            }

            return damageTaken;
        }

        private string GetNameString()
        {
            string name = this.Name;

            if (name == null)
                name = String.Format("#{0}", this.LabelNumber);

            return name;
        }

        [Hue, CommandProperty(AccessLevel.GameMaster)]
        public override int Hue
        {
            get
            {
                return base.Hue;
            }
            set
            {
                base.Hue = value;
                this.InvalidateProperties();
            }
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            string oreType;

            if (Hue == 0)
            {
                oreType = "";
            }
            else
            {
                switch (m_Resource)
                {
                    case CraftResource.DullCopper: oreType = " z Matowej Miedzi"; break; // dull copper
                    case CraftResource.ShadowIron: oreType = " z Cienistego �elaza"; break; // shadow iron
                    case CraftResource.Copper: oreType = " z Miedzi"; break; // copper
                    case CraftResource.Bronze: oreType = " z Br�zu"; break; // bronze					
                    case CraftResource.Gold: oreType = " ze Z�ota"; break; // golden
                    case CraftResource.Agapite: oreType = " z Agapitu"; break; // agapite					
                    case CraftResource.Verite: oreType = " z Verytu"; break; // verite
                    case CraftResource.Valorite: oreType = " z Valorytu"; break; // valorite
                    case CraftResource.SpinedLeather: oreType = " z Szarych sk�r"; break; // Spined
                    case CraftResource.HornedLeather: oreType = " z Czerwonych sk�r"; break; // Horned
                    case CraftResource.BarbedLeather: oreType = " z Zielonych sk�r"; break; // Barbed

                    default: oreType = ""; break;
                }
            }

            if (this.Name == null)
            {
                list.Add(this.LabelNumber);
            }

            if (m_Identified == false)
            {
                if (this.Name != null && m_Sprawdzony == false)
                {
                    list.Add(this.Name);
                    list.Add("<BASEFONT COLOR=YELLOW><B>[Niezidentyfikowany]</B><BASEFONT COLOR=WHITE>"); // Unidentified

                }

                if (this.Name != null && m_Sprawdzony == true)
                {
                    list.Add(this.Name);
                    //list.Add("<BASEFONT COLOR=YELLOW><B>[W�a�ciwo�ci]</B><BASEFONT COLOR=WHITE>"); // Unidentified

                    base.AddResistanceProperties(list);
                }
            }

            if (m_Identified == true)
            {

                ////Rodzaj M�ski
                if (this is PlateGorget || this is PlateHelm || this is ChainCoif || this is ChainHatsuburi || this is LeatherBustierArms || this is LeatherGorget || this is LeatherCap || this is StuddedBustierArms || this is StuddedGorget || this is Buckler || this is BoneHelm || this is WingedHelm || this is VultureHelm || this is RavenHelm
                || this is RoyalCirclet || this is RoyalCirclet || this is GemmedCirclet || this is Circlet || this is OrcHelm || this is NorseHelm || this is Helmet || this is CloseHelm)
                {
                    if (m_Resource == CraftResource.None)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null)
                        {
                            list.Add(this.Name);
                        }
                    }

                    if (m_Resource == CraftResource.Iron)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add(this.Name);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add("{0}y " + this.Name, this.Quality);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{1}y {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{0}y " + this.Name, this.m_Cechy);
                            }
                        }
                    }

                    if (m_Resource == CraftResource.RegularLeather)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add(this.Name);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add("{0}y " + this.Name, this.Quality);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{1}y {0}y " + this.Name, this.Quality, this.m_Cechy);
                            }
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{0}y</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{0}y " + this.Name, this.m_Cechy);
                            }
                        }
                    }

                    if (m_Cechy == ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather)
                    {
                        if (this.m_Quality == ArmorQuality.None)
                        {
                            list.Add("{1}{0}", oreType, this.GetNameString());
                        }

                        if (this.m_Quality == ArmorQuality.S�ab)
                        {
                            list.Add(1041522, "S�aby \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Przeci�tn)
                        {
                            list.Add(1041522, "Przeci�tny \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Zwyk�)
                        {
                            list.Add(1041522, "Zwyk�y \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Dobr)
                        {
                            list.Add(1041522, "Dobry \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Doskona�)
                        {
                            list.Add(1041522, "Doskona�y \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Wspania�)
                        {
                            list.Add(1041522, "Wspania�y \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Wyj�tkow)
                        {
                            list.Add(1041522, "Wyj�tkowy \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Niezwyk�)
                        {
                            list.Add(1041522, "Niezwyk�y \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Cudown)
                        {
                            list.Add(1041522, "Cudowny \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Mistyczn)
                        {
                            list.Add(1041522, "Mistyczny \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Legendarn)
                        {
                            list.Add(1041522, "Legendarny \t{0}\t{1}", this.GetNameString(), oreType);
                        }
                    }

                    if (m_Cechy != ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather)
                    {
                        if (this.m_Quality == ArmorQuality.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, " {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.S�ab)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "S�aby <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "S�aby {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Przeci�tn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Przeci�tny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Przeci�tny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Zwyk�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Zwyk�y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Zwyk�y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Dobr)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Dobry <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Dobry {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Doskona�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Doskona�y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Doskona�y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Wspania�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Wspania�y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Wspania�y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Wyj�tkow)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Wyj�tkowy <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Wyj�tkowy {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Niezwyk�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Niezwyk�y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Niezwyk�y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Cudown)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Cudowny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Cudowny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Mistyczn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Mistyczny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Mistyczny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Legendarn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Legendarny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Legendarny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }
                    }
                }
                ////Rodzaj �e�ski
                if (this is FemalePlateChest || this is PlateChest || this is ChainChest || this is RingmailChest || this is FemaleLeatherChest || this is LeatherChest || this is LeatherSkirt || this is FemaleStuddedChest || this is StuddedChest || this is BronzeShield || this is ChaosShield || this is OrderShield || this is MetalKiteShield || this is HeaterShield || this is MetalShield || this is BoneChest
                || this is Bascinet)
                {
                    if (m_Resource == CraftResource.None)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null)
                        {
                            list.Add(this.Name);
                        }
                    }

                    if (m_Resource == CraftResource.Iron)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add(this.Name);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add("{0}a " + this.Name, this.Quality);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{1}a {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{0}a " + this.Name, this.m_Cechy);
                            }
                        }
                    }

                    if (m_Resource == CraftResource.RegularLeather)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add(this.Name);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add("{0}a " + this.Name, this.Quality);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{1}a {0}a " + this.Name, this.Quality, this.m_Cechy);
                            }
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{0}a</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{0}a " + this.Name, this.m_Cechy);
                            }
                        }
                    }

                    if (m_Cechy == ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather)
                    {
                        if (this.m_Quality == ArmorQuality.None)
                        {
                            list.Add("{1}{0}", oreType, this.GetNameString());
                        }

                        if (this.m_Quality == ArmorQuality.S�ab)
                        {
                            list.Add(1041522, "S�aba \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Przeci�tn)
                        {
                            list.Add(1041522, "Przeci�tna \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Zwyk�)
                        {
                            list.Add(1041522, "Zwyk�a \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Dobr)
                        {
                            list.Add(1041522, "Dobra \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Doskona�)
                        {
                            list.Add(1041522, "Doskona�a \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Wspania�)
                        {
                            list.Add(1041522, "Wspania�a \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Wyj�tkow)
                        {
                            list.Add(1041522, "Wyj�tkowa \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Niezwyk�)
                        {
                            list.Add(1041522, "Niezwyk�a \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Cudown)
                        {
                            list.Add(1041522, "Cudowna \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Mistyczn)
                        {
                            list.Add(1041522, "Mistyczna \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Legendarn)
                        {
                            list.Add(1041522, "Legendarna \t{0}\t{1}", this.GetNameString(), oreType);
                        }
                    }

                    if (m_Cechy != ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather)
                    {
                        if (this.m_Quality == ArmorQuality.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, " {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.S�ab)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "S�aba <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "S�aba {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Przeci�tn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Przeci�tna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Przeci�tna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Zwyk�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Zwyk�a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Zwyk�a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Dobr)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Dobra <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Dobra {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Doskona�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Doskona�a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Doskona�a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Wspania�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Wspania�a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Wspania�a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Wyj�tkow)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Wyj�tkowa <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Wyj�tkowa {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Niezwyk�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Niezwyk�a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Niezwyk�a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Cudown)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Cudowna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Cudowna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Mistyczn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Mistyczna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Mistyczna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Legendarn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Legendarna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Legendarna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }
                    }
                }
                ////Rodzaj Nijaki
                if (this is PlateGloves || this is PlateArms || this is PlateLegs || this is ChainLegs || this is RingmailArms || this is RingmailGloves || this is RingmailLegs || this is LeatherArms || this is LeatherGloves || this is LeatherLegs || this is LeatherShorts
                     || this is StuddedGloves || this is StuddedLegs || this is StuddedArms || this is BoneArms || this is BoneGloves || this is BoneLegs || this is LeafChest || this is LeafArms || this is LeafGloves || this is LeafGorget || this is LeafLegs || this is LeafTonlet || this is FemaleLeafChest
                     || this is LeatherNinjaHood || this is LeatherNinjaJacket || this is LeatherNinjaMitts || this is LeatherNinjaPants
                     || this is HideChest || this is HideFemaleChest || this is HideGloves || this is HideGorget || this is HidePants || this is HidePauldrons
                     || this is StuddedDo || this is StuddedHaidate || this is StuddedHiroSode || this is StuddedMempo || this is StuddedSuneate
                     || this is LeatherShorts || this is LeatherSkirt || this is LeatherBustierArms || this is StuddedBustierArms || this is FemaleLeatherChest || this is FemaleStuddedChest
                     || this is PlateBattleKabuto || this is PlateDo || this is PlateHatsuburi || this is PlateMempo || this is PlateSuneate || this is PlateHiroSode)
                {
                    if (m_Resource == CraftResource.None)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null)
                        {
                            list.Add(this.Name);
                        }
                    }

                    if (m_Resource == CraftResource.Iron)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add(this.Name);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add("{0}e " + this.Name, this.Quality);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{1}e {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{0}e " + this.Name, this.m_Cechy);
                            }
                        }
                    }

                    if (m_Resource == CraftResource.RegularLeather)
                    {
                        if (this.Name == null)
                        {
                            list.Add(this.LabelNumber);
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add(this.Name);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                        {
                            list.Add("{0}e " + this.Name, this.Quality);
                        }
                        if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{1}e {0}e " + this.Name, this.Quality, this.m_Cechy);
                            }
                        }
                        if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add("<BASEFONT COLOR=#CC0033><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add("<BASEFONT COLOR=#0066FF><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add("<BASEFONT COLOR=#2F4F4F><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add("<BASEFONT COLOR=#6B238E><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add("<BASEFONT COLOR=#CC9900><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add("<BASEFONT COLOR=#666600><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add("<BASEFONT COLOR=#855E42><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add("<BASEFONT COLOR=#CC3300><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add("<BASEFONT COLOR=#238E23><B>{0}e</B><BASEFONT COLOR=YELLOW> " + this.Name, this.m_Cechy);
                            }
                            else
                            {
                                list.Add("{0}e " + this.Name, this.m_Cechy);
                            }
                        }
                    }

                    if (m_Cechy == ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather)
                    {
                        if (this.m_Quality == ArmorQuality.None)
                        {
                            list.Add("{1}{0}", oreType, this.GetNameString());
                        }

                        if (this.m_Quality == ArmorQuality.S�ab)
                        {
                            list.Add(1041522, "S�abe \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Przeci�tn)
                        {
                            list.Add(1041522, "Przeci�tne \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Zwyk�)
                        {
                            list.Add(1041522, "Zwyk�e \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Dobr)
                        {
                            list.Add(1041522, "Dobre \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Doskona�)
                        {
                            list.Add(1041522, "Doskona�e \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Wspania�)
                        {
                            list.Add(1041522, "Wspania�e \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Wyj�tkow)
                        {
                            list.Add(1041522, "Wyj�tkowe \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Niezwyk�)
                        {
                            list.Add(1041522, "Niezwyk�e \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Cudown)
                        {
                            list.Add(1041522, "Cudowne \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Mistyczn)
                        {
                            list.Add(1041522, "Mistyczne \t{0}\t{1}", this.GetNameString(), oreType);
                        }

                        if (this.m_Quality == ArmorQuality.Legendarn)
                        {
                            list.Add(1041522, "Legendarne \t{0}\t{1}", this.GetNameString(), oreType);
                        }
                    }

                    if (m_Cechy != ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather)
                    {
                        if (this.m_Quality == ArmorQuality.None)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, " <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, " {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.S�ab)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "S�abe <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "S�abe {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Przeci�tn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Przeci�tne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Przeci�tne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Zwyk�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Zwyk�e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Zwyk�e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Dobr)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Dobre <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Dobre {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Doskona�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Doskona�e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Doskona�e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Wspania�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Wspania�e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Wspania�e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Wyj�tkow)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Wyj�tkowe <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Wyj�tkowe {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Niezwyk�)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Niezwyk�e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Niezwyk�e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Cudown)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Cudowne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Cudowne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Mistyczn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Mistyczne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Mistyczne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }

                        if (this.m_Quality == ArmorQuality.Legendarn)
                        {
                            if (this.m_Cechy == ArmorCechy.Witaln)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.M�dr)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Stabiln)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Ochronn)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Szcz�liw)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Oszcz�dn)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Wytrzyma�)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Odbijaj�c)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else if (this.m_Cechy == ArmorCechy.Obronn)
                            {
                                list.Add(1041522, "Legendarne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                            else
                            {
                                list.Add(1041522, "Legendarne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy);
                            }
                        }
                    }
                }

                if (m_Sprawdzony == true)
                {
                    base.AddResistanceProperties(list);
                }

            }  ////Identified
        }

        public override bool AllowEquipedCast(Mobile from)
        {
            if (base.AllowEquipedCast(from))
                return true;

            return (this.m_AosAttributes.SpellChanneling != 0);
        }

        public virtual int GetLuckBonus()
        {
            if (this.m_Resource == CraftResource.Heartwood)
                return 0;

            CraftResourceInfo resInfo = CraftResources.GetInfo(this.m_Resource);

            if (resInfo == null)
                return 0;

            CraftAttributeInfo attrInfo = resInfo.AttributeInfo;

            if (attrInfo == null)
                return 0;

            return attrInfo.ArmorLuck;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            if (this.m_TimesImbued > 0)
                list.Add(1080418); // (Imbued)

            if (this.m_Crafter != null)
                list.Add(1050043, this.m_Crafter.Name); // crafted by ~1_NAME~

            #region Factions
            if (this.m_FactionState != null)
                list.Add(1041350); // faction item
            #endregion

            #region Mondain's Legacy Sets
            if (this.IsSetItem)
            {
                if (this.MixedSet)
                    list.Add(1073491, this.Pieces.ToString()); // Part of a Weapon/Armor Set (~1_val~ pieces)
                else
                    list.Add(1072376, this.Pieces.ToString()); // Part of an Armor Set (~1_val~ pieces)

                if (this.m_SetEquipped)
                {
                    if (this.MixedSet)
                        list.Add(1073492); // Full Weapon/Armor Set Present
                    else
                        list.Add(1072377); // Full Armor Set Present

                    this.GetSetProperties(list);
                }
            }
            #endregion

            if (this.RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only - Tylko Elfy
            else if (this.RequiredRace == Race.Gargoyle)
                list.Add(1111709); // Gargoyles Only - Tylko Demony
            this.m_AosSkillBonuses.GetProperties(list);

            int prop;

            if (m_Identified == true)
            {

                if (m_Ukrycie == false && (ArtifactRarity) > 0 || m_Ukrycie == false && (m_AosAttributes.WeaponDamage) != 0 || m_Ukrycie == false && (m_AosAttributes.DefendChance) != 0 ||
                m_Ukrycie == false && (m_AosAttributes.BonusDex) != 0 || m_Ukrycie == false && (m_AosAttributes.EnhancePotions) != 0 || m_Ukrycie == false && (m_AosAttributes.CastRecovery) != 0 ||
                m_Ukrycie == false && (m_AosAttributes.CastSpeed) != 0 || m_Ukrycie == false && (m_AosAttributes.AttackChance) != 0 || m_Ukrycie == false && (m_AosAttributes.BonusHits) != 0 ||
                m_Ukrycie == false && (m_AosAttributes.BonusInt) != 0 || m_Ukrycie == false && (m_AosAttributes.LowerManaCost) != 0 || m_Ukrycie == false && (m_AosAttributes.LowerRegCost) != 0 ||
                m_Ukrycie == false && (GetLowerStatReq()) != 0 || m_Ukrycie == false && ((GetLuckBonus() + m_AosAttributes.Luck)) != 0 || m_Ukrycie == false && (m_AosArmorAttributes.MageArmor) != 0 ||
                m_Ukrycie == false && (m_AosAttributes.BonusMana) != 0 || m_Ukrycie == false && (m_AosAttributes.RegenMana) != 0 || m_Ukrycie == false && (m_AosAttributes.NightSight) != 0 ||
                m_Ukrycie == false && (m_AosAttributes.ReflectPhysical) != 0 || m_Ukrycie == false && (m_AosAttributes.RegenStam) != 0 || m_Ukrycie == false && (m_AosAttributes.RegenHits) != 0 ||
                m_Ukrycie == false && (m_AosArmorAttributes.SelfRepair) != 0 || m_Ukrycie == false && (m_AosAttributes.SpellChanneling) != 0 || m_Ukrycie == false && (m_AosAttributes.SpellDamage) != 0 ||
                m_Ukrycie == false && (m_AosAttributes.BonusStam) != 0 || m_Ukrycie == false && (m_AosAttributes.BonusStr) != 0 || m_Ukrycie == false && (m_AosAttributes.WeaponSpeed) != 0)
                {

                    string liststring = null;
                    string liststring1 = null;
                    string liststring2 = null;
                    string liststring3 = null;
                    string liststring4 = null;
                    string liststring5 = null;
                    string liststring6 = null;
                    string liststring7 = null;
                    string liststring8 = null;
                    string liststring9 = null;
                    string liststring10 = null;
                    string liststring11 = null;
                    string liststring12 = null;
                    string liststring13 = null;
                    string liststring14 = null;
                    string liststring15 = null;
                    string liststring16 = null;
                    string liststring17 = null;
                    string liststring18 = null;
                    string liststring19 = null;
                    string liststring20 = null;

                    string liststring21 = null;
                    string liststring22 = null;
                    string liststring23 = null;
                    string liststring24 = null;
                    string liststring25 = null;
                    string liststring26 = null;
                    string liststring27 = null;
                    string liststring28 = null;
                    string liststring29 = null;
                    string liststring30 = null;
                    string liststring31 = null;
                    string liststring32 = null;
                    string liststring33 = null;
                    string liststring34 = null;
                    string liststring35 = null;
                    string liststring36 = null;
                    string liststring37 = null;
                    string liststring38 = null;
                    string liststring39 = null;
                    string liststring40 = null;
                    string liststring41 = null;

                    if (this.ArtifactRarity > 0)
                        liststring += "<CENTER>Artefaktyczna Jako�� " + ArtifactRarity;
                    if (this.ArtifactRarity == 0)
                        liststring += null;

                    if (this.m_AosAttributes.WeaponDamage != 0)
                        liststring1 += "<CENTER>Zwi�ksza Obra�enia " + m_AosAttributes.WeaponDamage + "%";
                    if (this.m_AosAttributes.WeaponDamage == 0)
                        liststring1 += null;

                    if (this.m_AosAttributes.DefendChance != 0)
                        liststring2 += "<CENTER>Zwi�ksza Szanse Obrony " + m_AosAttributes.DefendChance + "%";
                    if (this.m_AosAttributes.DefendChance == 0)
                        liststring2 += null;

                    if (this.m_AosAttributes.BonusDex != 0)
                        liststring3 += "<CENTER>Zwi�ksza Zr�czno�� " + m_AosAttributes.BonusDex;
                    if (this.m_AosAttributes.BonusDex == 0)
                        liststring3 += null;

                    if (this.m_AosAttributes.EnhancePotions != 0)
                        liststring4 += "<CENTER>Wzmocnienie Mikstur " + m_AosAttributes.EnhancePotions + "%";
                    if (this.m_AosAttributes.EnhancePotions == 0)
                        liststring4 += null;

                    if (this.m_AosAttributes.CastRecovery != 0)
                        liststring5 += "<CENTER>Odzyskiwanie R�wnowagi " + m_AosAttributes.CastRecovery;
                    if (this.m_AosAttributes.CastRecovery == 0)
                        liststring5 += null;

                    if (this.m_AosAttributes.CastSpeed != 0)
                        liststring6 += "<CENTER>Szybkie Czarowanie " + m_AosAttributes.CastSpeed;
                    if (this.m_AosAttributes.CastSpeed == 0)
                        liststring6 += null;

                    if (this.m_AosAttributes.AttackChance != 0)
                        liststring7 += "<CENTER>Zwi�ksza Szanse Ataku " + m_AosAttributes.AttackChance + "%";
                    if (this.m_AosAttributes.AttackChance == 0)
                        liststring7 += null;

                    if (this.m_AosAttributes.BonusHits != 0)
                        liststring8 += "<CENTER>Zwi�ksza �ywotno�� " + m_AosAttributes.BonusHits;
                    if (this.m_AosAttributes.BonusHits == 0)
                        liststring8 += null;

                    if (this.m_AosAttributes.BonusInt != 0)
                        liststring9 += "<CENTER>Zwi�ksza Inteligencje " + m_AosAttributes.BonusInt;
                    if (this.m_AosAttributes.BonusInt == 0)
                        liststring9 += null;

                    if (this.m_AosAttributes.LowerManaCost != 0)
                        liststring10 += "<CENTER>Zmniejsza Zu�ycie Many " + m_AosAttributes.LowerManaCost + "%";
                    if (this.m_AosAttributes.LowerManaCost == 0)
                        liststring10 += null;

                    if (this.m_AosAttributes.LowerRegCost != 0)
                        liststring11 += "<CENTER>Zmniejsza Zu�ycie Sk�adnik�w " + m_AosAttributes.LowerRegCost + "%";
                    if (this.m_AosAttributes.LowerRegCost == 0)
                        liststring11 += null;

                    if (this.GetLowerStatReq() != 0)
                        liststring12 += "<CENTER>Zmniejsza Wymagania " + GetLowerStatReq() + "%";
                    if (this.GetLowerStatReq() == 0)
                        liststring12 += null;

                    if (this.m_AosAttributes.Luck != 0)
                        liststring13 += "<CENTER>Szcz�cie " + m_AosAttributes.Luck;
                    if (this.m_AosAttributes.Luck == 0)
                        liststring13 += null;

                    if (this.m_AosArmorAttributes.MageArmor != 0)
                        liststring14 += "<CENTER>Zbroja Maga ";
                    if (this.m_AosArmorAttributes.MageArmor == 0)
                        liststring14 += null;

                    if (this.m_AosAttributes.BonusMana != 0)
                        liststring15 += "<CENTER>Zwi�ksza Mane " + m_AosAttributes.BonusMana;
                    if (this.m_AosAttributes.BonusMana == 0)
                        liststring15 += null;

                    if (this.m_AosAttributes.RegenMana != 0)
                        liststring16 += "<CENTER>Regeneruje Mane " + m_AosAttributes.RegenMana;
                    if (this.m_AosAttributes.RegenMana == 0)
                        liststring16 += null;

                    if (this.m_AosAttributes.NightSight != 0)
                        liststring17 += "<CENTER>Nocne Widzenie ";
                    if (this.m_AosAttributes.NightSight == 0)
                        liststring17 += null;

                    if (this.m_AosAttributes.ReflectPhysical != 0)
                        liststring18 += "<CENTER>Odbija Obra�enia Fizyczne " + m_AosAttributes.ReflectPhysical + "%";
                    if (this.m_AosAttributes.ReflectPhysical == 0)
                        liststring18 += null;

                    if (this.m_AosAttributes.RegenStam != 0)
                        liststring19 += "<CENTER>Regeneruje Wytrzyma�o�� " + m_AosAttributes.RegenStam;
                    if (this.m_AosAttributes.RegenStam == 0)
                        liststring19 += null;

                    if (this.m_AosAttributes.RegenHits != 0)
                        liststring20 += "<CENTER>Regeneruje �ywotno�� " + m_AosAttributes.RegenHits;
                    if (this.m_AosAttributes.RegenHits == 0)
                        liststring20 += null;

                    if (this.m_AosArmorAttributes.SelfRepair != 0)
                        liststring21 += "<CENTER>Samonaprawialny " + m_AosArmorAttributes.SelfRepair;
                    if (this.m_AosArmorAttributes.SelfRepair == 0)
                        liststring21 += null;

                    if (this.m_AosAttributes.SpellChanneling != 0)
                        liststring22 += "<CENTER>Przepuszczalno�� Zakl�� ";
                    if (this.m_AosAttributes.SpellChanneling == 0)
                        liststring22 += null;

                    if (this.m_AosAttributes.SpellDamage != 0)
                        liststring23 += "<CENTER>Zwi�ksza Moc Zakl�� " + m_AosAttributes.SpellDamage + "%";
                    if (this.m_AosAttributes.SpellDamage == 0)
                        liststring23 += null;

                    if (this.m_AosAttributes.BonusStam != 0)
                        liststring24 += "<CENTER>Zwi�ksza Wytrzyma�o�� " + m_AosAttributes.BonusStam;
                    if (this.m_AosAttributes.BonusStam == 0)
                        liststring24 += null;

                    if (this.m_AosAttributes.BonusStr != 0)
                        liststring25 += "<CENTER>Zwi�ksza Si�e " + m_AosAttributes.BonusStr;
                    if (this.m_AosAttributes.BonusStr == 0)
                        liststring25 += null;

                    if (this.m_AosAttributes.WeaponSpeed != 0)
                        liststring26 += "<CENTER>Zwi�ksza Szybko�� Zamachu " + m_AosAttributes.WeaponSpeed + "%";
                    if (this.m_AosAttributes.WeaponSpeed == 0)
                        liststring26 += null;

                    if (this.m_AosAttributes.IncreasedKarmaLoss != 0)
                        liststring27 += "<CENTER>Zwi�ksza Utrat� Karmy " + m_AosAttributes.IncreasedKarmaLoss + "%";
                    if (this.m_AosAttributes.IncreasedKarmaLoss == 0)
                        liststring27 += null;

                    if (this.m_SAAbsorptionAttributes.EaterFire != 0)
                        liststring28 += "Poch�ania Ogie� " + m_SAAbsorptionAttributes.EaterFire + "%";
                    if (this.m_SAAbsorptionAttributes.EaterCold != 0)
                        liststring29 += "Poch�ania Zimno " + m_SAAbsorptionAttributes.EaterCold + "%";
                    if (this.m_SAAbsorptionAttributes.EaterPoison != 0)
                        liststring30 += "Poch�ania Trucizne " + m_SAAbsorptionAttributes.EaterPoison + "%";
                    if (this.m_SAAbsorptionAttributes.EaterEnergy != 0)
                        liststring31 += "Poch�ania Energie " + m_SAAbsorptionAttributes.EaterEnergy + "%";
                    if (this.m_SAAbsorptionAttributes.EaterKinetic != 0)
                        liststring32 += "Poch�ania Kinetyczne " + m_SAAbsorptionAttributes.EaterKinetic + "%";
                    if (this.m_SAAbsorptionAttributes.EaterDamage != 0)
                        liststring33 += "Poch�ania Obra�enia " + m_SAAbsorptionAttributes.EaterDamage + "%";

                    if (this.m_SAAbsorptionAttributes.ResonanceFire != 0)
                        liststring34 += "<CENTER>Ochrona - Ogie� " + m_SAAbsorptionAttributes.ResonanceFire + "%";
                    if (this.m_SAAbsorptionAttributes.ResonanceCold != 0)
                        liststring35 += "<CENTER>Ochrona - Zimno " + m_SAAbsorptionAttributes.ResonanceCold + "%";
                    if (this.m_SAAbsorptionAttributes.ResonancePoison != 0)
                        liststring36 += "<CENTER> Ochrona - Trucizna " + m_SAAbsorptionAttributes.ResonancePoison + "%";
                    if (this.m_SAAbsorptionAttributes.ResonanceEnergy != 0)
                        liststring37 += "<CENTER>Ochrona - Energia " + m_SAAbsorptionAttributes.ResonanceEnergy + "%";
                    if (this.m_SAAbsorptionAttributes.ResonanceKinetic != 0)
                        liststring38 += "<CENTER>Ochrona - Kinetyczne " + m_SAAbsorptionAttributes.ResonanceKinetic + "%";


                    //if ((prop = this.ArtifactRarity) > 0)
                    //    list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

                    //if ((prop = this.m_AosAttributes.WeaponDamage) != 0)
                    //    list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

                    //if ((prop = this.m_AosAttributes.DefendChance) != 0)
                    //    list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

                    //if ((prop = this.m_AosAttributes.BonusDex) != 0)
                    //    list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

                    //if ((prop = this.m_AosAttributes.EnhancePotions) != 0)
                    //    list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

                    //if ((prop = this.m_AosAttributes.CastRecovery) != 0)
                    //    list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

                    //if ((prop = this.m_AosAttributes.CastSpeed) != 0)
                    //    list.Add(1060413, prop.ToString()); // faster casting ~1_val~

                    //if ((prop = this.m_AosAttributes.AttackChance) != 0)
                    //    list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

                    //if ((prop = this.m_AosAttributes.BonusHits) != 0)
                    //    list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

                    //if ((prop = this.m_AosAttributes.BonusInt) != 0)
                    //    list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

                    //if ((prop = this.m_AosAttributes.LowerManaCost) != 0)
                    //    list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

                    //if ((prop = this.m_AosAttributes.LowerRegCost) != 0)
                    //    list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

                    //if ((prop = this.GetLowerStatReq()) != 0)
                    //    list.Add(1060435, prop.ToString()); // lower requirements ~1_val~%

                    //if ((prop = (this.GetLuckBonus() + this.m_AosAttributes.Luck)) != 0)
                    //    list.Add(1060436, prop.ToString()); // luck ~1_val~

                    //if ((prop = this.m_AosArmorAttributes.MageArmor) != 0)
                    //    list.Add(1060437); // mage armor

                    //if ((prop = this.m_AosAttributes.BonusMana) != 0)
                    //    list.Add(1060439, prop.ToString()); // mana increase ~1_val~

                    //if ((prop = this.m_AosAttributes.RegenMana) != 0)
                    //    list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

                    //if ((prop = this.m_AosAttributes.NightSight) != 0)
                    //    list.Add(1060441); // night sight

                    //if ((prop = this.m_AosAttributes.ReflectPhysical) != 0)
                    //    list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

                    //if ((prop = this.m_AosAttributes.RegenStam) != 0)
                    //    list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

                    //if ((prop = this.m_AosAttributes.RegenHits) != 0)
                    //    list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

                    //if ((prop = this.m_AosArmorAttributes.SelfRepair) != 0)
                    //    list.Add(1060450, prop.ToString()); // self repair ~1_val~

                    //if ((prop = this.m_AosAttributes.SpellChanneling) != 0)
                    //    list.Add(1060482); // spell channeling

                    //if ((prop = this.m_AosAttributes.SpellDamage) != 0)
                    //    list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

                    //if ((prop = this.m_AosAttributes.BonusStam) != 0)
                    //    list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

                    //if ((prop = this.m_AosAttributes.BonusStr) != 0)
                    //    list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

                    //if ((prop = this.m_AosAttributes.WeaponSpeed) != 0)
                    //    list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

                    //if (Core.ML && (prop = this.m_AosAttributes.IncreasedKarmaLoss) != 0)
                    //    list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.EaterFire) != 0)
                    //    list.Add(1113593, prop.ToString()); // Fire Eater ~1_Val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.EaterCold) != 0)
                    //    list.Add(1113594, prop.ToString()); // Cold Eater ~1_Val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.EaterPoison) != 0)
                    //    list.Add(1113595, prop.ToString()); // Poison Eater ~1_Val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.EaterEnergy) != 0)
                    //    list.Add(1113596, prop.ToString()); // Energy Eater ~1_Val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.EaterKinetic) != 0)
                    //    list.Add(1113597, prop.ToString()); // Kinetic Eater ~1_Val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.EaterDamage) != 0)
                    //    list.Add(1113598, prop.ToString()); // Damage Eater ~1_Val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.ResonanceFire) != 0)
                    //    list.Add(1113691, prop.ToString()); // Fire Resonance ~1_val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.ResonanceCold) != 0)
                    //    list.Add(1113692, prop.ToString()); // Cold Resonance ~1_val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.ResonancePoison) != 0)
                    //    list.Add(1113693, prop.ToString()); // Poison Resonance ~1_val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.ResonanceEnergy) != 0)
                    //    list.Add(1113694, prop.ToString()); // Energy Resonance ~1_val~%

                    //if ((prop = this.m_SAAbsorptionAttributes.ResonanceKinetic) != 0)
                    //    list.Add(1113695, prop.ToString()); // Kinetic Resonance ~1_val~%

                    //if ((prop = this.GetDurabilityBonus()) > 0)
                    //list.Add(1060410, prop.ToString()); // durability ~1_val~%

                    //if ((prop = this.ComputeStatReq(StatType.Str)) > 0)
                    //list.Add(1061170, prop.ToString()); // strength requirement ~1_val~

                    //if (this.m_HitPoints >= 0 && this.m_MaxHitPoints > 0)
                    //list.Add(1060639, "{0}\t{1}", this.m_HitPoints, this.m_MaxHitPoints); // durability ~1_val~ / ~2_val~



                    list.Add("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}{16}{17}{18}{19}{20}{21}{22}{23}{24}{25}{26}{27}{28}{29}{30}{31}{32}{33}{34}{35}{36}{37}{38}", liststring, liststring1, liststring2, liststring3, liststring4, liststring5, liststring6, liststring7, liststring8, liststring9, liststring10,
                    liststring11, liststring12, liststring13, liststring14, liststring15, liststring16, liststring17, liststring18, liststring19, liststring20, liststring21, liststring22, liststring23, liststring24, liststring25, liststring26, liststring27, liststring28, liststring29, liststring30, liststring31, liststring32, liststring33, liststring34, liststring35,
                    liststring36, liststring37, liststring38);


                    Server.Engines.XmlSpawner2.XmlAttach.AddAttachmentProperties(this, list);

                    if (this.IsSetItem && !this.m_SetEquipped)
                    {
                        list.Add(1072378); // <br>Only when full set is present:				
                        this.GetSetProperties(list);
                    }

                } // Ukrycie False

                if (m_Ukrycie == true && (ArtifactRarity) > 0 || m_Ukrycie == true && (m_AosAttributes.WeaponDamage) != 0 || m_Ukrycie == true && (m_AosAttributes.DefendChance) != 0 ||
                m_Ukrycie == true && (m_AosAttributes.BonusDex) != 0 || m_Ukrycie == true && (m_AosAttributes.EnhancePotions) != 0 || m_Ukrycie == true && (m_AosAttributes.CastRecovery) != 0 ||
                m_Ukrycie == true && (m_AosAttributes.CastSpeed) != 0 || m_Ukrycie == true && (m_AosAttributes.AttackChance) != 0 || m_Ukrycie == true && (m_AosAttributes.BonusHits) != 0 ||
                m_Ukrycie == true && (m_AosAttributes.BonusInt) != 0 || m_Ukrycie == true && (m_AosAttributes.LowerManaCost) != 0 || m_Ukrycie == true && (m_AosAttributes.LowerRegCost) != 0 ||
                m_Ukrycie == true && (GetLowerStatReq()) != 0 || m_Ukrycie == true && ((GetLuckBonus() + m_AosAttributes.Luck)) != 0 || m_Ukrycie == true && (m_AosArmorAttributes.MageArmor) != 0 ||
                m_Ukrycie == true && (m_AosAttributes.BonusMana) != 0 || m_Ukrycie == true && (m_AosAttributes.RegenMana) != 0 || m_Ukrycie == true && (m_AosAttributes.NightSight) != 0 ||
                m_Ukrycie == true && (m_AosAttributes.ReflectPhysical) != 0 || m_Ukrycie == true && (m_AosAttributes.RegenStam) != 0 || m_Ukrycie == true && (m_AosAttributes.RegenHits) != 0 ||
                m_Ukrycie == true && (m_AosArmorAttributes.SelfRepair) != 0 || m_Ukrycie == true && (m_AosAttributes.SpellChanneling) != 0 || m_Ukrycie == true && (m_AosAttributes.SpellDamage) != 0 ||
                m_Ukrycie == true && (m_AosAttributes.BonusStam) != 0 || m_Ukrycie == true && (m_AosAttributes.BonusStr) != 0 || m_Ukrycie == true && (m_AosAttributes.WeaponSpeed) != 0)
                {
                    list.Add("<BASEFONT COLOR=YELLOW><B>[W�a�ciwo�ci Ukryte]</B><BASEFONT COLOR=WHITE>");
                }

            } //Identified True

            else if ((ArtifactRarity) > 0 || (m_AosAttributes.WeaponDamage) != 0 || (m_AosAttributes.DefendChance) != 0 ||
                    (m_AosAttributes.BonusDex) != 0 || (m_AosAttributes.EnhancePotions) != 0 || (m_AosAttributes.CastRecovery) != 0 ||
                    (m_AosAttributes.CastSpeed) != 0 || (m_AosAttributes.AttackChance) != 0 || (m_AosAttributes.BonusHits) != 0 ||
                    (m_AosAttributes.BonusInt) != 0 || (m_AosAttributes.LowerManaCost) != 0 || (m_AosAttributes.LowerRegCost) != 0 ||
                    (GetLowerStatReq()) != 0 || ((GetLuckBonus() + m_AosAttributes.Luck)) != 0 || (m_AosArmorAttributes.MageArmor) != 0 ||
                    (m_AosAttributes.BonusMana) != 0 || (m_AosAttributes.RegenMana) != 0 || (m_AosAttributes.NightSight) != 0 ||
                    (m_AosAttributes.ReflectPhysical) != 0 || (m_AosAttributes.RegenStam) != 0 || (m_AosAttributes.RegenHits) != 0 ||
                    (m_AosArmorAttributes.SelfRepair) != 0 || (m_AosAttributes.SpellChanneling) != 0 || (m_AosAttributes.SpellDamage) != 0 ||
                    (m_AosAttributes.BonusStam) != 0 || (m_AosAttributes.BonusStr) != 0 || (m_AosAttributes.WeaponSpeed) != 0 || this.m_Cechy == ArmorCechy.Ochronn)
                //list.Add(1038000); // Unidentified
                //list.Add("<BASEFONT COLOR=YELLOW><B>[Niezidentyfikowany]</B><BASEFONT COLOR=WHITE>"); // Unidentified

                if (m_Sprawdzony == true)
                {
                    if ((prop = this.GetDurabilityBonus()) > 0)
                        list.Add(1060410, prop.ToString()); // kondycja + ~1_val~%

                    if ((prop = this.ComputeStatReq(StatType.Str)) > 0)
                        list.Add(1061170, prop.ToString()); // wymaga str ~1_val~

                    if (this.m_HitPoints >= 0 && this.m_MaxHitPoints > 0)
                        list.Add(1060639, "{0}\t{1}", this.m_HitPoints, this.m_MaxHitPoints); // kondycja ~1_val~ / ~2_val~
                }

        }

        public override void OnSingleClick(Mobile from)
        {
            List<EquipInfoAttribute> attrs = new List<EquipInfoAttribute>();

            if (this.DisplayLootType)
            {
                if (this.LootType == LootType.Blessed)
                    attrs.Add(new EquipInfoAttribute(1038021)); // blessed
                else if (this.LootType == LootType.Cursed)
                    attrs.Add(new EquipInfoAttribute(1049643)); // cursed
            }

            #region Factions
            if (this.m_FactionState != null)
                attrs.Add(new EquipInfoAttribute(1041350)); // faction item
            #endregion

            if (this.m_Quality == ArmorQuality.Doskona�)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Quality == ArmorQuality.Wspania�)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Quality == ArmorQuality.Wyj�tkow)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Quality == ArmorQuality.Niezwyk�)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Quality == ArmorQuality.Cudown)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Quality == ArmorQuality.Mistyczn)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Quality == ArmorQuality.Legendarn)
                attrs.Add(new EquipInfoAttribute(1018305 - (int)this.m_Quality));

            if (this.m_Identified || from.AccessLevel >= AccessLevel.GameMaster)
            {
                if (this.m_Durability != ArmorDurabilityLevel.Regular)
                    attrs.Add(new EquipInfoAttribute(1038000 + (int)this.m_Durability));

                if (this.m_Protection > ArmorProtectionLevel.Regular && this.m_Protection <= ArmorProtectionLevel.Invulnerability)
                    attrs.Add(new EquipInfoAttribute(1038005 + (int)this.m_Protection));
            }
            else if (this.m_Durability != ArmorDurabilityLevel.Regular || (this.m_Protection > ArmorProtectionLevel.Regular && this.m_Protection <= ArmorProtectionLevel.Invulnerability))
                attrs.Add(new EquipInfoAttribute(1038000)); // Unidentified

            int number;

            if (this.Name == null)
            {
                number = this.LabelNumber;
            }
            else
            {
                this.LabelTo(from, this.Name);
                number = 1041000;
            }

            //            if (attrs.Count == 0 && this.Crafter == null && this.Name != null)
            if (attrs.Count == 0 && this.Crafter == null)
                return;

            EquipmentInfo eqInfo = new EquipmentInfo(number, this.m_Crafter, false, attrs.ToArray());

            from.Send(new DisplayEquipmentInfo(this, eqInfo));
        }

        #region ICraftable Members

        public virtual int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            this.Quality = (ArmorQuality)quality;
            this.Identified = true;

            if (makersMark)
                this.Crafter = from;

            #region Mondain's Legacy
            if (!craftItem.ForceNonExceptional)
            {
                Type type = typeRes;

                if (type == null)
                    type = craftItem.Resources.GetAt(0).ItemType;

                this.Resource = CraftResources.GetFromType(type);
            }
            #endregion

            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            this.Resource = CraftResources.GetFromType(resourceType);
            this.PlayerConstructed = true;

            CraftContext context = craftSystem.GetContext(from);

            if (context != null && context.DoNotColor)
                this.Hue = 0;

            if (this.Quality == ArmorQuality.Doskona� || this.Quality == ArmorQuality.Wspania� || this.Quality == ArmorQuality.Wspania� || this.Quality == ArmorQuality.Wyj�tkow || this.Quality == ArmorQuality.Niezwyk� || this.Quality == ArmorQuality.Cudown || this.Quality == ArmorQuality.Mistyczn || this.Quality == ArmorQuality.Legendarn)
            {
                if (!(Core.ML && this is BaseShield))		// Guessed Core.ML removed exceptional resist bonuses from crafted shields
                    this.DistributeBonuses((tool is BaseRunicTool ? 6 : Core.SE ? 15 : 14)); // Not sure since when, but right now 15 points are added, not 14.

                if (Core.ML && !(this is BaseShield))
                {
                    int bonus = (int)(from.Skills.WiedzaOUzbrojeniu.Value / 20);

                    for (int i = 0; i < bonus; i++)
                    {
                        switch (Utility.Random(5))
                        {
                            case 0:
                                this.m_PhysicalBonus++;
                                break;
                            case 1:
                                this.m_FireBonus++;
                                break;
                            case 2:
                                this.m_ColdBonus++;
                                break;
                            case 3:
                                this.m_EnergyBonus++;
                                break;
                            case 4:
                                this.m_PoisonBonus++;
                                break;
                        }
                    }

                    from.CheckSkill(SkillName.WiedzaOUzbrojeniu, 0, 100);
                }

                if (Core.SE && (this is HeavyPlateJingasa || this is LightPlateJingasa || this is PlateDo || this is PlateHaidate || this is PlateHiroSode || this is StuddedHiroSode || this is PlateMempo || this is PlateSuneate || this is StuddedSuneate || this is SmallPlateJingasa))
                    this.m_AosArmorAttributes.MageArmor = 1;
            }

            #region Mondain's Legacy
            if (craftItem != null && !craftItem.ForceNonExceptional)
            {
                if (Core.ML)
                {
                    CraftResourceInfo resInfo = CraftResources.GetInfo(this.m_Resource);

                    if (resInfo == null)
                        return quality;

                    CraftAttributeInfo attrInfo = resInfo.AttributeInfo;

                    if (attrInfo == null)
                        return quality;

                    if (this.m_Resource != CraftResource.Heartwood)
                    {
                        this.m_AosAttributes.WeaponDamage += attrInfo.ArmorDamage;
                        this.m_AosAttributes.AttackChance += attrInfo.ArmorHitChance;
                        this.m_AosAttributes.RegenHits += attrInfo.ArmorRegenHits;
                        this.m_AosArmorAttributes.MageArmor += attrInfo.ArmorMage;
                    }
                    else
                    {
                        switch (Utility.Random(5))
                        {
                            case 0:
                                this.m_AosAttributes.WeaponDamage += attrInfo.ArmorDamage;
                                break;
                            case 1:
                                this.m_AosAttributes.AttackChance += attrInfo.ArmorHitChance;
                                break;
                            case 2:
                                this.m_AosArmorAttributes.MageArmor += attrInfo.ArmorMage;
                                break;
                            case 3:
                                this.m_AosAttributes.Luck += attrInfo.ArmorLuck;
                                break;
                            case 4:
                                this.m_AosArmorAttributes.LowerStatReq += attrInfo.ArmorLowerRequirements;
                                break;
                        }
                    }
                }
            }
            #endregion

            if (Core.AOS && tool is BaseRunicTool)
                ((BaseRunicTool)tool).ApplyAttributesTo(this);

            return quality;
        }

        #endregion

        #region Mondain's Legacy Sets
        public override bool OnDragLift(Mobile from)
        {
            if (this.Parent is Mobile && from == this.Parent)
            {
                if (this.IsSetItem && this.m_SetEquipped)
                    SetHelper.RemoveSetBonus(from, this.SetID, this);
            }

            return base.OnDragLift(from);
        }

        public virtual SetItem SetID
        {
            get
            {
                return SetItem.None;
            }
        }
        public virtual bool MixedSet
        {
            get
            {
                return false;
            }
        }
        public virtual int Pieces
        {
            get
            {
                return 0;
            }
        }

        public bool IsSetItem
        {
            get
            {
                return (this.SetID != SetItem.None);
            }
        }

        private int m_SetHue;
        private bool m_SetEquipped;
        private bool m_LastEquipped;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetHue
        {
            get
            {
                return this.m_SetHue;
            }
            set
            {
                this.m_SetHue = value;
                this.InvalidateProperties();
            }
        }

        public bool SetEquipped
        {
            get
            {
                return this.m_SetEquipped;
            }
            set
            {
                this.m_SetEquipped = value;
            }
        }

        public bool LastEquipped
        {
            get
            {
                return this.m_LastEquipped;
            }
            set
            {
                this.m_LastEquipped = value;
            }
        }

        private AosAttributes m_SetAttributes;
        private AosSkillBonuses m_SetSkillBonuses;
        private int m_SetSelfRepair;

        [CommandProperty(AccessLevel.GameMaster)]
        public AosAttributes SetAttributes
        {
            get
            {
                return this.m_SetAttributes;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public AosSkillBonuses SetSkillBonuses
        {
            get
            {
                return this.m_SetSkillBonuses;
            }
            set
            {
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetSelfRepair
        {
            get
            {
                return this.m_SetSelfRepair;
            }
            set
            {
                this.m_SetSelfRepair = value;
                this.InvalidateProperties();
            }
        }

        private int m_SetPhysicalBonus, m_SetFireBonus, m_SetColdBonus, m_SetPoisonBonus, m_SetEnergyBonus;

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetPhysicalBonus
        {
            get
            {
                return this.m_SetPhysicalBonus;
            }
            set
            {
                this.m_SetPhysicalBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetFireBonus
        {
            get
            {
                return this.m_SetFireBonus;
            }
            set
            {
                this.m_SetFireBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetColdBonus
        {
            get
            {
                return this.m_SetColdBonus;
            }
            set
            {
                this.m_SetColdBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetPoisonBonus
        {
            get
            {
                return this.m_SetPoisonBonus;
            }
            set
            {
                this.m_SetPoisonBonus = value;
                this.InvalidateProperties();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int SetEnergyBonus
        {
            get
            {
                return this.m_SetEnergyBonus;
            }
            set
            {
                this.m_SetEnergyBonus = value;
                this.InvalidateProperties();
            }
        }

        public virtual void GetSetProperties(ObjectPropertyList list)
        {
            if (!this.m_SetEquipped)
            {
                if (this.m_SetPhysicalBonus != 0)
                    list.Add(1072382, this.m_SetPhysicalBonus.ToString()); // physical resist +~1_val~%

                if (this.m_SetFireBonus != 0)
                    list.Add(1072383, this.m_SetFireBonus.ToString()); // fire resist +~1_val~%

                if (this.m_SetColdBonus != 0)
                    list.Add(1072384, this.m_SetColdBonus.ToString()); // cold resist +~1_val~%

                if (this.m_SetPoisonBonus != 0)
                    list.Add(1072385, this.m_SetPoisonBonus.ToString()); // poison resist +~1_val~%

                if (this.m_SetEnergyBonus != 0)
                    list.Add(1072386, this.m_SetEnergyBonus.ToString()); // energy resist +~1_val~%		
            }

            SetHelper.GetSetProperties(list, this);

            int prop;

            if ((prop = this.m_SetSelfRepair) != 0 && this.m_AosArmorAttributes.SelfRepair == 0)
                list.Add(1060450, prop.ToString()); // self repair ~1_val~
        }
        #endregion
    }
}