/**********************************************************************************************************************
Created by Ashlar, beloved of Morrigan

Version 1.0

Feel free to modify this script at will, please however include my credit line "Created by Ashlar, beloved of Morrigan"

CraftTrainer is an item/gump which allows players to raise their crafting skills without using resources 
	or producing masses of crafted items. 
***********************************************************************************************************************

Players must be within 2 tiles and have less than m_MaxSkill to use.

Players take m_Ouch damage to hits, stam, and mana for each train. (fatigue factor)

There is a delay of m_Studytime between train attempts.  

MinSkill and MaxSkill are setable in-game as well as Ouch and StudyTime.

During the StudyTime delay, the bool value Studying is set to true. While Studying is true, players 
	cannot doubleclick the item for a new gump.
*/

using System; 
using Server; 
using Server.Gumps; 
using Server.Network; 
using Server.Mobiles;
using Server.Items;

namespace Server.Items 
{    
	public class CraftTrainer : Item 
	{
		private StudyTimer m_StudyTimer;
		private Mobile m_From;
		private CraftTrainer m_Trainer;

		private int m_StudyTime;
		private int m_Ouch;
		private double m_MinSkill;
		private double m_MaxSkill;
		private bool m_Studying;

		[CommandProperty( AccessLevel.GameMaster )]
		public int StudyTime{ get{ return m_StudyTime; } set{ m_StudyTime = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Ouch{ get{ return m_Ouch; } set{ m_Ouch = value; InvalidateProperties(); } }

		[CommandProperty( AccessLevel.GameMaster )]
		public double MinSkill { get{ return m_MinSkill; } set{ m_MinSkill = value; }	}

		[CommandProperty( AccessLevel.GameMaster )]
		public double MaxSkill { get{ return m_MaxSkill; } set{ m_MaxSkill = value; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool Studying { get{ return m_Studying; } set{ m_Studying = value; } }

		[Constructable] 
		public CraftTrainer() : base( 0x1E25 ) 
		{
			Name = "Podręcznik szkoleniowy dla początkujących rzemieślników";
			Movable = true;  
         		LootType = LootType.Blessed;
         		Weight = 5;
			StudyTime = 10;
			Ouch = 5;
			MinSkill = -10.0;
			MaxSkill = 50.0;
			Studying = false;
		}

		[Constructable] 
		public CraftTrainer( int studyTime, int ouch, double minSkill, double maxSkill, bool studying) : base( 0x1E25 ) 
		{
			Name = "Podręcznik szkoleniowy dla początkujących rzemieślników";
			Movable = true;  
         		LootType = LootType.Blessed;
         		Weight = 5;
			StudyTime = studyTime;
			Ouch = ouch;
			MinSkill = minSkill;
			MaxSkill = maxSkill;
			Studying = studying;
		}

		public CraftTrainer( Serial serial ) : base( serial ) 
		{ 
		}

		public void UseCraftTrainer( Mobile from )
		{
			if ( !this.Studying )
				from.SendGump( new CraftTrainerGump( from, this ) );
			else
				from.SendMessage( "You must wait until your current studies are completed." );
		}

		public override void OnDoubleClick( Mobile from )
		{  
			UseCraftTrainer( from );
		}

		public override void Serialize( GenericWriter writer ) 
		{ 
			 base.Serialize( writer ); 
			 writer.Write( (int) 0 ); // version 

			writer.Write( (int) m_StudyTime);
			writer.Write( (int) m_Ouch);

			writer.Write( m_MinSkill );
			writer.Write( m_MaxSkill );

			writer.Write( (bool) m_Studying );
		} 

		public override void Deserialize( GenericReader reader ) 
		{
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 

			m_StudyTime = reader.ReadInt();
			m_Ouch = reader.ReadInt();

			m_MinSkill = reader.ReadDouble();
			m_MaxSkill = reader.ReadDouble();

			m_Studying = reader.ReadBool();
		}
	}
}

namespace Server.Items
{
	public class CraftTrainerGump : Gump 
	{ 
		private Mobile m_From;
		private CraftTrainer m_Trainer;

		public CraftTrainerGump( Mobile from, CraftTrainer trainer ) : base( 25,25 ) 
		  {	 
			m_From = from;
			m_Trainer = trainer;
			m_From.CloseGump( typeof( CraftTrainerGump ) );
				
         	AddPage( 0 ); 

			AddBackground( 50, 10, 425, 174, 5054 );
			AddImageTiled( 58, 20, 408, 155, 2624 );
			AddAlphaRegion( 58, 20, 408, 155 );

 			AddLabel( 75, 25, 88, "What do you want to study?");

         	AddButton( 75, 50, 4005, 4007, 1, GumpButtonType.Reply, 0 );
 			AddLabel( 125, 50, 0x486, "Blacksmith" ); 

         	AddButton( 75, 75, 4005, 4007, 2, GumpButtonType.Reply, 0 );
         	AddLabel( 125, 75, 0x486, "Drwalnictwo" ); 

         	AddButton( 75, 100, 4005, 4007, 3, GumpButtonType.Reply, 0 );
         	AddLabel( 125, 100, 0x486, "Krawiectwo" ); 

         	AddButton( 75, 125, 4005, 4007, 4, GumpButtonType.Reply, 0 );
         	AddLabel( 125, 125, 0x486, "Alchemia" ); 

         	AddButton( 75, 150, 4005, 4007, 5, GumpButtonType.Reply, 0 );
         	AddLabel( 125, 150, 0x486, "Inscription" ); 

         	AddButton( 275, 50, 4005, 4007, 6, GumpButtonType.Reply, 0 );
        	AddLabel( 325, 50, 0x486, "Gotowanie" ); 

         	AddButton( 275, 75, 4005, 4007, 7, GumpButtonType.Reply, 0 );
         	AddLabel( 325, 75, 0x486, "Lukmistrzostwo" ); 

         	AddButton( 275, 100, 4005, 4007, 8, GumpButtonType.Reply, 0 );
         	AddLabel( 325, 100, 0x486, "Cartography" ); 

         	AddButton( 275, 125, 4005, 4007, 9, GumpButtonType.Reply, 0 );
         	AddLabel( 325, 125, 0x486, "Majsterkowanie" ); 

         	AddButton( 275, 150, 4005, 4007, 10, GumpButtonType.Reply, 0 );
         	AddLabel( 325, 150, 0x486, "Zatruwanie" ); 
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{ 
			if ( m_Trainer.Deleted )
					return;

			else if ( info.ButtonID == 1 )
			{				
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Kowalstwo.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding blacksmithy");
						else
							m_From.SendMessage( "You turn to the blacksmithy section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Kowalstwo, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;
					}
				}
			}
			else if ( info.ButtonID == 2 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Drwalnictwo.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Drwalnictwo.");
						else
							m_From.SendMessage( "You turn to the Drwalnictwo section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Drwalnictwo, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 3 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Krawiectwo.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Krawiectwo.");
						else
							m_From.SendMessage( "You turn to the Krawiectwo section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Krawiectwo, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 4 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Alchemia.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Alchemia.");
						else
							m_From.SendMessage( "You turn to the Alchemia section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Alchemia, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 5 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Inskrypcja.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding inscription.");
						else
							m_From.SendMessage( "You turn to the inscription section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Inskrypcja, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 6 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Gotowanie.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Gotowanie.");
						else
							m_From.SendMessage( "You turn to the Gotowanie section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Gotowanie, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 7 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Lukmistrzostwo.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Lukmistrzostwo.");
						else
							m_From.SendMessage( "You turn to the Lukmistrzostwo section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Lukmistrzostwo, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 8 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )	
				{	
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Kartografia.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Kartografia");
						else
							m_From.SendMessage( "You turn to the cartography section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Kartografia, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 9 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )
				{		
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Majsterkowanie.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Majsterkowanie.");
						else
							m_From.SendMessage( "You turn to the Majsterkowanie section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Majsterkowanie, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
			else if ( info.ButtonID == 10 )
			{
				if ( !m_From.InRange( m_Trainer.GetWorldLocation(), 2 ) )
				{		
					m_From.SendMessage( "Your eyes are not quite up to the challenge, get a little closer." );
				}
				else
				{	
					if ( m_From.Hits <= m_Trainer.Ouch )
					{
						m_From.SendMessage( "You are too weak!");
					}
					else
					{
						new StudyTimer( m_From, m_Trainer.StudyTime, m_Trainer ).Start();
						if ( m_From.Skills.Zatruwanie.Base >= m_Trainer.MaxSkill )
							m_From.SendMessage( "You have mastered all that these books have to teach regarding Zatruwanie.");
						else
							m_From.SendMessage( "You turn to the Zatruwanie section of the books and study for a while." );
							m_From.CheckSkill( SkillName.Zatruwanie, m_Trainer.MinSkill, m_Trainer.MaxSkill );
							m_From.Hits = (m_From.Hits - m_Trainer.Ouch);
							m_From.Stam = (m_From.Stam - m_Trainer.Ouch);
							m_From.Mana = (m_From.Mana - m_Trainer.Ouch);
							m_Trainer.Studying = true;	
					}
				}
			}
		}
	}

	public class StudyTimer : Timer
	{
		private Mobile m_From;
		private CraftTrainer m_Trainer;

		public StudyTimer( Mobile from, int studyTime, CraftTrainer craftTrainer ) : base( TimeSpan.FromSeconds( studyTime ) )
		{
			m_From = from;
			m_Trainer = craftTrainer;
		}

		protected override void OnTick()
        {
            m_Trainer.Studying = false;	
			m_From.SendGump( new CraftTrainerGump( m_From, m_Trainer ) );
		}
	}
}
