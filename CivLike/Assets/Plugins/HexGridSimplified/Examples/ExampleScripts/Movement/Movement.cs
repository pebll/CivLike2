using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wunderwunsch.HexGridSimplified
{
    [DisallowMultipleComponent]
    public abstract class Movement : MonoBehaviour
    {        
        [SerializeField]
        protected MovementRulesData movementRulesData;
        [SerializeField]
        protected GameManager gameManager = null;
        protected Map map;
        [SerializeField]
        protected int maxMovementPoints;
        public int RemainingMovementPoints { get; protected set; }
        public List<PathNode> PathToTarget { get; protected set; } //provided by Pathfinder, will be updated when something blocks the path or new target is assigned        

        public abstract bool TryMove(Vector3Int target); //attempts to move to the next PathNode, returns false if it just couldn't do it!, if target has a swappable unit then it tries to swap if that fails no movement)
        //public abstract bool TryMovePath(); // for automovement at turnEnd, if an obstacle appears then it should cancel and return control to player and focus on it
        
        public abstract bool TileIsWalkable(Vector3Int targetPos);

        public void SetPathToTarget(Vector3Int target)
        {
            PathToTarget = CalculatePathToTarget(target);
        }

        public List<PathNode> CalculatePathToTarget(Vector3Int target)
        {
            Vector3Int positionOnMap = HexConverter.WorldPositionToCubeCoord(transform.position);
            return Pathfinder.GetPath(this, positionOnMap, target, movementRulesData.minimalCost);
        }

        public abstract int CalculateCostBetweenTiles(Vector3Int posA, Vector3Int posB); //we could also get mapID from where MapEntity stands on, but as pathfinder already has the number, why not pass it, should be faster

        public int GetMaxMovementPoints()
        {
            return maxMovementPoints;
        }        
    }
}
