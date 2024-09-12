using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Druid
{
    public class HibernateSpell : DruidSpell
    {
        private SleepingBody m_Body;
        private bool m_Blessed;
        private static SpellInfo m_Info = new SpellInfo(
                "Uśpienie", "En Xen Zu",
            //SpellCircle.Third,
                206,
                9002,
                Reagent.PetrifiedWood,
                Reagent.Pumice
            );
        public override string SpellDescription
        {
            get
            {
                return "Dzięki magii natury usypiasz wskazany cel.";
            }
        }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1); } }
        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 23; } }

        public HibernateSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            {
                SpellHelper.Turn(Caster, m);
                if (this.Scroll != null)
                    Scroll.Consume();
                Effects.SendLocationParticles(EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5045);
                m.PlaySound(0x3C4);

                m.Hidden = true;
                m.Frozen = true;
                m.Squelched = true;

                ArrayList sleepequip = new ArrayList();


                Item hat = m.FindItemOnLayer(Layer.Helm);
                if (hat != null)
                {
                    sleepequip.Add(hat);
                }
                SleepingBody body = new SleepingBody(m, m_Blessed);
                body.Map = m.Map;
                body.Location = m.Location;
                m_Body = body;
                m.Z -= 100;

                m.SendMessage("You fall asleep");
                Caster.SendMessage("Your target falls asleep");

                RemoveTimer(m);

                TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[SkillName.WiedzaOBestiach].Value * 0.06);  //100% of animal lore

                Timer t = new InternalTimer(m, duration, m_Body);

                m_Table[m] = t;

                t.Start();
            }
        }

        private static Hashtable m_Table = new Hashtable();

        public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile;
            private Item m_Body;

            public InternalTimer(Mobile m, TimeSpan duration, Item body)
                : base(duration)
            {
                m_Mobile = m;
                m_Body = body;
            }

            protected override void OnTick()
            {
                m_Mobile.RevealingAction();
                m_Mobile.Frozen = false;
                m_Mobile.Squelched = false;



                if (m_Body != null)
                {
                    m_Body.Delete();
                    m_Mobile.SendMessage("You wake up!");
                    m_Mobile.Z = m_Body.Z;
                    m_Mobile.Animate(21, 6, 1, false, false, 0);
                }
                RemoveTimer(m_Mobile);
            }
        }

        public class InternalTarget : Target
        {
            private HibernateSpell m_Owner;

            public InternalTarget(HibernateSpell owner)
                : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
