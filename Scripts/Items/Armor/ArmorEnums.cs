using System;

namespace Server.Items
{
    public enum ArmorQuality
    {
        None,
        S³ab,
        Przeciêtn,
        Zwyk³,
        Dobr,
        Doskona³,
        Wspania³,
        Wyj¹tkow,
        Niezwyk³,
        Cudown,
        Mistyczn,
        Legendarn
    }

    public enum ArmorCechy
    {
        None,
//tarcze
        Obronn,
        Waleczn,
//ogolne (zbroja i tarcza)
        Wytrzyma³,
        Odbijaj¹c,
//zbroje
        Witaln,
        M¹dr,
        Stabiln,
        Ochronn,
        Szczêœliw,
        Oszczêdn,
//bizuteria
        Niewidzialn,
        Jasn,
        Ciemn,
        Mocn,
        Zrêczn,
        Inteligentn,
        Umiejêtn,
        Mia¿dz¹c
    }

    public enum ArmorDurabilityLevel
    {
        Regular,
        Durable,
        Substantial,
        Massive,
        Fortified,
        Indestructible
    }

    public enum ArmorProtectionLevel
    {
        Regular,
        Defense,
        Guarding,
        Hardening,
        Fortification,
        Invulnerability,
    }

    public enum ArmorBodyType
    {
        Gorget,
        Gloves,
        Helmet,
        Arms,
        Legs, 
        Chest,
        Shield
    }

    public enum ArmorMaterialType
    {
        Cloth,
        Leather,
        Studded,
        Bone,
        Spined,
        Horned,
        Barbed,
        Ringmail,
        Chainmail,
        Plate,
        Dragon	// On OSI, Dragon is seen and considered its own type.
    }

    public enum ArmorMeditationAllowance
    {
        All,
        Half,
        None
    }
}