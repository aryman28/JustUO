/* Modified by Talow */
using System; 
using System.Collections; 
using Server; 
using Server.Items; 
using Server.Network; 
using Server.Spells;
using Server.Gumps;
using Server.Prompts;

namespace Server.Gumps
{
    public class DruidSpellbookGump : Gump
    {
        private DruidSpellbook m_Book;

        int gth = 0x903;
        private void AddBackground()
        {
            AddPage(0);
            AddImage(0, 0, 0x89B, 0);
            AddLabel(60, 10, gth, "Magia Natury");
        }

        public bool HasSpell(Mobile from, int spellID)
        {
            return (m_Book.HasSpell(spellID));
        }


        public DruidSpellbookGump(Mobile from, DruidSpellbook book) : base(150, 200)
        {
            m_Book = book;
            AddBackground();

            var sbtn = 0x93A;
            var sbtn1 = 0x7585;
            var page = 1;

            for (var i = 0; i < m_Book.BookCount; i++)
            {
                if (i % 14 == 0)
                {
                    AddPage(page++);
                    if (page > 2)
                        AddButton(23, 5, 0x89D, 0x89D, m_Book.BookCount + 1, GumpButtonType.Page, page - 2);

                    AddButton(296, 4, 0x89E, 0x89E, m_Book.BookCount + 2, GumpButtonType.Page, page);
                }

                if (HasSpell(from, i + m_Book.BookOffset))
                {
                    var spell = SpellRegistry.NewSpell(i + m_Book.BookOffset, from, null);

                    AddLabel((i / 7) % 2 == 0 ? 45 : 215, 30 + (22 * (i % 7)), gth, spell.Name);
                    AddButton((i / 7) % 2 == 0 ? 28 : 195, 33 + (22 * (i % 7)), sbtn, sbtn, i + 1, GumpButtonType.Reply, 1);
                    AddButton(183, 179, sbtn1, sbtn1, 19, GumpButtonType.Reply, 0 );
                    AddLabel(206, 181, gth, @"Poka¿ pasek zaklêæ");
                }
            }

            for (var i = 0; i < m_Book.BookCount; i++)
            {
                if (HasSpell(from, i + m_Book.BookOffset))
                {
                    var spell = SpellRegistry.NewSpell(i + m_Book.BookOffset, from, null);

                    AddPage(page++);
                    AddButton(23, 5, 0x89D, 0x89D, m_Book.BookCount + 1, GumpButtonType.Page, page - 2);

                    if (i < m_Book.BookCount - 1)
                        AddButton(296, 4, 0x89E, 0x89E, m_Book.BookCount + 2, GumpButtonType.Page, page);

                    AddLabel(50, 27, gth, spell.Info.Name);
                    
                    AddLabel(195, 27, gth, "Sk³adniki:");
                    for (var r = 0; r < spell.Reagents.Length; r++)
                    {
                        AddLabel(195, 47 + (r * 20), gth, spell.Reagents[r].Name);
                    }

                    var dspell = spell as Spells.Druid.DruidSpell;
                    if (dspell != null)
                    {
                        AddHtml(30, 49, 123, 132, dspell.SpellDescription, false, false);
                        AddLabel(195, 157, gth, "Min. Umiej.: " + dspell.RequiredSkill);
                        AddLabel(195, 177, gth, "Min. Mana: " + dspell.RequiredMana);
                    }
                }
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            Mobile from = state.Mobile;
            if (info.ButtonID == 0)
                return;
            else if (info.ButtonID == 19)
            {
                from.CloseGump( typeof( MagiaNaturyScrollGump  ) );
                from.SendGump( new MagiaNaturyScrollGump ( from, new MagiaNaturyScroll() ) );
                return;
            }


            var spell = SpellRegistry.NewSpell(info.ButtonID - 1 + m_Book.BookOffset, from, null);
            if (spell != null)
                spell.Cast();
        }

        }
}
