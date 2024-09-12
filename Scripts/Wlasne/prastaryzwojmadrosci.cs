using System; 
using Server.Network; 
using Server.Items; 
using Server.Gumps; 

namespace Server.Items 
{ 
   public class prastaryzwojmadrosci: Item 
      { 
      private int m_SkillBonus = 20; 
      private string m_BaseName = "Prastary zwój m¹droœci +"; 
      public bool GumpOpen = false; 

      [CommandProperty( AccessLevel.GameMaster )] 
      public int SkillBonus 
      { 
   get { return m_SkillBonus; } 
   set { 
     m_SkillBonus = value; 
     this.Name = m_BaseName + Convert.ToString(m_SkillBonus); 
   } 
      } 

      [Constructable] 
      public prastaryzwojmadrosci( int SkillBonus ) : base( 0xE73 ) 
      { 
        Movable = false; 
        Hue = 1161; 
        m_SkillBonus = SkillBonus; 
        Name = m_BaseName + Convert.ToString(SkillBonus); 
      } 

      [Constructable] 
      public prastaryzwojmadrosci() : base( 0x14ED )
       
      {
        Hue = 1161; 
        Name = "Prastary zwój m¹droœci" + Convert.ToString(SkillBonus); 
        Movable = false;
      } 

      public prastaryzwojmadrosci( Serial serial ) : base( serial ) 
      { 
      } 

      public override void OnDoubleClick( Mobile from ) 
      { 
   if ( (this.SkillBonus == 80) && (from.AccessLevel < AccessLevel.GameMaster) ) { 
     from.SendMessage("This Skill Ball isn't charged. Please page for a GM."); 
     return; 
   } 
   else if ( (from.AccessLevel >= AccessLevel.GameMaster) && (this.SkillBonus == 0) ) { 
     from.SendGump( new PropertiesGump( from, this ) ); 
     return; 
   } 

   if ( !IsChildOf( from.Backpack ) ) 
     from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it. 
   else if (!GumpOpen) { 
     GumpOpen = true; 
     from.SendGump( new Plus25SkillBallGump( from, this ) ); 
   } 
   else if (GumpOpen) 
     from.SendMessage("U¿ywasz ju¿ zwoju."); 
      } 

      public override void Serialize( GenericWriter writer ) 
      { 
   base.Serialize( writer ); 

   writer.Write( (int) 0 ); // version 
   writer.Write( m_SkillBonus ); 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
   base.Deserialize( reader ); 

   int version = reader.ReadInt(); 

   switch (version) { 

     case 0 : { 
       m_SkillBonus = reader.ReadInt(); 
       break; 
     } 
   } 
      } 
   } 
} 

namespace Server.Gumps 
{ 
   public class Plus25SkillBallGump : Gump 
   { 
      private const int FieldsPerPage = 14; 

      private Skill m_Skill; 
      private prastaryzwojmadrosci m_skb; 

      public Plus25SkillBallGump ( Mobile from, prastaryzwojmadrosci skb ) : base ( 20, 30 ) 
      { 
   m_skb = skb; 
    
   AddPage ( 0 ); 
   AddBackground( 0, 0, 260, 351, 5054 ); 
    
   AddImageTiled( 10, 10, 240, 23, 0x52 ); 
   AddImageTiled( 11, 11, 238, 21, 0xBBC ); 
    
   AddLabel( 45, 11, 0, "Umiejêtnoœæ do zwiêkszenia" ); 
    
   AddPage( 1 ); 
    
   int page = 1; 
   int index = 0; 
    
   Skills skills = from.Skills;
    
   for ( int i = 0; i < ( skills.Length - 6 ); ++i ) { 
     if ( index >= FieldsPerPage ) { 
       AddButton( 231, 13, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1 ); 
       
       ++page; 
       index = 0; 
       
       AddPage( page ); 
       
       AddButton( 213, 13, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page - 1 ); 
     } 
     
     Skill skill = skills[i]; 
     
     if ( (skill.Base + m_skb.SkillBonus) <= 100 ) { 
       
       AddImageTiled( 10, 32 + (index * 22), 240, 23, 0x52 ); 
       AddImageTiled( 11, 33 + (index * 22), 238, 21, 0xBBC ); 
       
       AddLabelCropped( 13, 33 + (index * 22), 150, 21, 0, skill.Name ); 
       AddImageTiled( 180, 34 + (index * 22), 50, 19, 0x52 ); 
       AddImageTiled( 181, 35 + (index * 22), 48, 17, 0xBBC ); 
       AddLabelCropped( 182, 35 + (index * 22), 234, 21, 0, skill.Base.ToString( "F1" ) ); 
       
       if ( from.AccessLevel >= AccessLevel.Player ) 
         AddButton( 231, 35 + (index * 22), 0x15E1, 0x15E5, i + 1, GumpButtonType.Reply, 0 ); 
       else 
         AddImage( 231, 35 + (index * 22), 0x2622 ); 
       
       ++index; 
     } 
   } 
      } 
      
      public override void OnResponse( NetState state, RelayInfo info ) 
      { 
   Mobile from = state.Mobile; 
    
   if ( (from == null) || (m_skb.Deleted) ) 
     return; 

   m_skb.GumpOpen = false; 

   if ( info.ButtonID > 0 ) { 
     m_Skill = from.Skills[(info.ButtonID-1)]; 
     
     if ( m_Skill == null ) 
            return; 
     
          double count = 0; 
     for ( int i = 0; i < from.Skills.Length; ++i ) 
       count += from.Skills[i].Base; 
     
          if ( (count + m_skb.SkillBonus) > (from.SkillsCap / 10) ) { 
       from.SendMessage( "Twój zakres umiejêtnoœci jest zbyt du¿y." ); 
       return; 
     } 
       
     if (m_Skill.Base + m_skb.SkillBonus <= 100) { 
       m_Skill.Base += m_skb.SkillBonus; 
       m_skb.Delete(); 
     } 
     else 
       from.SendMessage("Musisz wybraæ inn¹ umiejêtnoœæ."); 
   } 
      } 
   } 
}