using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using Server.Items;
using Server.Multis;
using Server.ContextMenus;

namespace Server.ACC.YS
{

/////////YardIronGate : IronGate
	public class YardIronGate : IronGate
	{
		public Mobile m_Placer;
		public int m_Value = 0;
                private BaseHouse m_House;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

	[Constructable]
        public YardIronGate(int itemID, Mobile from, int price, BaseHouse house, Point3D loc, DoorFacing facing) : base(facing)
	{
	m_Value = price;
	m_Placer = from;
	Movable = false;
	MoveToWorld( loc, from.Map );
	Name = from.Name + "(Bramka)";

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
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

		public override void Use( Mobile from )
		{
			if( ((BaseDoor)this).Locked && from == this.m_Placer )
			{
				((BaseDoor)this).Locked = false;
				from.SendMessage("Szybko otworzyleœ bramke wejdz, i zamknij za sob¹!");
				base.Use(from);
				((BaseDoor)this).Locked = true;
			}
			else if( ((BaseDoor)this).Locked && from != this.m_Placer )
			{
				from.SendMessage("Jesteœ nieproszonym goœciem.  Odejdz st¹d!");
			}
			else
			{
				base.Use(from);
			}
		}

                public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries( from, list );
			if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
			{
				list.Add( new YardSecurityEntry( from, (BaseDoor)this ));
				list.Add( new RefundEntry( from, (BaseDoor)this, m_Value ));
			}
		}

		public YardIronGate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_Placer );
			writer.Write( m_Value );

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

		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Placer = reader.ReadMobile();
			m_Value = reader.ReadInt();

            if (reader.ReadBool())
            {
                House = reader.ReadItem() as BaseHouse;
            }

            if (House == null)
            {
                FindHouseOfPlacer();
                if (House == null)
                {
                    Delete();
                }
            }

    }
}
/////////YardIronGate : IronGate

/////////YardShortIronGate : IronGateShort
	public class YardShortIronGate : IronGateShort
	{
		public Mobile m_Placer;
		public int m_Value = 0;
                private BaseHouse m_House;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

	[Constructable]
        public YardShortIronGate(int itemID, Mobile from, int price, BaseHouse house, Point3D loc, DoorFacing facing) : base(facing)
	{
	m_Value = price;
	m_Placer = from;
	Movable = false;
	MoveToWorld( loc, from.Map );
	Name = from.Name + "(Bramka)";

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
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

		public override void Use( Mobile from )
		{
			if( ((BaseDoor)this).Locked && from == this.m_Placer )
			{
				((BaseDoor)this).Locked = false;
				from.SendMessage("Szybko otworzyleœ bramke, wejdz i zamknij za sob¹!");
				base.Use(from);
				((BaseDoor)this).Locked = true;
			}
			else if( ((BaseDoor)this).Locked && from != this.m_Placer )
			{
				from.SendMessage("Jesteœ nieproszonym goœciem.  Odejdz st¹d!");
			}
			else
			{
				base.Use(from);
			}
		}

                public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries( from, list );
			if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
			{
				list.Add( new YardSecurityEntry( from, (BaseDoor)this ));
				list.Add( new RefundEntry( from, (BaseDoor)this, m_Value ));
			}
		}

		public YardShortIronGate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_Placer );
			writer.Write( m_Value );

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

		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Placer = reader.ReadMobile();
			m_Value = reader.ReadInt();

            if (reader.ReadBool())
            {
                House = reader.ReadItem() as BaseHouse;
            }

            if (House == null)
            {
                FindHouseOfPlacer();
                if (House == null)
                {
                    Delete();
                }
            }

    }
}
/////////YardShortIronGate : IronGateShort

/////////YardLightWoodGate : LightWoodGate
	public class YardLightWoodGate : LightWoodGate
	{
		public Mobile m_Placer;
		public int m_Value = 0;
                private BaseHouse m_House;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

	[Constructable]
        public YardLightWoodGate(int itemID, Mobile from, int price, BaseHouse house, Point3D loc, DoorFacing facing) : base(facing)
	{
	m_Value = price;
	m_Placer = from;
	Movable = false;
	MoveToWorld( loc, from.Map );
	Name = from.Name + "(Bramka)";

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
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

		public override void Use( Mobile from )
		{
			if( ((BaseDoor)this).Locked && from == this.m_Placer )
			{
				((BaseDoor)this).Locked = false;
				from.SendMessage("Szybko otworzyleœ bramke, wejdz i zamknij za sob¹!");
				base.Use(from);
				((BaseDoor)this).Locked = true;
			}
			else if( ((BaseDoor)this).Locked && from != this.m_Placer )
			{
				from.SendMessage("Jesteœ nieproszonym goœciem.  Odejdz st¹d!");
			}
			else
			{
				base.Use(from);
			}
		}

                public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries( from, list );
			if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
			{
				list.Add( new YardSecurityEntry( from, (BaseDoor)this ));
				list.Add( new RefundEntry( from, (BaseDoor)this, m_Value ));
			}
		}

		public YardLightWoodGate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_Placer );
			writer.Write( m_Value );

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

		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Placer = reader.ReadMobile();
			m_Value = reader.ReadInt();

            if (reader.ReadBool())
            {
                House = reader.ReadItem() as BaseHouse;
            }

            if (House == null)
            {
                FindHouseOfPlacer();
                if (House == null)
                {
                    Delete();
                }
            }

    }
}
/////////YardLightWoodGate : LightWoodGate

/////////YardDarkWoodGate : DarkWoodGate
	public class YardDarkWoodGate : DarkWoodGate
	{
		public Mobile m_Placer;
		public int m_Value = 0;
                private BaseHouse m_House;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

	[Constructable]
        public YardDarkWoodGate(int itemID, Mobile from, int price, BaseHouse house, Point3D loc, DoorFacing facing) : base(facing)
	{
	m_Value = price;
	m_Placer = from;
	Movable = false;
	MoveToWorld( loc, from.Map );
	Name = from.Name + "(Bramka)";

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
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

		public override void Use( Mobile from )
		{
			if( ((BaseDoor)this).Locked && from == this.m_Placer )
			{
				((BaseDoor)this).Locked = false;
				from.SendMessage("Szybko otworzyleœ bramke, wejdz i zamknij za sob¹!");
				base.Use(from);
				((BaseDoor)this).Locked = true;
			}
			else if( ((BaseDoor)this).Locked && from != this.m_Placer )
			{
				from.SendMessage("Jesteœ nieproszonym goœciem.  Odejdz st¹d!");
			}
			else
			{
				base.Use(from);
			}
		}

                public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries( from, list );
			if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
			{
				list.Add( new YardSecurityEntry( from, (BaseDoor)this ));
				list.Add( new RefundEntry( from, (BaseDoor)this, m_Value ));
			}
		}

		public YardDarkWoodGate( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_Placer );
			writer.Write( m_Value );

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

		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			m_Placer = reader.ReadMobile();
			m_Value = reader.ReadInt();

            if (reader.ReadBool())
            {
                House = reader.ReadItem() as BaseHouse;
            }

            if (House == null)
            {
                FindHouseOfPlacer();
                if (House == null)
                {
                    Delete();
                }
            }

    }
}
/////////YardDarkWoodGate : DarkWoodGate
}