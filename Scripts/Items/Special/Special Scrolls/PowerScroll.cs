using System;
using System.Collections.Generic;

namespace Server.Items
{
    public class PowerScroll : SpecialScroll
    {
        private static readonly SkillName[] m_Skills = new SkillName[]
        {
            SkillName.Kowalstwo,
            SkillName.Krawiectwo,
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
            SkillName.Uspokajanie
        };
        private static readonly SkillName[] m_AOSSkills = new SkillName[]
        {
            SkillName.Rycerstwo,
            SkillName.Logistyka,
            SkillName.Nekromancja,
            SkillName.Okradanie,
            SkillName.Zakradanie,
            SkillName.MowaDuchow
        };
        private static readonly SkillName[] m_SESkills = new SkillName[]
        {
            SkillName.Skrytobojstwo,
            SkillName.Fanatyzm
        };
        private static readonly SkillName[] m_MLSkills = new SkillName[]
        {
            SkillName.Druidyzm
        };
        /*
        private static SkillName[] m_SASkills = new SkillName[]
        {
        SkillName.Rzucanie,
        SkillName.Mistycyzm,
        SkillName.Umagicznianie
        };

        private static SkillName[] m_HSSkills = new SkillName[]
        {
        SkillName.Rybactwo
        };
        */
        private static readonly List<SkillName> _Skills = new List<SkillName>();
        public PowerScroll()
            : this(SkillName.Alchemia, 0.0)
        {
        }

        [Constructable]
        public PowerScroll(SkillName skill, double value)
            : base(skill, value)
        {
            this.Hue = 0x481;

            if (this.Value == 105.0 || skill == Server.SkillName.Kowalstwo || skill == Server.SkillName.Krawiectwo)
                this.LootType = LootType.Regular;
        }

        public PowerScroll(Serial serial)
            : base(serial)
        {
        }

        public static List<SkillName> Skills
        {
            get
            {
                if (_Skills.Count == 0)
                {
                    _Skills.AddRange(m_Skills);
                    if (Core.AOS)
                    {
                        _Skills.AddRange(m_AOSSkills);
                        if (Core.SE)
                        {
                            _Skills.AddRange(m_SESkills);
                            if (Core.ML)
                            {
                                _Skills.AddRange(m_MLSkills);
                                /*
                                if (Core.SA)
                                {
                                _Skills.AddRange( m_SASkills );
                                if (Core.HS)
                                _Skills.AddRange( m_HSSkills );
                                }
                                */
                            }
                        }
                    }
                }
                return _Skills;
            }
        }
        public override int Message
        {
            get
            {
                return 1049469;
            }
        }/* Using a scroll increases the maximum amount of a specific skill or your maximum statistics.
        * When used, the effect is not immediately seen without a gain of points with that skill or statistics.
        * You can view your maximum skill values in your skills window.
        * You can view your maximum statistic value in your statistics window. */
        public override int Title
        {
            get
            {
                double level = (this.Value - 105.0) / 5.0;

                if (level >= 0.0 && level <= 3.0 && this.Value % 5.0 == 0.0)
                    return 1049635 + (int)level;	/* Wonderous Scroll (105 Skill): OR
                * Exalted Scroll (110 Skill): OR
                * Mythical Scroll (115 Skill): OR
                * Legendary Scroll (120 Skill): */

                return 0;
            }
        }
        public override string DefaultTitle
        {
            get
            {
                return String.Format("<basefont color=#FFFFFF>Power Scroll ({0} Skill):</basefont>", this.Value);
            }
        }
        public static PowerScroll CreateRandom(int min, int max)
        {
            min /= 5;
            max /= 5;

            return new PowerScroll(Skills[Utility.Random(Skills.Count)], 100 + (Utility.RandomMinMax(min, max) * 5));
        }

        public static PowerScroll CreateRandomNoCraft(int min, int max)
        {
            min /= 5;
            max /= 5;

            SkillName skillName;

            do
            {
                skillName = Skills[Utility.Random(Skills.Count)];
            }
            while (skillName == SkillName.Kowalstwo || skillName == SkillName.Krawiectwo);

            return new PowerScroll(skillName, 100 + (Utility.RandomMinMax(min, max) * 5));
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            double level = (this.Value - 105.0) / 5.0;

            if (level >= 0.0 && level <= 3.0 && this.Value % 5.0 == 0.0)
                list.Add(1049639 + (int)level, this.GetNameLocalized());	/* a wonderous scroll of ~1_type~ (105 Skill) OR
            * an exalted scroll of ~1_type~ (110 Skill) OR
            * a mythical scroll of ~1_type~ (115 Skill) OR
            * a legendary scroll of ~1_type~ (120 Skill) */
            else
                list.Add("a power scroll of {0} ({1} Skill)", this.GetName(), this.Value);
        }

        public override void OnSingleClick(Mobile from)
        {
            double level = (this.Value - 105.0) / 5.0;

            if (level >= 0.0 && level <= 3.0 && this.Value % 5.0 == 0.0)
                base.LabelTo(from, 1049639 + (int)level, this.GetNameLocalized());
            else
                base.LabelTo(from, "a power scroll of {0} ({1} Skill)", this.GetName(), this.Value);
        }

        public override bool CanUse(Mobile from)
        {
            if (!base.CanUse(from))
                return false;

            Skill skill = from.Skills[this.Skill];

            if (skill == null)
                return false;

            if (skill.Cap >= this.Value)
            {
                from.SendLocalizedMessage(1049511, this.GetNameLocalized()); // Your ~1_type~ is too high for this power scroll.
                return false;
            }

            return true;
        }

        public override void Use(Mobile from)
        {
            if (!this.CanUse(from))
                return;

            from.SendLocalizedMessage(1049513, this.GetNameLocalized()); // You feel a surge of magic as the scroll enhances your ~1_type~!

            from.Skills[this.Skill].Cap = this.Value;

            Effects.SendLocationParticles(EffectItem.Create(from.Location, from.Map, EffectItem.DefaultDuration), 0, 0, 0, 0, 0, 5060, 0);
            Effects.PlaySound(from.Location, from.Map, 0x243);

            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 4, from.Y - 6, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);
            Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(from.X - 6, from.Y - 4, from.Z + 15), from.Map), from, 0x36D4, 7, 0, false, true, 0x497, 0, 9502, 1, 0, (EffectLayer)255, 0x100);

            Effects.SendTargetParticles(from, 0x375A, 35, 90, 0x00, 0x00, 9502, (EffectLayer)255, 0x100);

            this.Delete();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = (this.InheritsItem ? 0 : reader.ReadInt()); // Required for SpecialScroll insertion

            if (this.Value == 105.0 || this.Skill == SkillName.Kowalstwo || this.Skill == SkillName.Krawiectwo)
            {
                this.LootType = LootType.Regular;
            }
            else
            {
                this.LootType = LootType.Cursed;
                this.Insured = false;
            }
        }
    }
}