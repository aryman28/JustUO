using System;
using Server.Mobiles;

namespace Server.Spells.Eighth
{
    public class EarthElementalSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Earth Elemental", "Kal Vas Xen Ylem",
            269,
            9020,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk);
        public EarthElementalSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override SpellCircle Circle
        {
            get
            {
                return SpellCircle.Eighth;
            }
        }
        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            if ((this.Caster.Followers + 2) > this.Caster.FollowersMax)
            {
                this.Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }
            // Magowie
            if (this.Caster is PlayerMobile && (((PlayerMobile)this.Caster).Klasa != Klasa.Mag 
            && ((PlayerMobile)this.Caster).Klasa != Klasa.Adept 
            && ((PlayerMobile)this.Caster).Klasa != Klasa.Arcymag
            && ((PlayerMobile)this.Caster).Klasa != Klasa.Czarnoksi�nik))
            {   
                this.Caster.SendMessage("Tylko magowie mog� u�y� tego czaru!"); // Only Elves may use this ability
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (this.CheckSequence())
            {
                TimeSpan duration = TimeSpan.FromSeconds((2 * this.Caster.Skills.Magia.Fixed) / 5);

                if (Core.AOS)
                    SpellHelper.Summon(new SummonedEarthElemental(), this.Caster, 0x217, duration, false, false);
                else
                    SpellHelper.Summon(new EarthElemental(), this.Caster, 0x217, duration, false, false);
            }

            this.FinishSequence();
        }
    }
}