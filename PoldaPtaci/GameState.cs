using System;
using System.Collections.Generic;
using System.Text;

namespace PoldaPtaci
{
    public class GameState
    {
        // aktualni scena
        public string CurrentScene { get; set; } = "Menu";

        // inventar
        public List<string> Inventory { get; set; } = new List<string>();

        // predmety na zemi
        public bool GlassOnGround { get; set; } = true;
        public bool FeatherOnGround { get; set; } = true;
        public bool BranchesOnGround { get; set; } = true;
        public bool LogOnGround { get; set; } = true;
        public bool MossOnGround { get; set; } = true;

        // questy
        public bool HasCompass { get; set; } = false;      // spojeni sklo + pirko
        public bool CarolPassed { get; set; } = false;     // pustil nas Carol
        public bool JohnPassed { get; set; } = false;      // dal John info
        public bool OwlGaveBerries { get; set; } = false;  // mame bobule
        public bool NoirGaveEggs { get; set; } = false;    // dal vajicka
        public bool HasNestWithEggs { get; set; } = false; // mame hnizdo
        public bool GameFinished { get; set; } = false;    // konec hry 
    }
}
