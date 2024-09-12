using System;
using Server;

namespace Server.Items
{
	public class Pamietnik : BrownBook
	{
		[Constructable]
		public Pamietnik() : base( "Pamietnik", "napisany twoja reka", 4, false ) // true writable so players can make notes
		{
		// NOTE: There are 8 lines per page and 
		// approx 22 to 24 characters per line! 
		//		0----+----1----+----2----+ 
			int cnt = 0; 
			string[] lines; 
			lines = new string[] 
			{ 
				"Po wielu dniach na morzu", 
				",wkoñcu podró¿ zdaje siê", 
				"dobiegaæ koñca.", 
				"Mam przed sob¹ nowy l¹d", 
				"oraz nowe mo¿liwoœci.", 
				"Za moj¹ prace na statku", 
				"kapitan wrêczy³ mi moje", 
				"wynagrodzenie.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Zdobyte podczas podró¿y",
				"doœwiadczenie pozwoli mi", 
				"obraæ drogê któr¹ chce", 
				"pod¹¿yæ.",
				"Jeœli chce zostaæ magiem", 
				"udam siê do wielkiej", 
				"biblioteki Nowej Brytani,", 
				"nowej stolicy tego l¹du.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Jeœli zechce zostaæ", 
				"giermkiem w zakonie", 
				"muszê odwiedziæ zakon", 
				"na wyspie za pó³nocnym", 
				"mostem Brytanii.", 
				"Jeœli jednak bêde kroczyæ", 
				"scie¿ka mrocznej magii", 
				"muszê znaleœæ nauczyciela.", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Dzieñ 234 na morzu zrywa", 
				"siê wielki sztorm...",
				"Nasz statek nabiera wody,", 
				"zdaje siê nie ma ju¿", 
				"nadzieji.", 
				"Dzieñ 235...*Rozmazane*",  
				"", 
				"", 
			};

			Pages[cnt++].Lines = lines;

		}

		public Pamietnik( Serial serial ) : base( serial )
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