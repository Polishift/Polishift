using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    public static class PoliticalFamilyColors
    {
        public static readonly Dictionary<string, Color> ColorPerFamily = new Dictionary<string, Color>()
        {
            {"unknown", Color.white}, //white
            {"christdem", new Color(1, 0.631f, 0.101f)}, //orange  
            {"libdem", Color.yellow}, //gold
            {"confessional", Color.blue},
            {"socialist", Color.magenta}, //pink            
            {"liberal", new Color(0.101f, 0.968f, 1)}, 
            {"rad left", Color.red}, //red       
            {"rad right", new Color(0.545f, 0.270f, 0.074f)}, //brown
            {"green", Color.green}, //green 
            {"agrarian/center", new Color(0.843f, 0.639f, 0.537f)}, //light brown            
            {"regionalist", new Color(0.470f, 1, 0.101f)}, //light green
            {"no family", Color.gray} //grey            
        };
    }
}