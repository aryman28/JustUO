using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells.Nekromancja
{
    public abstract class NecromancerSpell : Spell
    {
        public NecromancerSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
        }

        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }
        public override SkillName CastSkill
        {
            get
            {
                return SkillName.Nekromancja;
            }
        }
        public override SkillName DamageSkill
        {
            get
            {
                return SkillName.MowaDuchow;
            }
        }
        //public override int CastDelayBase{ get{ return base.CastDelayBase; } } // Reference, 3
        public override bool ClearHandsOnCast
        {
            get
            {
                return false;
            }
        }
        public override double CastDelayFastScalar
        {
            get
            {
                return (Core.SE ? base.CastDelayFastScalar : 0);
            }
        }// Necromancer spells are not affected by fast cast items, though they are by fast cast recovery
        public override int ComputeKarmaAward()
        {
            //TODO: Verify this formula being that Necro spells don't HAVE a circle.
            //int karma = -(70 + (10 * (int)Circle));
            int karma = -(40 + (int)(10 * (this.CastDelayBase.TotalSeconds / this.CastDelaySecondsPerTick)));

            if (Core.ML) // Pub 36: "Added a new property called Increased Karma Loss which grants higher karma loss for casting necromancy spells."
                karma += AOS.Scale(karma, AosAttributes.GetValue(this.Caster, AosAttribute.IncreasedKarmaLoss));

            return karma;
        }

        // Sprawdzenie klasy postaci przed rzuceniem czaru
        public override bool CheckCast()
        {
            if (this.Caster is PlayerMobile && 
                ((PlayerMobile)this.Caster).Klasa != Klasa.Kultysta && 
        		((PlayerMobile)this.Caster).Klasa != Klasa.Nekromanta && 
        		
        		((PlayerMobile)this.Caster).Klasa != Klasa.Czarnoksiê¿nik)

            {
                this.Caster.SendMessage("Tylko Kultyœci, Nekromanci i Czarnoksiê¿nicy mog¹ u¿ywaæ tych zaklêæ."); 
                return false;
            }

            return base.CheckCast();
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = this.RequiredSkill;
            max = this.Scroll != null ? min : this.RequiredSkill + 40.0;
        }

        public override bool ConsumeReagents()
        {
            if (base.ConsumeReagents())
                return true;

            if (ArcaneGem.ConsumeCharges(this.Caster, 1))
                return true;

            return false;
        }

        public override int GetMana()
        {
            return this.RequiredMana;
        }
    }
}
