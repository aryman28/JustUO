using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    // G��wna klasa Questa
    public class ProbaRycerzaCzesc1 : BaseQuest
    {
        public ProbaRycerzaCzesc1() : base()
        {
            // Usuni�to cele, aby quest opiera� si� na quizie
        }

        // Tytu� zadania
        public override object Title { get { return "Pr�ba Rycerza Cz�� 1"; } }

        // Opis zadania
        public override object Description { get { return "Aby zosta� Rycerzem, musisz odpowiedzie� poprawnie na kilka pyta� dotycz�cych Twojej wiedzy o rycerstwie. Je�eli odpowiesz prawid�owo otrzymasz ode mnie List polecaj�cy kt�ry zaniesiesz Adlerowi. On ma dla ciebie kolejne zadanie. Powodzenia!"; } }

        // Wiadomo�� odmowy
        public override object Refuse { get { return "Rozumiem, �e nie chcesz spr�bowa� teraz, ale zawsze mo�esz wr�ci� p�niej."; } }

        // Wiadomo�� o nieuko�czeniu zadania
        public override object Uncomplete { get { return "Jeszcze nie odpowiedzia�e� poprawnie na wszystkie pytania."; } }

        // Wiadomo�� o uko�czeniu zadania
        public override object Complete { get { return "Gratulacje! Odpowiedzia�e� poprawnie na wszystkie pytania i teraz mo�esz nazywa� si� Wojownikiem!"; } }

        // Metoda sprawdzaj�ca, czy quest mo�e by� zaoferowany
        public override bool CanOffer()
        {
            // Sprawdzenie, czy gracz posiada przedmiot LORKnight w plecaku
            if (!HasLORKnight())
            {
                Owner.SendMessage("Musisz posiada� List polecaj�cy w plecaku, aby rozpocz�� ten quest.");
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
            // Usuni�cie przedmiotu LORKnight z plecaka
            Item lorknight = Owner.Backpack.FindItemByType(typeof(LORKnight));
            if (lorknight != null)
            {
                lorknight.Delete();
            }
        }

        public override void GiveRewards()
        {
            // Usuni�cie przedmiotu LORKnight po uko�czeniu questa
            DeleteLORKnight();

            // Przekazanie listu polecaj�cego graczowi po uko�czeniu quizu
            Item letter = new ListAdler();
            if (!Owner.AddToBackpack(letter))
                letter.MoveToWorld(Owner.Location, Owner.Map);

            Owner.SendMessage("Gratulacje! Otrzyma�e� List Polecaj�cy od Aldera.");

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
            AddLabel(100, 20, 1152, "Znajomo�c kodeksu Rycerskiego: Pytania " + (m_QuestionIndex + 1));

            string[] questions = new string[]
            {
                "Jaka jest najwa�niejsza cecha Rycerza?",
                "Co nie pozwala Rycerzowi z�ama� zasad kodeksu?",
                "O jednym z kt�rych grzech�w m�wi kodeks ?",
                "Co jest twoim towarzyszem w walce ze z�em?",
                "Kto kieruje trwymi poczynaniami?"
            };

            string[] answer1 = new string[]
            {
                "Wiara",
                "M�dro��",
                "Morderstwo",
                "Miecz",
                "Kodeks"
            };

            string[] answer2 = new string[]
            {
                "Honor",
                "Honor",
                "G�upota",
                "Sprawiedliwo��",
                "B�g"
            };

            string[] answer3 = new string[]
            {
                "M�stwo",
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
                    correct = (info.ButtonID == 1); // Poprawna odpowied� dla pytania 1
                    break;
                case 1:
                    correct = (info.ButtonID == 2); // Poprawna odpowied� dla pytania 2
                    break;
                case 2:
                    correct = (info.ButtonID == 3); // Poprawna odpowied� dla pytania 3
                    break;
                case 3:
                    correct = (info.ButtonID == 2); // Poprawna odpowied� dla pytania 4
                    break;
                case 4:
                    correct = (info.ButtonID == 2); // Poprawna odpowied� dla pytania 5
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
                m_Player.SendMessage("Niepoprawna odpowied�! Spr�buj jeszcze raz.");
                m_Player.SendGump(new QuizGump(m_Player, m_Quest));
            }
        }
    }

    // List Polecaj�cy
    public class ListAdler : Item
    {
        [Constructable]
        public ListAdler() : base(0x14F0)
        {
            Name = "List Polecaj�cy dla Adlera";
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
