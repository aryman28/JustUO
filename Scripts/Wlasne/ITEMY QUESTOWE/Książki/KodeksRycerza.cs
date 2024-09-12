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
				"1.Najwa�niejsz� cech�", 
				"Rycerz po wierze", 
				"honor jest.", 
				"Honor nie pozwala ", 
				"Rycerzowi �adnego ", 
				"punktu Kodeksu z�ama�.",
				"",
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"2.Tylko wiara wa�niejsza", 
				"od honoru b�dzie dla", 
				"Ciebie i tylko ona honor", 
				"naruszy� mo�e.", 
				"3.Wiedz, i� kiedy m�odzi", 
				"czesto nie widz� r�nicy", 
				"mi�dzy dobrem i z�em,", 
				"to bardziej do�wiadczeni", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"czesto obrastaj� w pyche", 
				"kt�ra jest grzechem.", 
				"4.Rycerz poprzez czyny", 
				"na rzecz swej wiary", 
				"i koscio�a, do kt�rego", 
				"nale�y dzia�a, poprzez", 
				"serce czyste, nienaganny", 
				"j�zyk, czujn� dusz�", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"i miecz skierowany w", 
				"strone z�a, czaj�cego", 
				"si� po�r�d nas.", 
				"5.Dlatego pami�taj, i�", 
				"kiedy przechodz�c przez", 
				"sfery zostaniesz wy�miany", 
				"czy splugawiony", 
				"nieczysto�ciami,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nie wolno ci, jak", 
				"wojownikowi hardemu czy", 
				"wojowniczce dumnej,", 
				"wyzywa� innych na",
				"pojedynek.",
				"6.Niech sprawiedliwo��", 
				"b�dzie twym towarzyszem", 
				"nawet, kiedy odpu�cisz", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"grzesznym winy.", 
				"7.Je�li b�dziesz pewien", 
				"win swego wroga oraz", 
				"tego, i� ju� nie mo�e",
				"sk�oni� si� ku jasnej",
				"scie�ce, zabij go", 
				"i popro� bog�w o czysto��", 
				", gdy� ka�da �mier�,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"nawet w s�usznej wierze", 
				"czyniona, pozostaje", 
				"plama dla twych r�k.", 
				"8. Kiedy za sk�re czy",
				"w�os lub znak na ciele",
				"zabijesz dusze my�l�ca,", 
				"wiedz, i� s�d nad tob�", 
				"b�dzie surowy po �mierci.", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"10.Jednak zarazem nie", 
				"wierz w przeznaczenie!", 
				"Rece twe nie s�", 
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
				"11.Za najwi�ksz� �wi�to��", 
				"blizniego uznawaj, �ycie", 
				"niewinnego.", 
				"12.Nie s�dz mylnie,",
				"gdy� s�dzic ci� b�d�", 
				"wedle twej w�asnej", 
				"sprawiedliwo�ci.", 
				"", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"13.Do ka�dej kobiety", 
				"odno� si� z szacunkiem,", 
				"jednak nieznacznie tylko", 
				"wiekszym do swych braci.", 
				"14.Nie zachwalaj cia�a", 
				"czy duszy lub serca", 
				"nieprawdziwie, gdy�", 
				"kiedy raz nieprawdziwie", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"wywy�szysz blizniego,", 
				"d�ugo Ci nie zaufa", 
				"i wiedzie� nie b�dzie,", 
				"kiedy tylko si�", 
				"przypodoba� pragniesz", 
				"a kiedy prawd� rzeczesz.", 
				"15.Pamietaj, i� rodzina", 
				"dla Ciebie b�dzie", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"wielkim skarbem kt�ry", 
				"odwiedzie ci� od bog�w", 
				"i narazi na l�k,", 
				"strach i cierpienie.", 
				"16.Rycerz, jako wojak", 
				"bo�y, wykazywa� si�", 
				"cnotami wiary winien.", 
				"Przestrzega� praw bo�ych.", 
			}; 
			Pages[cnt++].Lines = lines; 
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Nigdy swej wiary wyprze�", 
				"si� nie mo�e, a wrecz", 
				"swe �ycie w jej obronie", 
				"winien ofiarowa�.", 
				"17.Rycerz swemu", 
				"przeciwnikowi szacunek", 
				"okazywa� powinien.", 
				"Pokonanego z honorami", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"potraktowa� musi,", 
				"a zmar�emu zagwarantowa�", 
				"nale�ny poch�wek,", 
				"chocby najwi�kszym", 
				"wrogiem by�.", 
				"18.Mi�osierdzie jedn� z", 
				"cnot boskich jest.", 
				"Rycerz obok nieszcz�cia", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"przechodzi� bokiem nie", 
				"powinien.", 
				"Nie mog� by� mu", 
				"oboj�tne bieda, b�l", 
				"i nieszcz�cie.", 
				"19.Gdy za� Rycerz o", 
				"pomoc poproszony", 
				"zostanie,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"udzieli� jej jest", 
				"zobowi�zany,", 
				"je�li ma ku temu", 
				"mo�liwo�ci i gdy nie", 
				"godzi to w jego honor", 
				"i Kodeks.", 
				"20.Je�liby punkt Kodeksu", 
				"z�amany zosta� �wiadomie,", 
			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Rycerz nie jest ju�", 
				"godzien swojego stanu i", 
				"swoje insygnia porzuci�", 
				"powinien.", 
				"", 
				"Niech kieruje Tob� serce", 
				"zew duszy sprawiedliwej.", 
				"Oby Ci� �wiat�o twego", 

			}; 
			Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"patrona i niebios", 
				"prowadzi�o -", 
				"przez ciemne drogi. ", 
				"�ycz� Ci najlepszego,", 
				"co mo�esz napotka�,", 
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