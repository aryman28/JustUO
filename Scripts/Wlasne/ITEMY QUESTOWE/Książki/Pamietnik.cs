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
				",wko�cu podr� zdaje si�", 
				"dobiega� ko�ca.", 
				"Mam przed sob� nowy l�d", 
				"oraz nowe mo�liwo�ci.", 
				"Za moj� prace na statku", 
				"kapitan wr�czy� mi moje", 
				"wynagrodzenie.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Zdobyte podczas podr�y",
				"do�wiadczenie pozwoli mi", 
				"obra� drog� kt�r� chce", 
				"pod��y�.",
				"Je�li chce zosta� magiem", 
				"udam si� do wielkiej", 
				"biblioteki Nowej Brytani,", 
				"nowej stolicy tego l�du.",
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Je�li zechce zosta�", 
				"giermkiem w zakonie", 
				"musz� odwiedzi� zakon", 
				"na wyspie za p�nocnym", 
				"mostem Brytanii.", 
				"Je�li jednak b�de kroczy�", 
				"scie�ka mrocznej magii", 
				"musz� znale�� nauczyciela.", 
			};
		Pages[cnt++].Lines = lines;
		//		0----+----1----+----2----+
			lines = new string[] 
			{ 
				"Dzie� 234 na morzu zrywa", 
				"si� wielki sztorm...",
				"Nasz statek nabiera wody,", 
				"zdaje si� nie ma ju�", 
				"nadzieji.", 
				"Dzie� 235...*Rozmazane*",  
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