namespace Server.Auction
{
    class StringList
    {
        public const string

            //board.cs
            BoardName = "Tablica Aukcyjna",
            Dead = "Nie mozesz tego zrobic gdy jestes martwy.",
            TooFar = "Jestes za daleko.",
            TipName = "{0} Tablica Aukcyjna", //{0} = region name
            TipCount = "Jest {0} przedmiotów aukcji.", // {0} = item count
            EndOwnerWarn = "Twój przedmiot '{0}' zostal kupiony przez '{1}' za '{2}' sztuk zlota. Czek znajduje sie w twoim depozycie bankowym.",
            EndBuyerWarn = "Twoja cena '{0}' byla najwyzsza. '{1}' sztuk zlota zostalo pobrane a przedmiot aukcji znajduje sie w banku.",
            EndOwnerNoBids = "Twój przedmiot '{0}' nie zostal kupiony i zjaduje sie w banku.",
            NoGoldWarn = "Twoja licytacja '{0}' przerwana z powodu braku z?ota w skrytce bankowej.",

            //AuctionGump.cs // addauctiongump // ViewItemgump
            GumpTitle = "{0}'s TABLICA AUKCYJNA - {1} {2}",
            OwnerHeader = "WLASCICIEL",
            ItemHeader = "PRZEDMIOT",
            PriceHeader = "CENA",
            LastBidHeader = "OSTATNIA",
            EndHeader = "ZAKONCZENIE",
            DonationTag = "WPLATA",
            DescHeader = "OPIS",
            DaysHeader = "DNI",
            DashTag = "--",
            EndTodayTag = "Dzis {0}", //0 = datetime
            EmptyTag = "-- PUSTY --",
            Page = "Strona {0} z {1}",
            AuctionItem = "DODAJ",
            SelectItem = "Co chcesz wystawic na aukcje?",
            BadSelection = "To nie jest dozwolony przedmiot aukcji.",
            MustBeOnPack = "Przedmiot musi znajdowac sie w twoim plecaku.",
            InvalidName = "Wybrales niepoprawna nazwe.",
            InvalidDesc = "Niewlasciwy opis",
            InvalidPrice = "Niewlasciwa cena",
            InvalidDuration = "Niewlasciwa ilosc dni",

            StartAuctWarnMessage = "Chcesz zlicytowac '{0}' za  '{1}'. Nie bedziesz mógl usunac przedmiotu z aukcji.",
            StartAuctGold = "sztuk zlota",
            StartAuctDonation = "wplata",

            CantBidOwn = "Nie mozesz licytowac wlasnych przedmiotów.",
            DonationMoved = "Wplata przeniesiona do twojego plecaka.",
            BidRegist = "Twoja licytacja zostala odnotowana, zostaniesz powiadomiony jesli wygrasz licytacje.",
            AlreadyAuctioned = "Przedmiot zostal juz zlicytowany.",
            HigherValue = "Musisz podac wartosc wyzsza od ostatniej licytacji.",
            InvalidValue = "Niewlasciwa cena licytacji.";
    }
}
