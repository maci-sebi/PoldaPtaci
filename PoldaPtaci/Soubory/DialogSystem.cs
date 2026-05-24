using System;
using System.Collections.Generic;
using System.Text;

namespace PoldaPtaci.Soubory
{
    public class Replika
    {
        public string Jmeno { get; set; }
        public string Text { get; set; }

        public Replika(string jmeno, string text)
        {
            Jmeno = jmeno;
            Text = text;
        }
    }

    // sktivni dialog
    public class AktivniDialog
    {
        private List<Replika> repliky;
        private int aktualniIndex = 0;
        private Action poSkonceni;

        public AktivniDialog(List<Replika> repliky, Action poSkonceni)
        {
            this.repliky = repliky;
            this.poSkonceni = poSkonceni;
        }

        // jmeno vrati
        public string ZobrazAktualni()
        {
            if (aktualniIndex >= repliky.Count) return "";
            var r = repliky[aktualniIndex];
            return $"{r.Jmeno}: {r.Text}";
        }

        public bool DalsiReplika()
        {
            aktualniIndex++;
            if (aktualniIndex >= repliky.Count)
            {
                poSkonceni?.Invoke(); 
                return false; 
            }
            return true; 
        }
    }
}
