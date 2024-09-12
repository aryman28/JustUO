using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using Server.Spells;

namespace Server.Spells.Song
{
    public class BalladaMaga : Song
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Ballada o magu", "Mentus",
            //SpellCircle.First,
            //212,9041
            -1);

        private SongBook m_Book;
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(6); } }
        public override double RequiredSkill { get { return 65; } }
        public override int RequiredMana { get { return 15; } }

        public BalladaMaga(Mobile caster, Item scroll): base(caster, scroll, m_Info){}

        public override void OnCast()
        {

           
            //get songbook instrument
            Spellbook book = Spellbook.Find(Caster, -1, SpellbookType.Song);
            if (book == null)
               {
                return;
               }
            m_Book = (SongBook)book;
            if (m_Book.Instrument == null || !(Caster.InRange(m_Book.Instrument.GetWorldLocation(), 1)))
            {
                Caster.SendMessage("The instrument you are trying to play is unaccessible. You can select the instrument through your Song Book");
                return;
            }
            Caster.PlaySound(m_Book.Instrument.SuccessSound);


            if (CheckSequence())
            {
                double allvalue = Caster.Skills[SkillName.Muzykowanie].Value + Caster.Skills[SkillName.Prowokacja].Value + Caster.Skills[SkillName.Manipulacja].Value + Caster.Skills[SkillName.Uspokajanie].Value;

                    {
                        ArrayList targets = new ArrayList();

                        foreach (Mobile m in Caster.GetMobilesInRange(3))
                        {
                            if (Caster.CanBeBeneficial(m, false, true) && !(m is Golem) && !(m is BaseCreature))
                                targets.Add(m);
                            //added: && !(m is BaseCreature ) , if it compiles. add to others
                        }

                        for (int i = 0; i < targets.Count; ++i)
                        {
                            Mobile m = (Mobile)targets[i];

                            //TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[SkillName.Musicianship].Value * 0.375);
                            TimeSpan duration = TimeSpan.FromSeconds(allvalue/4 * 0.5);
                            int rounds = (int)(Caster.Skills[SkillName.Muzykowanie].Value * .16);

                            if (allvalue < 120)
                            { new ExpireTimer(m, 0, rounds, TimeSpan.FromSeconds(2)).Start(); }
                            //2 mana
                            else if (allvalue < 240)
                            { new ExpireTimer1(m, 0, rounds, TimeSpan.FromSeconds(2)).Start(); }
                            //3 mana
                            else if (allvalue < 360)
                            { new ExpireTimer2(m, 0, rounds, TimeSpan.FromSeconds(2)).Start(); }
                            //4 mana
                            else if (allvalue < 480)
                            { new ExpireTimer3(m, 0, rounds, TimeSpan.FromSeconds(2)).Start(); }
                            //5
                            else if (allvalue >= 480)
                            { new ExpireTimer4(m, 0, rounds, TimeSpan.FromSeconds(2)).Start(); }
                            //10
                            else
                            { new ExpireTimer(m, 0, rounds, TimeSpan.FromSeconds(2)).Start(); }
                            //not required, just in case the else if dont cover it all, same as first if 

                            m.FixedParticles(0x376A, 9, 32, 5030, 0x256, 3, EffectLayer.Waist);
                            m.PlaySound(0x1F2);
                        }
                    }
                FinishSequence();
            }

        }

        private class ExpireTimer : Timer
        {
            private Mobile m_Mobile;
            private int m_Round;
            private int m_Totalrounds;

            public ExpireTimer(Mobile m, int round, int totalrounds, TimeSpan delay)
                : base(delay)
            {
                m_Mobile = m;
                m_Round = round;
                m_Totalrounds = totalrounds;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.Mana += 2;

                    if (m_Round >= m_Totalrounds)
                    {
                        m_Mobile.SendMessage("Wracasz do normalknego stanu.");
                    }
                    else
                    {
                        m_Round += 1;
                        new ExpireTimer(m_Mobile, m_Round, m_Totalrounds, TimeSpan.FromSeconds(2)).Start();
                    }
                }
            }
        }





        private class ExpireTimer1 : Timer
        {
            private Mobile m_Mobile;
            private int m_Round;
            private int m_Totalrounds;

            public ExpireTimer1(Mobile m, int round, int totalrounds, TimeSpan delay)
                : base(delay)
            {
                m_Mobile = m;
                m_Round = round;
                m_Totalrounds = totalrounds;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.Mana += 3;

                    if (m_Round >= m_Totalrounds)
                    {
                        m_Mobile.SendMessage("Wracasz do normalnego stanu.");
                    }
                    else
                    {
                        m_Round += 1;
                        new ExpireTimer1(m_Mobile, m_Round, m_Totalrounds, TimeSpan.FromSeconds(2)).Start();
                    }
                }
            }
        }


        private class ExpireTimer2 : Timer
        {
            private Mobile m_Mobile;
            private int m_Round;
            private int m_Totalrounds;

            public ExpireTimer2(Mobile m, int round, int totalrounds, TimeSpan delay)
                : base(delay)
            {
                m_Mobile = m;
                m_Round = round;
                m_Totalrounds = totalrounds;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.Mana += 4;

                    if (m_Round >= m_Totalrounds)
                    {
                        m_Mobile.SendMessage("Wracasz do normalknego stanu.");
                    }
                    else
                    {
                        m_Round += 1;
                        new ExpireTimer2(m_Mobile, m_Round, m_Totalrounds, TimeSpan.FromSeconds(2)).Start();
                    }
                }
            }
        }


        private class ExpireTimer3 : Timer
        {
            private Mobile m_Mobile;
            private int m_Round;
            private int m_Totalrounds;

            public ExpireTimer3(Mobile m, int round, int totalrounds, TimeSpan delay)
                : base(delay)
            {
                m_Mobile = m;
                m_Round = round;
                m_Totalrounds = totalrounds;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.Mana += 5;

                    if (m_Round >= m_Totalrounds)
                    {
                        m_Mobile.SendMessage("Wracasz do normalknego stanu.");
                    }
                    else
                    {
                        m_Round += 1;
                        new ExpireTimer3(m_Mobile, m_Round, m_Totalrounds, TimeSpan.FromSeconds(2)).Start();
                    }
                }
            }
        }



        private class ExpireTimer4 : Timer
        {
            private Mobile m_Mobile;
            private int m_Round;
            private int m_Totalrounds;

            public ExpireTimer4(Mobile m, int round, int totalrounds, TimeSpan delay)
                : base(delay)
            {
                m_Mobile = m;
                m_Round = round;
                m_Totalrounds = totalrounds;
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.Mana += 10;

                    if (m_Round >= m_Totalrounds)
                    {
                        m_Mobile.SendMessage("Wracasz do normalknego stanu.");
                    }
                    else
                    {
                        m_Round += 1;
                        new ExpireTimer4(m_Mobile, m_Round, m_Totalrounds, TimeSpan.FromSeconds(2)).Start();
                    }
                }
            }
        }

        


    }
}