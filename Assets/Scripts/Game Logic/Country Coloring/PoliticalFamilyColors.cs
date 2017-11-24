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
            {"christdem", new Color(255, 154, 0)}, //orange  
            {"libdem", Color.yellow}, //gold
            {"confessional", new Color(0, 128, 255)}, //light blue
            {"socialist", Color.magenta}, //pink            
            {"liberal", Color.blue}, //dark blue
            {"radical left", Color.red}, //red       
            {"radical right", new Color(165,42,42)}, //brown
            {"green", Color.green}, //green 
            {"agrarian/center", new Color(121, 93, 93)}, //light brown            
            {"regionalist", new Color(112, 255, 117)}, //light green
            {"no family", Color.gray} //grey            
        };
    }
}