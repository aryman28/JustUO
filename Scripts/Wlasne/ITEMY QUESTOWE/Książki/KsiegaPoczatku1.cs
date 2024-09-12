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
				"Historia obecnego œwiata", 
				"okie³znanego tajemniczoœci¹", 
				"i dzikoœci¹ rozpocze³a", 
				"siê stu laty po upadku", 
				"wielkiego ksiêstwa Brytania.", 
				"Niegdyœ potê¿ne i bogate",
				"ciesz¹cego siê s³aw¹",
				"i spokojem.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Dziœ jedynie garstka ludzi", 
				"która niebawem wyzionie", 
				"ducha pamiêta ów czasy", 
				"nazywane czasem pustki", 
				"Kres wielkiemu ksiêstwu", 
				"przynios³a wielka wojna", 
				"domowa spowodowana", 
				"sporem o w³adze.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Po ciê¿kich walkach", 
				"w gruzach leg³y mury", 
				"najwspanialszych miast...", 
				"¯niwo wojny zosta³o", 
				"spotêgowane przez zarazê", 
				"która zabiera³a ¿ywym", 
				"istotom dech z piersi", 
				"przeobra¿aj¹c ca³e krainy", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"w pustkowia.", 
				"Przesiakniêta krwi¹ ziemia", 
				"nie rodzi³a ju¿ plonów.", 
				"Ówczesne w³adze Brytanii", 
				"stane³y przed decyzj¹", 
				"gdy kraine blasku s³oñca", 
				"okry³y ciemne chmury", 
				"nios¹c ze sob¹ z³aknionych", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ludzkimi duszami Ponurych", 
				"¯niwiarzy.", 
				"Pastwi³y siê na poleg³ych", 
				"duszach ofiar niegodziwych.",
				"Przeobra¿one w œmiertelne",
				"pachn¹ce siark¹ krainy", 
				"nie mog³y byæ schronieniem", 
				"a niebezpieczenstwo", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"ze strony plugawych istot ", 
				"pogr¹¿a³y ocala³ych w ", 
				"smutku, ¿alu i rozpaczy.", 
				"W³adze ów spoczywa³y w ",
				"rêkach m³odej Valerii,",
				"która wywodzi³a siê z", 
				"krwi królewskiego rodu", 
				"rz¹dz¹cego ksiêstwem.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"W obliczu nieub³aganej", 
				"zag³ady ksiêstwa i pewnej", 
				"œmierci jego mieszkanców", 
				"zadecydowa³a o kolonizacji",
				"nowych ziem...",
				"Ostatkami zapasów", 
				"zbudowano wielk¹ flote", 
				"morsk¹, która wyruszy³a", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"szlakiem pod opatrznoœci¹", 
				"œwiec¹cych gwiazd by", 
				"osiedliæ nowe tereny.", 
				"Podró¿ by³a surowym",
				"sprawdzianem ludzkiej",
				"wytrwa³oœci i wiary...", 
				"Po wielu miesi¹cach", 
				"w koñcu odnaleziono l¹d.", 
			};			
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Rozpoczeto budowê", 
				"pierwszej osady, która", 
				"nazwano Linhal.", 
				"¯ycie w ma³ej wiosce",
				"szybko nabra³o têpa,", 
				"tak wiec po up³yniêciu", 
				"kilku wiosen ulice sta³y", 
				"siê przepe³nione ludzmi", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Handel i zbiory plonów ", 
				"kwit³y wype³niaj¹c ", 
				"spichlerze i niektóre", 
				"sakwy mieszkanców po ", 
				"same brzegi.", 
				"Podjêto wówczac decyzjê", 
				"o budowie stolicy,", 
				"która nazwano Valera", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"od imienia w³adczyni,", 
				"której to ludzie", 
				"zawdziêczali ocalenie.", 
				"Dawne czasy odesz³y", 
				"ju¿ w zapomnienie...", 
				"kwesti¹ czasu by³a budowa", 
				"nowego Zakonu Rycerzy", 
				"którego zadaniem by³o", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"zapewnienie bezpieczenstwa", 
				"mieszkañcom stolicy.", 
				"Stolica zosta³a ", 
				"ufortyfikowana", 
				"a surowce na jej budowê", 
				"jak i powstajacy zakon", 
				"dostarczane by³y z nowo", 
				"odkrytego l¹du na pólnocy.", 
			}; 
			Pages[cnt++].Lines = lines; 
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Panowa³a tam sroga zima,", 
				"tak wiêc do pozyskiwania", 
				"i wydobywania surowców", 
				"wykorzystano si³ê wiezniów", 
				"i pojmanych orków,", 
				"którzy zniewoleni pracowali", 
				"pod nadzorem zakonu.", 
				"Ukszta³towaly siê osady", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Barna i Guisula,", 
				"gdzie zapieczêtowany zosta³", 
				"los pojmanych jenców", 
				" i zbrodniarzy...", 
				"Czasy te jednak nie ", 
				"zawsze by³y sielankowe", 
				"jak opisuje historia.", 
				"Wiele razy populacja", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"przysz³ej Linhali i Valery", 
				"musia³a skrzy¿owaæ swój", 
				"orê¿ z rdzennymi ", 
				"mieszkancami nowego ladu,", 
				"broniacymi zaciekle swoich ", 
				"terenów.", 
				"Brak porozumienia stron", 
				"przyczynil sie do", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"krwawej rzezi tubylczych", 
				"plemion,których drewniane", 
				"w³ócznie,strza³y,kamienie", 
				"by³y niczym w porównaniu", 
				"tarczami i stalowym ", 
				"orê¿em najezdzców.", 
				"Bior¹ce udzia³ w walce", 
				"plemiona zosta³y pokonane.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Jedyna ocala³a wioska ", 
				"byla Jorfa znajduj¹ca sie ", 
				"na wyspie o tej samej ", 
				"nazwie, która podda³a sie", 
				"bez rozlewu krwi,", 
				"widz¹c przerastaj¹c¹ si³e", 
				"zbrojn¹ ludzi z po³udnia.", 
				"Jorfa mia³a wiêksze", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"znaczenie dla stolicy", 
				"ani¿eli inne plemiona.", 
				"Ludzie dostrzegli, i¿ ", 
				"tubylcy ci potrafi¹ ", 
				"zniewalaæ orków.", 
				"Tak wiec w zamian za ", 
				"zachowanie swoich ziem ", 
				"Jorfa dostarcza³a ", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"egzotycznych wyrobów", 
				"i zniewolon¹ orcz¹ si³e ", 
				"robocz¹, która okaza³a ", 
				"siê bardzo cenna dla ", 
				"ludnoœci stolicy... ", 
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