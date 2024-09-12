using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles; // Dodane odniesienie do przestrzeni nazw, gdzie znajduje siê PlayerMobile

namespace Server.Spells.Rycerstwo
{
    public class HolyLightSpell : PaladinSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Holy Light", "Augus Luminos",
            -1,
            9002);
        
        public HolyLightSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(1.75);
            }
        }

        public override double RequiredSkill
        {
            get
            {
                return 55.0;
            }
        }

        public override int RequiredMana
        {
            get
            {
                return 10;
            }
        }

        public override int RequiredTithing
        {
            get
            {
                return 10;
            }
        }

        public override int MantraNumber
        {
            get
            {
                return 1060724;
            }
        }// Augus Luminos

        public override bool BlocksMovement
        {
            get
            {
                return false;
            }
        }

        public override bool DelayedDamage
        {
            get
            {
                return false;
            }
        }

        public override void OnCast()
        {
            if (this.CheckSequence())
            {
                List<Mobile> targets = new List<Mobile>();

                foreach (Mobile m in this.Caster.GetMobilesInRange(3))
                {
                    if (this.Caster != m && SpellHelper.ValidIndirectTarget(this.Caster, m) && this.Caster.CanBeHarmful(m, false) && (!Core.AOS || this.Caster.InLOS(m)))
                        targets.Add(m);
                }

                this.Caster.PlaySound(0x212);

                // Sprawdzenie, czy klasa gracza to Mœciciel
                bool isAvenger = this.Caster is PlayerMobile && ((PlayerMobile)this.Caster).Klasa == Klasa.Mœciciel;

                // Zmiana koloru efektu w zale¿noœci od klasy
                int hue = isAvenger ? 0x48E : 0x47D; // Zielony dla Mœciciela, standardowy dla innych

                // Efekty wizualne
                Effects.SendLocationParticles(EffectItem.Create(this.Caster.Location, this.Caster.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, hue, 2, 9962, 0);
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(this.Caster.X, this.Caster.Y, this.Caster.Z - 7), this.Caster.Map, EffectItem.DefaultDuration), 0x37C4, 1, 29, hue, 2, 9502, 0);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = targets[i];

                    int damage = this.ComputePowerValue(10) + Utility.RandomMinMax(0, 2);

                    // Zmiana rodzaju obra¿eñ: trucizna dla Mœciciela, energia dla innych
                    int poisonDamage = isAvenger ? 100 : 0; // Obra¿enia od trucizny dla Mœciciela
                    int energyDamage = isAvenger ? 0 : 100; // Obra¿enia od energii dla innych

                    // Próg obra¿eñ
                    if (damage < 8)
                        damage = 8;
                    else if (damage > 24)
                        damage = 24;

                    this.Caster.DoHarmful(m);
                    SpellHelper.Damage(this, m, damage, 0, energyDamage, poisonDamage, 0, 100); // Trucizna lub energia zale¿nie od klasy
                }
            }

            this.FinishSequence();
        }
    }
}
