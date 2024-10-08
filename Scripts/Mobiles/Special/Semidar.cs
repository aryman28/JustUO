using System;
using System.Collections;
using Server.Engines.CannedEvil;
using Server.Items;

namespace Server.Mobiles
{
    public class Semidar : BaseChampion
    {
        [Constructable]
        public Semidar()
            : base(AIType.AI_Mage)
        {
            this.Name = "Semidar";
            this.Body = 174;
            this.BaseSoundID = 0x4B0;

            this.SetStr(502, 600);
            this.SetDex(102, 200);
            this.SetInt(601, 750);

            this.SetHits(10000);
            this.SetStam(103, 250);

            this.SetDamage(29, 35);

            this.SetDamageType(ResistanceType.Physical, 75);
            this.SetDamageType(ResistanceType.Fire, 25);

            this.SetResistance(ResistanceType.Physical, 75, 90);
            this.SetResistance(ResistanceType.Fire, 65, 75);
            this.SetResistance(ResistanceType.Cold, 60, 70);
            this.SetResistance(ResistanceType.Poison, 65, 75);
            this.SetResistance(ResistanceType.Energy, 65, 75);

            this.SetSkill(SkillName.Intelekt, 95.1, 100.0);
            this.SetSkill(SkillName.Magia, 90.1, 105.0);
            this.SetSkill(SkillName.Medytacja, 95.1, 100.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 120.2, 140.0);
            this.SetSkill(SkillName.Taktyka, 90.1, 105.0);
            this.SetSkill(SkillName.Boks, 90.1, 105.0);

            this.Fame = 24000;
            this.Karma = -24000;

            this.VirtualArmor = 20;
        }

        public Semidar(Serial serial)
            : base(serial)
        {
        }

        public override ChampionSkullType SkullType
        {
            get
            {
                return ChampionSkullType.Pain;
            }
        }
        public override Type[] UniqueList
        {
            get
            {
                return new Type[] { typeof(GladiatorsCollar) };
            }
        }
        public override Type[] SharedList
        {
            get
            {
                return new Type[] { typeof(RoyalGuardSurvivalKnife), typeof(ANecromancerShroud), typeof(LieutenantOfTheBritannianRoyalGuard) };
            }
        }
        public override Type[] DecorativeList
        {
            get
            {
                return new Type[] { typeof(LavaTile), typeof(DemonSkull) };
            }
        }
        public override MonsterStatuetteType[] StatueTypes
        {
            get
            {
                return new MonsterStatuetteType[] { };
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich, 4);
            this.AddLoot(LootPack.FilthyRich);
        }

        public override void CheckReflect(Mobile caster, ref bool reflect)
        {
            if (caster.Body.IsMale)
                reflect = true; // Always reflect if caster isn't female
        }

        public override void AlterDamageScalarFrom(Mobile caster, ref double scalar)
        {
            if (caster.Body.IsMale)
                scalar = 20; // Male bodies always reflect.. damage scaled 20x
        }

        public void DrainLife()
        {
            if (this.Map == null)
                return;

            ArrayList list = new ArrayList();

            foreach (Mobile m in this.GetMobilesInRange(2))
            {
                if (m == this || !this.CanBeHarmful(m))
                    continue;

                if (m is BaseCreature && (((BaseCreature)m).Controlled || ((BaseCreature)m).Summoned || ((BaseCreature)m).Team != this.Team))
                    list.Add(m);
                else if (m.Player)
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                this.DoHarmful(m);

                m.FixedParticles(0x374A, 10, 15, 5013, 0x496, 0, EffectLayer.Waist);
                m.PlaySound(0x231);

                m.SendMessage("You feel the life drain out of you!");

                int toDrain = Utility.RandomMinMax(10, 40);

                this.Hits += toDrain;
                m.Damage(toDrain, this);
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (0.25 >= Utility.RandomDouble())
                this.DrainLife();
        }

        public override void OnGotMeleeAttack(Mobile attacker)
        {
            base.OnGotMeleeAttack(attacker);

            if (0.25 >= Utility.RandomDouble())
                this.DrainLife();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}