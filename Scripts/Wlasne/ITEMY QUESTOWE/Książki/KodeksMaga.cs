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
				"Otwierasz ksi�ge poczym", 
				"twoim oczom ukazuje si�", 
				"pradawne symbole, kt�re", 
				"poprzez magie zaczynaj�", 
				"nabiera� kszta�t�w", 
				"znanych ci liter, po", 
				"chwili jeste� w stanie", 
				"przeczyta� jej tre��.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"O istoto obdarowana",
				"darem my�lenia masz oto", 
				"kodeks Akademi", 
				"Magow �ywio��w, tre��",
				"kodeksu jest w istocie", 
				"spisem zasad, kt�re ka�dy", 
				"mag �ywio��w winien", 
				"przestrzega�.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"I.Ka�dy mag, czy dobry", 
				"czy z�y w swych czynach", 
				"winien jest szanowa�", 
				"istote magii i w miar�", 
				"mo�liwo�ci unika�", 
				"wywy�szania si� ni� nad", 
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
				"najsurowsz� zg��biania", 
				"tajemnic magii", 
				"martwego porz�dku",  
				",kto podejmie", 
				"si� takiego dzia�ania na", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"zawsze b�dzie wygnanym z",
				"kregu mag�w i nie dost�pi",
				"pot�gi �ywio��w.",
				"III.M�ody mag �ywio��w", 
				"przed u�ywaniem magii", 
				"zaawansowanej przeciwko", 
				"istotom �ywym lub w celu", 
				"przywo�ania sobie s�ug", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"moc� �ywio��w jest", 
				"zobowi�zany do", 
				"wcze�niejszego poznania",
				"tych zakle�, jest to", 
				"wynikiem jawnej grozby", 
				"jak� stanowi mo�liwo��", 
				"przywo�ania istoty", 
				"nieporz�danej.", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"IV.Zakazuje si�",
				"zakl�� o stopniu", 
				"zaawansowanym w miastach", 
				"i bend�dz w blisko�ci", 
				"bezstronnych istot", 
				"niemagicznych.",
				"V.Tajniki magii �ywio��w", 
				"podzielono na 8 kr�g�w.", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Dozwolone jest zg��bianie", 
				"bada� nad magia natury", 
				"magom �ywio��w.",
				"VI.Ka�dy z kolejno", 
				"nastepuj�cych po sobie", 
				"kr�g�w magii stanowi kr�g", 
				"zakle� trudniejszych",
				"wzgledem poprzedniego,", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ka�dy z nich ma swoj�", 
				"tajemn� nazwe kt�r� musi", 
				"zna� ka�dy mag stosuj�cy", 
				"dany kr�g.",
				"Kr�g 1 - zwany kr�giem", 
				"Podstawy �ywio��w",
				"Kr�g II - zwany kr�giem", 
				"�aski �ywio��w",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Kr�g III - zwany kr�giem",  
				"Pomocy �ywio��w",
				"Kr�g IV - zwany kr�giem", 
				"Obrony �ywio��w",
				"Kr�g V - zwany kr�giem",
				"Wtajemniczenia",
				"Kr�g VI - zwany kr�giem",
				"Zniszczenia",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Kr�g VII - zwany kr�giem", 
				"Mistrzowskim",
				"Kr�g VIII - zwany kr�giem",
				"Formy Najwy�szej �ywio�u", 
				"lub Arcymistrzowskim",
				"VII.Ka�dy kto stosuje", 
				"magi� �ywio��w robi to na", 
				"w�asne ryzyko i", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"odpowiedzialno��.",
				"VIII.Zakazuje si�", 
				"szkolenia w magi istot", 
				"nie�miertelnych",
				"i nieludzkich, w obawie",
				"przed skutkami takiego",
				"dzia�ania.",
				"IX.Kto nie jest cz�onkiem",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Akademii magii �ywio��w,",
				"a mimo to stosuje jej",
				"wszystkie lub niekt�re",
				"zaklecia , nie jest",
				"uznany za czynnego maga,",
				"uznany jest za maga", 
				"renegat lub", 
				"czarnoksi�nika.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"X.Opuszczenie Akademii",
				"jest zakazane.", 
				"Je�li osoba kt�ra odesz�a",
				"z Akademi chce powr�ci�", 
				"do niej,musi stana� przed", 
				"Rada Mag�w - ponien��",
				"pokut� i wykaza� skruch�.",
				"",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"XI.Magowie Akademii s�", 
				"zobowi�zani do poznawania", 
				"zasad, a tak�e do", 
				"stosowania si� do polece�", 
				"Rady Mag�w i Rektora.", 
				"Nieznajomo�� zasad i", 
				"rozporz�dze� nie",
				"uprawnia do ich", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nieprzestrzegania.",
				"XII.Wszelkie spory mi�dzy", 
				"magami powinny by�",
				"rozstrzygamy wewn�trz",
				"Akademii przy udziale",
				"kt�regokolwiek z",
				"cz�onkow Rady Mag�w.",
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