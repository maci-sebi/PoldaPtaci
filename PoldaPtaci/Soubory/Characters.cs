using System;
using System.Collections.Generic;
using PoldaPtaci.Soubory;

namespace PoldaPtaci
{
    public static class CharacterManager
    {
        public static List<Replika> GetDialogKosi(GameState stav)
        {
            var repliky = new List<Replika>();

            // 1. FÁZE: Hráč má hnízdo s vajíčky (FINÁLE HRY)
            if (stav.HasNestWithEggs)
            {
                repliky.Add(new Replika("Detektiv", "Paní Kosi! Podívejte, co nesu. Vaše vajíčka jsou v pořádku a v bezpečí."));
                repliky.Add(new Replika("Paní Kosi", "Ach, nebesa! Moje drahá vajíčka! Detektive, vy jste hotový hrdina! Celý ptačí park vám bude do smrti vděčný!"));
                repliky.Add(new Replika("Detektiv", "To nestojí za řeč, paní Kosi. Jen další vyřešený případ pro ptačí kriminálku."));
            }
            // 2. FÁZE: Hráč ještě nemá kompas (začátek hry)
            else if (!stav.HasCompass)
            {
                repliky.Add(new Replika("Detektiv", "Dobrý den, paní Kosi. Slyšel jsem, že se stala hrozná věc."));
                repliky.Add(new Replika("Paní Kosi", "Ach, detektive! Moje drahocenná vajíčka jsou pryč! Někdo je ukradl přímo z hnízda!"));
                repliky.Add(new Replika("Detektiv", "Nezoufejte, jsem na stopě. Viděla jste někoho podezřelého?"));
                repliky.Add(new Replika("Paní Kosi", "Záblesk velkých černých křídel... letěl směrem k Městu! Prosím, pospěšte si!"));
                repliky.Add(new Replika("Detektiv", "Vydám se tam. Nejdřív ale musím na zemi najít kousek ostrého skla a pírko, abych si vyrobil Ptačí kompas. Bez něj v lese zabloudím."));
            }
            // 3. FÁZE: Hráč vyrobil kompas, ale vajíčka ještě nemá (hledá dál)
            else
            {
                repliky.Add(new Replika("Paní Kosi", "Skvělé, vidím, že už máš ptačí kompas! Cesty do Lesa i do Města jsou teď volné."));
                repliky.Add(new Replika("Detektiv", "Díky, paní Kosi. Jdu prohledat okolí a vyslechnout podezřelé. Držte se!"));
            }

            return repliky;
        }

        // ==========================================
        // DIALOG: DATEL CAROL (LES) - AUTOMATICKÝ PRŮCHOD
        // ==========================================
        public static List<Replika> GetDialogCarola(GameState stav, ref string vybranyPredmetVInventari)
        {
            var repliky = new List<Replika>();

            // Protože hráč bez kompasu do lesa nevstoupí, Carol ho rovnou pochválí a pustí dál
            if (stav.CarolPassed)
            {
                repliky.Add(new Replika("Datel Carol", "Cesta k Potoku je volná, detektive. Hodně štěstí při pátrání."));
            }
            else
            {
                repliky.Add(new Replika("Detektiv", "Zdravím, Carol. Potřebuji projít dál hlouběji do lesa."));
                repliky.Add(new Replika("Datel Carol", "Ťuk, ťuk! Normálně bych tě nepustil, protože v tomhle lese každý zabloudí..."));
                repliky.Add(new Replika("Datel Carol", "Ale koukám, že ti z kapsy kouká prvotřídní Ptačí kompas! S ním to zvládneš. Běž dál k Potoku, cesta je volná!"));
                repliky.Add(new Replika("Detektiv", "Díky, Carol. Budu opatrný."));

                // Odemknutí cesty rovnou po dialogu
                stav.CarolPassed = true;
            }

            return repliky;
        }

        // ==========================================
        // DIALOG: OREL NOIR (MĚSTO)
        // ==========================================
        public static List<Replika> GetDialogNoira(GameState stav, ref string vybranyPredmetVInventari)
        {
            var repliky = new List<Replika>();

            if (vybranyPredmetVInventari == "Bobule")
            {
                repliky.Add(new Replika("Detektiv", "Zadrž, Noire! Vím, že máš ta vajíčka. Vyměním je za tyhle sladké lesní bobule."));
                repliky.Add(new Replika("Orel Noir", "Ooo, bobule! Beru! Tak já ti je teda hodím, chytej!"));

                if (!stav.Inventory.Contains("Hnízdo"))
                {
                    repliky.Add(new Replika("Detektiv", "Počkej, neházej je jen tak! Já je nemám do čeho chyt..."));
                }
            }
            else
            {
                repliky.Add(new Replika("Orel Noir", "Co tu okouníš? Bez pořádného úplatku se s tebou nebavím."));
                repliky.Add(new Replika("Detektiv", "Musím zjistit, co má tenhle orel rád..."));
            }

            return repliky;
        }



        // ==========================================
        // DIALOG: LEDŇÁČEK JOHN (POTOK) - CHCE KLÁDU
        // ==========================================
        public static List<Replika> GetDialogJohna(GameState stav, ref string vybranyPredmetVInventari)
        {
            var repliky = new List<Replika>();

            if (stav.JohnPassed)
            {
                repliky.Add(new Replika("Ledňáček John", "Užij si klid u Bukového stromu. Moudrá Sova už na tebe čeká."));
            }
            else if (vybranyPredmetVInventari == "Kláda")
            {
                repliky.Add(new Replika("Detektiv", "Nesu ti z lesa pořádný kus dřeva, Johne. Tahle těžká kláda udrží proud vody."));
                repliky.Add(new Replika("Ledňáček John", "U všech potoků, to je masivní kousek! Přesně takhle těžkou kládu jsem potřeboval, abych si zpevnil břeh proti divoké vodě!"));
                repliky.Add(new Replika("Detektiv", "Rádo se stalo. Můžu teď projít dál ke starému Buku?"));
                repliky.Add(new Replika("Ledňáček John", "Samozřejmě, detektive! Jdi rovnou za Sovou, ta ví o všem, co se v revíru šustne."));

                // Úprava stavu hry
                stav.JohnPassed = true;
                stav.Inventory.Remove("Kláda");
                vybranyPredmetVInventari = null;
            }
            else
            {
                repliky.Add(new Replika("Ledňáček John", "Bacha, poldo! Divoká voda mi podemílá hnízdo a já nemám čím zpevnit břeh. Nikam dál tě nepustím, dokud to nevyřeším."));
                repliky.Add(new Replika("Detektiv", "Co by ti pomohlo?"));
                repliky.Add(new Replika("Ledňáček John", "Potřeboval bych pořádně těžkou kládu, která ten divoký proud zastaví. Neviděl jsi nějakou v lese?"));
                repliky.Add(new Replika("Detektiv", "Viděl jsem jednu padlou kládu hluboko v lese. Skočím pro ni."));
            }

            return repliky;
        }

        // ==========================================
        // DIALOG: SOVA (BUK)
        // ==========================================
        public static List<Replika> GetDialogSovy(GameState stav)
        {
            var repliky = new List<Replika>();

            if (!stav.OwlGaveBerries)
            {
                repliky.Add(new Replika("Detektiv", "Zdravím vás, moudrá Sovo. Potřebuji radu ohledně ztracených vajíček paní Kosi."));
                repliky.Add(new Replika("Sova", "Vím, co a koho hledáš, detektive. Orel Noir sice vypadá nebezpečně, ale má jednu obrovskou slabost..."));
                repliky.Add(new Replika("Detektiv", "Sem s ní!"));
                repliky.Add(new Replika("Sova", "Miluje sladké lesní bobule. Tady ti jedny dávám ze svých zásob. Zkus mu je ve městě nabídnout výměnou za vajíčka. Určitě neodolá."));
            }
            else
            {
                repliky.Add(new Replika("Sova", "Více ti už poradit nedokážu, detektive. Použij lesní bobule správně u Noira ve městě."));
            }

            return repliky;
        }
    }
}