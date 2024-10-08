using System;
using System.Collections.Generic;

namespace Server.Items
{
    // The thrown projectile will arc to a second target after hitting the primary target. Chaos energy will burst from the projectile at each target. 
    // This will only hit targets that are in combat with the user.
    public class MysticArc : WeaponAbility
    {
        private readonly int m_Damage = 15;
        private Mobile m_Target;
        private Mobile m_Mobile;
        public override int BaseStam
        {
            get
            {
                return 20;
            }
        }
        public override void OnHit(Mobile attacker, Mobile defender, int damage)
        {
            if (!this.CheckStam(attacker, true) && defender != null)
                return;

            BaseThrown weapon = attacker.Weapon as BaseThrown;

            if (weapon == null)
                return;

            List<Mobile> targets = new List<Mobile>();

            foreach (Mobile m in attacker.GetMobilesInRange(weapon.MaxRange))
            {
                if (m == defender)
                    continue;

                if (m.Combatant != attacker)
                    continue;

                targets.Add(m);
            }

            if (targets.Count > 0)
                this.m_Target = targets[Utility.Random(targets.Count)];

            /*
            Mobile m = null;

            foreach( DamageEntry de in attacker.DamageEntries )
            {
            m = Mobile.GetDamagerFrom( de );

            if ( m != null )
            {
            if ( defender != m && defender.InRange( m, 3 ) )
            {
            m_Target = m;
            break;
            }
            }
            }
            */

            AOS.Damage(defender, attacker, this.m_Damage, 0, 0, 0, 0, 100);

            if (this.m_Target != null)
            {
                defender.MovingEffect(this.m_Target, weapon.ItemID, 18, 1, false, false);
                Timer.DelayCall(TimeSpan.FromMilliseconds(333.0), new TimerCallback(ThrowAgain));
                this.m_Mobile = attacker;
            }

            ClearCurrentAbility(attacker);
        }

        public void ThrowAgain()
        {
            if (this.m_Target != null && this.m_Mobile != null)
            {
                BaseThrown weapon = this.m_Mobile.Weapon as BaseThrown;

                if (weapon == null)
                    return;

                if (WeaponAbility.GetCurrentAbility(this.m_Mobile) is MysticArc)
                    ClearCurrentAbility(this.m_Mobile);

                if (weapon.CheckHit(this.m_Mobile, this.m_Target))
                {
                    weapon.OnHit(this.m_Mobile, this.m_Target, 0.0);
                    AOS.Damage(this.m_Target, this.m_Mobile, this.m_Damage, 0, 0, 0, 0, 100);
                }
            }
        }
    }
}