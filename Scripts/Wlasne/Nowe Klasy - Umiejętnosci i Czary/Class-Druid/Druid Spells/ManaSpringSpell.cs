using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Spells.Druid
{
    public class ManaSpringSpell : DruidSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Źródło Many", "En Sepa Aete",
            //SpellCircle.Eighth,
            266,
            9040,
            false,
            Reagent.FertileDirt,
            Reagent.SpringWater,
            Reagent.FenMoss
            );
        public override string SpellDescription
        {
            get
            {
                return "Tworzy magiczne zródło które odnawia mane tobie oraz członkom twojej drużyny.";
            }
        }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(6); } }
        public override SpellCircle Circle { get { return SpellCircle.Eighth; } }
        public override double RequiredSkill { get { return 85.0; } }
        public override int RequiredMana { get { return 60; } }

        public ManaSpringSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x11);
                Effects.PlaySound(p, Caster.Map, 0x1E0);

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                int grovex;
                int grovey;
                int grovez;
                InternalItem groveStone = new InternalItem(Caster.Location, Caster.Map, Caster);
                grovex = loc.X;
                grovey = loc.Y;
                grovez = loc.Z;
                groveStone.ItemID = 0x2A04;
                groveStone.Name = "mana spring";
                Point3D stonexyz = new Point3D(grovex, grovey, grovez);
                groveStone.MoveToWorld(stonexyz, Caster.Map);

                InternalItem grovea = new InternalItem(Caster.Location, Caster.Map, Caster);
                grovex = loc.X;
                grovey = loc.Y;
                grovez = loc.Z + 3;
                grovea.ItemID = 0x1797;
                grovea.Hue = 93;
                grovea.Name = "mana spring";
                Point3D grovexyz = new Point3D(grovex, grovey, grovez);
                grovea.MoveToWorld(grovexyz, Caster.Map);

                InternalItem grovec = new InternalItem(Caster.Location, Caster.Map, Caster);
                grovex = loc.X;
                grovey = loc.Y;
                grovez = loc.Z + 4;
                grovec.ItemID = 0x373A;
                grovec.Hue = 93;
                Point3D grovexyzb = new Point3D(grovex, grovey, grovez);
                grovec.MoveToWorld(grovexyzb, Caster.Map);

            }

            FinishSequence();
        }

        [DispellableField]
        private class InternalItem : Item
        {
            private Timer m_Timer;
            private Timer m_Bless;
            private DateTime m_End;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public InternalItem(Point3D loc, Map map, Mobile caster)
                : base(0x3274)
            {
                Visible = false;
                Movable = false;
                MoveToWorld(loc, map);
                m_Caster = caster;

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(60.0));
                m_Timer.Start();
                m_Bless = new BlessTimer(this, m_Caster);
                m_Bless.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(60.0);
            }

            public InternalItem(Serial serial)
                : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)1); // version

                writer.Write(m_End - DateTime.Now);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();

                switch (version)
                {
                    case 1:
                        {
                            TimeSpan duration = reader.ReadTimeSpan();

                            m_Timer = new InternalTimer(this, duration);
                            m_Timer.Start();

                            m_End = DateTime.Now + duration;

                            break;
                        }
                    case 0:
                        {
                            TimeSpan duration = TimeSpan.FromSeconds(10.0);

                            m_Timer = new InternalTimer(this, duration);
                            m_Timer.Start();

                            m_End = DateTime.Now + duration;

                            break;
                        }
                }
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                if (m_Timer != null)
                    m_Timer.Stop();
            }

            private class InternalTimer : Timer
            {
                private Timer m_Bless;

                private InternalItem m_Item;

                public InternalTimer(InternalItem item, TimeSpan duration)
                    : base(duration)
                {
                    m_Item = item;
                }

                protected override void OnTick()
                {
                    m_Item.Delete();
                }
            }
        }
        private class InternalTarget : Target
        {
            private ManaSpringSpell m_Owner;

            public InternalTarget(ManaSpringSpell owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
        private class BlessTimer : Timer
        {
            private Item m_EnchantedGrove;
            private Mobile m_Caster;
            private DateTime m_Duration;

            private static Queue m_Queue = new Queue();

            public BlessTimer(Item ap, Mobile ca)
                : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
            {
                Priority = TimerPriority.FiftyMS;

                m_EnchantedGrove = ap;
                m_Caster = ca;
                m_Duration = DateTime.Now + TimeSpan.FromSeconds(15.0 + (Utility.RandomDouble() * 15.0));
            }

            protected override void OnTick()
            {
                if (m_EnchantedGrove.Deleted)
                    return;

                if (DateTime.Now > m_Duration)
                {

                    Stop();
                }
                else
                {
                    ArrayList list = new ArrayList();

                    foreach (Mobile m in m_EnchantedGrove.GetMobilesInRange(5))
                    {
                        if (m.Player && m.Karma >= 0 && m.Alive)
                            list.Add(m);
                    }

                    for (int i = 0; i < list.Count; ++i)
                    {
                        Mobile m = (Mobile)list[i];
                        bool friendly = true;

                        for (int j = 0; friendly && j < m_Caster.Aggressors.Count; ++j)
                            friendly = (((AggressorInfo)m_Caster.Aggressors[j]).Attacker != m);

                        for (int j = 0; friendly && j < m_Caster.Aggressed.Count; ++j)
                            friendly = (((AggressorInfo)m_Caster.Aggressed[j]).Defender != m);

                        if (friendly)
                        {
                            m.FixedParticles(0x3779, 9, 32, 5030, EffectLayer.Waist); // At player
                            m.Mana += (1);
                        }
                    }
                }
            }
        }
    }
}
