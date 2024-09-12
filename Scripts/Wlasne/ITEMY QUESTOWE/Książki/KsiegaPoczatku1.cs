using System;
using Server;

namespace Server.Items
{
	public class KsiegaPoczatku1 : BrownBook
	{
		[Constructable]
		public KsiegaPoczatku1() : base( "Ksiega Poczatku Tom 1", "nadworny skryba Blaz ", 19, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
		int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"Historia obecnego �wiata", 
				"okie�znanego tajemniczo�ci�", 
				"i dziko�ci� rozpocze�a", 
				"si� stu laty po upadku", 
				"wielkiego ksi�stwa Brytania.", 
				"Niegdy� pot�ne i bogate",
				"ciesz�cego si� s�aw�",
				"i spokojem.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Dzi� jedynie garstka ludzi", 
				"kt�ra niebawem wyzionie", 
				"ducha pami�ta �w czasy", 
				"nazywane czasem pustki", 
				"Kres wielkiemu ksi�stwu", 
				"przynios�a wielka wojna", 
				"domowa spowodowana", 
				"sporem o w�adze.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Po ci�kich walkach", 
				"w gruzach leg�y mury", 
				"najwspanialszych miast...", 
				"�niwo wojny zosta�o", 
				"spot�gowane przez zaraz�", 
				"kt�ra zabiera�a �ywym", 
				"istotom dech z piersi", 
				"przeobra�aj�c ca�e krainy", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"w pustkowia.", 
				"Przesiakni�ta krwi� ziemia", 
				"nie rodzi�a ju� plon�w.", 
				"�wczesne w�adze Brytanii", 
				"stane�y przed decyzj�", 
				"gdy kraine blasku s�o�ca", 
				"okry�y ciemne chmury", 
				"nios�c ze sob� z�aknionych", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ludzkimi duszami Ponurych", 
				"�niwiarzy.", 
				"Pastwi�y si� na poleg�ych", 
				"duszach ofiar niegodziwych.",
				"Przeobra�one w �miertelne",
				"pachn�ce siark� krainy", 
				"nie mog�y by� schronieniem", 
				"a niebezpieczenstwo", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ze strony plugawych istot ", 
				"pogr��a�y ocala�ych w ", 
				"smutku, �alu i rozpaczy.", 
				"W�adze �w spoczywa�y w ",
				"r�kach m�odej Valerii,",
				"kt�ra wywodzi�a si� z", 
				"krwi kr�lewskiego rodu", 
				"rz�dz�cego ksi�stwem.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"W obliczu nieub�aganej", 
				"zag�ady ksi�stwa i pewnej", 
				"�mierci jego mieszkanc�w", 
				"zadecydowa�a o kolonizacji",
				"nowych ziem...",
				"Ostatkami zapas�w", 
				"zbudowano wielk� flote", 
				"morsk�, kt�ra wyruszy�a", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"szlakiem pod opatrzno�ci�", 
				"�wiec�cych gwiazd by", 
				"osiedli� nowe tereny.", 
				"Podr� by�a surowym",
				"sprawdzianem ludzkiej",
				"wytrwa�o�ci i wiary...", 
				"Po wielu miesi�cach", 
				"w ko�cu odnaleziono l�d.", 
			};			
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Rozpoczeto budow�", 
				"pierwszej osady, kt�ra", 
				"nazwano Linhal.", 
				"�ycie w ma�ej wiosce",
				"szybko nabra�o t�pa,", 
				"tak wiec po up�yni�ciu", 
				"kilku wiosen ulice sta�y", 
				"si� przepe�nione ludzmi", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Handel i zbiory plon�w ", 
				"kwit�y wype�niaj�c ", 
				"spichlerze i niekt�re", 
				"sakwy mieszkanc�w po ", 
				"same brzegi.", 
				"Podj�to w�wczac decyzj�", 
				"o budowie stolicy,", 
				"kt�ra nazwano Valera", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"od imienia w�adczyni,", 
				"kt�rej to ludzie", 
				"zawdzi�czali ocalenie.", 
				"Dawne czasy odesz�y", 
				"ju� w zapomnienie...", 
				"kwesti� czasu by�a budowa", 
				"nowego Zakonu Rycerzy", 
				"kt�rego zadaniem by�o", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"zapewnienie bezpieczenstwa", 
				"mieszka�com stolicy.", 
				"Stolica zosta�a ", 
				"ufortyfikowana", 
				"a surowce na jej budow�", 
				"jak i powstajacy zakon", 
				"dostarczane by�y z nowo", 
				"odkrytego l�du na p�lnocy.", 
			}; 
			Pages[cnt++].Lines = lines; 
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Panowa�a tam sroga zima,", 
				"tak wi�c do pozyskiwania", 
				"i wydobywania surowc�w", 
				"wykorzystano si�� wiezni�w", 
				"i pojmanych ork�w,", 
				"kt�rzy zniewoleni pracowali", 
				"pod nadzorem zakonu.", 
				"Ukszta�towaly si� osady", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Barna i Guisula,", 
				"gdzie zapiecz�towany zosta�", 
				"los pojmanych jenc�w", 
				" i zbrodniarzy...", 
				"Czasy te jednak nie ", 
				"zawsze by�y sielankowe", 
				"jak opisuje historia.", 
				"Wiele razy populacja", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"przysz�ej Linhali i Valery", 
				"musia�a skrzy�owa� sw�j", 
				"or� z rdzennymi ", 
				"mieszkancami nowego ladu,", 
				"broniacymi zaciekle swoich ", 
				"teren�w.", 
				"Brak porozumienia stron", 
				"przyczynil sie do", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"krwawej rzezi tubylczych", 
				"plemion,kt�rych drewniane", 
				"w��cznie,strza�y,kamienie", 
				"by�y niczym w por�wnaniu", 
				"tarczami i stalowym ", 
				"or�em najezdzc�w.", 
				"Bior�ce udzia� w walce", 
				"plemiona zosta�y pokonane.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Jedyna ocala�a wioska ", 
				"byla Jorfa znajduj�ca sie ", 
				"na wyspie o tej samej ", 
				"nazwie, kt�ra podda�a sie", 
				"bez rozlewu krwi,", 
				"widz�c przerastaj�c� si�e", 
				"zbrojn� ludzi z po�udnia.", 
				"Jorfa mia�a wi�ksze", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"znaczenie dla stolicy", 
				"ani�eli inne plemiona.", 
				"Ludzie dostrzegli, i� ", 
				"tubylcy ci potrafi� ", 
				"zniewala� ork�w.", 
				"Tak wiec w zamian za ", 
				"zachowanie swoich ziem ", 
				"Jorfa dostarcza�a ", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"egzotycznych wyrob�w", 
				"i zniewolon� orcz� si�e ", 
				"robocz�, kt�ra okaza�a ", 
				"si� bardzo cenna dla ", 
				"ludno�ci stolicy... ", 
				"", 
				"", 
				"", 

			}; 
			Pages[cnt++].Lines = lines;
		}

		public KsiegaPoczatku1( Serial serial ) : base( serial )
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