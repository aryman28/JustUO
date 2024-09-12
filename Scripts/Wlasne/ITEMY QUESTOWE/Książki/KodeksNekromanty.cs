using System;
using Server;

namespace Server.Items
{
	public class KodeksNekromanty : BlueBook
	{
		[Constructable]
		public KodeksNekromanty() : base( "Kodeks Nekromanty", "skryba Vega ", 11, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
		int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"OTWIERASZ KSI�GE I", 
				"ZACZYNASZ CZYTA�", 
				"ROZMAZANE STROFY",
                                "",
				"Kodeks Martwego Porz�dku",
				"I.W zamian za dar", 
				"nie�miertelno�ci wyznawca", 
				"winien co 7 noc sk�ada� w", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ofierze �mierteln� dusz�", 
				"zwierz�cia lub istoty", 
				"ludzkiej.",
				"II.W zamian za dar", 
				"nie�miertelnej magii", 
				"wyznawca winien", 
				"zrezygnowa� z nietrwa�ej", 
				"magi �miertelnej.",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"III.W zamian za dar", 
				"rozumienia dusz wyznawca", 
				"winien nie rozmawia� z", 
				"istotami �miertelnymi.",
				"IV.W zamian za dar", 
				"m�dro�ci kultysta winien", 
				"co 30-t� noc po�wi�ci�", 
				"na zadume i mod�y.",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"V.W zamian za dar", 
				"braku b�lu winien wierny", 
				"czci� symbol pentagramu.",
				"VI.W zamian za dar", 
				"�wi�tej sk�ry wyznawca", 
				"winien unika� kontaktu", 
				"ze �wiat�em.",
				"VII.W zamian za dar", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"o�ywiania wierny winien", 
				"posiada� przy sobie", 
				"Mroczny Grimmur.",
				"VIII.W zamian za dar", 
				"rzucania zakl�� magii", 
				"�mierci wierny winien", 
				"rzuca� je z rozwag�.",
				"IX.W zamian za dar", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"u�miercania wierny winien", 
				"u�mierca� co �miertelne.",
				"X.W zamian za dar bycia", 
				"nekromant�, wyznawca", 
				"winien czci� kodeks.",
                                "",
				"*DOSTRZEGASZ DOPISKI", 
				"POWSTA�E POZNIEJ*",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Prawda o zapomnianym",
				"kulcie Martwego Porz�dku.",
                                "",				
				"Kiedy ju� magia by�a", 
				"kunsztem powszechnym", 
				"w�r�d brudu �miertelnych,", 
				"kt�rzy stracili szacunek", 
				"dla swych plugawych �y�,", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"morduj�c si� nawzajem i", 
				"niszcz�c wszystko", 
				"na swej drodze.",
				"Dzieci ladacznic, sieroty", 
				"falszywej wiary, paladyni,", 
				"nios�c falszywy �ad i", 
				"porz�dek oparty na", 
				"k�amliwej wierze,", 
			};				
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"przekonywali si�� o z�ych", 
				"skutkach p�yn�cych z", 
				"w�adania magi�.",
				"Po wiekach ich dzia�ania", 
				"wyniszczy�y prawie", 
				"zakony magii, osta�y si�", 
				"tylko dwa: mag�w �ywio�u,", 
				"zdrajc�w kt�rzy weszli w", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"plugawy sojusz z", 
				"paladynami i nasz kult", 
				"Martwego Porz�dku.",
				"Ta zgraja �miertelnych", 
				"b�azn�w szybko wzie�a", 
				"nas za cel ich heretyckiej", 
				"ekspansi, co skutkowa�o", 
				"wiekami walk.",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"U progu ich zwyci�stwa",  
				"postanowili�my ukry�", 
				"si� w morokach czasu", 
				"aby ponownie powsta�",
				"gdy nadejdzie pora.", 
			}; 
			Pages[cnt++].Lines = lines;
		}

		public KodeksNekromanty( Serial serial ) : base( serial )
		{
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int)0 ); // version
		}
	}
}