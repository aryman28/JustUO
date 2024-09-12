using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Multis;
using Server.Misc;
using Server.Regions;
using Server.Gumps;
using Server.Spells.Druid;


namespace Server.Spells.Druid
{
    public class BarkSkinSpell : DruidSpell
    {
        private static SpellInfo m_Info = new SpellInfo
            (
             "Debowa SkÃ³ra", "Porm Helma",
            //SpellCircle.First,
             224,
             9011,
            Reagent.PetrifiedWood,
            Reagent.FertileEarth
           );
        public override string SpellDescription
        {
            get
            {
                return "Skóra Druida okrywa siê dêbow¹ kor¹. Wzrasta odpornoœæ na ataki fizyczne,energetyczne oraz od trucizn.";
            }
        }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(2); } }
        public override SpellCircle Circle { get { return SpellCircle.First; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public BarkSkinSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {

            if (CheckSequence())
            {
                TimeSpan duration = TimeSpan.FromSeconds(Caster.Skills[SkillName.WiedzaOBestiach].Value * 6);
                int amount = (int)(Caster.Skills[SkillName.WiedzaOBestiach].Value * .125);

                Caster.SendMessage("Your skin has turned to bark.");

                Caster.HueMod = 1885;

                ResistanceMod mod1 = new ResistanceMod(ResistanceType.Physical, +amount);
                ResistanceMod mod2 = new ResistanceMod(ResistanceType.Energy, +amount);
                ResistanceMod mod3 = new ResistanceMod(ResistanceType.Poison, +amount);

                Caster.FixedParticles(0x373A, 10, 15, 1272, 0x4f8, 3, EffectLayer.Waist);
                Caster.PlaySound(443);

                Caster.AddResistanceMod(mod1);
                Caster.AddResistanceMod(mod2);
                Caster.AddResistanceMod(mod3);

                new ExpireTimer(Caster, mod1, duration).Start();
                new ExpireTimer(Caster, mod2, duration).Start();
                new ExpireTimer(Caster, mod3, duration).Start();

            }

            FinishSequence();
        }

        private class ExpireTimer : Timer
        {
            private Mobile m_Mobile;
            private ResistanceMod m_Mods;

            public ExpireTimer(Mobile m, ResistanceMod mod, TimeSpan delay)
                : base(delay)
            {
                m_Mobile = m;
                m_Mods = mod;
            }

            public void DoExpire()
            {
                m_Mobile.RemoveResistanceMod(m_Mods);

                Stop();
            }

            protected override void OnTick()
            {
                if (m_Mobile != null)
                {
                    m_Mobile.SendMessage("your skin returns to normal.");
                    m_Mobile.HueMod = -1;
                    DoExpire();
                }
            }
        }
    }
}
