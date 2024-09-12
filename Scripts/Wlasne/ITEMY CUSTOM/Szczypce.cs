using System;
using Server.Network; 
using Server.Misc; 
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{

    public class Szczypce : Item
    {
        [Constructable]
        public Szczypce()
            : base(0xFBB)
        {
            this.Weight = 2.0;
            this.Name = "Szczypce do hartowania";
        }

        public Szczypce(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
////Znajdz Kowad�o/Paleniski
                        bool anvil, forge;
			
                        Szczypce.CheckAnvilAndForgeH(from, 2, out anvil, out forge);

                        if (!anvil)
                        {
                            from.SendMessage(33, "Musisz by� obok kowad�a!");
                        }
                        if (!forge)
                        {
                           from.SendMessage(33, "Musisz by� obok paleniska!");
                        }
////Je�li przy kowadle i palenisku

			if ( from.Mounted )
			{
				from.SendMessage( 33, "Nie mo�esz tego robi� na wierzchowcu!" ); 
				return; 
			}

   if ( anvil && forge )
   {
            if (this.IsChildOf(from.Backpack) || this.Parent == from)
            {
                    from.Target = new SztabTarget(this, from);
                    from.SendMessage("Wska� sztaby?");
            }
            else
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
            }

   }//Przy Kowadle/palenisku koniec

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

      private class SztabTarget : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public SztabTarget( Szczypce item, Mobile from ) : base ( 1, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 

            if ( m_Item.Deleted ) return;

            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

if ( target is BaseIngot )
{
	       BaseIngot ing = (BaseIngot)target;

          if ( ing.Resource == CraftResource.Iron )
          {
               if (((IronIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((IronIngot)target).Amount -= 10;
                    from.Target = new HartoTarget(m_Item, from);
                    from.SendMessage("Wska� przedmiot z �elaza.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." ); 
               }
               if (((IronIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek �elaza aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.DullCopper )
          {
               if (((DullCopperIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((DullCopperIngot)target).Amount -= 10;
                    from.Target = new HartoTargetDullCopper(m_Item, from);
                    from.SendMessage("Wska� przedmiot z matowej miedzi.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." ); 
               }
               if (((DullCopperIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek matowej miedzi aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.ShadowIron )
          {
               if (((ShadowIronIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((ShadowIronIngot)target).Amount -= 10;
                    from.Target = new HartoTargetShadowIron(m_Item, from);
                    from.SendMessage("Wska� przedmiot z cienistego �elaza.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." ); 
               }
               if (((ShadowIronIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek cienistego �elaza aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.Copper )
          {
               if (((CopperIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((CopperIngot)target).Amount -= 10;
                    from.Target = new HartoTargetCopper(m_Item, from);
                    from.SendMessage("Wska� przedmiot z miedzi.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." ); 
               }
               if (((CopperIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek miedzi aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.Bronze )
          {
               if (((BronzeIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((BronzeIngot)target).Amount -= 10;
                    from.Target = new HartoTargetBronze(m_Item, from);
                    from.SendMessage("Wska� przedmiot z br�zu.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." ); 
               }
               if (((BronzeIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek br�zu aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.Gold )
          {
               if (((GoldIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((GoldIngot)target).Amount -= 10;
                    from.Target = new HartoTargetGold(m_Item, from);
                    from.SendMessage("Wska� przedmiot ze z�ota.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." ); 
 }
               if (((GoldIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek z�ota aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.Agapite )
          {
               if (((AgapiteIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((AgapiteIngot)target).Amount -= 10;
                    from.Target = new HartoTargetAgapite(m_Item, from);
                    from.SendMessage("Wska� przedmiot z agapitu.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." );
               }
               if (((AgapiteIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek agapitu aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.Verite )
          {
               if (((VeriteIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((VeriteIngot)target).Amount -= 10;
                    from.Target = new HartoTargetVerite(m_Item, from);
                    from.SendMessage("Wska� przedmiot z verytu.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." );
               }
               if (((VeriteIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek verytu aby zahartowa� przedmiot!" ); 
               }
           }
          else if ( ing.Resource == CraftResource.Valorite )
          {
               if (((ValoriteIngot)target).Amount >= 10)
               {
                      from.PlaySound(0x021);

                    ((ValoriteIngot)target).Amount -= 10;
                    from.Target = new HartoTargetValorite(m_Item, from);
                    from.SendMessage("Wska� przedmiot z valorytu.");
                    from.SendMessage( 33, "przetapiasz 10 sztab." );
               }
               if (((ValoriteIngot)target).Amount < 10)
               {
                       from.SendMessage( "Potrzebujesz 10 sztabek valorytu aby zahartowa� przedmiot!" ); 
               }
           }
}
else
{
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
}    

         } 
      } 

////Iron

      private class HartoTarget : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTarget( Szczypce item, Mobile from ) : base ( 2, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 

            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Iron && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z �elaza!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.S�ab;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                 }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Przeci�tn;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }
                                              
                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Zwyk�;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Dobr;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Doskona�;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Wspania�;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Wyj�tkow;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Niezwyk�;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                              /////Wymagany M�ot Kowalski
                                              if (from.FindItemOnLayer(Layer.OneHanded) is SmithHammer)
                                              {
                                                  BaseTool tool = from.FindItemOnLayer(Layer.OneHanded) as BaseTool;
                                                  tool.UsesRemaining -= 1;
                                                  if ( tool.UsesRemaining <= 0 )
                                                  {
                                                       tool.Delete();
                                                       from.SendMessage( 33, "M�ot rozpadla si�!" ); 
                                                  }

                                              if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                              {
                                              from.Animate( 9, 5, 1, true, false, 0 );
                                              }
                                              from.PlaySound(0x2A);
		                              bas.Quality = ArmorQuality.Cudown;

                                              }//Wymagany M�ot Kowalski Koniec
                                              else
                                              {
                                              from.SendMessage( 33, "Musisz trzyma� w r�ku m�ot kowalski!" ); 
                                              }
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////DullCopper

      private class HartoTargetDullCopper : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetDullCopper( Szczypce item, Mobile from ) : base ( 3, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.DullCopper && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z matowej miedzi!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////ShadowIron

      private class HartoTargetShadowIron : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetShadowIron( Szczypce item, Mobile from ) : base ( 4, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.ShadowIron && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z cienistego �elaza!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////Copper

      private class HartoTargetCopper : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetCopper( Szczypce item, Mobile from ) : base ( 5, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Copper && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z miedzi!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////Bronze

      private class HartoTargetBronze : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetBronze( Szczypce item, Mobile from ) : base ( 6, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Bronze && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z br�zu!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////Gold

      private class HartoTargetGold : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetGold( Szczypce item, Mobile from ) : base ( 7, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Gold && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany ze z�ota!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////Agapite

      private class HartoTargetAgapite : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetAgapite( Szczypce item, Mobile from ) : base ( 8, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Agapite && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z agapitu!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////Verite

      private class HartoTargetVerite : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetVerite( Szczypce item, Mobile from ) : base ( 9, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Verite && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z verytu!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

////Valorite

      private class HartoTargetValorite : Target 
      { 
			private Szczypce m_Item;
			private Mobile m_From;

         public HartoTargetValorite( Szczypce item, Mobile from ) : base ( 10, false, TargetFlags.None ) 
         { 
				m_Item = item;
				m_From = from;
         } 
          
         protected override void OnTarget( Mobile from, object target ) 
         { 
            if( target == from ) 
               from.SendMessage( "To narz�dzie do hartowania metalu a nie swojego ducha!" ); 

            if ( target is BaseArmor ) 
            { 
               BaseArmor bas = (BaseArmor)target;

if ( bas.Identified == false )
{
from.SendMessage( 33, "Najpierw to zidentyfikuj!" );
return;
}
if ( bas.Resource != CraftResource.Valorite && bas.Identified != false )
{
from.SendMessage( 33, "Przedmiot musi by� wykonany z valorytu!" );
return;
}
	                          if( bas.Quality == ArmorQuality.None )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 80)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.S�ab;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.S�ab )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 85)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Przeci�tn;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Przeci�tn )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 90)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Zwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Zwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 94)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Dobr;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Dobr )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 96)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Doskona�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Doskona� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 100)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wspania�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wspania� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 106)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Wyj�tkow;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Wyj�tkow )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 111)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Niezwyk�;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
	                          else if( bas.Quality == ArmorQuality.Niezwyk� )
                                  {
                                    if (from.Skills[SkillName.Kowalstwo].Value >= 120)
                                    {
                                      if ( from.Body.Type == BodyType.Human && !from.Mounted )
                                      {
                                           from.Animate( 9, 5, 1, true, false, 0 );
                                      }
                                      from.PlaySound(0x2A);
		                      bas.Quality = ArmorQuality.Cudown;
                                    }
                                    else
                                    {
                                      from.SendMessage( "Twoja umiej�tno�� - kowalstwo jest za ma�a!" ); 
                                    }
                                  }
            }
            else
            {
               from.SendMessage( "To nie jest w�a�ciwy cel." ); 
            }                             


         } 
      } 

        public static void CheckAnvilAndForgeH(Mobile from, int range, out bool anvil, out bool forge)
        {
            anvil = false;
            forge = false;

            Map map = from.Map;

            if (map == null)
            {
                return;
            }

            IPooledEnumerable eable = map.GetItemsInRange(from.Location, range);

            foreach (Item item in eable)
            {
                Type type = item.GetType();

                bool isAnvil = (item.ItemID == 4015 || item.ItemID == 4016 ||
                                item.ItemID == 0x2DD5 || item.ItemID == 0x2DD6);
                bool isForge = (item.ItemID == 4017 ||
                                (item.ItemID >= 6522 && item.ItemID <= 6569) || item.ItemID == 0x2DD8);

                if (!isAnvil && !isForge)
                {
                    continue;
                }

                if ((from.Z + 16) < item.Z || (item.Z + 16) < from.Z || !from.InLOS(item))
                {
                    continue;
                }

                anvil = anvil || isAnvil;
                forge = forge || isForge;

                if (anvil && forge)
                {
                    break;
                }
            }

            eable.Free();

            for (int x = -range; (!anvil || !forge) && x <= range; ++x)
            {
                for (int y = -range; (!anvil || !forge) && y <= range; ++y)
                {
                    var tiles = map.Tiles.GetStaticTiles(from.X + x, from.Y + y, true);

                    for (int i = 0; (!anvil || !forge) && i < tiles.Length; ++i)
                    {
                        int id = tiles[i].ID;

                        bool isAnvil = (id == 4015 || id == 4016 || id == 0x2DD5 || id == 0x2DD6);
                        bool isForge = (id == 4017 || (id >= 6522 && id <= 6569) || id == 0x2DD8);

                        if (!isAnvil && !isForge)
                        {
                            continue;
                        }

                        if ((from.Z + 16) < tiles[i].Z || (tiles[i].Z + 16) < from.Z ||
                            !from.InLOS(new Point3D(from.X + x, from.Y + y, tiles[i].Z + (tiles[i].Height / 2) + 1)))
                        {
                            continue;
                        }

                        anvil = anvil || isAnvil;
                        forge = forge || isForge;
                    }
                }
            }
        }


    }
}