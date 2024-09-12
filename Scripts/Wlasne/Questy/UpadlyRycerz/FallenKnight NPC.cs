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
        public FallenKnight() : base("Mordred", "- Upad³y Rycerz")
        {
        }

        public FallenKnight(Serial serial) : base(serial)
        {
        }

        // Zmiana questa na czêœæ dotycz¹c¹ upad³ego rycerza
        public override Type[] Quests { get { return new Type[] { typeof(UpadlyRycerzZemsta) }; } }

        public override void InitBody()
        {
            this.Race = Race.Human;
            this.Hue = 2406; // Ciemna karnacja, sugeruj¹ca upadek
            this.HairItemID = 0x203C; // D³ugie, rozwiane w³osy
            this.HairHue = 1175; // Srebrzyste w³osy, symbolizuj¹ce utratê œwiat³a
            this.FacialHairItemID = 0x204B; // Broda, dodaj¹ca mrocznego wygl¹du
            this.FacialHairHue = 1175; // Dopasowanie koloru brody do w³osów
            this.Body = 0x190;
        }

        public override void InitOutfit()
        {
            // Zmiana ekwipunku na mroczniejszy i bardziej z³owieszczy
            AddItem(new PlateChest { Hue = 1109 }); // Czarna, z³owieszcza zbroja
            AddItem(new PlateLegs { Hue = 1109 });
            AddItem(new PlateArms { Hue = 1109 });
            AddItem(new PlateGloves { Hue = 1109 });
            AddItem(new PlateHelm { Hue = 1109 }); // Czarny he³m zas³aniaj¹cy twarz
            AddItem(new Cloak { Hue = 1175 }); // Mroczny p³aszcz, srebrzysty odcieñ

            // Dodanie mrocznego miecza i tarczy
            AddItem(new Kryss { Hue = 1150 }); // Ciemny, z³owieszczy miecz
            AddItem(new ChaosShield { Hue = 1175 }); // Mroczna tarcza

            // Ewentualnie dodajemy inne elementy, które podkreœlaj¹ mroczn¹ naturê postaci
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
