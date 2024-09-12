using System;
using Server;

namespace Server.Items
{
	public class KodeksMaga : BlueBook
	{
		[Constructable]
		public KodeksMaga() : base( "Kodeks Maga", "skryba Vega", 16, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
			int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"Otwierasz ksiêge poczym", 
				"twoim oczom ukazuje siê", 
				"pradawne symbole, które", 
				"poprzez magie zaczynaj¹", 
				"nabieraæ kszta³tów", 
				"znanych ci liter, po", 
				"chwili jesteœ w stanie", 
				"przeczytaæ jej treœæ.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"O istoto obdarowana",
				"darem myœlenia masz oto", 
				"kodeks Akademi", 
				"Magow ¯ywio³ów, treœæ",
				"kodeksu jest w istocie", 
				"spisem zasad, które ka¿dy", 
				"mag ¿ywio³ów winien", 
				"przestrzegaæ.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"I.Ka¿dy mag, czy dobry", 
				"czy z³y w swych czynach", 
				"winien jest szanowaæ", 
				"istote magii i w miarê", 
				"mo¿liwoœci unikaæ", 
				"wywy¿szania siê ni¹ nad", 
				"rozumnymi istotami", 
				"niemagicznymi, albowiem", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nic nawet magia nie", 
				"trwa wiecznie.",
				"II.Zakazuje sie pod kara", 
				"najsurowsz¹ zg³êbiania", 
				"tajemnic magii", 
				"martwego porz¹dku",  
				",kto podejmie", 
				"siê takiego dzia³ania na", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"zawsze bêdzie wygnanym z",
				"kregu magów i nie dost¹pi",
				"potêgi ¿ywio³ów.",
				"III.M³ody mag ¿ywio³ów", 
				"przed u¿ywaniem magii", 
				"zaawansowanej przeciwko", 
				"istotom ¿ywym lub w celu", 
				"przywo³ania sobie s³ug", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"moc¹ ¿ywio³ów jest", 
				"zobowi¹zany do", 
				"wczeœniejszego poznania",
				"tych zakleæ, jest to", 
				"wynikiem jawnej grozby", 
				"jak¹ stanowi mo¿liwoœæ", 
				"przywo³ania istoty", 
				"nieporz¹danej.", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"IV.Zakazuje siê",
				"zaklêæ o stopniu", 
				"zaawansowanym w miastach", 
				"i bend¹dz w bliskoœci", 
				"bezstronnych istot", 
				"niemagicznych.",
				"V.Tajniki magii ¿ywio³ów", 
				"podzielono na 8 krêgów.", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Dozwolone jest zg³êbianie", 
				"badañ nad magia natury", 
				"magom ¿ywio³ów.",
				"VI.Ka¿dy z kolejno", 
				"nastepuj¹cych po sobie", 
				"krêgów magii stanowi kr¹g", 
				"zakleæ trudniejszych",
				"wzgledem poprzedniego,", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ka¿dy z nich ma swoj¹", 
				"tajemn¹ nazwe któr¹ musi", 
				"znaæ ka¿dy mag stosuj¹cy", 
				"dany kr¹g.",
				"Kr¹g 1 - zwany krêgiem", 
				"Podstawy ¯ywio³ów",
				"Kr¹g II - zwany krêgiem", 
				"£aski ¿ywio³ów",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Kr¹g III - zwany krêgiem",  
				"Pomocy ¯ywio³ów",
				"Kr¹g IV - zwany krêgiem", 
				"Obrony ¯ywio³ów",
				"Kr¹g V - zwany krêgiem",
				"Wtajemniczenia",
				"Kr¹g VI - zwany krêgiem",
				"Zniszczenia",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Kr¹g VII - zwany krêgiem", 
				"Mistrzowskim",
				"Kr¹g VIII - zwany krêgiem",
				"Formy Najwy¿szej ¯ywio³u", 
				"lub Arcymistrzowskim",
				"VII.Ka¿dy kto stosuje", 
				"magiê ¿ywio³ów robi to na", 
				"w³asne ryzyko i", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"odpowiedzialnoœæ.",
				"VIII.Zakazuje siê", 
				"szkolenia w magi istot", 
				"nieœmiertelnych",
				"i nieludzkich, w obawie",
				"przed skutkami takiego",
				"dzia³ania.",
				"IX.Kto nie jest cz³onkiem",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Akademii magii ¿ywio³ów,",
				"a mimo to stosuje jej",
				"wszystkie lub niektóre",
				"zaklecia , nie jest",
				"uznany za czynnego maga,",
				"uznany jest za maga", 
				"renegat lub", 
				"czarnoksiê¿nika.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"X.Opuszczenie Akademii",
				"jest zakazane.", 
				"Jeœli osoba która odesz³a",
				"z Akademi chce powróciæ", 
				"do niej,musi stanaæ przed", 
				"Rada Magów - ponienœæ",
				"pokutê i wykazaæ skruchê.",
				"",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"XI.Magowie Akademii s¹", 
				"zobowi¹zani do poznawania", 
				"zasad, a tak¿e do", 
				"stosowania siê do poleceñ", 
				"Rady Magów i Rektora.", 
				"Nieznajomoœæ zasad i", 
				"rozporz¹dzeñ nie",
				"uprawnia do ich", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nieprzestrzegania.",
				"XII.Wszelkie spory miêdzy", 
				"magami powinny byæ",
				"rozstrzygamy wewn¹trz",
				"Akademii przy udziale",
				"któregokolwiek z",
				"cz³onkow Rady Magów.",
				"",
			}; 
			Pages[cnt++].Lines = lines;

		}

		public KodeksMaga( Serial serial ) : base( serial )
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