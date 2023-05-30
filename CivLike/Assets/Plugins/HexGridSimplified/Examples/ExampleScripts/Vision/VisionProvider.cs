using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to MapEntities who should provide Vision of the Map
/// </summary>
namespace Wunderwunsch.HexGridSimplified
{
    [DisallowMultipleComponent]
    public abstract class VisionProvider : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager = null;
        [SerializeField]
        protected VisibilityManager visibilityExample;
        protected Map map;
        [SerializeField]
        protected int VisionRange;//{ get; private set; }
        public HashSet<Vector2Int> VisibleTilesBefore { get; protected set; }
        public HashSet<Vector2Int> VisibleTiles { get; protected set; }
        private Vector3Int previousPosition = new Vector3Int(-1, -1, -1);

        protected abstract HashSet<Vector2Int> CalculateVisibleTiles();

        public void Start()
        {
            VisibleTiles = new HashSet<Vector2Int>();
        }

        public void Update()
        {            
            Vector3Int pos = HexConverter.WorldPositionToCubeCoord(transform.position);
            if(pos != previousPosition)
            {
                UpdateVisibleTiles();
            }
            previousPosition = pos;
        }

        public void SetRange(int range)
        {
            VisionRange = range;
        }

        public void UpdateVisibleTiles()
        {
            if (map == null) map = gameManager.Map;
            VisibleTilesBefore = new HashSet<Vector2Int>(VisibleTiles);
            VisibleTiles = CalculateVisibleTiles(); // we need to update the world FoW after that, who should be responsible for that?
            SendVisionToMapData();
        }

        public void SendVisionToMapData()
        {
            {
                HashSet<Vector2Int> lostVision = new HashSet<Vector2Int>(VisibleTilesBefore);
                lostVision.ExceptWith(VisibleTiles); //
                HashSet<Vector2Int> gainedVision = new HashSet<Vector2Int>(VisibleTiles);
                gainedVision.ExceptWith(VisibleTilesBefore);
                visibilityExample.UpdateVisibility(lostVision, gainedVision);
            }
        }
    }
}