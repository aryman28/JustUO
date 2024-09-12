//   ___|========================|___
//   \  |  Written by Felladrin  |  /	This script was released on RunUO Community under the GPL licensing terms.
//    > |      February 2010     | < 
//   /__|========================|__\	[Age System] - Current version: 1.3 (April 7, 2013)

using System;
using Server.Items;
using Server.Prompts;
using Server.Mobiles;
using Server.Targeting;
using Server.Accounting;

namespace Server.Commands
{
	public class AgeCommands
	{
		//===== System Config =====//

		public static bool AutoRenewAgeEnabled = true; // Should the characters get older through time automatically?

		private static TimeSpan AutoRenewDelay = TimeSpan.FromDays( 15 ); // How many Earth Days are equivalent to One Year for characters?

		private static TimeSpan AutoRenewCheck = TimeSpan.FromMinutes( 30 ); // Check for new birthdays every 30 minutes.

		
		public static void Initialize() 
		{
			CommandSystem.Register( "VerifyAge", AccessLevel.Administrator, new CommandEventHandler( VerifyAge_OnCommand ) );
			CommandSystem.Register( "ClearAgeSystem", AccessLevel.Administrator, new CommandEventHandler( ClearAgeSystem_OnCommand ) );
			CommandSystem.Register( "SetAge", AccessLevel.Administrator, new CommandEventHandler( SetAge_OnCommand ) );
			CommandSystem.Register( "NewAge", AccessLevel.Administrator, new CommandEventHandler( NewAge_OnCommand ) );
			CommandSystem.Register( "Age", AccessLevel.Player, new CommandEventHandler( MyAge_OnCommand ) );
			
			if ( AutoRenewAgeEnabled )
			{
				new AutoRenewAgeTimer().Start();
			}


		}
		
		[Usage( "VerifyAge" )]
		[Description( "Checks the age of all characters, sends a warning to those who have not recorded their age, and shows statistics on the population's age." )]
		public static void VerifyAge_OnCommand( CommandEventArgs e )
		{
			int WithoutAge = 0, TotalCounted = 0, SumAges = 0, EldestCharAge = 0, YoungestCharAge = 100, Unreadable = 0;
			
			Console.WriteLine("--- Age System ---");
			Console.WriteLine("Postaci ktore nie maja ustalonego wieku:");
			
			foreach ( Mobile pm in World.Mobiles.Values )
			{
				if ( pm is PlayerMobile )
				{
					try
					{
						if ( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) == null || ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) == "")
						{
							if ( pm.Backpack == null )
							{
								pm.SendMessage( 33, "[Uwaga] Nie masz plecaka! Zglos to ekipie natychmiast!" );
								Console.WriteLine("- {0} (Account: {1}) [BRAK PLECAKA]", pm.Name, ((Account)pm.Account).Username);
							}
							else
							{
								Item AgeChangeDeed = pm.Backpack.FindItemByType( typeof( AgeChangeDeed ) );
					
								if ( AgeChangeDeed == null )
								{
									pm.SendMessage( Utility.RandomMinMax(2,600), "[UWAGA] Deed zmiany wieku zostal umieszczony w twoim plecaku. Uzyj go aby obnizyc wiek." );
									pm.AddToBackpack( new AgeChangeDeed() );
								}
								else
								{
									pm.SendMessage( Utility.RandomMinMax(2,600), "[UWAGA] Deed zmiany wieku zostal umieszczony w twoim plecaku. Uzyj go aby obnizyc wiek." );
								}
					
								Console.WriteLine("- {0} (Account: {1})", pm.Name, ((Account)pm.Account).Username);
							}
					
							WithoutAge++;
						}
						else
						{
							int age = int.Parse( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) );
					
							if ( age > EldestCharAge )
								EldestCharAge = age;
					
							if ( age < YoungestCharAge )
								YoungestCharAge = age;

							SumAges = SumAges + age;

							TotalCounted++;
						}
					}
					catch
					{
						Unreadable++; //The unreadable accounts are ignored to avoid server crash.
					}
				}
			}			
			
			if ( SumAges != 0 )
				e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "[Age System] Obecna populacja liczaca: {0} osob, posiada srednia wieku: {1} lat. Najwyzszy zanotowany wiek to {2} lat, a najnizszy to, {3} lat.", (TotalCounted + WithoutAge), (SumAges / TotalCounted), EldestCharAge, YoungestCharAge);
			else
				e.Mobile.SendMessage( 33, "[Age System] Wiek nie zostal przpisany." );
			
			if ( WithoutAge == 0 )
			{
				e.Mobile.SendMessage( 67, "[Age System] Wszystkie postaci maja wiek prawidlowo przypisany." );
				Console.WriteLine( "Wszystkie postaci maja prawidlowo przypisany wiek." );
			}
			else
			{
				e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "[Age System] {0} postaci nie ma przypisanego wieku. Otrzymali ostrzezenie aby przypisac wiek za pomoca deeda zmiany wieku w ich plecakach. Sprawdz konsole aby zobaczyc ich imiona.", WithoutAge );
				Console.WriteLine("Total: {0} postaci wymagajace odmlodzenia.", WithoutAge);
				if ( Unreadable != 0 )
					Console.WriteLine("Uwaga: {0} wykryto nieodczytywalne konta.", Unreadable);
			}
			
			Console.WriteLine("--- Age System ---");
		}
		
		[Usage( "ClearAgeSystem" )]
		[Description( "Removes all tags and items of the Age System from your shard. After that you can re-enable the system or delete the script from RunUO folder and restart the server." )]
		public static void ClearAgeSystem_OnCommand( CommandEventArgs e )
		{
			foreach ( Mobile pm in World.Mobiles.Values )
			{
				if ( pm is PlayerMobile )
				{
					try
					{
						if ( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) != null )
						{
							((Account)pm.Account).RemoveTag( "Age of " + (pm.RawName) );
							
							if ( pm.NameMod != null )
								pm.NameMod = null;
	
						}
					}
					catch
					{
					}
				}
			}
			
			System.Collections.ArrayList itemlist = new System.Collections.ArrayList();
			
			foreach ( Item item in World.Items.Values )
			{
				if ( item is AgeChangeDeed || item is RejuvenationPotion )
					itemlist.Add( item );
			}
			
			foreach (Item item in itemlist)
				item.Delete();			
			
			Misc.AutoSave.Save();
			
			e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "[Age System] Wszystkie tagi i przedmioty pochodzace z Age System zostaly usuniete z serwera." );
			e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "[Age System] Type [VerifyAge, jesli chcesz ponownie uruchomic system." );
			e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "[Age System] Usun skrypt z foldera Runuo, jesli nie chcesz juz nigdy z niego korzystac." );
		}

		[Usage( "SetAge" )]
		[Description( "Set the age of a character to the specified value." )]
		public static void SetAge_OnCommand( CommandEventArgs e )
		{ 		
			string NewAgeValue = e.ArgString;
			
			if ( NewAgeValue != null && NewAgeValue.Length >= 1 && System.Text.RegularExpressions.Regex.IsMatch( NewAgeValue, @"^[0-9]+$" ) )
			{
				e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "Wskaz postac ktorej chcesz dac {0} lat.", NewAgeValue );
				e.Mobile.Target = new SetAgeTarget( NewAgeValue );
			}	
			else
				e.Mobile.SendMessage("Usage: [SetAge <PositiveNumber>");
		}
	
		private class SetAgeTarget : Target
		{
			string ReceivedAge;
		
			public SetAgeTarget( string NewAgeValue ) : base( -1, false, TargetFlags.None )
			{
				ReceivedAge = NewAgeValue;
			}

			protected override void OnTarget( Mobile from, object targeted ) 
			{
				if ( targeted is PlayerMobile )
				{
					((Mobile)targeted).SendMessage( Utility.RandomMinMax(2,600), "Twoj wiek zostal zmieniony przez {0}. Masz teraz {1} lat.", from.Name, ReceivedAge );					
					((Account)((Mobile)targeted).Account).SetTag( "Age of " + (((Mobile)targeted).RawName), ReceivedAge );
					((Account)((Mobile)targeted).Account).SetTag( "LastRenewAge " + (((Mobile)targeted).RawName), (DateTime.Now).ToString() );
					from.SendMessage( Utility.RandomMinMax(2,600), "Wiek {0} zostal zmieniony pomyslnie.", ((Mobile)targeted).Name );
				}
				else
					from.SendMessage( Utility.RandomMinMax(2,600), "Mozesz zmienic tylko wiek postaci graczy." );
			}		
		}
		
		[Usage( "NewAge" )]
		[Description( "Makes all characters become one year older." )]
		public static void NewAge_OnCommand( CommandEventArgs e )
		{
			foreach ( Mobile pm in World.Mobiles.Values )
			{
				if ( pm is PlayerMobile )
				{
					try
					{
						if ( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) == null || ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) == "")
						{
							//Ignore them. To make a check on the server and adjust the characters who have not recorded their age yet use the comand [VerifyAge.
						}
						else
						{
							int age = int.Parse( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) );
							((Account)pm.Account).SetTag( "Age of " + (pm.RawName), (age + 1).ToString() );
							pm.SendMessage( Utility.RandomMinMax(2,600), "Gratulacje! masz teraz {0} lat!", (age + 1) );
						}
					}
					catch
					{
					}
				}
			}
			
			e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "Gotowe. Wszystkie postaci sa teraz 1 rok starsze. Uzyj [VerifyAge aby zobaczyc statystyki." );
		}

		[Usage( "Age" )]
		[Description( "Say your age and let others know about it. (Toggle age being shown in your name)" )]
		public static void MyAge_OnCommand( CommandEventArgs e )
		{
			if ( ((Account)e.Mobile.Account).GetTag( "Age of " + (e.Mobile.RawName) ) == null || ((Account)e.Mobile.Account).GetTag( "Age of " + (e.Mobile.RawName) ) == "")
			{
				if ( e.Mobile.Backpack == null )
				{
					e.Mobile.SendMessage( 33, "[Uwaga] Nie masz plecaka! Zglos to ekipie natychmiast!" );
					Console.WriteLine("[Age System: Uwaga] Postac '{0}' (Account: {1}) nie ma plecaka.", e.Mobile.Name, ((Account)e.Mobile.Account).Username);
				}
				else
				{
					Item AgeChangeDeed = e.Mobile.Backpack.FindItemByType( typeof( AgeChangeDeed ) );
					
					if ( AgeChangeDeed == null )
					{
						e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "Nie ustaliles swojego wieku. Uzyj deeda zmiany wieku ktory znajduje sie w twym plecaku." );
						e.Mobile.AddToBackpack( new AgeChangeDeed() );
					}
					else
					{
						e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "Nie ustaliles swojego wieku. Uzyj deeda zmiany wieku ktory znajduje sie w twym plecaku." );
					}
				}
			}
			else
			{
				if ( e.Mobile.NameMod == null )
				{
					e.Mobile.Say( "Mam {0} Lat.", ((Account)e.Mobile.Account).GetTag( "Age of " + (e.Mobile.RawName) ) );
					//e.Mobile.NameMod = e.Mobile.Name + " [Age: " + ((Account)e.Mobile.Account).GetTag( "Age of " + (e.Mobile.RawName) ) + "]";
					//e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "Twoj wiek zostal pokazany w twojej nazwie. Wszyscy poznaja teraz twoj wiek." );
				}
				else
				{
					e.Mobile.Whisper( "*Mam {0} Lat*", ((Account)e.Mobile.Account).GetTag( "Age of " + (e.Mobile.RawName) ) );
					//e.Mobile.NameMod = null;
					//e.Mobile.SendMessage( Utility.RandomMinMax(2,600), "Twoj wiek zostal usuniety z twojej nazwy." );
				}
			}
		}
		
		public class AutoRenewAgeTimer : Timer
		{
			public AutoRenewAgeTimer() : base( AutoRenewCheck, AutoRenewCheck )
			{
				Priority = TimerPriority.OneMinute;
			}

			protected override void OnTick()
			{
				foreach ( Mobile pm in World.Mobiles.Values )
				{
					if ( pm is PlayerMobile )
					{
						try
						{
							if ( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) == null || ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) == "" || ((Account)pm.Account).GetTag( "LastRenewAge " + (pm.RawName) ) == null )
							{
								//Ignore them.
							}
							else
							{
								DateTime LastRenew = DateTime.Parse( ((Account)pm.Account).GetTag( "LastRenewAge " + (pm.RawName) ) );
								
								if ( DateTime.Now > (LastRenew + AutoRenewDelay)  )
								{
									int age = int.Parse( ((Account)pm.Account).GetTag( "Age of " + (pm.RawName) ) );
									((Account)pm.Account).SetTag( "Age of " + (pm.RawName), (age + 1).ToString() );
									((Account)pm.Account).SetTag( "LastRenewAge " + (pm.RawName), (DateTime.Now).ToString() );
									pm.SendMessage( Utility.RandomMinMax(2,600), "Dzis masz urodziny! Masz teraz {0} lat! Gratulacje!", (age + 1) );
								}	
							}
						}
						catch
						{
						}
					}
				}
			}
		}
	}
}

namespace Server.Items
{
	public class AgeChangeDeed : Item
	{ 
		[Constructable] 
		public AgeChangeDeed() : base( 0x14F0 )
		{ 
			Name = "Deed zmiany wieku";
			Movable = false;
		}

		public override void GetProperties( ObjectPropertyList list )
		{
			list.Add( this.Name );
			list.Add( "Ile masz lat?" );
		}

		public override void OnDoubleClick( Mobile from ) 
		{
			if ( IsChildOf( from.Backpack ) )
			{
				from.SendMessage( Utility.RandomMinMax(2,600), "Ustal startowy wiek swojej postaci. Musi to byc miedzy 18 a 40." );
				from.Prompt = new ChooseAge( from );
				this.Delete();
			}
			else
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
		}

		private class ChooseAge : Prompt
		{ 
			public ChooseAge( Mobile from )
			{
			}

			public override void OnResponse( Mobile from, string number ) 
			{
				if ( !(System.Text.RegularExpressions.Regex.IsMatch(number, @"^[0-9]+$")) )
				{
					from.SendMessage( Utility.RandomMinMax(2,600), "Nie wyglupiaj sie wprowadz sama liczbe!" );
					from.AddToBackpack( new AgeChangeDeed() );
					return;
				}
				else if ( int.Parse(number) < 18 )
				{
					from.SendMessage( Utility.RandomMinMax(2,600), "Nie mozesz rozpoczac majac mniej niz 18 lat!" );
					from.AddToBackpack( new AgeChangeDeed() );
					return;
				}
				else if ( int.Parse(number) > 40 )
				{
					from.SendMessage( Utility.RandomMinMax(2,600), "Nie mozesz rozpoczac majac wiecej niz 40 lat!" );
					from.AddToBackpack( new AgeChangeDeed() );
					return;
				}
				else
				{
					((Account)from.Account).SetTag( "Age of " + (from.RawName), number );
					((Account)from.Account).SetTag( "LastRenewAge " + (from.RawName), (DateTime.Now).ToString() );
					from.SendMessage( Utility.RandomMinMax(2,600), "Twoj wiek zostal przypisany. Teraz masz {0} lat.", number );
				}	
			}

			public override void OnCancel( Mobile from )
			{
				from.AddToBackpack( new AgeChangeDeed() );
			}
		}
		
		public AgeChangeDeed( Serial serial ) : base( serial )
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
	}

	public class RejuvenationPotion : Item
	{
		[Constructable] 
		public RejuvenationPotion() : base( 0xF0D )
		{
			Name = "Mikstura Odmladzajaca";
		}
		
		public override void GetProperties( ObjectPropertyList list )
		{
			list.Add( this.Name );
			list.Add( "Zmniejsz swoj wiek" );
		}
		
		public override void OnDoubleClick( Mobile from ) 
		{
			if ( ((Account)from.Account).GetTag( "Age of " + (from.RawName) ) == null || ((Account)from.Account).GetTag( "Age of " + (from.RawName) ) == "")
			{
				from.SendMessage( 33, "Przed wypisciem tej mikstury musisz znac swoj wiek!" );
			}
			else
			{
				try
				{
					int age = int.Parse( ((Account)from.Account).GetTag( "Age of " + (from.RawName) ) );				
				
					if ( age < 23 )
					{
						from.SendMessage( Utility.RandomMinMax(2,600), "Pomysl lepiej, jesli wypijesz ta miksture moze cie ona zbytnio odmlodzic." );
					}
					else
					{
						((Account)from.Account).SetTag( "Age of " + (from.RawName), (age - Utility.RandomMinMax(1,5)).ToString() );
						this.Delete();
						from.SendMessage( Utility.RandomMinMax(2,600), "Wypiles miksture i czujesz sie mlodszy! Teraz masz {0} lat!", ((Account)from.Account).GetTag( "Age of " + (from.RawName) ) );
					}
				}
				catch
				{
					from.SendMessage( 33, "Twoj wiek nie jest przypisany jako liczba! Zglos to ekipie natychmiast!" );
				}
			}
		}
		
		public RejuvenationPotion( Serial serial ) : base( serial )
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
	}
}
