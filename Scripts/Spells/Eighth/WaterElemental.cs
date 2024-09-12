using System;
using Server.Mobiles;

namespace Server.Spells.Eighth
{
    public class WaterElementalSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Water Elemental", "Kal Vas Xen An Flam",
            269,
            9070,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk);
        public WaterElementalSpell(Mobile caster, Item scroll)
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

            if ((this.Caster.Followers + 3) > this.Caster.FollowersMax)
            {
                this.Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            // Magowie
            if (this.Caster is PlayerMobile && (((PlayerMobile)this.Caster).Klasa != Klasa.Mag 
            && ((PlayerMobile)this.Caster).Klasa != Klasa.Adept 
            && ((PlayerMobile)this.Caster).Klasa != Klasa.Arcymag
            && ((PlayerMobile)this.Caster).Klasa != Klasa.Czarnoksiê¿nik))
            {   
                this.Caster.SendMessage("Tylko magowie mog¹ u¿yæ tego czaru!"); // Only Elves may use this ability
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
                    SpellHelper.Summon(new SummonedWaterElemental(), this.Caster, 0x217, duration, false, false);
                else
                    SpellHelper.Summon(new WaterElemental(), this.Caster, 0x217, duration, false, false);
            }

            this.FinishSequence();
        }
    }
}