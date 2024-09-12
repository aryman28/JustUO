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
				"OTWIERASZ KSIÊGE I", 
				"ZACZYNASZ CZYTAÆ", 
				"ROZMAZANE STROFY",
                                "",
				"Kodeks Martwego Porz¹dku",
				"I.W zamian za dar", 
				"nieœmiertelnoœci wyznawca", 
				"winien co 7 noc sk³adaæ w", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ofierze œmierteln¹ duszê", 
				"zwierzêcia lub istoty", 
				"ludzkiej.",
				"II.W zamian za dar", 
				"nieœmiertelnej magii", 
				"wyznawca winien", 
				"zrezygnowaæ z nietrwa³ej", 
				"magi œmiertelnej.",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"III.W zamian za dar", 
				"rozumienia dusz wyznawca", 
				"winien nie rozmawiaæ z", 
				"istotami œmiertelnymi.",
				"IV.W zamian za dar", 
				"m¹droœci kultysta winien", 
				"co 30-t¹ noc poœwiêciæ", 
				"na zadume i mod³y.",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"V.W zamian za dar", 
				"braku bólu winien wierny", 
				"czciæ symbol pentagramu.",
				"VI.W zamian za dar", 
				"œwiêtej skóry wyznawca", 
				"winien unikaæ kontaktu", 
				"ze œwiat³em.",
				"VII.W zamian za dar", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"o¿ywiania wierny winien", 
				"posiadaæ przy sobie", 
				"Mroczny Grimmur.",
				"VIII.W zamian za dar", 
				"rzucania zaklêæ magii", 
				"œmierci wierny winien", 
				"rzucaæ je z rozwag¹.",
				"IX.W zamian za dar", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"uœmiercania wierny winien", 
				"uœmiercaæ co œmiertelne.",
				"X.W zamian za dar bycia", 
				"nekromant¹, wyznawca", 
				"winien czciæ kodeks.",
                                "",
				"*DOSTRZEGASZ DOPISKI", 
				"POWSTA£E POZNIEJ*",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Prawda o zapomnianym",
				"kulcie Martwego Porz¹dku.",
                                "",				
				"Kiedy ju¿ magia by³a", 
				"kunsztem powszechnym", 
				"wœród brudu œmiertelnych,", 
				"którzy stracili szacunek", 
				"dla swych plugawych ¿yæ,", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"morduj¹c siê nawzajem i", 
				"niszcz¹c wszystko", 
				"na swej drodze.",
				"Dzieci ladacznic, sieroty", 
				"falszywej wiary, paladyni,", 
				"nios¹c falszywy ³ad i", 
				"porz¹dek oparty na", 
				"k³amliwej wierze,", 
			};				
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"przekonywali si³¹ o z³ych", 
				"skutkach p³yn¹cych z", 
				"w³adania magi¹.",
				"Po wiekach ich dzia³ania", 
				"wyniszczy³y prawie", 
				"zakony magii, osta³y siê", 
				"tylko dwa: magów ¿ywio³u,", 
				"zdrajców którzy weszli w", 
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"plugawy sojusz z", 
				"paladynami i nasz kult", 
				"Martwego Porz¹dku.",
				"Ta zgraja œmiertelnych", 
				"b³aznów szybko wzie³a", 
				"nas za cel ich heretyckiej", 
				"ekspansi, co skutkowa³o", 
				"wiekami walk.",
			};
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 				
				"U progu ich zwyciêstwa",  
				"postanowiliœmy ukryæ", 
				"siê w morokach czasu", 
				"aby ponownie powstaæ",
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