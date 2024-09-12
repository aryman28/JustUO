using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    // G³ówna klasa Questa
    public class ProbaRycerzaCzesc1 : BaseQuest
    {
        public ProbaRycerzaCzesc1() : base()
        {
            // Usuniêto cele, aby quest opiera³ siê na quizie
        }

        // Tytu³ zadania
        public override object Title { get { return "Próba Rycerza Czêœæ 1"; } }

        // Opis zadania
        public override object Description { get { return "Aby zostaæ Rycerzem, musisz odpowiedzieæ poprawnie na kilka pytañ dotycz¹cych Twojej wiedzy o rycerstwie. Je¿eli odpowiesz prawid³owo otrzymasz ode mnie List polecaj¹cy który zaniesiesz Adlerowi. On ma dla ciebie kolejne zadanie. Powodzenia!"; } }

        // Wiadomoœæ odmowy
        public override object Refuse { get { return "Rozumiem, ¿e nie chcesz spróbowaæ teraz, ale zawsze mo¿esz wróciæ póŸniej."; } }

        // Wiadomoœæ o nieukoñczeniu zadania
        public override object Uncomplete { get { return "Jeszcze nie odpowiedzia³eœ poprawnie na wszystkie pytania."; } }

        // Wiadomoœæ o ukoñczeniu zadania
        public override object Complete { get { return "Gratulacje! Odpowiedzia³eœ poprawnie na wszystkie pytania i teraz mo¿esz nazywaæ siê Wojownikiem!"; } }

        // Metoda sprawdzaj¹ca, czy quest mo¿e byæ zaoferowany
        public override bool CanOffer()
        {
            // Sprawdzenie, czy gracz posiada przedmiot LORKnight w plecaku
            if (!HasLORKnight())
            {
                Owner.SendMessage("Musisz posiadaæ List polecaj¹cy w plecaku, aby rozpocz¹æ ten quest.");
                return false;
            }

            return true;
        }

        public override void OnAccept()
        {
            base.OnAccept();
            // Uruchomienie quizu po zaakceptowaniu questa
            Owner.SendGump(new QuizGump(Owner, this));
        }

        private bool HasLORKnight()
        {
            // Sprawdzenie, czy gracz posiada przedmiot LORKnight w plecaku
            Item lorknight = Owner.Backpack.FindItemByType(typeof(LORKnight));
            return lorknight != null;
        }

        private void DeleteLORKnight()
        {
            // Usuniêcie przedmiotu LORKnight z plecaka
            Item lorknight = Owner.Backpack.FindItemByType(typeof(LORKnight));
            if (lorknight != null)
            {
                lorknight.Delete();
            }
        }

        public override void GiveRewards()
        {
            // Usuniêcie przedmiotu LORKnight po ukoñczeniu questa
            DeleteLORKnight();

            // Przekazanie listu polecaj¹cego graczowi po ukoñczeniu quizu
            Item letter = new ListAdler();
            if (!Owner.AddToBackpack(letter))
                letter.MoveToWorld(Owner.Location, Owner.Map);

            Owner.SendMessage("Gratulacje! Otrzyma³eœ List Polecaj¹cy od Aldera.");

            base.GiveRewards();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // wersja
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }

    // Nowy Gump z quizem
    public class QuizGump : Gump
    {
        private Mobile m_Player;
        private ProbaRycerzaCzesc1 m_Quest;
        private int m_QuestionIndex;

        public QuizGump(Mobile player, ProbaRycerzaCzesc1 quest, int questionIndex = 0) : base(50, 50)
        {
            m_Player = player;
            m_Quest = quest;
            m_QuestionIndex = questionIndex;

            AddBackground(0, 0, 350, 200, 9270);
            AddLabel(100, 20, 1152, "Znajomoœc kodeksu Rycerskiego: Pytania " + (m_QuestionIndex + 1));

            string[] questions = new string[]
            {
                "Jaka jest najwa¿niejsza cecha Rycerza?",
                "Co nie pozwala Rycerzowi z³amaæ zasad kodeksu?",
                "O jednym z których grzechów mówi kodeks ?",
                "Co jest twoim towarzyszem w walce ze z³em?",
                "Kto kieruje trwymi poczynaniami?"
            };

            string[] answer1 = new string[]
            {
                "Wiara",
                "M¹droœæ",
                "Morderstwo",
                "Miecz",
                "Kodeks"
            };

            string[] answer2 = new string[]
            {
                "Honor",
                "Honor",
                "G³upota",
                "Sprawiedliwoœæ",
                "Bóg"
            };

            string[] answer3 = new string[]
            {
                "Mêstwo",
                "Zasady",
                "Pycha",
                "Duma",
                "Wiara"
            };

            AddLabel(50, 70, 1153, questions[m_QuestionIndex]);
            AddButton(50, 100, 4023, 4024, 1, GumpButtonType.Reply, 0);
            AddLabel(85, 100, 1153, answer1[m_QuestionIndex]);
            AddButton(50, 130, 4023, 4024, 2, GumpButtonType.Reply, 0);
            AddLabel(85, 130, 1153, answer2[m_QuestionIndex]);
            AddButton(50, 160, 4023, 4024, 3, GumpButtonType.Reply, 0);
            AddLabel(85, 160, 1153, answer3[m_QuestionIndex]);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Player == null || m_Player.Deleted)
                return;

            bool correct = false;

            switch (m_QuestionIndex)
            {
                case 0:
                    correct = (info.ButtonID == 1); // Poprawna odpowiedŸ dla pytania 1
                    break;
                case 1:
                    correct = (info.ButtonID == 2); // Poprawna odpowiedŸ dla pytania 2
                    break;
                case 2:
                    correct = (info.ButtonID == 3); // Poprawna odpowiedŸ dla pytania 3
                    break;
                case 3:
                    correct = (info.ButtonID == 2); // Poprawna odpowiedŸ dla pytania 4
                    break;
                case 4:
                    correct = (info.ButtonID == 2); // Poprawna odpowiedŸ dla pytania 5
                    break;
            }

            if (correct)
            {
                if (m_QuestionIndex < 4)
                {
                    m_Player.SendGump(new QuizGump(m_Player, m_Quest, m_QuestionIndex + 1));
                }
                else
                {
                    m_Quest.GiveRewards();
                }
            }
            else
            {
                m_Player.SendMessage("Niepoprawna odpowiedŸ! Spróbuj jeszcze raz.");
                m_Player.SendGump(new QuizGump(m_Player, m_Quest));
            }
        }
    }

    // List Polecaj¹cy
    public class ListAdler : Item
    {
        [Constructable]
        public ListAdler() : base(0x14F0)
        {
            Name = "List Polecaj¹cy dla Adlera";
            Hue = 1153;
            LootType = LootType.Blessed;
            Movable = false;
        }

        public ListAdler(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }    
}
