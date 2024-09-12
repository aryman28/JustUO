//===============================================================================
//                      This script was created by Gizmo's UoDevPro
//                      This script was created on 27.08.2024 12:20:26
//===============================================================================

using System;
using Server.Items;
using Server.Engines.Quests;

namespace Server.Mobiles
{
    public class FallenKnight : MondainQuester
    {
        [Constructable]
        public FallenKnight() : base("Mordred", "- Upad�y Rycerz")
        {
        }

        public FallenKnight(Serial serial) : base(serial)
        {
        }

        // Zmiana questa na cz�� dotycz�c� upad�ego rycerza
        public override Type[] Quests { get { return new Type[] { typeof(UpadlyRycerzZemsta) }; } }

        public override void InitBody()
        {
            this.Race = Race.Human;
            this.Hue = 2406; // Ciemna karnacja, sugeruj�ca upadek
            this.HairItemID = 0x203C; // D�ugie, rozwiane w�osy
            this.HairHue = 1175; // Srebrzyste w�osy, symbolizuj�ce utrat� �wiat�a
            this.FacialHairItemID = 0x204B; // Broda, dodaj�ca mrocznego wygl�du
            this.FacialHairHue = 1175; // Dopasowanie koloru brody do w�os�w
            this.Body = 0x190;
        }

        public override void InitOutfit()
        {
            // Zmiana ekwipunku na mroczniejszy i bardziej z�owieszczy
            AddItem(new PlateChest { Hue = 1109 }); // Czarna, z�owieszcza zbroja
            AddItem(new PlateLegs { Hue = 1109 });
            AddItem(new PlateArms { Hue = 1109 });
            AddItem(new PlateGloves { Hue = 1109 });
            AddItem(new PlateHelm { Hue = 1109 }); // Czarny he�m zas�aniaj�cy twarz
            AddItem(new Cloak { Hue = 1175 }); // Mroczny p�aszcz, srebrzysty odcie�

            // Dodanie mrocznego miecza i tarczy
            AddItem(new Kryss { Hue = 1150 }); // Ciemny, z�owieszczy miecz
            AddItem(new ChaosShield { Hue = 1175 }); // Mroczna tarcza

            // Ewentualnie dodajemy inne elementy, kt�re podkre�laj� mroczn� natur� postaci
            AddItem(new Boots { Hue = 1109 }); // Czarny buty
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
