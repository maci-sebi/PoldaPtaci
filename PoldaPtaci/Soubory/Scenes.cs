using System;
using System.Collections.Generic;
using System.Text;

namespace PoldaPtaci.Soubory
{
    public static class SceneManager
    {
        // Jednoduchá metoda, která spáruje název scény s jejím souborem pozadí
        public static string GetSceneBackground(string sceneName)
        {
            return sceneName switch
            {
                "Park" => "ParkQ.png",
                "Mesto" => "MestoQ.png",
                "Les" => "LesQ.png",
                "Potok" => "PotokQ.png",
                "Buk" => "BukQ.png",
                _ => "ParkQ.png"
            };
        }
    }
}
