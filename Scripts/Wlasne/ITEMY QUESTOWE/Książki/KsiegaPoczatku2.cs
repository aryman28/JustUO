using System;
using Server;

namespace Server.Items
{
	public class KsiegaPoczatku2 : BrownBook
	{
		[Constructable]
		public KsiegaPoczatku2() : base( "Ksiega Poczatku Tom 2", "nadworny skryba Blaz ", 9, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
		int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"¯niwo wojny z tubylcami", 
				"przynios³o wiele ofiar", 
				"W obawie przed wybuchem", 
				"nastêpnej epidemii czarnej", 
				"œmierci", 
				"w³adze stolicy obarczy³y",
				"trudnym zadaniem grupê",
				"uzdrowicieli.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"która mia³a za zadanie", 
				"pozbywanie siê cia³ ", 
				"poleglych ofiar.", 
				"Grupa ta z czasem sta³a", 
				"siê otumaniona czarn¹ ", 
				"œmierci¹ i pragieniem", 
				"zdobycia wiecznego ¿ycia", 
				"organizacj¹ nekromantów.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"która mieœci³a siê w", 
				"odbudowanej wiosce Azzan", 
				"gdzie dokonano ogromnej", 
				"krwawej rzezi podczas", 
				"wojny z tubylcami.", 
				"Dokonane tu rytua³y", 
				"które wysysa³y si³y ", 
				"¿yciowe z wszystkiego", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"co ¿ywe z czasem", 
				"przeobrazi³y te tereny", 
				"w czarne pustkowia.", 
				"Truj¹ce chmury,zalegaj¹ce", 
				"wszêdzie rozk³adaj¹ce siê", 
				"zw³oki z których s¹czy³a", 
				"siê zgni³a krew z czasem", 
				"odcisne³y swe piêtno na", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"budowlach i pobliskich", 
				"terenach.", 
				"Zg³êbiaj¹c tajniki œmierci", 
				"i mrocznych rytua³ów",
				"grupa nekromantów chcia³a",
				"zyskaæ na sile by przej¹æ ", 
				"w³adze, a przywódc¹ by³:", 
				"Balzac szanowany Mag.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Balzac by³ te¿ pierwszym", 
				"magiem który rozpocz¹", 
				"badania nad nekromancj¹", 
				"plugaw¹ magi¹ czerpi¹c¹",
				"si³e z istot ¿ywych.",
				"Po stronie zakonu stane³y", 
				"poproszone o pomoc", 
				"szko³y magii.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Mroczni poplecznicy", 
				"œmierci zostali zmuszeni", 
				"do odwrotu w ciemne ", 
				"zakamarki Azzanu",
				"gdzie przysiêgli zemsty i",
				"przejêcie w³adzy...", 
				"", 
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Mine³o ju¿ wiele lat", 
				"od wspomnianych wydarzeñ.", 
				"Nasta³ czas pokoju, ", 
				"a miasta rozrasta³y siê.",
				"Ludzkoœæ powróci³a na",
				"niegdyœ opuszczone ziemie", 
				"Do swego domu kontynentu", 
				"zwanego Felucca. ", 
			};			
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Felucca przyje³a w swe", 
				"progi coraz bardziej", 
				"rosn¹c¹ populacjê Valery.", 
				"Tak wiêc wraz z powrotem",
				"ludzi nasta³a nowa era", 
				",wróci³y te¿ dawne obawy.", 
				"Wszak dawny wróg nie", 
				"zosta³ pokonany.", 
			}; 
			Pages[cnt++].Lines = lines;
		}

		public KsiegaPoczatku2( Serial serial ) : base( serial )
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