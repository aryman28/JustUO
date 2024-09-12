using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    public class ProbaGiermka : BaseQuest
    {
        public ProbaGiermka() : base()
        {
            //The player must slay 10 Skeleton
            this.AddObjective(new SlayObjective(typeof(Skeleton), "Skeleton", 10));
            //The player must slay 10 Zombie
            this.AddObjective(new SlayObjective(typeof(Zombie), "Zombie", 10));
            //The player must slay 1 Lich
            this.AddObjective(new SlayObjective(typeof(Lich), "Lich", 1));
            //Reward the Player Gold
            this.AddReward(new BaseReward("5000-5000 Gold"));
        }

        //Quest Title
        public override object Title { get { return "Proba Giermka"; } }
        //Quest Description
        public override object Description { get { return "Witaj drogi w�drowcze. Rad jestem, �e chcesz podj�� si� pr�by m�stwa i odwagi, aby sta� si� Giermkiem. Aby tak si� sta�o, b�dziesz musia� spe�ni� kilka warunk�w:"; } }
        //Quest Refuse Message
        public override object Refuse { get { return "Przykro mi, �e nie chcesz spr�bowa�, ale zawsze mo�esz wr�ci� do mnie z powrotem."; } }
        //Quest Uncompleted Message
        public override object Uncomplete { get { return "Nie wykona�e� jeszcze wszystkich cel�w misji."; } }
        //Quest Completed Message
        public override object Complete { get { return "Gratulacje! Uda�o Ci si� spe�ni� wszystkie warunki. Od teraz mo�esz nazywa� si� Giermkiem!"; } }

        public override void GiveRewards()
        {
            //Give Gold to player in form of a bank check
            BankCheck gold = new BankCheck(Utility.RandomMinMax(5000, 5000));
            if (!Owner.AddToBackpack(gold))
                gold.MoveToWorld(Owner.Location, Owner.Map);

            // Show a gump to allow the player to choose a letter
            Owner.SendGump(new ChooseLetterGump(Owner));

            base.GiveRewards();
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
    }

    // Gump to choose the letter of recommendation
    public class ChooseLetterGump : Gump
    {
        private Mobile m_Player;

        public ChooseLetterGump(Mobile player) : base(50, 50)
        {
            m_Player = player;

            AddBackground(0, 0, 300, 150, 9270);
            AddLabel(50, 20, 1152, "Wybierz List Polecaj�cy:");
            AddButton(50, 70, 4023, 4024, 1, GumpButtonType.Reply, 0);
            AddLabel(85, 70, 1153, "Rycerz");
            AddButton(50, 100, 4023, 4024, 2, GumpButtonType.Reply, 0);
            AddLabel(85, 100, 1153, "M�ciciel");
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            if (m_Player == null || m_Player.Deleted)
                return;

            PlayerMobile player = m_Player as PlayerMobile;

            Item letter = null;
            Item additionalItem = null; // Zmienna do dodatkowego przedmiotu

            switch (info.ButtonID)
            {
                case 1:
                    // Player chose Knight
                    letter = new LORKnight();
                    additionalItem = new KodeksRycerza(); // Dodanie Kodeksu Rycerza
                    player.Klasa = Klasa.Giermek; // Ustawienie klasy na Wojownik
                    player.DisplayKlasaTitle = true;
                    m_Player.SendMessage("Zosta�e� Giermkiem!");
                    break;
                case 2:
                    // Player chose Avenger
                    letter = new LORAvenger();
                    additionalItem = new KodeksUpadlegoRycerza(); // Dodanie Kodeksu Upad�ego Rycerza
                    player.Klasa = Klasa.Giermek; // Ustawienie klasy na Wojownik
                    player.DisplayKlasaTitle = true;
                    m_Player.SendMessage("Zosta�e� Giermkiem!");
                    break;
                default:
                    m_Player.SendMessage("Nie wybra�e� �adnej opcji.");
                    return;
            }

            if (letter != null)
            {
                // Ensure the item is blessed and cannot be removed from the backpack
                letter.LootType = LootType.Blessed;
                letter.Movable = false;

                if (!m_Player.AddToBackpack(letter))
                    letter.MoveToWorld(m_Player.Location, m_Player.Map);
            }

            if (additionalItem != null)
            {
                // Dodatkowy przedmiot jest r�wnie� dodawany do ekwipunku gracza
                additionalItem.LootType = LootType.Blessed;
                additionalItem.Movable = false;

                if (!m_Player.AddToBackpack(additionalItem))
                    additionalItem.MoveToWorld(m_Player.Location, m_Player.Map);
            }
        }
    }

    // Class representing the Letter of Recommendation for Knight
    public class LORKnight : Item
    {
        [Constructable]
        public LORKnight() : base(0x14F0) // Using the Scroll item ID as the base
        {
            Name = "List Polecaj�cy: Rycerz";
            Hue = 1153; // Color of the letter, you can change the hue value
            LootType = LootType.Blessed; // Item is blessed
            Movable = false; // Item cannot be moved from the backpack
        }

        public LORKnight(Serial serial) : base(serial)
        {
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
    }

    // Class representing the Letter of Recommendation for Avenger
    public class LORAvenger : Item
    {
        [Constructable]
        public LORAvenger() : base(0x14F0) // Using the Scroll item ID as the base
        {
            Name = "List Polecaj�cy: M�ciciel";
            Hue = 1154; // Color of the letter, you can change the hue value
            LootType = LootType.Blessed; // Item is blessed
            Movable = false; // Item cannot be moved from the backpack
        }

        public LORAvenger(Serial serial) : base(serial)
        {
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
    }
}
