using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Wunderwunsch.HexGridSimplified
{
    [CreateAssetMenu(fileName = "VisionRulesData", menuName = "ScriptableObjects/VisionRulesData", order = 1)]
    public class VisionRulesData : ScriptableObject
    {
        public List<int> VisibilityByTopographyID;
        public List<int> VisibilityValueByVegetationID;
    }
}