using System;
using System.Collections;

namespace Server.Items
{
    /// <summary>
    /// Attack with increased damage with additional damage over time.
    /// </summary>
    public class TalonStrike : WeaponAbility
    {
        private static readonly Hashtable m_Registry = new Hashtable();
        public TalonStrike()
        {
        }

        public static Hashtable Registry
        {
            get
            {
                return m_Registry;
            }
        }
        public override int BaseStam
        {
            get
            {
                return 20;
            }
        }
        public override double DamageScalar
        {
            get
            {
                return 1.2;
            }
        }
        public override bool CheckSkills(Mobile from)
        {
            if (this.GetSkill(from, SkillName.Skrytobojstwo) < 50.0)
            {
                from.SendLocalizedMessage(1063352, "50"); // You need ~1_SKILL_REQUIREMENT~ Skrytobojstwo skill to perform that attack!
                return false;
            }

            return base.CheckSkills(from);
        }

        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (Registry.Contains(defender) || !this.Validate(attacker) || !this.CheckStam(attacker, true))
                return;

            ClearCurrentAbility(attacker);

            attacker.SendLocalizedMessage(1063358); // You deliver a talon strike!
            defender.SendLocalizedMessage(1063359); // Your attacker delivers a talon strike!

            defender.FixedParticles(0x373A, 1, 17, 0x26BC, 0x662, 0, EffectLayer.Waist);

            Timer t = new InternalTimer(defender, (int)(10.0 * (attacker.Skills[SkillName.Skrytobojstwo].Value - 50.0) / 70.0 + 5), attacker);	//5 - 15 damage

            t.Start();

            Registry.Add(defender, t);
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Defender;
            private readonly Mobile m_Attacker;
            private readonly double DamagePerTick;
            private double m_DamageRemaining;
            private double m_DamageToDo;
            public InternalTimer(Mobile defender, int totalDamage, Mobile attacker)
                : base(TimeSpan.Zero, TimeSpan.FromSeconds(0.25), 12)// 3 seconds at .25 seconds apart = 12.  Confirm delay inbetween of .25 each.
            {
                this.m_Defender = defender;
                this.m_DamageRemaining = (double)totalDamage;
                this.Priority = TimerPriority.TwentyFiveMS;

                this.m_Attacker = attacker;

                this.DamagePerTick = (double)totalDamage / 12 + .01;
            }

            protected override void OnTick()
            {
                if (!this.m_Defender.Alive || this.m_DamageRemaining <= 0)
                {
                    this.Stop();
                    Server.Items.TalonStrike.Registry.Remove(this.m_Defender);
                    return;
                }

                this.m_DamageRemaining -= this.DamagePerTick;
                this.m_DamageToDo += this.DamagePerTick;

                if (this.m_DamageRemaining <= 0 && this.m_DamageToDo < 1)
                    this.m_DamageToDo = 1.0; //Confirm this 'round up' at the end

                int damage = (int)this.m_DamageToDo;

                if (damage > 0)
                {
                    //m_Defender.Damage( damage, m_Attacker, false );
                    this.m_Defender.Hits -= damage;	//Don't show damage, don't disrupt
                    this.m_DamageToDo -= damage;
                }

                if (!this.m_Defender.Alive || this.m_DamageRemaining <= 0)
                {
                    this.Stop();
                    Server.Items.TalonStrike.Registry.Remove(this.m_Defender);
                }
            }
        }
    }
}