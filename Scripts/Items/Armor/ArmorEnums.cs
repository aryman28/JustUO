using System;

namespace Server.Items
{
    public enum ArmorQuality
    {
        None,
        S�ab,
        Przeci�tn,
        Zwyk�,
        Dobr,
        Doskona�,
        Wspania�,
        Wyj�tkow,
        Niezwyk�,
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
        Wytrzyma�,
        Odbijaj�c,
//zbroje
        Witaln,
        M�dr,
        Stabiln,
        Ochronn,
        Szcz�liw,
        Oszcz�dn,
//bizuteria
        Niewidzialn,
        Jasn,
        Ciemn,
        Mocn,
        Zr�czn,
        Inteligentn,
        Umiej�tn,
        Mia�dz�c
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