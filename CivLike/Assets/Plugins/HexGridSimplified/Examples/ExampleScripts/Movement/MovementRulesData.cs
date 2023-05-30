using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Wunderwunsch.HexGridSimplified
{
    [CreateAssetMenu(fileName = "MovementRulesData", menuName = "ScriptableObjects/MovementRulesData", order = 2)]
    public class MovementRulesData : ScriptableObject
    {
        public List<int> CostByBaseTerrain;
        public List<int> CostByVegetation;
        public List<int> CostByTopography;
        public int CostCrossingRiver;
        public int minimalCost; 
    }
}