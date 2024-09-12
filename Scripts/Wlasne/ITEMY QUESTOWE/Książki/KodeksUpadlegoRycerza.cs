using System;
using Server;

namespace Server.Items
{
    public class KodeksUpadlegoRycerza : BlueBook
    {
        [Constructable]
        public KodeksUpadlegoRycerza() : base("Kodeks Upad³ego Rycerza", "skryba Vega", 19, false)
        {
            int cnt = 0;
            string[] lines;

            lines = new string[]
            {
                "1. Nie ma ju¿ honoru,",
                "który niegdyœ prowadzi³",
                "mnie przez ¿ycie.",
                "Zdrada i krew splami³y",
                "moj¹ duszê.",
                "Teraz tylko zemsta",
                "napêdza moje kroki.",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "2. Wiara upad³a, a ja",
                "z ni¹. Œlepe zaufanie",
                "prowadzi³o mnie do",
                "zguby. Teraz tylko",
                "w³asne pragnienie zemsty",
                "jest moim przewodnikiem.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "3. Widzia³em z³o w sercach",
                "ludzi, którym zaufa³em.",
                "Nie ma ju¿ ró¿nicy",
                "miêdzy dobrem a z³em.",
                "Jedynie si³a i",
                "determinacja przetrwa.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "4. Moje czyny s¹ teraz",
                "nacechowane gniewem i",
                "gorycz¹. Mój miecz",
                "przebija tych, którzy",
                "stoj¹ na drodze do",
                "sprawiedliwoœci, której",
                "nikt mi nie odda³.",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "5. Kiedy napotkam na",
                "swojej drodze upokorzenie,",
                "nie bêdê ju¿ milcza³.",
                "Nie ma pojedynków,",
                "jest tylko œmieræ dla tych,",
                "którzy stan¹ mi na drodze.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "6. Sprawiedliwoœæ jest teraz",
                "moim narzêdziem zemsty.",
                "Nie ma odpuszczenia,",
                "tylko surowy os¹d",
                "i krew za krew.",
                "",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "7. Kiedy zabijam, robiê to",
                "bez wahania. Nie proszê",
                "bogów o przebaczenie,",
                "bo oni mnie opuœcili.",
                "Teraz to ja wymierzam",
                "sprawiedliwoœæ.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "8. Ka¿da œmieræ ci¹¿y na",
                "mojej duszy, ale nie cofam",
                "siê. Gdy zabi³em dla zemsty,",
                "wiedzia³em, ¿e jestem",
                "gotów zap³aciæ za to cenê",
                "po œmierci.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "9. Nie wierzê ju¿ w",
                "przeznaczenie. Ka¿dy krok",
                "jest mój, a moje rêce",
                "s¹ moim narzêdziem.",
                "Nie ma boskiej woli,",
                "tylko moja.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "10. ¯ycie niewinnych",
                "straci³o dla mnie wartoœæ.",
                "Liczy siê tylko zemsta.",
                "S¹d nad moj¹ dusz¹",
                "jest nieunikniony,",
                "ale do tego czasu",
                "bêdê niszczy³ swoich wrogów.",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "11. Kobiety i mê¿czyŸni",
                "s¹ teraz dla mnie równo",
                "bez znaczenia. Nie ma",
                "wœród nich ¿adnych,",
                "którzy mogliby mi pomóc",
                "na mojej drodze.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "12. Prawda sta³a siê",
                "relatywna, tak jak moje",
                "s³owa. Mówiê, co",
                "potrzebne, by osi¹gn¹æ",
                "swoje cele. Nikomu",
                "ju¿ nie ufam.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "13. Rodzina by³a kiedyœ",
                "moim wsparciem, ale",
                "teraz tylko przypomina",
                "o tym, co straci³em.",
                "Mi³oœæ sta³a siê",
                "s³aboœci¹, któr¹ porzuci³em.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "14. Nie wierzê ju¿ w",
                "cnoty ani wiernoœæ.",
                "Moja wiara zosta³a",
                "z³amana, a teraz",
                "wiem, ¿e tylko zemsta",
                "przyniesie mi ukojenie.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "15. Mój wróg nie zas³uguje",
                "na szacunek. Pokonany",
                "nie jest godzien niczego",
                "poza œmierci¹.",
                "Nie ma godnego pochówku,",
                "tylko zapomnienie.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "16. Mi³osierdzie to cnota,",
                "któr¹ dawno porzuci³em.",
                "Nie litujê siê nad",
                "nikim, tak jak oni nie",
                "litowali siê nade mn¹.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "17. Pomoc? Nie oczekuj",
                "ode mnie litoœci,",
                "chyba ¿e mo¿e mi to",
                "przynieœæ korzyœæ.",
                "Honor jest teraz",
                "pojêciem obcym.",
                "",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "18. Jeœli œwiadomie",
                "z³ama³em swój kodeks,",
                "to dlatego, ¿e kodeks",
                "ju¿ nie istnieje.",
                "Moje insygnia s¹ teraz",
                "tylko przypomnieniem",
                "tego, kim kiedyœ by³em.",
                "",
            };
            Pages[cnt++].Lines = lines;

            lines = new string[]
            {
                "19. Teraz kierujê siê",
                "jedynie gniewem.",
                "Œwiat³o mojego patrona",
                "zgas³o, a ja idê",
                "ciemnymi œcie¿kami.",
                "¯yczê Ci, bracie,",
                "abyœ nigdy nie upad³",
                "tak jak ja.",
            };
            Pages[cnt++].Lines = lines;
        }

        public KodeksUpadlegoRycerza(Serial serial) : base(serial)
        {
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }
    }
}
