using System;
using System.Collections.Generic;
using Server.ContextMenus;
using Server.Engines.Craft;

namespace Server.Items
{
    public enum GemType
    {
        None,
        GwiezdnySzafir,
        Szmaragd,
        Szafir,
        Rubin,
        Cytryn,
        Ametyst,
        Tourmalin,
        Bursztyn,
        Diament
    }

    public abstract class BaseJewel : Item, ICraftable, ISetItem
    {
        private int m_MaxHitPoints;
        private int m_HitPoints;

        private AosAttributes m_AosAttributes;
        private AosElementAttributes m_AosResistances;
        private AosSkillBonuses m_AosSkillBonuses;
        private SAAbsorptionAttributes m_SAAbsorptionAttributes;
        private CraftResource m_Resource;
        private GemType m_GemType;
        private int m_TimesImbued;

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
            private readonly BaseJewel m_Item;

            public UnBlessEntry(Mobile from, BaseJewel item)
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

        [CommandProperty(AccessLevel.Player)]
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
        public AosElementAttributes Resistances
        {
            get
            {
                return this.m_AosResistances;
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

                    this.UnscaleDurability();
                    this.InvalidateProperties();
                    this.ScaleDurability();

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
                this.InvalidateProperties();
                this.ScaleDurability();
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public GemType GemType
        {
            get
            {
                return this.m_GemType;
            }
            set
            {
                this.m_GemType = value;
                this.InvalidateProperties();
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

        public override int PhysicalResistance
        {
            get
            {
                return this.m_AosResistances.Physical;
            }
        }
        public override int FireResistance
        {
            get
            {
                return this.m_AosResistances.Fire;
            }
        }
        public override int ColdResistance
        {
            get
            {
                return this.m_AosResistances.Cold;
            }
        }
        public override int PoisonResistance
        {
            get
            {
                return this.m_AosResistances.Poison;
            }
        }
        public override int EnergyResistance
        {
            get
            {
                return this.m_AosResistances.Energy;
            }
        }
        public virtual int BaseGemTypeNumber
        {
            get
            {
                return 0;
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

        public override int LabelNumber
        {
            get
            {
                if (this.m_GemType == GemType.None)
                    return base.LabelNumber;

                return this.BaseGemTypeNumber + (int)this.m_GemType - 1;
            }
        }
////
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

            if (this.m_Quality == ArmorQuality.None)
                bonus += 0;
            if (this.m_Quality == ArmorQuality.S³ab)
                bonus += 10;
            if (this.m_Quality == ArmorQuality.Przeciêtn)
                bonus += 10;
            if (this.m_Quality == ArmorQuality.Zwyk³)
                bonus += 20;
            if (this.m_Quality == ArmorQuality.Dobr)
                bonus += 20;
            if (this.m_Quality == ArmorQuality.Doskona³)
                bonus += 30;
            if (this.m_Quality == ArmorQuality.Wspania³)
                bonus += 30;
            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
                bonus += 40;
            if (this.m_Quality == ArmorQuality.Niezwyk³)
                bonus += 40;
            if (this.m_Quality == ArmorQuality.Cudown)
                bonus += 50;
            if (this.m_Quality == ArmorQuality.Mistyczn)
                bonus += 50;
            if (this.m_Quality == ArmorQuality.Legendarn)
                bonus += 60;

			switch ( m_Resource )
			{
				case CraftResource.Iron: bonus += 0; break;
				case CraftResource.DullCopper: bonus += 20; break;
				case CraftResource.ShadowIron: bonus += 40; break;
				case CraftResource.Copper: bonus += 60; break;
				case CraftResource.Bronze: bonus += 80; break;
				case CraftResource.Gold: bonus += 100; break;
				case CraftResource.Agapite: bonus += 120; break;				
				case CraftResource.Verite: bonus += 140; break;				
				case CraftResource.Valorite: bonus += 150; break;					
			}

                bonus += this.m_HitPoints = this.m_MaxHitPoints = 10;

            return bonus;
        }
////
        public override void OnAfterDuped(Item newItem)
        {
            BaseJewel jewel = newItem as BaseJewel;

            if (jewel == null)
                return;

            jewel.m_AosAttributes = new AosAttributes(newItem, this.m_AosAttributes);
            jewel.m_AosResistances = new AosElementAttributes(newItem, this.m_AosResistances);
            jewel.m_AosSkillBonuses = new AosSkillBonuses(newItem, this.m_AosSkillBonuses);

            #region Mondain's Legacy
            jewel.m_SetAttributes = new AosAttributes(newItem, this.m_SetAttributes);
            jewel.m_SetSkillBonuses = new AosSkillBonuses(newItem, this.m_SetSkillBonuses);
            #endregion

            #region SA
            jewel.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(newItem, this.m_SAAbsorptionAttributes);
            #endregion
        }

        public virtual int ArtifactRarity
        {
            get
            {
                return 0;
            }
        }

        private Mobile m_Crafter;
        private ArmorQuality m_Quality;
//DODANE
        private ArmorCechy m_Cechy;
        private bool m_Identified, m_Ukrycie;

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Crafter
        {
            get
            {
                return this.m_Crafter;
            }
            set
            {
                this.m_Crafter = value;
                m_Identified = true;
                //m_Ukrycie = false;
                this.InvalidateProperties();
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
                this.InvalidateProperties();
            }
        }
///

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

        public BaseJewel(int itemID, Layer layer)
            : base(itemID)
        {
            this.m_AosAttributes = new AosAttributes(this);
            this.m_AosResistances = new AosElementAttributes(this);
            this.m_AosSkillBonuses = new AosSkillBonuses(this);
            this.m_Resource = CraftResource.Iron;
            this.m_GemType = GemType.None;

            this.Layer = layer;

            this.m_HitPoints = this.m_MaxHitPoints = Utility.RandomMinMax(this.InitMinHits, this.InitMaxHits);

            this.m_SetAttributes = new AosAttributes(this);
            this.m_SetSkillBonuses = new AosSkillBonuses(this);
            this.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this);
        }

        #region Stygian Abyss
        public override bool CanEquip(Mobile from)
        {
//
if ( this.m_HitPoints == 4 )
{
from.SendMessage( 33, "Twoja bi¿uteria jest zniszczona!" );
}
if ( this.m_HitPoints == 3 )
{
from.SendMessage( 33, "Twoja bi¿uteria jest bardzo zniszczona!" );
}
if ( this.m_HitPoints == 2 )
{
from.SendMessage( 33, "Twoja bi¿uteria zaraz siê rozpadnie!" );
}
//
            if (this.BlessedBy != null && this.BlessedBy != from)
            {
                from.SendLocalizedMessage(1075277); // That item is blessed by another player.
                return false;
            }

            if (from.AccessLevel < AccessLevel.GameMaster)
            {
//
                if (m_Identified == false)
                {
                    from.SendMessage("Ten przedmiot jest niezidentyfikowany!");
                    return false;
                }
//
                if (from.Race == Race.Gargoyle && !this.CanBeWornByGargoyles)
                {
                    from.SendLocalizedMessage(1111708); // Gargoyles can't wear this.
                    return false;
                }
                else if (this.RequiredRace != null && from.Race != this.RequiredRace)
                {
                    if (this.RequiredRace == Race.Elf)
                        from.SendLocalizedMessage(1072203); // Only Elves may use this.
                    else if (this.RequiredRace == Race.Gargoyle)
                        from.SendLocalizedMessage(1111707); // Only gargoyles can wear this.
                    else
                        from.SendMessage("Only {0} may use this.", this.RequiredRace.PluralName);

                    return false;
                }
            }
		
            return base.CanEquip(from);
        }

        #endregion

        public override void OnAdded(object parent)
        {
            if (Core.AOS && parent is Mobile)
            {
                Mobile from = (Mobile)parent;

                this.m_AosSkillBonuses.AddTo(from);

                int strBonus = this.m_AosAttributes.BonusStr;
                int dexBonus = this.m_AosAttributes.BonusDex;
                int intBonus = this.m_AosAttributes.BonusInt;

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

                from.CheckStatTimers();

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
            }

            if (parent is Mobile)
            {
                if (Server.Engines.XmlSpawner2.XmlAttach.CheckCanEquip(this, (Mobile)parent))
                    Server.Engines.XmlSpawner2.XmlAttach.CheckOnEquip(this, (Mobile)parent);
                else
                    ((Mobile)parent).AddToBackpack(this);
            }
        }

        public override void OnRemoved(object parent)
        {
            if (Core.AOS && parent is Mobile)
            {
                Mobile from = (Mobile)parent;

if ( this.m_HitPoints > 0 )
{
this.m_HitPoints -= 1;
}
if ( this.m_HitPoints <= 0 )
{
from.SendMessage( 33, "Twoja bi¿uteria rozpad³a siê!" );
this.Delete();
}

                this.m_AosSkillBonuses.Remove();

                string modName = this.Serial.ToString();

                from.RemoveStatMod(modName + "Str");
                from.RemoveStatMod(modName + "Dex");
                from.RemoveStatMod(modName + "Int");

                from.CheckStatTimers();

                #region Mondain's Legacy Sets
                if (this.IsSetItem && this.m_SetEquipped)
                    SetHelper.RemoveSetBonus(from, this.SetID, this);
                #endregion
            }

            Server.Engines.XmlSpawner2.XmlAttach.CheckOnRemoved(this, parent);
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

	public override void AddNameProperty( ObjectPropertyList list )
	{
		string oreType;

		if ( Hue == 0 )
		{
			oreType = "";
		}
		else
		{
			switch ( m_Resource )
			{
				case CraftResource.DullCopper:			oreType = " z Matowej Miedzi"; break; // dull copper
				case CraftResource.ShadowIron:			oreType = " z Cienistego ¯elaza"; break; // shadow iron
				case CraftResource.Copper:			oreType = " z Miedzi"; break; // copper
				case CraftResource.Bronze:			oreType = " z Br¹zu"; break; // bronze					
				case CraftResource.Gold:			oreType = " ze Z³ota"; break; // golden
				case CraftResource.Agapite:			oreType = " z Agapitu"; break; // agapite					
				case CraftResource.Verite:			oreType = " z Verytu"; break; // verite
				case CraftResource.Valorite:			oreType = " z Valorytu"; break; // valorite
					case CraftResource.SpinedLeather:		oreType = " z Spined skór"; break; // Spined
					case CraftResource.HornedLeather:		oreType = " z Horned skór"; break; // Horned
					case CraftResource.BarbedLeather:		oreType = " z Barbed skór"; break; // Barbed

				default: oreType = ""; break;
			}
		}

                      if (this.Name == null)
                      {
                         list.Add(this.LabelNumber);
                      }

if(m_Identified == false)
{
                      if (this.Name != null)
                      {
                         list.Add(this.Name);
                      }
}

if (m_Identified == true)
{

////Rodzaj Mêski
if ( this is GoldRing || this is SilverRing || this is Necklace || this is GoldNecklace || this is GoldBeadNecklace || this is SilverNecklace || this is SilverBeadNecklace 
    /*|| this is GargishRing || this is GargishNecklace*/ )
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
                      if ( this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add(this.Name);
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add( "{0}y "+this.Name, this.Quality );
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else
{
                      list.Add(  "{1}y {0}y "+this.Name, this.Quality, this.m_Cechy );
}
                      }
                      if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else
{
                      list.Add(  "{0}y "+this.Name, this.m_Cechy );
}
                      }
            }

            if (m_Resource == CraftResource.RegularLeather)
            {
                      if (this.Name == null)
                      {
                         list.Add(this.LabelNumber);
                      }
                      if ( this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add(this.Name);
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add( "{0}y "+this.Name, this.Quality );
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{1}y</B><BASEFONT COLOR=YELLOW> {0}y "+this.Name, this.Quality, this.m_Cechy );
}
else
{
                      list.Add(  "{1}y {0}y "+this.Name, this.Quality, this.m_Cechy );
}
                      }
                      if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{0}y</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else
{
                      list.Add(  "{0}y "+this.Name, this.m_Cechy );
}
                      }
            }

if ( m_Cechy == ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather )
{
            if (this.m_Quality == ArmorQuality.None)
            {
                    list.Add("{1}{0}", oreType, this.GetNameString() );
            }

            if (this.m_Quality == ArmorQuality.S³ab)
            {
                    list.Add(1041522, "S³aby \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Przeciêtn)
            {
                   list.Add(1041522,"Przeciêtny \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Zwyk³)
            {
                   list.Add(1041522, "Zwyk³y \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Dobr)
            {
                   list.Add(1041522, "Dobry \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Doskona³)
            {
                   list.Add(1041522, "Doskona³y \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Wspania³)
            {
                   list.Add(1041522, "Wspania³y \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
            {
                   list.Add(1041522, "Wyj¹tkowy \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Niezwyk³)
            {
                   list.Add(1041522, "Niezwyk³y \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Cudown)
            {
                   list.Add(1041522, "Cudowny \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Mistyczn)
            {
                   list.Add(1041522, "Mistyczny \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Legendarn)
            {
                   list.Add(1041522, "Legendarny \t{0}\t{1}", this.GetNameString(), oreType );
            }
}

if ( m_Cechy != ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather )
{
            if (this.m_Quality == ArmorQuality.None)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, " <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, " <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, " <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, " {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.S³ab)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "S³aby <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "S³aby {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Przeciêtn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Przeciêtny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522,"Przeciêtny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Zwyk³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Zwyk³y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Zwyk³y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Dobr)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Dobry <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Dobry {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Doskona³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Doskona³y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                   list.Add(1041522, "Doskona³y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Wspania³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Wspania³y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Wspania³y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Wyj¹tkowy <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Wyj¹tkowy {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Niezwyk³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Niezwyk³y <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Niezwyk³y {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Cudown)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Cudowny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Cudowny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Mistyczn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Mistyczny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Mistyczny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Legendarn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#CC0033><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#0066FF><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#2F4F4F><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#6B238E><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#CC9900><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#666600><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#855E42><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#CC3300><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Legendarny <BASEFONT COLOR=#238E23><B>{2}y</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Legendarny {2}y \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }
}
}
////Rodzaj ¯eñski
if ( this is GoldBracelet || this is SilverBracelet )
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
                      if ( this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add(this.Name);
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add( "{0}a "+this.Name, this.Quality );
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else
{
                      list.Add(  "{1}a {0}a "+this.Name, this.Quality, this.m_Cechy );
}
                      }
                      if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else
{
                      list.Add(  "{0}a "+this.Name, this.m_Cechy );
}
                      }
            }

            if (m_Resource == CraftResource.RegularLeather)
            {
                      if (this.Name == null)
                      {
                         list.Add(this.LabelNumber);
                      }
                      if ( this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add(this.Name);
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add( "{0}a "+this.Name, this.Quality );
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{1}a</B><BASEFONT COLOR=YELLOW> {0}a "+this.Name, this.Quality, this.m_Cechy );
}
else
{
                      list.Add(  "{1}a {0}a "+this.Name, this.Quality, this.m_Cechy );
}
                      }
                      if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{0}a</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else
{
                      list.Add(  "{0}a "+this.Name, this.m_Cechy );
}
                      }
            }

if ( m_Cechy == ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather )
{
            if (this.m_Quality == ArmorQuality.None)
            {
                    list.Add("{1}{0}", oreType, this.GetNameString() );
            }

            if (this.m_Quality == ArmorQuality.S³ab)
            {
                    list.Add(1041522, "S³aba \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Przeciêtn)
            {
                   list.Add(1041522,"Przeciêtna \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Zwyk³)
            {
                   list.Add(1041522, "Zwyk³a \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Dobr)
            {
                   list.Add(1041522, "Dobra \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Doskona³)
            {
                   list.Add(1041522, "Doskona³a \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Wspania³)
            {
                   list.Add(1041522, "Wspania³a \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
            {
                   list.Add(1041522, "Wyj¹tkowa \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Niezwyk³)
            {
                   list.Add(1041522, "Niezwyk³a \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Cudown)
            {
                   list.Add(1041522, "Cudowna \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Mistyczn)
            {
                   list.Add(1041522, "Mistyczna \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Legendarn)
            {
                   list.Add(1041522, "Legendarna \t{0}\t{1}", this.GetNameString(), oreType );
            }
}

if ( m_Cechy != ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather )
{
            if (this.m_Quality == ArmorQuality.None)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, " <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, " <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, " <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, " {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.S³ab)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "S³aba <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "S³aba {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Przeciêtn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Przeciêtna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522,"Przeciêtna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Zwyk³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Zwyk³a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Zwyk³a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Dobr)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Dobra <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Dobra {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Doskona³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Doskona³a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                   list.Add(1041522, "Doskona³a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Wspania³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Wspania³a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Wspania³a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Wyj¹tkowa <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Wyj¹tkowa {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Niezwyk³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Niezwyk³a <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Niezwyk³a {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Cudown)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Cudowna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Cudowna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Mistyczn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Mistyczna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Mistyczna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Legendarn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#CC0033><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#0066FF><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#2F4F4F><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#6B238E><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#CC9900><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#666600><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#855E42><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#CC3300><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Legendarna <BASEFONT COLOR=#238E23><B>{2}a</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Legendarna {2}a \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }
}
}
////Rodzaj Nijaki
if ( this is PlateGloves || this is PlateArms || this is PlateLegs || this is ChainLegs || this is RingmailArms || this is RingmailGloves || this is RingmailLegs || this is LeatherArms || this is LeatherGloves || this is LeatherLegs || this is LeatherShorts 
     || this is StuddedGloves || this is StuddedLegs || this is StuddedArms || this is BoneArms || this is BoneGloves || this is BoneLegs )
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
                      if ( this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add(this.Name);
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add( "{0}e "+this.Name, this.Quality );
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else
{
                      list.Add(  "{1}e {0}e "+this.Name, this.Quality, this.m_Cechy );
}
                      }
                      if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else
{
                      list.Add(  "{0}e "+this.Name, this.m_Cechy );
}
                      }
            }

            if (m_Resource == CraftResource.RegularLeather)
            {
                      if (this.Name == null)
                      {
                         list.Add(this.LabelNumber);
                      }
                      if ( this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add(this.Name);
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy == ArmorCechy.None)
                      {
                      list.Add( "{0}e "+this.Name, this.Quality );
                      }
                      if (this.Name != null && this.m_Quality != ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{1}e</B><BASEFONT COLOR=YELLOW> {0}e "+this.Name, this.Quality, this.m_Cechy );
}
else
{
                      list.Add(  "{1}e {0}e "+this.Name, this.Quality, this.m_Cechy );
}
                      }
                      if (this.Name != null && this.m_Quality == ArmorQuality.None && this.m_Cechy != ArmorCechy.None)
                      {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                      list.Add(  "<BASEFONT COLOR=#CC0033><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                      list.Add(  "<BASEFONT COLOR=#0066FF><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                      list.Add(  "<BASEFONT COLOR=#2F4F4F><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                      list.Add(  "<BASEFONT COLOR=#6B238E><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                      list.Add(  "<BASEFONT COLOR=#CC9900><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                      list.Add(  "<BASEFONT COLOR=#666600><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                      list.Add(  "<BASEFONT COLOR=#855E42><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                      list.Add(  "<BASEFONT COLOR=#CC3300><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                      list.Add(  "<BASEFONT COLOR=#238E23><B>{0}e</B><BASEFONT COLOR=YELLOW> "+this.Name, this.m_Cechy );
}
else
{
                      list.Add(  "{0}e "+this.Name, this.m_Cechy );
}
                      }
            }

if ( m_Cechy == ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather )
{
            if (this.m_Quality == ArmorQuality.None)
            {
                    list.Add("{1}{0}", oreType, this.GetNameString() );
            }

            if (this.m_Quality == ArmorQuality.S³ab)
            {
                    list.Add(1041522, "S³abe \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Przeciêtn)
            {
                   list.Add(1041522,"Przeciêtne \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Zwyk³)
            {
                   list.Add(1041522, "Zwyk³e \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Dobr)
            {
                   list.Add(1041522, "Dobre \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Doskona³)
            {
                   list.Add(1041522, "Doskona³e \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Wspania³)
            {
                   list.Add(1041522, "Wspania³e \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
            {
                   list.Add(1041522, "Wyj¹tkowe \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Niezwyk³)
            {
                   list.Add(1041522, "Niezwyk³e \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Cudown)
            {
                   list.Add(1041522, "Cudowne \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Mistyczn)
            {
                   list.Add(1041522, "Mistyczne \t{0}\t{1}", this.GetNameString(), oreType );
            }

            if (this.m_Quality == ArmorQuality.Legendarn)
            {
                   list.Add(1041522, "Legendarne \t{0}\t{1}", this.GetNameString(), oreType );
            }
}

if ( m_Cechy != ArmorCechy.None && m_Resource != CraftResource.Iron && m_Resource != CraftResource.RegularLeather )
{
            if (this.m_Quality == ArmorQuality.None)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, " <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, " <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, " <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, " <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, " <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, " {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.S³ab)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "S³abe <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "S³abe {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Przeciêtn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Przeciêtne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522,"Przeciêtne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Zwyk³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Zwyk³e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Zwyk³e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Dobr)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Dobre <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Dobre {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Doskona³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Doskona³e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                   list.Add(1041522, "Doskona³e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Wspania³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Wspania³e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Wspania³e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Wyj¹tkow)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Wyj¹tkowe <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Wyj¹tkowe {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Niezwyk³)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Niezwyk³e <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Niezwyk³e {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Cudown)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Cudowne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Cudowne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Mistyczn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Mistyczne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Mistyczne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }

            if (this.m_Quality == ArmorQuality.Legendarn)
            {
if (this.m_Cechy == ArmorCechy.Witaln)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#CC0033><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.M¹dr)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#0066FF><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Stabiln)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#2F4F4F><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Ochronn)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#6B238E><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Szczêœliw)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#CC9900><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Oszczêdn)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#666600><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Wytrzyma³)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#855E42><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Odbijaj¹c)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#CC3300><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else if (this.m_Cechy == ArmorCechy.Obronn)
{
                    list.Add(1041522, "Legendarne <BASEFONT COLOR=#238E23><B>{2}e</B><BASEFONT COLOR=YELLOW> \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
else
{
                    list.Add(1041522, "Legendarne {2}e \t{0}\t{1}", this.GetNameString(), oreType, m_Cechy );
}
            }
}
}

            if (m_GemType == GemType.GwiezdnySzafir)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Szmaragd)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Rubin)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Cytryn)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Ametyst)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Tourmalin)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Bursztyn)
            {
                      list.Add( "[{0}]", this.GemType );
            }
            if (m_GemType == GemType.Diament)
            {
                      list.Add( "[{0}]", this.GemType );
            }

}  ////Identified
        }

        public BaseJewel(Serial serial)
            : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            #region Umagicznianie
            if (this.m_TimesImbued > 0)
                list.Add(1080418); // (Imbued)
            #endregion

            #region Mondain's Legacy
            //if (this.m_Quality == ArmorQuality.Exceptional)
            //    list.Add(1063341); // exceptional

            if (this.m_Crafter != null)
                list.Add(1050043, this.m_Crafter.Name); // crafted by ~1_NAME~
            #endregion

            #region Mondain's Legacy Sets
            if (this.IsSetItem)
            {
                list.Add(1080240, this.Pieces.ToString()); // Part of a Jewelry Set (~1_val~ pieces)

                if (this.m_SetEquipped)
                {
                    list.Add(1080241); // Full Jewelry Set Present					
                    SetHelper.GetSetProperties(list, this);
                }
            }
            #endregion


            int prop;

            #region Stygian Abyss
            if (this.RequiredRace == Race.Elf)
                list.Add(1075086); // Elves Only
            else if (this.RequiredRace == Race.Gargoyle)
                list.Add(1111709); // Gargoyles Only
            #endregion
//
            if ( m_Identified )
            {
//
            this.m_AosSkillBonuses.GetProperties(list);

            if ((prop = this.ArtifactRarity) > 0)
                list.Add(1061078, prop.ToString()); // artifact rarity ~1_val~

            //Critical chance
            if ((prop = this.m_AosAttributes.CritChance) != 0)
                list.Add("Critical Chance Increase " + prop.ToString() + "%"); // crit chance increase ~1_val~%
                //list.Add(1060409, prop.ToString());

            if ((prop = this.m_AosAttributes.WeaponDamage) != 0)
                list.Add(1060401, prop.ToString()); // damage increase ~1_val~%

            if ((prop = this.m_AosAttributes.DefendChance) != 0)
                list.Add(1060408, prop.ToString()); // defense chance increase ~1_val~%

            if ((prop = this.m_AosAttributes.BonusDex) != 0)
                list.Add(1060409, prop.ToString()); // dexterity bonus ~1_val~

            if ((prop = this.m_AosAttributes.EnhancePotions) != 0)
                list.Add(1060411, prop.ToString()); // enhance potions ~1_val~%

            if ((prop = this.m_AosAttributes.CastRecovery) != 0)
                list.Add(1060412, prop.ToString()); // faster cast recovery ~1_val~

            if ((prop = this.m_AosAttributes.CastSpeed) != 0)
                list.Add(1060413, prop.ToString()); // faster casting ~1_val~

            if ((prop = this.m_AosAttributes.AttackChance) != 0)
                list.Add(1060415, prop.ToString()); // hit chance increase ~1_val~%

            if ((prop = this.m_AosAttributes.BonusHits) != 0)
                list.Add(1060431, prop.ToString()); // hit point increase ~1_val~

            if ((prop = this.m_AosAttributes.BonusInt) != 0)
                list.Add(1060432, prop.ToString()); // intelligence bonus ~1_val~

            if ((prop = this.m_AosAttributes.LowerManaCost) != 0)
                list.Add(1060433, prop.ToString()); // lower mana cost ~1_val~%

            if ((prop = this.m_AosAttributes.LowerRegCost) != 0)
                list.Add(1060434, prop.ToString()); // lower reagent cost ~1_val~%

            if ((prop = this.m_AosAttributes.Luck) != 0)
                list.Add(1060436, prop.ToString()); // luck ~1_val~

            if ((prop = this.m_AosAttributes.BonusMana) != 0)
                list.Add(1060439, prop.ToString()); // mana increase ~1_val~

            if ((prop = this.m_AosAttributes.RegenMana) != 0)
                list.Add(1060440, prop.ToString()); // mana regeneration ~1_val~

            if ((prop = this.m_AosAttributes.NightSight) != 0)
                list.Add(1060441); // night sight

            if ((prop = this.m_AosAttributes.ReflectPhysical) != 0)
                list.Add(1060442, prop.ToString()); // reflect physical damage ~1_val~%

            if ((prop = this.m_AosAttributes.RegenStam) != 0)
                list.Add(1060443, prop.ToString()); // stamina regeneration ~1_val~

            if ((prop = this.m_AosAttributes.RegenHits) != 0)
                list.Add(1060444, prop.ToString()); // hit point regeneration ~1_val~

            if ((prop = this.m_AosAttributes.SpellChanneling) != 0)
                list.Add(1060482); // spell channeling

            if ((prop = this.m_AosAttributes.SpellDamage) != 0)
                list.Add(1060483, prop.ToString()); // spell damage increase ~1_val~%

            if ((prop = this.m_AosAttributes.BonusStam) != 0)
                list.Add(1060484, prop.ToString()); // stamina increase ~1_val~

            if ((prop = this.m_AosAttributes.BonusStr) != 0)
                list.Add(1060485, prop.ToString()); // strength bonus ~1_val~

            if ((prop = this.m_AosAttributes.WeaponSpeed) != 0)
                list.Add(1060486, prop.ToString()); // swing speed increase ~1_val~%

            if (Core.ML && (prop = this.m_AosAttributes.IncreasedKarmaLoss) != 0)
                list.Add(1075210, prop.ToString()); // Increased Karma Loss ~1val~%

            #region SA
            if ((prop = this.m_SAAbsorptionAttributes.CastingFocus) != 0)
                list.Add(1113696, prop.ToString()); // Casting Focus ~1_val~%

            if ((prop = this.m_SAAbsorptionAttributes.EaterFire) != 0)
                list.Add(1113593, prop.ToString()); // Fire Eater ~1_Val~%

            if ((prop = this.m_SAAbsorptionAttributes.EaterCold) != 0)
                list.Add(1113594, prop.ToString()); // Cold Eater ~1_Val~%

            if ((prop = this.m_SAAbsorptionAttributes.EaterPoison) != 0)
                list.Add(1113595, prop.ToString()); // Poison Eater ~1_Val~%

            if ((prop = this.m_SAAbsorptionAttributes.EaterEnergy) != 0)
                list.Add(1113596, prop.ToString()); // Energy Eater ~1_Val~%

            if ((prop = this.m_SAAbsorptionAttributes.EaterKinetic) != 0)
                list.Add(1113597, prop.ToString()); // Kinetic Eater ~1_Val~%

            if ((prop = this.m_SAAbsorptionAttributes.EaterDamage) != 0)
                list.Add(1113598, prop.ToString()); // Damage Eater ~1_Val~%

            if ((prop = this.m_SAAbsorptionAttributes.ResonanceFire) != 0)
                list.Add(1113691, prop.ToString()); // Fire Resonance ~1_val~%

            if ((prop = this.m_SAAbsorptionAttributes.ResonanceCold) != 0)
                list.Add(1113692, prop.ToString()); // Cold Resonance ~1_val~%

            if ((prop = this.m_SAAbsorptionAttributes.ResonancePoison) != 0)
                list.Add(1113693, prop.ToString()); // Poison Resonance ~1_val~%

            if ((prop = this.m_SAAbsorptionAttributes.ResonanceEnergy) != 0)
                list.Add(1113694, prop.ToString()); // Energy Resonance ~1_val~%

            if ((prop = this.m_SAAbsorptionAttributes.ResonanceKinetic) != 0)
                list.Add(1113695, prop.ToString()); // Kinetic Resonance ~1_val~%
            #endregion

            base.AddResistanceProperties(list);

            Server.Engines.XmlSpawner2.XmlAttach.AddAttachmentProperties(this, list);

} //Identified Koniec

            else if ((ArtifactRarity) > 0 || (m_AosAttributes.WeaponDamage) != 0 || (m_AosAttributes.DefendChance) != 0 ||
                    (m_AosAttributes.BonusDex) != 0 || (m_AosAttributes.EnhancePotions) != 0 || (m_AosAttributes.CastRecovery) != 0 ||
                    (m_AosAttributes.CastSpeed) != 0 || (m_AosAttributes.AttackChance) != 0 || (m_AosAttributes.BonusHits) != 0 ||
                    (m_AosAttributes.BonusInt) != 0 || (m_AosAttributes.LowerManaCost) != 0 || (m_AosAttributes.LowerRegCost) != 0 ||
                    (m_AosAttributes.BonusMana) != 0 || (m_AosAttributes.RegenMana) != 0 || (m_AosAttributes.NightSight) != 0 ||
                    (m_AosAttributes.ReflectPhysical) != 0 || (m_AosAttributes.RegenStam) != 0 || (m_AosAttributes.RegenHits) != 0 ||
                    (m_AosAttributes.SpellChanneling) != 0 || (m_AosAttributes.SpellDamage) != 0 || (m_AosAttributes.CritChance) != 0 ||
                    (m_AosResistances.Physical) != 0 || (m_AosResistances.Fire) != 0 || (m_AosResistances.Cold) != 0 || (m_AosResistances.Poison) != 0 || (m_AosResistances.Energy) != 0 ||
                    (m_AosAttributes.BonusStam) != 0 || (m_AosAttributes.BonusStr) != 0 || (m_AosAttributes.WeaponSpeed) != 0 || (this.SkillBonuses.Skill_1_Value > 0) ||
                    (this.SkillBonuses.Skill_2_Value > 0) || (this.SkillBonuses.Skill_3_Value > 0) || (this.SkillBonuses.Skill_4_Value > 0) || (this.SkillBonuses.Skill_5_Value > 0) )
                    list.Add("<BASEFONT COLOR=YELLOW><B>[Niezidentyfikowany]</B><BASEFONT COLOR=WHITE>"); // Unidentified

            if (this.m_HitPoints >= 0 && this.m_MaxHitPoints > 0)
                list.Add(1060639, "{0}\t{1}", this.m_HitPoints, this.m_MaxHitPoints); // durability ~1_val~ / ~2_val~

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(4); // version

            // Version 4
            writer.WriteEncodedInt((int)this.m_TimesImbued);
            this.m_SAAbsorptionAttributes.Serialize(writer);
            writer.Write((Mobile)this.m_BlessedBy);
            writer.Write((bool)this.m_LastEquipped);
            writer.Write((bool)this.m_SetEquipped);
            writer.WriteEncodedInt((int)this.m_SetHue);

            this.m_SetAttributes.Serialize(writer);
            this.m_SetSkillBonuses.Serialize(writer);

            writer.Write(this.m_Crafter);
            writer.Write((int)this.m_Quality);
//
            writer.Write((int)this.m_Cechy);
            writer.Write((bool)this.m_Identified);
            writer.Write((bool)this.m_Ukrycie);
//
            // Version 3
            writer.WriteEncodedInt((int)this.m_MaxHitPoints);
            writer.WriteEncodedInt((int)this.m_HitPoints);

            writer.WriteEncodedInt((int)this.m_Resource);
            writer.WriteEncodedInt((int)this.m_GemType);

            this.m_AosAttributes.Serialize(writer);
            this.m_AosResistances.Serialize(writer);
            this.m_AosSkillBonuses.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch (version)
            {
                case 4:
                    {
                        this.m_TimesImbued = reader.ReadEncodedInt();
                        this.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this, reader);

                        this.m_BlessedBy = reader.ReadMobile();
                        this.m_LastEquipped = reader.ReadBool();
                        this.m_SetEquipped = reader.ReadBool();
                        this.m_SetHue = reader.ReadEncodedInt();

                        this.m_SetAttributes = new AosAttributes(this, reader);
                        this.m_SetSkillBonuses = new AosSkillBonuses(this, reader);

                        this.m_Crafter = reader.ReadMobile();
                        this.m_Quality = (ArmorQuality)reader.ReadInt();
//
                        this.m_Cechy = (ArmorCechy)reader.ReadInt();
                        this.m_Identified = reader.ReadBool();
                        this.m_Ukrycie = reader.ReadBool();
//
                        goto case 3;
                    }
                case 3:
                    {
                        this.m_MaxHitPoints = reader.ReadEncodedInt();
                        this.m_HitPoints = reader.ReadEncodedInt();

                        goto case 2;
                    }
                case 2:
                    {
                        this.m_Resource = (CraftResource)reader.ReadEncodedInt();
                        this.m_GemType = (GemType)reader.ReadEncodedInt();

                        goto case 1;
                    }
                case 1:
                    {
                        this.m_AosAttributes = new AosAttributes(this, reader);
                        this.m_AosResistances = new AosElementAttributes(this, reader);
                        this.m_AosSkillBonuses = new AosSkillBonuses(this, reader);

                        if (Core.AOS && this.Parent is Mobile)
                            this.m_AosSkillBonuses.AddTo((Mobile)this.Parent);

                        int strBonus = this.m_AosAttributes.BonusStr;
                        int dexBonus = this.m_AosAttributes.BonusDex;
                        int intBonus = this.m_AosAttributes.BonusInt;

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

                        break;
                    }
                case 0:
                    {
                        this.m_AosAttributes = new AosAttributes(this);
                        this.m_AosResistances = new AosElementAttributes(this);
                        this.m_AosSkillBonuses = new AosSkillBonuses(this);

                        break;
                    }
            }

            #region Mondain's Legacy Sets
            if (this.m_SetAttributes == null)
                this.m_SetAttributes = new AosAttributes(this);

            if (this.m_SetSkillBonuses == null)
                this.m_SetSkillBonuses = new AosSkillBonuses(this);
            #endregion

            #region SA
            if (this.m_SAAbsorptionAttributes == null)
                this.m_SAAbsorptionAttributes = new SAAbsorptionAttributes(this);
            #endregion

            if (version < 2)
            {
                this.m_Resource = CraftResource.Iron;
                this.m_GemType = GemType.None;
            }
        }

        #region ICraftable Members

        public int OnCraft(int quality, bool makersMark, Mobile from, CraftSystem craftSystem, Type typeRes, BaseTool tool, CraftItem craftItem, int resHue)
        {
            Type resourceType = typeRes;

            if (resourceType == null)
                resourceType = craftItem.Resources.GetAt(0).ItemType;

            if (!craftItem.ForceNonExceptional)
                this.Resource = CraftResources.GetFromType(resourceType);

            CraftContext context = craftSystem.GetContext(from);

            if (context != null && context.DoNotColor)
                this.Hue = 0;

            if (1 < craftItem.Resources.Count)
            {
                resourceType = craftItem.Resources.GetAt(1).ItemType;

                if (resourceType == typeof(StarSapphire))
                    this.GemType = GemType.GwiezdnySzafir;
                else if (resourceType == typeof(Emerald))
                    this.GemType = GemType.Szmaragd;
                else if (resourceType == typeof(Sapphire))
                    this.GemType = GemType.Szafir;
                else if (resourceType == typeof(Ruby))
                    this.GemType = GemType.Rubin;
                else if (resourceType == typeof(Citrine))
                    this.GemType = GemType.Cytryn;
                else if (resourceType == typeof(Amethyst))
                    this.GemType = GemType.Ametyst;
                else if (resourceType == typeof(Tourmaline))
                    this.GemType = GemType.Tourmalin;
                else if (resourceType == typeof(Amber))
                    this.GemType = GemType.Bursztyn;
                else if (resourceType == typeof(Diamond))
                    this.GemType = GemType.Diament;
            }

            #region Mondain's Legacy
            this.m_Quality = (ArmorQuality)quality;
            this.Identified = true;

            if (makersMark)
                this.m_Crafter = from;
            #endregion

            return 1;
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
        public virtual int Pieces
        {
            get
            {
                return 0;
            }
        }
        public virtual bool MixedSet
        {
            get
            {
                return false;
            }
        }

        public bool IsSetItem
        {
            get
            {
                return this.SetID == SetItem.None ? false : true;
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
        #endregion
    }
}
