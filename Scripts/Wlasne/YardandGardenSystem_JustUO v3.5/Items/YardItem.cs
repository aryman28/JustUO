using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Multis;

namespace Server.ACC.YS
{
	public class YardItem : Item
	{
		public Mobile m_Placer;
		public int m_Value = 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}
////
        private BaseHouse m_House;
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }
////
		[Constructable]
		public YardItem( Mobile from, int id, Point3D loc, int price, BaseHouse house)
		{
			m_Value = price;
			Movable = false;
			MoveToWorld( loc, from.Map );
			m_Placer = from;
			Name = from.Name + "(Dekoracja)";
			ItemID = id;
			Light = LightType.Circle150;
////
            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
            }
////
		}

		public YardItem( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{

            base.Serialize(writer);
            writer.Write((int)1); // version

            //Version 1
            if (House == null || House.Deleted)
            {
                writer.Write(false);
                YardSystem.AddOrphanedItem(this);
            }
            else
            {
                writer.Write(true);
                writer.Write(House);
            }

            //Version 0
	        writer.Write( m_Placer );
	        writer.Write( m_Value );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        if (reader.ReadBool())
                        {
                            House = reader.ReadItem() as BaseHouse;
                        }
                        goto case 0;
                    }
                case 0:
                    {
			m_Placer = reader.ReadMobile();
			m_Value = reader.ReadInt();

                        break;
                    }
             }
      }

        public void FindHouseOfPlacer()
        {
            if (Placer == null || House != null)
            {
                return;
            }

            IPooledEnumerable eable = Map.GetItemsInRange(Location, 20);
            foreach (Item item in eable)
            {
                if (item is BaseHouse)
                {
                    BaseHouse house = (BaseHouse)item;
                    if (house.Owner == Placer)
                    {
                        House = house;
                        return;
                    }
                }
            }
        }

      public override void OnDoubleClick( Mobile from )
      {
			if( from.InRange( this.GetWorldLocation(), 10 ) )
			{
				if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
				{
					Container c = m_Placer.Backpack;
					Gold t = new Gold( m_Value );
					if( c.TryDropItem( m_Placer, t, true ) )
					{
						this.Delete();
						m_Placer.SendMessage( "Ten przedmiot zostal usuniêty a monety zwrócono tobie" );
					}
					else
					{
						t.Delete();
						m_Placer.SendMessage("Z jakiegoœ powodu zwrot monet nie dzia³a! skontaktuj siê z ekip¹");
					}
				}
				else
				{
					from.SendMessage( "Wynocha z mojego ogrodu!" );
				}
			}
			else
			{
				from.SendMessage( "Ten przedmiot jest za daleko" );
			}
		}
	}
}