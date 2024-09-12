//This script was heavily modified by Ashenfall of The Mystic Lands shard. Feel free to do with it what you wish, but as with all 
//Third-party scripts, make sure you understand what you are loading into your server BEFORE you load it in. 
using System; 
using Server; 
using Server.Factions;
using Server.Mobiles; 

namespace Server.Misc 
{ 
   public class SkillCheck 
   { 
      private const bool AntiMacroCode = false;      //Change this to false to disable anti-macro code 
      //Anti Macro code is designed so that players cannot just stand in one spot to gain skill...they must actively move about in order to recieve skillgains. 

      public static TimeSpan AntiMacroExpire = TimeSpan.FromMinutes( 1.0 ); //How long do we remember targets/locations? 
      public const int Allowance = 3;   //How many times may we use the same location/target for gain 
      private const int LocationSize = 2; //The size of eeach location, make this smaller so players dont have to move as far 
      private static bool[] UseAntiMacro = new bool[]
      { 
         // true if this skill uses the anti-macro code, false if it does not 
         false,// Alchemia = 0, 
         true,// Anatomia = 1, 
         true,// WiedzaOBestiach = 2, 
         true,// ItemID = 3, 
         false,// WiedzaOUzbrojeniu = 4, 
         false,// Parowanie = 5, 
         true,// Rolnictwo = 6, 
         false,// Blacksmith = 7, 
         false,// Lukmistrzostwo = 8, 
         true,// Uspokajanie = 9, 
         false,// Obozowanie = 10, 
         false,// Stolarstwo = 11, 
         false,// Kartografia = 12, 
         false,// Gotowanie = 13, 
         true,// Wykrywanie = 14, 
         true,// Manipulacja = 15, 
         false,// Intelekt = 16, 
         false,// Leczenie = 17, 
         true,// Rybactwo = 18, 
         true,// Kryminalistyka = 19, 
         true,// Zielarstwo = 20, 
         true,// Ukrywanie = 21, 
         true,// Prowokacja = 22, 
         false,// Inskrypcja = 23, 
         false,// Wlamywanie = 24, 
         false,// Magia = 25, 
         false,// ObronaPrzedMagia = 26, 
         false,// Taktyka = 27, 
         false,// Zagladanie = 28, 
         true,// Muzykowanie = 29, 
         true,// Zatruwanie = 30, 
         false,// Lucznictwo = 31, 
         true,// MowaDuchow = 32, 
         true,// Okradanie = 33, 
         false,// Krawiectwo = 34, 
         true,// Oswajanie = 35, 
         true,// OcenaSmaku = 36, 
         false,// Majsterkowanie = 37, 
         true,// Tropienie = 38, 
         false,// Weterynaria = 39, 
         false,// WalkaMieczami = 40, 
         false,// WalkaObuchami = 41, 
         false,// WalkaSzpadami = 42, 
         false,// Boks = 43, 
         false,// Drwalnictwo = 44, 
         false,// Gornictwo = 45, 
         false,// Medytacja = 46, 
         true,// Zakradanie = 47, 
         false,// UsuwaniePulapek = 48, 
         false,// Nekromancja = 49, 
         false,// Logistyka = 50, 
         false,// Rycerstwo = 51, 
         false,// Fanatyzm = 52,
	 false,//Skrytobojstwo = 53,
         true,//Druidyzm = 54,
         #region Stygian Abyss
         true, // Mistycyzm = 55,
         true, // Umagicznianie = 56,
         false// Rzucanie = 57
         #endregion
      }; 

      public static void Initialize() 
      { 
         Mobile.SkillCheckLocationHandler = new SkillCheckLocationHandler( Mobile_SkillCheckLocation ); 
         Mobile.SkillCheckDirectLocationHandler = new SkillCheckDirectLocationHandler( Mobile_SkillCheckDirectLocation ); 

         Mobile.SkillCheckTargetHandler = new SkillCheckTargetHandler( Mobile_SkillCheckTarget ); 
         Mobile.SkillCheckDirectTargetHandler = new SkillCheckDirectTargetHandler( Mobile_SkillCheckDirectTarget ); 

         SkillInfo.Table[0].GainFactor = .4;// Alchemia = 0 
         SkillInfo.Table[1].GainFactor = .3;// Anatomia = 1 
         SkillInfo.Table[2].GainFactor = .3;// WiedzaOBestiach = 2 
         SkillInfo.Table[3].GainFactor = .2;// ItemID = 3 
         SkillInfo.Table[4].GainFactor = .2;// WiedzaOUzbrojeniu = 4 
         SkillInfo.Table[5].GainFactor = .3;// Parowanie = 5 
         SkillInfo.Table[6].GainFactor = .3;// Rolnictwo = 6 - dawniej Beeging
         SkillInfo.Table[7].GainFactor = .5;// Blacksmith = 7 
         SkillInfo.Table[8].GainFactor = .4;// Lukmistrzostwo = 8 
         SkillInfo.Table[9].GainFactor = .4;// Uspokajanie = 9 
         SkillInfo.Table[10].GainFactor = .3;// Obozowanie = 10 
         SkillInfo.Table[11].GainFactor = .5;// Stolarstwo = 11 
         SkillInfo.Table[12].GainFactor = .4;// Kartografia = 12 
         SkillInfo.Table[13].GainFactor = .3;// Gotowanie = 13 
         SkillInfo.Table[14].GainFactor = .2;// Wykrywanie = 14 
         SkillInfo.Table[15].GainFactor = .5;// Manipulacja = 15 
         SkillInfo.Table[16].GainFactor = .3;// Intelekt = 16 
         SkillInfo.Table[17].GainFactor = .5;// Leczenie = 17 
         SkillInfo.Table[18].GainFactor = .4;// Rybactwo = 18 
         SkillInfo.Table[19].GainFactor = .5;// Kryminalistyka = 19 
         SkillInfo.Table[20].GainFactor = .5;// Zielarstwo = 20 - dawniej Herding
         SkillInfo.Table[21].GainFactor = .5;// Ukrywanie = 21 
         SkillInfo.Table[22].GainFactor = .3;// Prowokacja = 22 
         SkillInfo.Table[23].GainFactor = .3;// Inskrypcja = 23 
         SkillInfo.Table[24].GainFactor = .4;// Wlamywanie = 24 
         SkillInfo.Table[25].GainFactor = .4;// Magia = 25 
         SkillInfo.Table[26].GainFactor = .5;// ObronaPrzedMagia = 26 
         SkillInfo.Table[27].GainFactor = .3;// Taktyka = 27 
         SkillInfo.Table[28].GainFactor = .2;// Zagladanie = 28 
         SkillInfo.Table[29].GainFactor = .5;// Muzykowanie = 29 
         SkillInfo.Table[30].GainFactor = .6;// Zatruwanie = 30
         SkillInfo.Table[31].GainFactor = .4;// Lucznictwo = 31 
         SkillInfo.Table[32].GainFactor = .3;// MowaDuchow = 32 
         SkillInfo.Table[33].GainFactor = .5;// Okradanie = 33 
         SkillInfo.Table[34].GainFactor = .3;// Krawiectwo = 34 
         SkillInfo.Table[35].GainFactor = .6;// Oswajanie = 35 
         SkillInfo.Table[36].GainFactor = .2;// OcenaSmaku = 36 
         SkillInfo.Table[37].GainFactor = .5;// Majsterkowanie = 37 
         SkillInfo.Table[38].GainFactor = .5;// Tropienie = 38 
         SkillInfo.Table[39].GainFactor = .4;// Weterynaria = 39 
         SkillInfo.Table[40].GainFactor = .4;// WalkaMieczami = 40 
         SkillInfo.Table[41].GainFactor = .4;// WalkaObuchami = 41 
         SkillInfo.Table[42].GainFactor = .4;// WalkaSzpadami = 42 
         SkillInfo.Table[43].GainFactor = .4;// Boks = 43 
         SkillInfo.Table[44].GainFactor = .3;// Drwalnictwo = 44 
         SkillInfo.Table[45].GainFactor = .3;// Gornictwo = 45 
         SkillInfo.Table[46].GainFactor = .4;// Medytacja = 46 
         SkillInfo.Table[47].GainFactor = .6;// Zakradanie = 47 
         SkillInfo.Table[48].GainFactor = .5;// UsuwaniePulapek = 48 
         SkillInfo.Table[49].GainFactor = .4;// Nekromancja = 49 
         SkillInfo.Table[50].GainFactor = .01;// Logistyka = 50 
         SkillInfo.Table[51].GainFactor = .4;// Rycerstwo = 51 
         SkillInfo.Table[52].GainFactor = .4;// Fanatyzm = 52 
         SkillInfo.Table[53].GainFactor = .5;// Skrytobojstwo = 53 
         SkillInfo.Table[54].GainFactor = .5;// Druidyzm = 54 
         SkillInfo.Table[55].GainFactor = .5;// Mistycyzm = 55 
         SkillInfo.Table[56].GainFactor = .5;// Umagicznianie = 56 
         SkillInfo.Table[57].GainFactor = .5;// Rzucanie = 57
  
         //The following examples are here for you to use when trying to adjust stat gains for individual skills. 
         //You may add lines like these for as many skill sas you wish. 
         SkillInfo.Table[(int)SkillName.Muzykowanie].StrGain = 0;
         SkillInfo.Table[(int)SkillName.Muzykowanie].DexGain = 2;
         SkillInfo.Table[(int)SkillName.Muzykowanie].IntGain /= 2;

         SkillInfo.Table[(int)SkillName.Ukrywanie].StrGain = 0;
         SkillInfo.Table[(int)SkillName.Ukrywanie].DexGain = 2;
         SkillInfo.Table[(int)SkillName.Ukrywanie].IntGain /= 2;
	
	      SkillInfo.Table[(int)SkillName.Magia].StrGain = 0;
         SkillInfo.Table[(int)SkillName.Magia].DexGain = 0;
         SkillInfo.Table[(int)SkillName.Magia].IntGain = 3;
		
	      SkillInfo.Table[(int)SkillName.WalkaMieczami].StrGain = 2;
         SkillInfo.Table[(int)SkillName.WalkaMieczami].DexGain /= 2;
         SkillInfo.Table[(int)SkillName.WalkaMieczami].IntGain = 0;

         SkillInfo.Table[(int)SkillName.WalkaObuchami].StrGain = 2;
         SkillInfo.Table[(int)SkillName.WalkaObuchami].DexGain /= 2;
         SkillInfo.Table[(int)SkillName.WalkaObuchami].IntGain = 0;

         SkillInfo.Table[(int)SkillName.WalkaSzpadami].StrGain = 2;
         SkillInfo.Table[(int)SkillName.WalkaSzpadami].DexGain /= 2;
         SkillInfo.Table[(int)SkillName.WalkaSzpadami].IntGain = 0;

         SkillInfo.Table[(int)SkillName.Oswajanie].StrGain = 0;
         SkillInfo.Table[(int)SkillName.Oswajanie].DexGain = 0;
         SkillInfo.Table[(int)SkillName.Oswajanie].IntGain = 2;

         SkillInfo.Table[(int)SkillName.Druidyzm].StrGain = 0;
         SkillInfo.Table[(int)SkillName.Druidyzm].DexGain = 0;
         SkillInfo.Table[(int)SkillName.Druidyzm].IntGain = 3;

         SkillInfo.Table[(int)SkillName.MowaDuchow].StrGain = 0;
         SkillInfo.Table[(int)SkillName.MowaDuchow].DexGain = 0;
         SkillInfo.Table[(int)SkillName.MowaDuchow].IntGain = 4;

      } 

      public static bool Mobile_SkillCheckLocation( Mobile from, SkillName skillName, double minSkill, double maxSkill ) 
      { 
         Skill skill = from.Skills[skillName]; 

         if ( skill == null ) 
            return false; 

         double value = skill.Value; 

         if ( value < minSkill ) 
            return false; // Too difficult 
         else if ( value >= maxSkill ) 
            return true; // No challenge 

         double chance = (value - minSkill) / (maxSkill - minSkill); 

         Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize ); 
         return CheckSkill( from, skill, loc, chance ); 
      } 

      public static bool Mobile_SkillCheckDirectLocation( Mobile from, SkillName skillName, double chance ) 
      { 
         Skill skill = from.Skills[skillName]; 

         if ( skill == null ) 
            return false; 

         if ( chance < 0.0 ) 
            return false; // Too difficult 
         else if ( chance >= 1.0 ) 
            return true; // No challenge 

         Point2D loc = new Point2D( from.Location.X / LocationSize, from.Location.Y / LocationSize ); 
         return CheckSkill( from, skill, loc, chance ); 
      } 

      public static bool CheckSkill( Mobile from, Skill skill, object amObj, double chance ) 
      { 

 
	switch ( Utility.Random( 30 ))
	{
		case 0:
		{
			break;
		}
		case 1:
		{		
		if ( skill.Lock == SkillLock.Up && skill.Base > 25.0 )//The recieved a skillgain. Now let's see if they qualify for a stat gain. 
	         { 
	            SkillInfo info = skill.Info; 
	            double statcalc = (double) ( from.StatCap - from.RawStatTotal ) / from.StatCap;//Base difficulty factor for gaining a stat 
	            double statscalar = ( (1 - statcalc) * 3.5 ) + 1;// Sets an adjustment multiplier below. 

             
	            if ( from.StrLock == StatLockType.Up && (info.StrGain / 30.0) > ( ( Utility.RandomDouble() ) * statscalar ) ) 
	               GainStat( from, Stat.Str ); 
	            else if ( from.DexLock == StatLockType.Up && (info.DexGain / 30.0) > ( ( Utility.RandomDouble() ) * statscalar ) ) 
	               GainStat( from, Stat.Dex ); 
	            else if ( from.IntLock == StatLockType.Up && (info.IntGain / 30.0) > ( ( Utility.RandomDouble() ) * statscalar ) ) 
	               GainStat( from, Stat.Int ); 

		    
	         } 
		break;		
		}
		default:
		{
			break;
		}
	}

         if ( from.Skills.Cap == 0 ) 
            return false; 

         bool success = ( chance >= Utility.RandomDouble() );//Is their skill success chance greater than a random double? 
         double gc = (double)(from.Skills.Cap - from.Skills.Total) / from.Skills.Cap;//Base skillgain chance (based on total distance from skillcap) 
         //from.SendMessage( 0x35, "TOTAL Skills Base GC is {0}", gc ); // Debugging 
         gc += ( skill.Cap - skill.Base ) / skill.Cap;////Skillgain chance based on individual skill's distance from cap. 
         //from.SendMessage( 0x35, "Skill that was used added to Base GC is {0}", gc ); // Debugging 
         gc /= 3; //Returns the average of the two gain chance numbers. 
         //from.SendMessage( 0x35, "Average Base GC is {0}", gc ); // Debugging 

          
         gc += ( 1.0 - chance ) * ( success ? 0.5 : 0.1 );//"Chance" is defined later in the script. It is basically the difficulty factor of the skill being performed. 
         //This line modifies our Gain Chance by adding in numbers based on success or failure. As you can see, I've modified the default values. 
         //from.SendMessage( 0x35, "Success Modified GC is {0}", gc ); // Debugging 
          
         gc /= 3;   //Makes all skills harder to gain////////////BEST PLACE TO MAKE ADJUSTMENTS FOR OVERALL SKILLGAIN! 

         //gc /= 3;   //Makes all skills even harder to gain/////// 
         //gc /= 0.5;   //Makes all skills easier to gain//////////// 
         //from.SendMessage( 0x35, "Final BaseGC is {0}", gc ); // Debugging 
          

         gc *= skill.Info.GainFactor;   //Pulls the Gainfactor info from above section and multiplies the final gainchance by it. If not specified, it is always 1.0 
                  //from.SendMessage( 0x35, "GainFactor Adjusted GC is {0}", gc ); // Debugging 



                  if ( skill.Base > 25.0 ) 
                     gc /= 1; 

                    else if ( skill.Base > 40.0 ) 
                     gc /= 1.5; 
                    
                    else if ( skill.Base > 50.0 ) 
                     gc /= 2.5; 

                    else if ( skill.Base > 60.0 ) 
                     gc /= 3; 
                     
                    else if ( skill.Base > 70.0 ) 
                     gc /= 3.5; 

                    else if ( skill.Base > 75.0 ) 
                     gc /= 4; 

                    else if ( skill.Base > 80.0 ) 
                     gc /= 4.3; 

                    else if ( skill.Base > 85.0 ) 
                     gc /= 4.6; 
                     
                    else if ( skill.Base > 90.0 ) 
                     gc /= 4.8; 
                     
                    else if ( skill.Base > 95.0 ) 
                     gc /= 4.9; 
          
                    else if ( skill.Base > 98.0 ) 
                     gc /= 5; 


         //The above section was added for a more "progressive" skillgain level system. As you hit the above values, you should see skillgain get harder. 
         //Sphere users should be quite familiar with the effects of this. Sphere "difficulty breakpoints" were at 33.3 and 66.6 skill. 
          
         if ( gc < 0.01 ) 
            gc = 0.01;//This MUST be put in place to ensure that there is still a chance to gain even if the above calculations would otherwise make it impossible. 
             
         // from.SendMessage( 0x35, "FinalGC with skill level modifier is {0}", gc ); // Debugging 
          
         if ( from.Alive && ( ( gc >= Utility.RandomDouble() && AllowGain( from, skill, amObj ) ) ) ) 
            Gain( from, skill );//This line is what actually determines whether the mobile should gain skill or not. 
            //Note the || skill.Base < 10.0 part... It's so that people with less than 10 skill ALWAYS gain. 

         return success; 

      } 

      public static bool Mobile_SkillCheckTarget( Mobile from, SkillName skillName, object target, double minSkill, double maxSkill ) 
      { 
         Skill skill = from.Skills[skillName]; 

         if ( skill == null ) 
            return false; 

         double value = skill.Value; 

         if ( value < minSkill ) 
            return false; // Too difficult 
         else if ( value >= maxSkill ) 
            return true; // No challenge 

         double chance = (value - minSkill) / (maxSkill - minSkill);//Determines the "chance" factor for a difficulty the based gain system 

         return CheckSkill( from, skill, target, chance ); 
      } 

      public static bool Mobile_SkillCheckDirectTarget( Mobile from, SkillName skillName, object target, double chance ) 
      { 
         Skill skill = from.Skills[skillName]; 

         if ( skill == null ) 
            return false; 

         if ( chance < 0.0 ) 
            return false; // Too difficult 
         else if ( chance >= 1.0 ) 
            return true; // No challenge 

         return CheckSkill( from, skill, target, chance ); 
      } 

      private static bool AllowGain( Mobile from, Skill skill, object obj ) 
      { 
          if (Core.AOS && Faction.InSkillLoss(from))	//Changed some time between the introduction of AoS and SE.
              return false;

          #region SA
          if (from is PlayerMobile && from.Race == Race.Gargoyle && skill.Info.SkillID == (int)SkillName.Lucznictwo)
              return false;
          else if (from is PlayerMobile && from.Race != Race.Gargoyle && skill.Info.SkillID == (int)SkillName.Rzucanie)
              return false;
          #endregion

         if ( from is PlayerMobile && AntiMacroCode && UseAntiMacro[skill.Info.SkillID] ) 
            return ((PlayerMobile)from).AntiMacroCheck( skill, obj ); 
         else 
            return true; 

      } 

      public enum Stat { Str, Dex, Int } 

      public static void Gain( Mobile from, Skill skill )// This section actually applies the gained skill to the mobile. 
      { 
////Dodano////        
                        if ( from.Region is Regions.Jail )
				return;

                        if ( from.Region is Regions.MrocznaForteca )
				return;

			if ( from is BaseCreature && ((BaseCreature)from).IsDeadPet )
				return;

			if ( skill.SkillName == SkillName.Logistyka && from is BaseCreature )
				return;
////dodano////        
        
        if ( skill.Base < skill.Cap && skill.Lock == SkillLock.Up )// Under the cap and their skill lock is up? 
         { 
            int toGain = 1;//gain one point (0.1), please. 

            if ( skill.Base <= 10.0 )//Are they under 10.0 skill? 
               toGain = Utility.Random( 3 ) + 1;//Gain between one (0.1) and three (0.3) points, please. 

            Skills skills = from.Skills; 

            if ( ( skills.Total / skills.Cap ) >= Utility.RandomDouble() )//( skills.Total >= skills.Cap ) 
            //This section allows for negative gaining. (Skill lock down) 
            { 
               for ( int i = 0; i < skills.Length; ++i ) 
               { 
                  Skill toLower = skills[i]; 

                  if ( toLower != skill && toLower.Lock == SkillLock.Down && toLower.BaseFixedPoint >= toGain ) 
                  { 
                     toLower.BaseFixedPoint -= toGain; 
                     break; 
                  } 
               } 
            } 

            if ( (skills.Total + toGain) <= skills.Cap ) 
            { 
               skill.BaseFixedPoint += toGain;//apply the gain 
            } 
         } 

         
      } 

                public static bool CanLower( Mobile from, Stat stat )
		{
			switch ( stat )
			{
				case Stat.Str: return ( from.StrLock == StatLockType.Down && from.RawStr > 10 );
				case Stat.Dex: return ( from.DexLock == StatLockType.Down && from.RawDex > 10 );
				case Stat.Int: return ( from.IntLock == StatLockType.Down && from.RawInt > 10 );
			}

			return false;
		}

		public static bool CanRaise( Mobile from, Stat stat )
		{
			if ( !(from is BaseCreature && ((BaseCreature)from).Controlled) )
			{
				if ( from.RawStatTotal >= from.StatCap )
					return false;
			}

			switch ( stat )
			{
				case Stat.Str: return ( from.StrLock == StatLockType.Up && from.RawStr < 125 );
				case Stat.Dex: return ( from.DexLock == StatLockType.Up && from.RawDex < 125 );
				case Stat.Int: return ( from.IntLock == StatLockType.Up && from.RawInt < 125 );
			}

			return false;
		}

		public static void IncreaseStat( Mobile from, Stat stat, bool atrophy )
		{
			atrophy = atrophy || (from.RawStatTotal >= from.StatCap);

			switch ( stat )
			{
				case Stat.Str:
				{
					if ( atrophy )
					{
						if ( CanLower( from, Stat.Dex ) && (from.RawDex < from.RawInt || !CanLower( from, Stat.Int )) )
							--from.RawDex;
						else if ( CanLower( from, Stat.Int ) )
							--from.RawInt;
					}

					if ( CanRaise( from, Stat.Str ) )
						++from.RawStr;

					break;
				}
				case Stat.Dex:
				{
					if ( atrophy )
					{
						if ( CanLower( from, Stat.Str ) && (from.RawStr < from.RawInt || !CanLower( from, Stat.Int )) )
							--from.RawStr;
						else if ( CanLower( from, Stat.Int ) )
							--from.RawInt;
					}

					if ( CanRaise( from, Stat.Dex ) )
						++from.RawDex;

					break;
				}
				case Stat.Int:
				{
					if ( atrophy )
					{
						if ( CanLower( from, Stat.Str ) && (from.RawStr < from.RawDex || !CanLower( from, Stat.Dex )) )
							--from.RawStr;
						else if ( CanLower( from, Stat.Dex ) )
							--from.RawDex;
					}

					if ( CanRaise( from, Stat.Int ) )
						++from.RawInt;

					break;
				}
			}
		}

      private static TimeSpan m_StatGainDelay = TimeSpan.FromHours( 1.6 );//How long should a mobile have to wait to gain another stat point? 

      public static void GainStat( Mobile from, Stat stat ) 
      { 
         if ( (from.LastStatGain + m_StatGainDelay) >= DateTime.Now ) 
            return; 

         from.LastStatGain = DateTime.Now; 

         bool atrophy = ( (from.RawStatTotal / (double)from.StatCap) >= Utility.RandomDouble() ); 

         IncreaseStat( from, stat, atrophy ); 
      } 
   } 
} 
