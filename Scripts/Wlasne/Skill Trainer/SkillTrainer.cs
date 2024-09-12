#region AuthorHeader
//
//	Skill Trainer version 1.2, by Delphi
//
//
#endregion AuthorHeader

using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Commands;
using Server.Mobiles;

namespace Server.Mobiles
{//Begin Mobile Section
	[CorpseName( "zw³oki trenera" )]
	public class SkillTrainer : Mobile
	{
		[Constructable]
		public SkillTrainer()
		{
			//Change NPC characteristics, gear and stats in this section
			InitStats(100, 100, 100);

			Karma = 1000;
			Fame = 1000;
			Hue = Utility.RandomSkinHue(); 
			Body = 0x190;
			Blessed = true;//Invulnerable true or false?

			AddItem( new QuarterStaff() );
			AddItem( new StuddedArms() );
			AddItem( new StuddedLegs() );
			AddItem( new StuddedChest() );
			AddItem( new StuddedGloves() );
			AddItem( new StuddedGorget() );
			AddItem( new Sandals( Utility.RandomNeutralHue() ) );
			AddItem( new BodySash( Utility.RandomNeutralHue() ) );
			AddItem( new Robe( Utility.RandomNeutralHue() ) );
			AddItem( new Cloak( Utility.RandomNeutralHue() ) );
			Direction = Direction.South;
			Name = NameList.RandomName( "male" ); 
			Title = "- Trener"; 
			CantWalk = true;
			
			Item hair = new Item( 0x203C );
                hair.Hue = Utility.RandomHairHue();
                hair.Layer = Layer.Hair;
                hair.Movable = false;
                AddItem( hair );
			
		}

		public SkillTrainer( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
        
		
		//Uncomment section below to charge money to use the skilltrainer.
		
		public override bool OnDragDrop( Mobile from, Item dropped )
		{
			
			if (dropped is Gold && dropped.Amount == 4000)//How much gold does this cost the player?
			{
					from.SendGump( new SkillTrainerGump( from, from ) );
                    //from.SendGump( new StatTrainerGump( from ) ); 
					
					dropped.Delete();
					
					Say(String.Format("I thank thee {0}, what wouldst thou like to learn today?", from.Name));

					return true;
			}
			else
			{
					Say(String.Format("I will train thee {0}, for 4,000 gold coins!! The price is non negotiable!", from.Name));//If the gold amount dropped is not at least 10000 the Skill Trainer responds with this phrase.
					
					return false;
			}

		} 
		
		public override bool HandlesOnSpeech( Mobile from )
		{
			if ( from.InRange( this.Location, 12 ) )//How many tiles away can this mobile hear you talk?
				return true;

			return base.HandlesOnSpeech( from );
		}
		
		public override void OnSpeech( SpeechEventArgs e )
		{
			if ( !e.Handled && e.Mobile.InRange( this.Location, 4 ) )//How many tiles away can this mobile hear you talk?
			{
				if (e.Speech == "ucz")//Keyword
				{
					
					
					//e.Mobile.SendGump( new SkillTrainerGump( e.Mobile, e.Mobile ) );
					//e.Mobile.SendGump( new StatTrainerGump( e.Mobile ) );
					
					Say(String.Format("Za 4,000 sztuk z³ota nauczê ciê czego  tylko chcesz."));//What response does the skilltrainer have when they hear the keyword? (Use this line instead of above line when charge money is enabled)
				}
				
			}

			base.OnSpeech( e );
		}
	}
}//End Mobile Section


//Begin Gump Section
namespace Server.Gumps 
{ 
   public class SkillTrainerGump : Gump 
   { 
      private const int FieldsPerPage = 14; 

      private Mobile m_From; 
      private Mobile m_Mobile; 

      public SkillTrainerGump ( Mobile from, Mobile mobile ) : base ( 20, 30 ) 
      { 
         
		 this.Closable = false;
         this.Disposable = true;
         this.Dragable = true;
         this.Resizable = false;
		 
		 m_From = from; 
         m_Mobile = mobile; 

         AddPage ( 0 ); 
         AddBackground( 0, 0, 260, 375, 5054 ); 
         AddButton(9, 344, 4005, 4007, 0, GumpButtonType.Reply, 0);//Ok Button
         AddLabel(45, 344, 0, @"ZAMKNIJ");
		 AddImageTiled( 10, 10, 240, 23, 0x52 ); 
         AddImageTiled( 11, 11, 238, 21, 0xBBC ); 

         AddLabel( 45, 11, 0, "UMIEJÊTNOŒCI (0-40)" ); 

         AddPage( 1 ); 

         int page = 1; 

         int index = 0; 

         Skills skills = mobile.Skills; 

         int number; 
         
		 number = 3; 
            
         for ( int i = 0; i < ( skills.Length - number ); ++i ) 
         { 
            if ( index >= FieldsPerPage ) 
            { 
               AddButton( 231, 13, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1 ); 

               ++page; 
               index = 0; 

               AddPage( page ); 

               AddButton( 213, 13, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page - 1 ); 
            } 

            Skill skill = skills[i]; 
            AddImageTiled( 10, 32 + (index * 22), 240, 23, 0x52 ); 
            AddImageTiled( 11, 33 + (index * 22), 238, 21, 0xBBC ); 

            AddLabelCropped( 13, 33 + (index * 22), 150, 21, 0, skill.Name ); 
            AddImageTiled( 180, 34 + (index * 22), 50, 19, 0x52 ); 
            AddImageTiled( 181, 35 + (index * 22), 48, 17, 0xBBC ); 
            AddLabelCropped( 182, 35 + (index * 22), 234, 21, 0, skill.Base.ToString( "F1" ) ); 

            if ( from.AccessLevel >= AccessLevel.Player ) 
               AddButton( 231, 35 + (index * 22), 0x15E1, 0x15E5, i + 1, GumpButtonType.Reply, 0 ); 
            else 
               AddImage( 231, 35 + (index * 22), 0x2622 ); 

            ++index; 
         } 
      } 

      public override void OnResponse( NetState state, RelayInfo info ) 
      { 
              if ( info.ButtonID > 0 ) 
                    m_From.SendGump( new EditTrainerGump( m_From, m_Mobile, info.ButtonID - 1 ) ); 
      } 
   } 

   public class EditTrainerGump : Gump 
   { 
      private Mobile m_From; 
      private Mobile m_Mobile; 
      private Skill m_Skill; 

      public EditTrainerGump( Mobile from, Mobile mobile, int skillNo ) : base ( 20, 30 ) 
      { 
         m_From = from; 
         m_Mobile = mobile; 
         m_Skill = mobile.Skills[skillNo]; 

         if ( m_Skill == null ) 
            return; 

         AddPage ( 0 ); 

         AddBackground( 0, 0, 90, 60, 5054 ); 

         AddImageTiled( 10, 10, 72, 22, 0x52 ); 
         AddImageTiled( 11, 11, 70, 20, 0xBBC ); 
         AddTextEntry( 11, 11, 70, 20, 0, 0, m_Skill.Base.ToString( "F1" ) ); 
         AddButton( 15, 35, 0xFB7, 0xFB8, 1, GumpButtonType.Reply, 0 ); 
         AddButton( 50, 35, 0xFB1, 0xFB2, 0, GumpButtonType.Reply, 0 ); 
      } 

      public override void OnResponse( NetState state, RelayInfo info ) 
      { 
         if ( info.ButtonID > 0 ) 
         { 
            TextRelay text = info.GetTextEntry( 0 ); 

            if ( text != null ) 
            { 
               try 
               { 
                  double count = 0; 
                  for ( int i = 0; i < state.Mobile.Skills.Length; ++i ) 
                  { 
                     if ( m_Skill != state.Mobile.Skills[i] ) 
                        count = count + state.Mobile.Skills[i].Base; 
                  } 

                  double value = Convert.ToDouble( text.Text ); 
                  if ( value < 0.0 || value > 40 ) //Max raise for 1 skill
                  { 
                     /* state.Mobile.SendLocalizedMessage( 502452 ); // Value too high. 0-100 only */
					   state.Mobile.SendMessage( 0x26, "Mogê uczyæ umiejêtnoœci tylko w przedziale od 0 do 40" );//0x26 red hue
                  } 
                  else if ( ( count + value ) >  800 ) //Skillcap allowed
                  { 
                  state.Mobile.SendMessage( 0x26, "Osi¹gno³eœ/aœ maksimum swoich umiejêtnoœci. Nie mogê ciê ju¿ wiêcej mauczyæ" ); //0x26 red hue
                  } 
                  else 
                  { 
                     m_Skill.Base = value; 
                  } 
               } 
               catch 
               { 
                  state.Mobile.SendMessage( 0x26, "Z³y format. ###.# oczekuiwano" ); //0x26 red hue
               } 
            } 
         } 
         state.Mobile.SendGump( new SkillTrainerGump( m_From, m_Mobile ) ); 
      } 
   } 

   /*public class StatTrainerGump : Gump 
   { 
      public override void OnResponse( NetState state, RelayInfo info ) 
      { 
         Mobile from = state.Mobile; 

         switch ( info.ButtonID ) 
         { 
            case 1: // Change Strength 
            { 
               TextRelay text = info.GetTextEntry( 0 ); 

               if ( text.Text != "" ) 
               { 
                  try 
                  { 
                     int value = Convert.ToInt32( text.Text ); 
                     if ( value < 0 || value > 100 )//Max Strength
                     { 
                        //from.SendLocalizedMessage( 502452 ); // Value too high. 0-100 only 
                     from.SendAsciiMessage( 0x26, "Value too high. 0-100 only" ); //0x26 red hue
                     
                     }
                     else 
                     { 
                        if ( ( value + from.RawDex + from.RawInt ) >  from.StatCap ) 
                        { 
                           //from.SendLocalizedMessage( 1005629 ); // You can not exceed the stat cap.  Try setting another stat lower first. 
						   from.SendAsciiMessage( 0x26, "You can not exceed the stat cap.  Try setting another stat lower first." ); //0x26 red hue
                        } 
                        else 
                        { 
                           from.RawStr = value; 
                           from.Hits = from.HitsMax; 
                           //from.SendLocalizedMessage( 1005630 ); // Your stats have been adjusted. 
						   from.SendAsciiMessage( 0x59, "Your stats have been adjusted" ); //0x59 blue hue
                        } 
                     } 
                  } 
                  catch 
                  { 
                     //state.Mobile.SendMessage( "Bad format. ### expected" ); 
					 from.SendAsciiMessage( 0x26, "Bad format. ### expected" ); //0x26 red hue
                  } 
               } 

               from.SendGump( new StatTrainerGump( from ) ); 
               break; 
            } 

            case 2: // Change Dexterity 
            { 
               TextRelay text = info.GetTextEntry( 0 ); 

               if ( text.Text != "" ) 
               { 
                  try 
                  { 
                     int value = Convert.ToInt32( text.Text ); 
                     if ( value < 0 || value > 100 )//Max Dexterity
                     { 
                        //from.SendLocalizedMessage( 502452 ); // Value too high. 0-100 only 
                         from.SendAsciiMessage( 0x26, "Value too high. 0-100 only" ); //0x26 red hue
                     
                     }
                     else 
                     { 
                        if ( ( value + from.RawStr + from.RawInt ) >  from.StatCap ) 
                        { 
                           //from.SendLocalizedMessage( 1005629 ); // You can not exceed the stat cap.  Try setting another stat lower first. 
						   from.SendAsciiMessage( 0x26, "You can not exceed the stat cap.  Try setting another stat lower first." ); //0x26 red hue
                        } 
                        else 
                        { 
                           from.RawDex = value; 
                           from.Stam = from.StamMax; 
                           //from.SendLocalizedMessage( 1005630 ); // Your stats have been adjusted. 
						   from.SendAsciiMessage( 0x59, "Your stats have been adjusted" ); //0x59 blue hue
                        } 
                     } 
                  } 
                  catch 
                  { 
                     //state.Mobile.SendMessage( "Bad format. ### expected" ); 
					 from.SendAsciiMessage( 0x26, "Bad format. ### expected" ); //0x26 red hue
                  } 
               } 

               from.SendGump( new StatTrainerGump( from ) ); 
               break; 
            } 

            case 3: // Change Intelligence 
            { 
               TextRelay text = info.GetTextEntry( 0 ); 

               if ( text.Text != "" ) 
               { 
                  try 
                  { 
                     int value = Convert.ToInt32( text.Text ); 
                     if ( value < 0 || value > 100 ) //Max Intelligence
                     { 
                     //   from.SendLocalizedMessage( 502452 ); // Value too high. 0-100 only 
                         from.SendAsciiMessage( 0x26, " Value too high. 0-100 only" );
                     }
                     else 
                     { 
                        if ( ( value + from.RawDex + from.RawStr ) >  from.StatCap ) 
                        { 
                           //from.SendLocalizedMessage( 1005629 ); // You can not exceed the stat cap.  Try setting another stat lower first. 
						   from.SendAsciiMessage( 0x26, "You can not exceed the stat cap.  Try setting another stat lower first." );
                        } 
                        else 
                        { 
                           from.RawInt = value; 
                           from.Mana = from.ManaMax; 
                           //from.SendLocalizedMessage( 1005630 ); // Your stats have been adjusted. 
						   from.SendAsciiMessage( 0x59, "Your stats have been adjusted" );
                        } 
                     } 
                  } 
                  catch 
                  { 
                     //state.Mobile.SendMessage( "Bad format. ### expected" ); 
					 from.SendAsciiMessage( 0x26, "Bad format. ### expected" ); 
                  } 
               } 
               from.SendGump( new StatTrainerGump( from ) ); 
               break; 
            } 
         } 
      }*/ 

      //public StatTrainerGump( Mobile from ) : this( from, "" ) 
      //{ 
      //} 

      /*public StatTrainerGump(  Mobile from, string initialText ) : base( 100, 400 ) 
      { 
         this.Closable = false;
         this.Disposable = true;
         this.Dragable = true;
         this.Resizable = false;
		 
		 AddPage( 0 ); 

         AddBackground( 0, 0, 110, 126, 5054 ); 
		 AddButton(5, 100, 4005, 4007, 0, GumpButtonType.Reply, 0);//Ok button
		 AddLabel(38, 101, 0, @"CLOSE");

         AddPage( 1 ); 

         int line = 0; 
         int x = Convert.ToInt32( from.RawStr ); 
         string Str = Convert.ToString( x ); 
         int y = Convert.ToInt32( from.RawDex ); 
         string Dex = Convert.ToString( y ); 
         int z = Convert.ToInt32( from.RawInt ); 
         string Int = Convert.ToString( z ); 
         AddImageTiled( 10, 10 + ( line * 22), 90, 22, 0xBBC ); 
         AddImageTiled( 11, 10 + (line * 22) + 1, 88, 20, 0x2426 ); 
         AddTextEntry( 11, 10 + (line++ * 22) + 1, 88, 20, 0x480, 0, initialText ); 
         AddButton( 10, 10 + (line * 22), 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0 ); // Str 
         AddButton( (10 * 3) + 10, 10 + (line * 22), 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0 ); // Dex 
         AddButton( (10 * 6) + 10, 10 + (line++ * 22), 0xFA5, 0xFA7, 3, GumpButtonType.Reply, 0 ); // Int 
         AddLabel( 10, 10 + (line * 22), 0x481, "Str" ); 
         AddLabel( (10 * 3) + 10, 10 + (line * 22), 0x481, "Dex" ); 
         AddLabel( (10 * 6) + 10, 10 + (line++ * 22), 0x481, "Int" ); 
         AddLabel( 10, 10 + (line * 22), 0x481, Str ); 
         AddLabel( (10 * 3) + 10, 10 + (line * 22), 0x481, Dex ); 
         AddLabel( (10 * 6) + 10, 10 + (line++ * 22), 0x481, Int ); 
      }*/ 
   //} 
}
//End Gump Section
//End of Script