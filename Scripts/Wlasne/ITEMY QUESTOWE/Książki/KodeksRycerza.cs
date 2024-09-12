using System;
using Server;

namespace Server.Items
{
	public class KodeksRycerza : BlueBook
	{
		[Constructable]
		public KodeksRycerza() : base( "Kodeks Rycerstwa", "skryba Vega ", 19, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
		int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"1.Najwa¿niejsz¹ cech¹", 
				"Rycerz po wierze", 
				"honor jest.", 
				"Honor nie pozwala ", 
				"Rycerzowi ¿adnego ", 
				"punktu Kodeksu z³amaæ.",
				"",
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"2.Tylko wiara wa¿niejsza", 
				"od honoru bêdzie dla", 
				"Ciebie i tylko ona honor", 
				"naruszyæ mo¿e.", 
				"3.Wiedz, i¿ kiedy m³odzi", 
				"czesto nie widz¹ ró¿nicy", 
				"miêdzy dobrem i z³em,", 
				"to bardziej doœwiadczeni", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"czesto obrastaj¹ w pyche", 
				"która jest grzechem.", 
				"4.Rycerz poprzez czyny", 
				"na rzecz swej wiary", 
				"i koscio³a, do którego", 
				"nale¿y dzia³a, poprzez", 
				"serce czyste, nienaganny", 
				"jêzyk, czujn¹ duszê", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"i miecz skierowany w", 
				"strone z³a, czaj¹cego", 
				"siê poœród nas.", 
				"5.Dlatego pamiêtaj, i¿", 
				"kiedy przechodz¹c przez", 
				"sfery zostaniesz wyœmiany", 
				"czy splugawiony", 
				"nieczystoœciami,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nie wolno ci, jak", 
				"wojownikowi hardemu czy", 
				"wojowniczce dumnej,", 
				"wyzywaæ innych na",
				"pojedynek.",
				"6.Niech sprawiedliwoœæ", 
				"bêdzie twym towarzyszem", 
				"nawet, kiedy odpuœcisz", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"grzesznym winy.", 
				"7.Jeœli bêdziesz pewien", 
				"win swego wroga oraz", 
				"tego, i¿ ju¿ nie mo¿e",
				"sk³oniæ siê ku jasnej",
				"scie¿ce, zabij go", 
				"i poproœ bogów o czystoœæ", 
				", gdy¿ ka¿da œmieræ,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nawet w s³usznej wierze", 
				"czyniona, pozostaje", 
				"plama dla twych r¹k.", 
				"8. Kiedy za skóre czy",
				"w³os lub znak na ciele",
				"zabijesz dusze myœl¹ca,", 
				"wiedz, i¿ s¹d nad tob¹", 
				"bêdzie surowy po œmierci.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"10.Jednak zarazem nie", 
				"wierz w przeznaczenie!", 
				"Rece twe nie s¹", 
				"prowadzone przez",
				"niewidzialne moce,",
				"jednak przez wole Twego", 
				"pana i ciebie samego!", 
				"", 
			};			
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"11.Za najwiêksz¹ œwiêtoœæ", 
				"blizniego uznawaj, ¿ycie", 
				"niewinnego.", 
				"12.Nie s¹dz mylnie,",
				"gdy¿ s¹dzic ciê bêd¹", 
				"wedle twej w³asnej", 
				"sprawiedliwoœci.", 
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"13.Do ka¿dej kobiety", 
				"odnoœ siê z szacunkiem,", 
				"jednak nieznacznie tylko", 
				"wiekszym do swych braci.", 
				"14.Nie zachwalaj cia³a", 
				"czy duszy lub serca", 
				"nieprawdziwie, gdy¿", 
				"kiedy raz nieprawdziwie", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"wywy¿szysz blizniego,", 
				"d³ugo Ci nie zaufa", 
				"i wiedzieæ nie bêdzie,", 
				"kiedy tylko siê", 
				"przypodobaæ pragniesz", 
				"a kiedy prawdê rzeczesz.", 
				"15.Pamietaj, i¿ rodzina", 
				"dla Ciebie bêdzie", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"wielkim skarbem który", 
				"odwiedzie ciê od bogów", 
				"i narazi na lêk,", 
				"strach i cierpienie.", 
				"16.Rycerz, jako wojak", 
				"bo¿y, wykazywaæ siê", 
				"cnotami wiary winien.", 
				"Przestrzegaæ praw bo¿ych.", 
			}; 
			Pages[cnt++].Lines = lines; 
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Nigdy swej wiary wyprzeæ", 
				"siê nie mo¿e, a wrecz", 
				"swe ¿ycie w jej obronie", 
				"winien ofiarowaæ.", 
				"17.Rycerz swemu", 
				"przeciwnikowi szacunek", 
				"okazywaæ powinien.", 
				"Pokonanego z honorami", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"potraktowaæ musi,", 
				"a zmar³emu zagwarantowaæ", 
				"nale¿ny pochówek,", 
				"chocby najwiêkszym", 
				"wrogiem by³.", 
				"18.Mi³osierdzie jedn¹ z", 
				"cnot boskich jest.", 
				"Rycerz obok nieszczêœcia", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"przechodziæ bokiem nie", 
				"powinien.", 
				"Nie mog¹ byæ mu", 
				"obojêtne bieda, ból", 
				"i nieszczêœcie.", 
				"19.Gdy zaœ Rycerz o", 
				"pomoc poproszony", 
				"zostanie,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"udzieliæ jej jest", 
				"zobowi¹zany,", 
				"jeœli ma ku temu", 
				"mo¿liwoœci i gdy nie", 
				"godzi to w jego honor", 
				"i Kodeks.", 
				"20.Jeœliby punkt Kodeksu", 
				"z³amany zosta³ œwiadomie,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Rycerz nie jest ju¿", 
				"godzien swojego stanu i", 
				"swoje insygnia porzuciæ", 
				"powinien.", 
				"", 
				"Niech kieruje Tob¹ serce", 
				"zew duszy sprawiedliwej.", 
				"Oby Ciê œwiat³o twego", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"patrona i niebios", 
				"prowadzi³o -", 
				"przez ciemne drogi. ", 
				"¯yczê Ci najlepszego,", 
				"co mo¿esz napotkaæ,", 
				"bracie, siostro.", 
				"", 
				"", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"", 
				"", 
				"", 
				"", 
				"", 
				"", 
				"", 
				"", 

			}; 
			Pages[cnt++].Lines = lines;
		}

		public KodeksRycerza( Serial serial ) : base( serial )
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