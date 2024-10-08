using System;
using System.Collections;
using Server.Mobiles;
using Server.Items; 
using Server; 

namespace Server.Spells.Rycerstwo
{
    public class EnemyOfOneSpell : PaladinSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Enemy of One", "Forul Solum",
            -1,
            9002);
        private static readonly Hashtable m_Table = new Hashtable();

        public EnemyOfOneSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase
        {
            get
            {
                return TimeSpan.FromSeconds(0.5);
            }
        }

        public override double RequiredSkill
        {
            get
            {
                return 45.0;
            }
        }

        public override int RequiredMana
        {
            get
            {
                return 20;
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
                return 1060723;
            }
        }

        public override bool BlocksMovement
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
                
                this.Caster.PlaySound(0x0F5);
                this.Caster.PlaySound(0x1ED);
                this.Caster.FixedParticles(0x375A, 1, 30, 9966, 33, 2, EffectLayer.Head);
                this.Caster.FixedParticles(0x37B9, 1, 30, 9502, 43, 3, EffectLayer.Head);

                Timer t = (Timer)m_Table[this.Caster];

                if (t != null)
                    t.Stop();

                double delay = (double)this.ComputePowerValue(1) / 60;

                if (delay < 1.5)
                    delay = 1.5;
                else if (delay > 3.5)
                    delay = 3.5;

                m_Table[this.Caster] = Timer.DelayCall(TimeSpan.FromMinutes(delay), new TimerStateCallback(Expire_Callback), this.Caster);

                if (this.Caster is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)this.Caster;

                    pm.EnemyOfOneType = null;
                    pm.WaitingForEnemy = true;

                    // Sprawdzenie klasy M�ciciela
                    if (pm.Klasa == Klasa.M�ciciel)
                    {
                        // Zmniejszenie dodatkowych obra�e�
                        delay *= 0.9; // Obra�enia zmniejszone o 10%

                        // Dodanie unikalnego efektu dla M�ciciela - zwi�kszona odporno�� na obra�enia
                        pm.VirtualArmorMod += 50; // Dodajemy 10% do pancerza, co zmniejsza obra�enia
                        Timer.DelayCall(TimeSpan.FromMinutes(delay), () => 
                        {
                            pm.VirtualArmorMod -= 50; // Po up�ywie czasu efekt znika
                        });
                    }

                    BuffInfo.AddBuff(this.Caster, new BuffInfo(BuffIcon.EnemyOfOne, 1075653, 1044111, TimeSpan.FromMinutes(delay), this.Caster));
                }
            }

            this.FinishSequence();
        }

        private static void Expire_Callback(object state)
        {
            Mobile m = (Mobile)state;

            m_Table.Remove(m);

            m.PlaySound(0x1F8);

            if (m is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)m;
                pm.EnemyOfOneType = null;
                pm.WaitingForEnemy = false;
            }
        }
    }
}
