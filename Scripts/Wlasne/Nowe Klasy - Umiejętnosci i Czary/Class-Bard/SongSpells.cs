using System; 
using Server; 

namespace Server.Spells.Song
{ 
   public abstract class Song : Spell 
   { 
      //public abstract double CastDelay{ get; } 
	  //public override TimeSpan CastDelayBase { get; }
      public abstract double RequiredSkill{ get; } 
      public abstract int RequiredMana{ get; } 

      public override SkillName CastSkill{ get{ return SkillName.Muzykowanie; } } 
      public override SkillName DamageSkill{ get{ return SkillName.Muzykowanie; } } 

      public override bool ClearHandsOnCast{ get{ return false; } } 

      public Song( Mobile caster, Item scroll, SpellInfo info ) : base( caster, scroll, info ) 
      { 
      } 

      public override void GetCastSkills( out double min, out double max ) 
      { 
         min = RequiredSkill; 
         max = RequiredSkill + 30.0; 
      } 

      public override int GetMana() 
      { 
         return RequiredMana; 
      } 

      //public override TimeSpan GetCastDelay() 
      //{ 
       //  return TimeSpan.FromSeconds( CastDelay ); 
      //} 
   } 
}