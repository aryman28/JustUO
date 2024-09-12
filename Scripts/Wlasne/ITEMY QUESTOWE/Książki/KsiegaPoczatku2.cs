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
				"�niwo wojny z tubylcami", 
				"przynios�o wiele ofiar", 
				"W obawie przed wybuchem", 
				"nast�pnej epidemii czarnej", 
				"�mierci", 
				"w�adze stolicy obarczy�y",
				"trudnym zadaniem grup�",
				"uzdrowicieli.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"kt�ra mia�a za zadanie", 
				"pozbywanie si� cia� ", 
				"poleglych ofiar.", 
				"Grupa ta z czasem sta�a", 
				"si� otumaniona czarn� ", 
				"�mierci� i pragieniem", 
				"zdobycia wiecznego �ycia", 
				"organizacj� nekromant�w.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"kt�ra mie�ci�a si� w", 
				"odbudowanej wiosce Azzan", 
				"gdzie dokonano ogromnej", 
				"krwawej rzezi podczas", 
				"wojny z tubylcami.", 
				"Dokonane tu rytua�y", 
				"kt�re wysysa�y si�y ", 
				"�yciowe z wszystkiego", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"co �ywe z czasem", 
				"przeobrazi�y te tereny", 
				"w czarne pustkowia.", 
				"Truj�ce chmury,zalegaj�ce", 
				"wsz�dzie rozk�adaj�ce si�", 
				"zw�oki z kt�rych s�czy�a", 
				"si� zgni�a krew z czasem", 
				"odcisne�y swe pi�tno na", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"budowlach i pobliskich", 
				"terenach.", 
				"Zg��biaj�c tajniki �mierci", 
				"i mrocznych rytua��w",
				"grupa nekromant�w chcia�a",
				"zyska� na sile by przej�� ", 
				"w�adze, a przyw�dc� by�:", 
				"Balzac szanowany Mag.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Balzac by� te� pierwszym", 
				"magiem kt�ry rozpocz�", 
				"badania nad nekromancj�", 
				"plugaw� magi� czerpi�c�",
				"si�e z istot �ywych.",
				"Po stronie zakonu stane�y", 
				"poproszone o pomoc", 
				"szko�y magii.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Mroczni poplecznicy", 
				"�mierci zostali zmuszeni", 
				"do odwrotu w ciemne ", 
				"zakamarki Azzanu",
				"gdzie przysi�gli zemsty i",
				"przej�cie w�adzy...", 
				"", 
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Mine�o ju� wiele lat", 
				"od wspomnianych wydarze�.", 
				"Nasta� czas pokoju, ", 
				"a miasta rozrasta�y si�.",
				"Ludzko�� powr�ci�a na",
				"niegdy� opuszczone ziemie", 
				"Do swego domu kontynentu", 
				"zwanego Felucca. ", 
			};			
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Felucca przyje�a w swe", 
				"progi coraz bardziej", 
				"rosn�c� populacj� Valery.", 
				"Tak wi�c wraz z powrotem",
				"ludzi nasta�a nowa era", 
				",wr�ci�y te� dawne obawy.", 
				"Wszak dawny wr�g nie", 
				"zosta� pokonany.", 
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