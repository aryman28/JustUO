using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.Quests
{
    // G³ówna klasa Questa
    public class DrogaZemstyCzesc2 : BaseQuest
    {
        public DrogaZemstyCzesc2() : base()
        {
            // Dodanie celów questa
            AddObjective(new ObtainObjective(typeof(SerceCienia), "ZnajdŸ Serce Cienia", 1));
            AddObjective(new SlayObjective(typeof(CienistaPostac), "Pokonaj 10 Cienistych Postaci", 10));
            AddObjective(new EscortObjective("Zdradzony Kaplan", 1));
        }

        // Tytu³ zadania
        public override object Title { get { return "Droga Zemsty - Czêœæ 2"; } }

        // Opis zadania
        public override object Description 
        { 
            get 
            { 
                return "Po tym, jak w pe³ni odda³eœ siê zemœcie, nadszed³ czas na kolejny krok. Musisz stawiæ czo³a swoim demonom, zebraæ niezbêdne artefakty i przelaæ krew, aby dowieœæ swojej gotowoœci. Czy masz doœæ si³y, by kontynuowaæ tê mroczn¹ œcie¿kê?"; 
            } 
        }

        // Wiadomoœæ odmowy
        public override object Refuse 
        { 
            get 
            { 
                return "Zawaha³eœ siê? Droga zemsty wymaga pe³nego oddania."; 
            } 
        }

        // Wiadomoœæ o nieukoñczeniu zadania
        public override object Uncomplete 
        { 
            get 
            { 
                return "Nie wszystkie cele zosta³y osi¹gniête. Wróæ, gdy wype³nisz swoje przeznaczenie."; 
            } 
        }

        // Wiadomoœæ o ukoñczeniu zadania
        public override object Complete 
        { 
            get 
            { 
                return "Doskonale! Osi¹gn¹³eœ wszystko, co by³o wymagane. Zemsta jest blisko."; 
            } 
        }

        // Metoda sprawdzaj¹ca, czy quest mo¿e byæ zaoferowany
        public override bool CanOffer()
        {
            // Sprawdzenie, czy gracz posiada przedmiot Miecz Zemsty w plecaku
            if (!HasMieczZemsty())
            {
                Owner.SendMessage("Musisz posiadaæ Miecz Zemsty w plecaku, aby rozpocz¹æ ten quest.");
                return false;
            }

            return true;
        }

        public override void OnAccept()
        {
            base.OnAccept();
            // Rozpoczêcie Questa
            Owner.SendMessage("Rozpoczê³o siê twoje nastêpne zadanie na drodze zemsty.");
        }

        private bool HasMieczZemsty()
        {
            // Sprawdzenie, czy gracz posiada przedmiot Miecz Zemsty w plecaku
            Item MieczZemsty = Owner.Backpack.FindItemByType(typeof(MieczZemsty));
            return MieczZemsty != null;
        }

        public override void GiveRewards()
        {       
            // Przyznanie nagrody za ukoñczenie questa
            Item shadowArmor = new ShadowArmor();
            Item krwawyAmulet = new KrwawyAmulet();

            // Dodanie Zbroi Cienia do plecaka gracza, jeœli jest miejsce
            if (!Owner.AddToBackpack(shadowArmor))
                shadowArmor.MoveToWorld(Owner.Location, Owner.Map);

            // Dodanie Krwawego Amuletu do plecaka gracza, jeœli jest miejsce
            if (!Owner.AddToBackpack(krwawyAmulet))
                krwawyAmulet.MoveToWorld(Owner.Location, Owner.Map);

            // Wiadomoœæ do gracza
            Owner.SendMessage("Gratulacje! Otrzyma³eœ Zbrojê Cienia oraz Krwawy Amulet.");

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

    // Serce Cienia
    public class SerceCienia : Item
    {
        [Constructable]
        public SerceCienia() : base(0x1F18)
        {
            Name = "Serce Cienia";
            Hue = 1175;
        }

        public SerceCienia(Serial serial) : base(serial)
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

    // Zbroja Cienia
    public class ShadowArmor : Item
    {
        [Constructable]
        public ShadowArmor() : base(0x1415)
        {
            Name = "Zbroja Cienia";
            Hue = 1109;
            LootType = LootType.Blessed;
        }

        public ShadowArmor(Serial serial) : base(serial)
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

    // Cienista Postac
    public class CienistaPostac : BaseCreature
    {
        [Constructable]
        public CienistaPostac() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cienista Postac";
            Body = 0x191;
            BaseSoundID = 0x47D;

            SetStr(250);
            SetDex(80);
            SetInt(80);

            SetHits(500);
            SetMana(20);
            SetStam(30);

            SetDamage(7, 9);
            SetSkill(SkillName.Taktyka, 75.0);
            //SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Anatomia, 50.0);
            SetSkill( SkillName.Boks, 85.1, 95.0 );
            

			Fame = 4000;
			Karma = -20000;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public CienistaPostac(Serial serial) : base(serial)
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

    // Zdradzony Kaplan
    public class ZdradzonyKaplan : BaseCreature
    {
        [Constructable]
        public ZdradzonyKaplan() : base(AIType.AI_Healer, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zdradzony Kaplan";
            Body = 0x190;
            Hue = Utility.RandomSkinHue();
            Blessed = true;
        }

        public override void OnThink()
        {
            base.OnThink();
            // Dodatkowa logika NPC, np. reagowanie na obecnoœæ gracza
        }

        public ZdradzonyKaplan(Serial serial) : base(serial)
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

    public class KrwawyAmulet : Item
    {
        [Constructable]
        public KrwawyAmulet() : base(0x2CFF) // U¿ycie ID dla odpowiedniego modelu amuletu
        {
            Name = "Krwawy Amulet";
            Hue = 1157; // Czerwony kolor amuletu
            Weight = 1.0;
            LootType = LootType.Blessed; // Ustawienie przedmiotu jako b³ogos³awiony (nie mo¿na go straciæ)
        }

        // Konstruktor do deserializacji
        public KrwawyAmulet(Serial serial) : base(serial)
        {
        }

        // Serializacja
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // wersja
        }

        // Deserializacja
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
