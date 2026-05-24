using System;
using System.Collections.Generic;
using System.Text;

namespace PoldaPtaci.Soubory
{
    public class Item
    {
        public string Name { get; set; }
        public string ImageName { get; set; }

        public Item(string name, string imageName)
        {
            Name = name;
            ImageName = imageName;
        }
    }

    public static class ItemManager
    {
        // Vrátí název obrázku pro daný předmět v inventáři
        public static string GetImageForExtension(string itemName)
        {
            return itemName switch
            {
                "Sklo" => "Glass.png",
                "Pírko" => "Feather.png",
                "Ptačí kompas" => "FeatherCompass.png",
                "Větvičky" => "Branches.png",
                "Kláda" => "Log.png",
                "Mech" => "Moss.png",
                "Hnízdo" => "Nest.png",
                "Bobule" => "Berries.png",
                "Hnízdo s vajíčky" => "NestE.png",
                _ => "Glass.png"
            };
        }
    }
}
