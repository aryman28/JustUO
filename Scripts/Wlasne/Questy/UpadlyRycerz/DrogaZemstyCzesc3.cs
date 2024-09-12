using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    // G��wna klasa Questa
    public class DrogaZemstyCzesc3 : BaseQuest
    {
        public DrogaZemstyCzesc3() : base()
        {
            // Dodanie cel�w questa
            AddObjective(new ObtainObjective(typeof(KrwawyKlejnot), "Zdoby� Krwawy Klejnot", 1));
            AddObjective(new SlayObjective(typeof(DuchMsciciela), "Pokonaj 15 Duch�w M�ciciela", 15));
            AddObjective(new EscortObjective("Op�tany Mistrz Miecza", 1));
        }

        // Tytu� zadania
        public override object Title { get { return "Droga Zemsty - Cz�� 3"; } }

        // Opis zadania
        public override object Description 
        { 
            get 
            { 
                return "Twoja zemsta jest prawie kompletna, ale musisz jeszcze pokona� najpot�niejszych wrog�w i zdoby� ostateczne artefakty. W r�kach losu spoczywa teraz Twoje przeznaczenie. Czy odwa�ysz si� podj�� ostatni krok na tej mrocznej �cie�ce?"; 
            } 
        }

        // Wiadomo�� odmowy
        public override object Refuse 
        { 
            get 
            { 
                return "Strach parali�uje Twoje serce? Musisz by� gotowy na wszystko, je�li chcesz kontynuowa�."; 
            } 
        }

        // Wiadomo�� o nieuko�czeniu zadania
        public override object Uncomplete 
        { 
            get 
            { 
                return "Brakuje Ci jeszcze kilku krok�w do zako�czenia swojej zemsty. Wr��, gdy wszystkie cele zostan� osi�gni�te."; 
            } 
        }

        // Wiadomo�� o uko�czeniu zadania
        public override object Complete 
        { 
            get 
            { 
                return "Uda�o si�! Twoja zemsta jest teraz bliska. Twoja podr� dobiega ko�ca, a Twoja dusza ulega przemianie."; 
            } 
        }

        // Metoda sprawdzaj�ca, czy quest mo�e by� zaoferowany
        public override bool CanOffer()
        {
            // Sprawdzenie, czy gracz posiada przedmiot Krwawy Amulet w plecaku
            if (!HasKrwawyAmulet())
            {
                Owner.SendMessage("Musisz posiada� Krwawy Amulet w plecaku, aby rozpocz�� ten quest.");
                return false;
            }

            return true;
        }

        public override void OnAccept()
        {
            base.OnAccept();
            Owner.SendMessage("Rozpocz��e� ostatnie zadanie na drodze zemsty.");
        }

        private bool HasKrwawyAmulet()
        {
            // Sprawdzenie, czy gracz posiada przedmiot Krwawy Amulet w plecaku
            Item krwawyAmulet = Owner.Backpack.FindItemByType(typeof(KrwawyAmulet));
            return krwawyAmulet != null;
        }

        public override void GiveRewards()
        {
            // Przyznanie nagrody za uko�czenie questa
            Item BookOfChivalry = new BookOfChivalry();
            if (!Owner.AddToBackpack(BookOfChivalry))
                BookOfChivalry.MoveToWorld(Owner.Location, Owner.Map);

            // Zmiana klasy gracza na "M�ciciel"
            Owner.Klasa = Klasa.M�ciciel;
            Owner.SendMessage("Gratulacje! Otrzyma�e� Ksi�g� Rycersko�ci, a Twoja klasa zosta�a zmieniona na M�ciciel.");

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

    // Krwawy Klejnot
    public class KrwawyKlejnot : Item
    {
        [Constructable]
        public KrwawyKlejnot() : base(0x1F19)
        {
            Name = "Krwawy Klejnot";
            Hue = 1365;
        }

        public KrwawyKlejnot(Serial serial) : base(serial)
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

    // Duch M�ciciela
    public class DuchMsciciela : BaseCreature
    {
        [Constructable]
        public DuchMsciciela() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Duch M�ciciela";
            Body = 0x190;
            BaseSoundID = 0x482;

            SetStr(300);
            SetDex(90);
            SetInt(100);

            SetHits(600);
            SetMana(100);
            SetStam(50);

            SetDamage(10, 15);
            SetSkill(SkillName.Magia, 90.0);
            //SetSkill(SkillName.MagicResist, 70.0);
            SetSkill(SkillName.Taktyka, 80.0);
            SetSkill(SkillName.Anatomia, 60.0);

            Fame = 5000;
            Karma = -25000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public DuchMsciciela(Serial serial) : base(serial)
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

    // Op�tany Mistrz Miecza
    public class OpetanyMistrzMiecza : BaseCreature
    {
        [Constructable]
        public OpetanyMistrzMiecza() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Op�tany Mistrz Miecza";
            Body = 0x190;
            Hue = Utility.RandomSkinHue();
            Blessed = true;

            SetStr(350);
            SetDex(100);
            SetInt(90);

            SetHits(700);
            SetMana(90);
            SetStam(70);

            SetDamage(12, 18);
            SetSkill(SkillName.WalkaMieczami, 95.0);
            SetSkill(SkillName.Taktyka, 85.0);
            SetSkill(SkillName.Parowanie, 80.0);
        }

        public override void OnThink()
        {
            base.OnThink();
            // Dodatkowa logika NPC, np. pomoc w walce z potworami
        }

        public OpetanyMistrzMiecza(Serial serial) : base(serial)
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
