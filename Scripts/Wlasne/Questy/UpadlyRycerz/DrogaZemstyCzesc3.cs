using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    // G³ówna klasa Questa
    public class DrogaZemstyCzesc3 : BaseQuest
    {
        public DrogaZemstyCzesc3() : base()
        {
            // Dodanie celów questa
            AddObjective(new ObtainObjective(typeof(KrwawyKlejnot), "Zdobyæ Krwawy Klejnot", 1));
            AddObjective(new SlayObjective(typeof(DuchMsciciela), "Pokonaj 15 Duchów Mœciciela", 15));
            AddObjective(new EscortObjective("Opêtany Mistrz Miecza", 1));
        }

        // Tytu³ zadania
        public override object Title { get { return "Droga Zemsty - Czêœæ 3"; } }

        // Opis zadania
        public override object Description 
        { 
            get 
            { 
                return "Twoja zemsta jest prawie kompletna, ale musisz jeszcze pokonaæ najpotê¿niejszych wrogów i zdobyæ ostateczne artefakty. W rêkach losu spoczywa teraz Twoje przeznaczenie. Czy odwa¿ysz siê podj¹æ ostatni krok na tej mrocznej œcie¿ce?"; 
            } 
        }

        // Wiadomoœæ odmowy
        public override object Refuse 
        { 
            get 
            { 
                return "Strach parali¿uje Twoje serce? Musisz byæ gotowy na wszystko, jeœli chcesz kontynuowaæ."; 
            } 
        }

        // Wiadomoœæ o nieukoñczeniu zadania
        public override object Uncomplete 
        { 
            get 
            { 
                return "Brakuje Ci jeszcze kilku kroków do zakoñczenia swojej zemsty. Wróæ, gdy wszystkie cele zostan¹ osi¹gniête."; 
            } 
        }

        // Wiadomoœæ o ukoñczeniu zadania
        public override object Complete 
        { 
            get 
            { 
                return "Uda³o siê! Twoja zemsta jest teraz bliska. Twoja podró¿ dobiega koñca, a Twoja dusza ulega przemianie."; 
            } 
        }

        // Metoda sprawdzaj¹ca, czy quest mo¿e byæ zaoferowany
        public override bool CanOffer()
        {
            // Sprawdzenie, czy gracz posiada przedmiot Krwawy Amulet w plecaku
            if (!HasKrwawyAmulet())
            {
                Owner.SendMessage("Musisz posiadaæ Krwawy Amulet w plecaku, aby rozpocz¹æ ten quest.");
                return false;
            }

            return true;
        }

        public override void OnAccept()
        {
            base.OnAccept();
            Owner.SendMessage("Rozpocz¹³eœ ostatnie zadanie na drodze zemsty.");
        }

        private bool HasKrwawyAmulet()
        {
            // Sprawdzenie, czy gracz posiada przedmiot Krwawy Amulet w plecaku
            Item krwawyAmulet = Owner.Backpack.FindItemByType(typeof(KrwawyAmulet));
            return krwawyAmulet != null;
        }

        public override void GiveRewards()
        {
            // Przyznanie nagrody za ukoñczenie questa
            Item BookOfChivalry = new BookOfChivalry();
            if (!Owner.AddToBackpack(BookOfChivalry))
                BookOfChivalry.MoveToWorld(Owner.Location, Owner.Map);

            // Zmiana klasy gracza na "Mœciciel"
            Owner.Klasa = Klasa.Mœciciel;
            Owner.SendMessage("Gratulacje! Otrzyma³eœ Ksiêgê Rycerskoœci, a Twoja klasa zosta³a zmieniona na Mœciciel.");

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

    // Duch Mœciciela
    public class DuchMsciciela : BaseCreature
    {
        [Constructable]
        public DuchMsciciela() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Duch Mœciciela";
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

    // Opêtany Mistrz Miecza
    public class OpetanyMistrzMiecza : BaseCreature
    {
        [Constructable]
        public OpetanyMistrzMiecza() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Opêtany Mistrz Miecza";
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
