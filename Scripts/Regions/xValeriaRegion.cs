using System;
using System.Xml;
using Server;
using Server.Spells;
using Server.Mobiles;

namespace Server.Regions
{
	public class Valeria : BaseRegion
	{
		public Valeria( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
		}
     
                public override void OnEnter( Mobile from )
		{
		from.SendMessage( 0x35, "Twoim oczom ukazuje sie stolica Valeria!" );
                }

		public override void OnExit( Mobile from )
		{
		from.SendMessage( 0x35, "Opusciles Valerie!" );
                }

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}



	}
}