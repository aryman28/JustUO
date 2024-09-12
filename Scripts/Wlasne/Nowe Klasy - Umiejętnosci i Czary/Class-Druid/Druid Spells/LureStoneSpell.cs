using System;
using Server.Targeting;
using Server.Network;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Druid
{
    public class LureStoneSpell : DruidSpell
    {
        private LureStone m_Circlea;
        private Item m_Circleb;
        private LureStone m_Circlec;
        private static SpellInfo m_Info = new SpellInfo(
                "Kuszący kamień", "En Kes Ohm Crur",
            //SpellCircle.Second,
                269,
                9020,
                false,
                Reagent.Pumice,
                Reagent.SpringWater
            );
        public override string SpellDescription
        {
            get
            {
                return "Tworzysz magiczny kamień który wabi do siebie zwierzęta.";
            }
        }
        public LureStoneSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1); } }
        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            return true;
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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Effects.PlaySound(p, Caster.Map, 0x243);

                int stonex;
                int stoney;
                int stonez;

                Point3D loc = new Point3D(p.X, p.Y, p.Z);
                Item item = new InternalItema(loc, Caster.Map, Caster);
                stonex = p.X;
                stoney = p.Y - 1;
                stonez = p.Z;
                Point3D loca = new Point3D(stonex, stoney, stonez);
                Item itema = new InternalItemb(loca, Caster.Map, Caster);

            }

            FinishSequence();
        }
        [DispellableField]
        private class InternalItema : Item
        {
            private Timer m_Timer;
            private DateTime m_End;
            private Mobile m_Owner;

            public override bool BlocksFit { get { return true; } }

            public InternalItema(Point3D loc, Map map, Mobile caster)
                : base(0x1355)
            {
                m_Owner = caster;
                Visible = false;
                Movable = false;
                Name = "Lure Stone";
                MoveToWorld(loc, map);

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0);
            }

            public InternalItema(Serial serial)
                : base(serial)
            {
            }
            public override bool HandlesOnMovement { get { return true; } }
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
            public override void OnMovement(Mobile m, Point3D oldLocation)
            {
                if (m_Owner != null)
                {
                    if (m.InRange(this, 600))
                    {
                        double tamer = m_Owner.Skills[SkillName.WiedzaOBestiach].Value;
                        double bonus = m_Owner.Skills[SkillName.Oswajanie].Value / 100;

                        BaseCreature cret = m as BaseCreature;
                        if (cret != null)
                            if (tamer >= 99.9 && (cret.Combatant == null || !cret.Combatant.Alive || cret.Combatant.Deleted))
                            {
                                cret.TargetLocation = new Point2D(this.X, this.Y);
                            }
                            else if (cret.Tamable && (cret.Combatant == null || !cret.Combatant.Alive || cret.Combatant.Deleted))
                            {

                                if (cret.MinTameSkill <= (tamer + bonus) + 0.1)
                                    cret.TargetLocation = new Point2D(this.X, this.Y);
                            }
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
                private InternalItema m_Item;

                public InternalTimer(InternalItema item, TimeSpan duration)
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
        [DispellableField]
        private class InternalItemb : Item
        {
            private Timer m_Timer;
            private DateTime m_End;

            public override bool BlocksFit { get { return true; } }

            public InternalItemb(Point3D loc, Map map, Mobile caster)
                : base(0x1356)
            {
                Visible = false;
                Movable = false;

                MoveToWorld(loc, map);

                if (caster.InLOS(this))
                    Visible = true;
                else
                    Delete();

                if (Deleted)
                    return;

                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(30.0));
                m_Timer.Start();

                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0);
            }

            public InternalItemb(Serial serial)
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
                private InternalItemb m_Item;

                public InternalTimer(InternalItemb item, TimeSpan duration)
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
            private LureStoneSpell m_Owner;

            public InternalTarget(LureStoneSpell owner)
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
    }
}
