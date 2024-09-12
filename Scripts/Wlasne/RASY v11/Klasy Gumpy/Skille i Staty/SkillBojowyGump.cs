using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Items;
using Server.Mobiles;
using Server.Regions;

namespace Server.Gumps
{

	public class SkillBojowyGump : Gump
	{
		public SkillBojowyGump()
			: base( 0, 0 )
		{
			this.Closable=false;
			this.Disposable=false;
			this.Dragable=false;
			this.Resizable=false;
			this.AddPage(0);
			this.AddBackground(100, 40, 500, 550, 9270);

                        this.AddLabel(290, 63, 137, @"WYBÓR UMIEJETNOŒCI BOJOWEJ:");
		                                            
			this.AddButton(140, 83, 4502, 4502, (int)Buttons.Button1, GumpButtonType.Reply, 0);
			this.AddButton(140, 113, 4502, 4502, (int)Buttons.Button2, GumpButtonType.Reply, 0);
			this.AddButton(140, 143, 4502, 4502, (int)Buttons.Button3, GumpButtonType.Reply, 0);
			this.AddButton(140, 173, 4502, 4502, (int)Buttons.Button4, GumpButtonType.Reply, 0);
			this.AddButton(140, 203, 4502, 4502, (int)Buttons.Button5, GumpButtonType.Reply, 0);
                        this.AddButton(140, 233, 4502, 4502, (int)Buttons.Button6, GumpButtonType.Reply, 0);
		
			this.AddButton(140, 523, 4502, 4502, (int)Buttons.Button7, GumpButtonType.Reply, 0);
			this.AddButton(340, 523, 4502, 4502, (int)Buttons.Button8, GumpButtonType.Reply, 0);

                        this.AddLabel(210, 100, 917, @"WALKA MIECZEM (+50%)");
			this.AddLabel(210, 130, 917, @"WALKA SZPAD¥ (+50%)");
			this.AddLabel(210, 160, 917, @"WALKA BRONI¥ OBUCHOW¥ (+50%)");
                        this.AddLabel(210, 190, 917, @"WALKA PIÊSCIAMI (+50%)");
                        this.AddLabel(210, 220, 917, @"£UCZNICTWO (+50%)");
                        this.AddLabel(210, 250, 917, @"WALKA BRONI¥ MIOTN¥ (+50%)");
                        
                        this.AddLabel(210, 540, 137, @"WYJDZ");
                        this.AddLabel(410, 540, 137, @"WRÓÆ");
		}
		
		public enum Buttons
		{
			Close,
			Button1,
			Button2,
			Button3,
		        Button4,
		        Button5,
		        Button6,
                        Button7,
                        Button8,
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{                  
			   Mobile m = sender.Mobile;
               
			if (m == null)
				return;

            PlayerMobile pm = (PlayerMobile)m;
            pm.PlaySound(0x2DF);
             
			switch ( info.ButtonID )
			{
				case 0:
				{
				m.SendMessage( "" );
				break;
				}

				case 1: 
				{						            
                                pm.Skills[SkillName.WalkaMieczami].Base += 50;
                                m.CloseGump( typeof ( SkillBojowyGump ) );
                                m.SendGump( new KlasyGump());
         		        break; 
				}
				
				case 2: 
				{
                                pm.Skills[SkillName.WalkaSzpadami].Base += 50;
                                m.CloseGump( typeof ( SkillBojowyGump ) );
                                m.SendGump( new KlasyGump());
				break; 
				}

				case 3: 
				{
                                pm.Skills[SkillName.WalkaObuchami].Base += 50;
                                m.CloseGump( typeof ( SkillBojowyGump ) );
                                m.SendGump( new KlasyGump());
                                break; 
				}
			
			        case 4:
			        {
                                pm.Skills[SkillName.Boks].Base += 50;
                                m.CloseGump( typeof ( SkillBojowyGump ) );
                                m.SendGump( new KlasyGump());
                                break; 
                                }		

			        case 5:
			        {
                                pm.Skills[SkillName.Lucznictwo].Base += 50;
                                m.CloseGump( typeof ( SkillBojowyGump ) );
                                m.SendGump( new KlasyGump());
                                break;
                                }

			        case 6:
			        {
                                pm.Skills[SkillName.Rzucanie].Base += 50;
                                m.CloseGump( typeof ( SkillBojowyGump ) );
                                m.SendGump( new KlasyGump());
                                break;
                                }

			        case 7:
			        {
                                      m.SendMessage( 0x35, "Zdecydowa³eœ zaczekaæ z wyborem!" );
                                      break;
                                }			

			        case 8:
			        {
                                      m.SendGump( new BóstwaG³ówneGump());
                                      break;
                                }
					
	     }
        }
    }
}