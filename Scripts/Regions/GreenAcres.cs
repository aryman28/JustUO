using System;
using System.Xml;
using Server.Spells.Rycerstwo;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;

namespace Server.Regions
{
    public class GreenAcres : BaseRegion
    {
        public GreenAcres(XmlElement xml, Map map, Region parent)
            : base(xml, map, parent)
        {
        }

        public override bool AllowHousing(Mobile from, Point3D p)
        {
            if (from.IsPlayer())
                return false;
            else
                return base.AllowHousing(from, p);
        }

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && m.IsPlayer())
            {
                m.SendMessage("You cannot cast that spell here.");
                return false;
            }
            else
            {
                return base.OnBeginSpellCast(m, s);
            }
        }
    }
}