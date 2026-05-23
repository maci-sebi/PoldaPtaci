using System;
using System.Collections.Generic;
using System.Text;

namespace PoldaPtaci.Soubory
{
    public static class CharacterManager
    {
        public static string KlikNaKosi(GameState stav)
        {
            if (stav.GameFinished) return "Paní Kosi: Děkuji ti moc, detektive! Zachránil jsi moji rodinu.";
            if (stav.HasNestWithEggs)
            {
                stav.GameFinished = true;
                return "Detektiv: Tady jsou vaše vajíčka, paní Kosi. Našel jsem je v bezpečí v hnízdě.\nPaní Kosi: Ó panečku, děkuji! Jste nejlepší detektiv!";
            }
            return "Paní Kosi: Chudák já! Někdo mi ukradl moje drahocenná vajíčka! Pomožte mi prosím, detektive!";
        }

        public static string KlikNaCarola(GameState stav, ref string vybranyPredmet)
        {
            if (stav.CarolPassed) return "Carol: Cesta k potoku je volná, kámo. Díky za ten kompas.";

            if (vybranyPredmet == "Ptačí kompas")
            {
                stav.Inventory.Remove("Ptačí kompas");
                stav.CarolPassed = true;
                vybranyPredmet = null;
                return "Detektiv: Tady máš tenhle nablýskaný ptačí kompas.\nCarol: Hustý ty vole! To se leskne jak hovado! Jdi dál, vole, cesta k potoku je tvoje.";
            }
            return "Carol: VOLE kam si myslíš, že jdeš!? Přes tuhle cestu k potoku nikoho nepustím.";
        }

        public static string KlikNaJohna(GameState stav, ref string vybranyPredmet)
        {
            if (stav.JohnPassed) return "John: Pamatuj si, jdi kolem potoka a narazíš na moji felačku sovu.";

            if (vybranyPredmet == "Kláda")
            {
                stav.Inventory.Remove("Kláda");
                stav.JohnPassed = true;
                vybranyPredmet = null;
                return "Detektiv: Tady máš kládu, bude se ti hodit na sezení při rybaření.\nJohn: Paráda, díky! Když se vydáš dál kolem potoka, najdeš kamojdu sovu u Buku. Ví úplně všechno je to felák.";
            }
            return "John: Ryby neberou... A bolí mě nohy. Kdybych tak měl pořádný kus dřeva na sezení.";
        }

        public static string KlikNaNoira(GameState stav, ref string vybranyPredmet)
        {
            if (stav.GameFinished) return "Noir: Vajíčka už nemám, tak mě nech a moje město na pokoji.";

            if (vybranyPredmet == "Bobule")
            {
                if (stav.Inventory.Contains("Hnízdo"))
                {
                    stav.Inventory.Remove("Bobule");
                    stav.Inventory.Remove("Hnízdo");
                    stav.Inventory.Add("Hnízdo s vajíčky");
                    stav.NoirGaveEggs = true;
                    stav.HasNestWithEggs = true;
                    vybranyPredmet = null;
                    return "Detektiv: Co takhle vyměnit vajíčka za tyhle lahodné bobule?\nNoir: Mmm, bobule! Moje oblíbené! Tady máš ty vejce....";
                }

                stav.Inventory.Remove("Bobule");
                stav.GameFinished = true;
                vybranyPredmet = null;
                return "Detektiv: Chci ta vajíčka! Tady máš bobule!\nNoir: Jupí, bobule! Na, chytej vejce!\n\n*KŘUP!*\n\nNeměl jsi hnízdo a vajíčka se rozbila o zem! Paní Kosi ti tohle nikdy neodpustí. Prohrál jsi!";
            }

            return "Noir: Ta vajíčka jsou moje! ´Dnes si je dám na večeři!";
        }
    }
}
