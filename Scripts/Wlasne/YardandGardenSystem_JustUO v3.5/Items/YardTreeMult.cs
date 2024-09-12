using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.ACC.YS
{
	public class YardTreeMulti : Item
	{
		public ArrayList m_Components;
		public Mobile m_Placer;
		public int m_Value = 0;
                private BaseHouse m_House;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value;}
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Price
		{
			get{ return m_Value; }
			set{ m_Value = value;}
		}

//PIECES\\
		private class TreePiece : Item
		{
			YardTreeMulti IsPartOf;
			public TreePiece( int itemID, String name, YardTreeMulti ThisTree ) : base( itemID )
			{
				Movable = false;
				Name = name;
				IsPartOf=ThisTree;
			}

			public TreePiece( Serial serial ) : base( serial )
			{
			}

			public override void OnAfterDelete()
			{
				if(IsPartOf != null)
					IsPartOf.OnAfterDelete();
				else
					base.OnAfterDelete();
			}

			public override void OnDoubleClick( Mobile from )
	      {
				if(IsPartOf != null)
					IsPartOf.OnDoubleClick(from);
				else
					base.OnDoubleClick(from);
			}
			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 ); // version
				writer.Write( IsPartOf );
			}
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();
				switch (version)
				{
					case 0:
					{
						IsPartOf = reader.ReadItem() as YardTreeMulti;
						break;
					}
				}
			}
		}
//END PIECES\\

		[Constructable]
		public YardTreeMulti( Mobile from, string n, int price, BaseHouse house, int id, int lowrange, int highrange, Point3D loc )
		{
			m_Value = price;
			m_Placer = from;
			string name = "";
			name = from.Name + " "+n;
			Name = name;
			Movable = false;
			MoveToWorld( loc, from.Map );

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
            }

			m_Components = new ArrayList();

			ItemID = id;
			int lr = lowrange;
			int hr = highrange;

			while(lr > 0)
			{
				AddTreePiece( -lr, +lr, 0, id-lr, name, loc);
				lr--;
			}
			while(hr > 0)
			{
				AddTreePiece( +hr, -hr, 0, id+hr, name, loc);
				hr--;
			}

			m_Components.Add( this );
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

		private void AddTreePiece( int x, int y, int z, int itemID, string name, Point3D loc)
		{
			PlaceAndAdd( x, y, z, new TreePiece( itemID, name, this), loc );
		}

		private void PlaceAndAdd( int x, int y, int z, Item item, Point3D loc )
		{
			item.MoveToWorld( new Point3D( loc.X+x, loc.Y+y, loc.Z+z), m_Placer.Map );
			m_Components.Add( item );
		}


		public YardTreeMulti( Serial serial ) : base( serial )
		{
		}



		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_Placer );
			writer.Write( m_Value );
			writer.WriteItemList( m_Components );

            if (House == null || House.Deleted)
            {
                YardSystem.AddOrphanedItem(this);
            }

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch( version )
			{
				case 0:
				{
					m_Placer = reader.ReadMobile();
					m_Value = reader.ReadInt();
					m_Components = reader.ReadItemList();
					break;
				}
			}
		}

		public override void OnAfterDelete()
		{
			for ( int i = 0; i < m_Components.Count; ++i )
				((Item)m_Components[i]).Delete();
		}

		public override void OnDoubleClick( Mobile from )
	   {
			if( from.InRange( this.GetWorldLocation(), 10 ) )
			{
				if( this.Placer == null || from == this.Placer || from.AccessLevel >= AccessLevel.GameMaster )
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
