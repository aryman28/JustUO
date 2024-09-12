using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    // G³ówna klasa Questa
    public class UpadlyRycerzZemsta : BaseQuest
    {
        public UpadlyRycerzZemsta() : base()
        {
            // Usuniêto cele, aby quest opiera³ siê na quizie
        }

        // Tytu³ zadania
        public override object Title { get { return "Droga Zemsty - Czêœæ 1"; } }

        // Opis zadania
        public override object Description 
        { 
            get 
            { 
                return "Twoje ¿ycie zosta³o zniszczone przez zdradê i krew. Teraz, gdy honor i cnoty ju¿ nie maj¹ znaczenia, jedynie zemsta ciê napêdza. Aby w pe³ni oddaæ siê tej drodze, musisz udowodniæ, ¿e jesteœ gotów porzuciæ wszystko, co kiedyœ by³o dla ciebie œwiête. Odpowiedz na pytania dotycz¹ce twojego nowego, mrocznego kodeksu."; 
            } 
        }

        // Wiadomoœæ odmowy
        public override object Refuse 
        { 
            get 
            { 
                return "Uciekasz przed przeznaczeniem? Zemsta nie czeka na nikogo."; 
            } 
        }

        // Wiadomoœæ o nieukoñczeniu zadania
        public override object Uncomplete 
        { 
            get 
            { 
                return "Jeszcze nie opanowa³eœ nowej drogi. Odpowiedz poprawnie na wszystkie pytania, aby staæ siê narzêdziem zemsty."; 
            } 
        }

        // Wiadomoœæ o ukoñczeniu zadania
        public override object Complete 
        { 
            get 
            { 
                return "Gratulacje! Porzuci³eœ to, co kiedyœ by³o dla ciebie wa¿ne, i teraz jesteœ w pe³ni gotów do zemsty. Udaj siê z mieczem do Daertha on dalej cie poprowadzi!"; 
            } 
        }

        // Metoda sprawdzaj¹ca, czy quest mo¿e byæ zaoferowany
        public override bool CanOffer()
        {
            // Sprawdzenie, czy gracz posiada przedmiot LORAvenger w plecaku
            if (!HasLORAvenger())
            {
                Owner.SendMessage("Musisz posiadaæ List Polecajacy w plecaku, aby rozpocz¹æ ten quest.");
                return false;
            }

            return true;
        }

        public override void OnAccept()
        {
            base.OnAccept();
            // Uruchomienie quizu po zaakceptowaniu questa
            Owner.SendGump(new QuizGump1(Owner, this));
        }

        private bool HasLORAvenger()
        {
            // Sprawdzenie, czy gracz posiada przedmiot LORAvenger w plecaku
            Item LORAvenger = Owner.Backpack.FindItemByType(typeof(LORAvenger));
            return LORAvenger != null;
        }

        private void DeleteLORAvenger()
        {
            // Usuniêcie przedmiotu LORAvenger z plecaka
            Item LORAvenger = Owner.Backpack.FindItemByType(typeof(LORAvenger));
            if (LORAvenger != null)
            {
                LORAvenger.Delete();
            }
        }

        public override void GiveRewards()
        {
            // Usuniêcie przedmiotu LORAvenger po ukoñczeniu questa
            DeleteLORAvenger();

            // Przekazanie Miecza Zemsty graczowi po ukoñczeniu quizu
            Item sword = new MieczZemsty();
            if (!Owner.AddToBackpack(sword))
                sword.MoveToWorld(Owner.Location, Owner.Map);

            Owner.SendMessage("Gratulacje! Otrzyma³eœ Miecz Zemsty.");

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
    public class QuizGump1 : Gump
    {
        private Mobile m_Player;
        private UpadlyRycerzZemsta m_Quest;
        private int m_QuestionIndex;

        public QuizGump1(Mobile player, UpadlyRycerzZemsta quest, int questionIndex = 0) : base(50, 50)
        {
            m_Player = player;
            m_Quest = quest;
            m_QuestionIndex = questionIndex;

            AddBackground(0, 0, 350, 200, 9270);
            AddLabel(100, 20, 1152, "Droga Zemsty: Pytania " + (m_QuestionIndex + 1));

            string[] questions = new string[]
            {
                "Co napêdza teraz twoje kroki?",
                "Co jest teraz twoim przewodnikiem?",
                "Co przetrwa w obliczu z³a?",
                "Co prowadzi twoje czyny?",
                "Co stanie siê z tymi, którzy stan¹ ci na drodze?"
            };

            string[] answer1 = new string[]
            {
                "Zemsta",
                "W³asne pragnienie zemsty",
                "Determinacja",
                "Gniew",
                "Œmieræ"
            };

            string[] answer2 = new string[]
            {
                "Honor",
                "Wiara",
                "M¹droœæ",
                "Sprawiedliwoœæ",
                "Ucieczka"
            };

            string[] answer3 = new string[]
            {
                "Sprawiedliwoœæ",
                "Sumienie",
                "Dobroæ",
                "Mi³osierdzie",
                "Pokój"
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
                    correct = (info.ButtonID == 1); // Poprawna odpowiedŸ dla pytania 2
                    break;
                case 2:
                    correct = (info.ButtonID == 1); // Poprawna odpowiedŸ dla pytania 3
                    break;
                case 3:
                    correct = (info.ButtonID == 1); // Poprawna odpowiedŸ dla pytania 4
                    break;
                case 4:
                    correct = (info.ButtonID == 1); // Poprawna odpowiedŸ dla pytania 5
                    break;
            }

            if (correct)
            {
                if (m_QuestionIndex < 4)
                {
                    m_Player.SendGump(new QuizGump1(m_Player, m_Quest, m_QuestionIndex + 1));
                }
                else
                {
                    m_Quest.GiveRewards();
                }
            }
            else
            {
                m_Player.SendMessage("Niepoprawna odpowiedŸ! Spróbuj jeszcze raz.");
                m_Player.SendGump(new QuizGump1(m_Player, m_Quest));
            }
        }
    }

    // Miecz Zemsty
    public class MieczZemsty : Item
    {
        [Constructable]
        public MieczZemsty() : base(0x13B9)
        {
            Name = "Miecz Zemsty [Zanieœ do Daertha]";
            Hue = 1175;
            LootType = LootType.Blessed;
            Movable = false;
        }

        public MieczZemsty(Serial serial) : base(serial)
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
