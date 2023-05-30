
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wunderwunsch.HexGridSimplified
{    
    public class Mouse : MonoBehaviour
    {
        [SerializeField]
        private GameObject positionMarker = null;
        [SerializeField]
        private Text cursorPosText = null;
        public Vector3 PixelPosition { get; private set; }
        public Vector3 WorldPositionRaw { get; private set; }
        public Vector3 WorldPositionWrapped { get; private set; } //used for raycasting for selection
        public Vector3Int CubePositionRaw { get; private set; }
        public Vector3Int CubePositionSanitized { get; private set; } 
        public Vector2Int OffsetPositionRaw { get; private set; } 
        public Vector2Int OffsetPositionSanitized { get; private set; }
        public Ray SelectionRayRaw { get; private set; }
        public Ray SelectionRayWrapped { get; private set; }

        public void Update()
        {
            PixelPosition = Input.mousePosition;
            WorldPositionRaw = PixelToWorldCoords(PixelPosition);
            CubePositionRaw = HexConverter.WorldPositionToCubeCoord(WorldPositionRaw);
            OffsetPositionRaw = HexConverter.WorldPositionToOffsetCoord(WorldPositionRaw);

            CubePositionSanitized = Hex.WrapTileHorizontal(CubePositionRaw);
            CubePositionSanitized = Hex.ClampToVerticalBounds(CubePositionSanitized);
            OffsetPositionSanitized = HexConverter.CubeCoordToOffsetCoord(CubePositionSanitized);

            float worldPositionWrapOffset = 0;
            if (CubePositionRaw.x < CubePositionSanitized.x)
            {
                worldPositionWrapOffset = +Hex.MapSize.x * Constants.sqrt3;
            }
            else if (CubePositionRaw.x > CubePositionSanitized.x)
            {
                worldPositionWrapOffset = -(Hex.MapSize.x * Constants.sqrt3);
            }

            WorldPositionWrapped = new Vector3(WorldPositionRaw.x + worldPositionWrapOffset, WorldPositionRaw.y, WorldPositionRaw.z);

            SelectionRayRaw = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 wrappedRayOrigin = new Vector3(SelectionRayRaw.origin.x + worldPositionWrapOffset, SelectionRayRaw.origin.y, SelectionRayRaw.origin.z);
            SelectionRayWrapped = new Ray(wrappedRayOrigin, SelectionRayRaw.direction);

            if (positionMarker != null)
            {
                positionMarker.transform.position = HexConverter.CubeCoordToWorldPosition(CubePositionSanitized);
            }
            
            if(cursorPosText != null)
            {
                UpdateUIText();
            }            
        }

        private Vector3 PixelToWorldCoords(Vector3 pixelCoords)
        {
            Plane plane = new Plane(Vector3.down, 0); // collision plane to cast rays against to get mouse position;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float dist;
            Vector3 point;
            plane.Raycast(ray, out dist);
            point = ray.GetPoint(dist);
            return point;
        }

        private void UpdateUIText()
        {
            string output = "pixelPosition: " + PixelPosition;
            output += "\r\nworldPositionRaw: " + WorldPositionRaw;
            output += "\r\nworldPositionWrapped: " + WorldPositionWrapped;
            output += "\r\ncubePositionRaw: " + CubePositionRaw;
            output += "\r\ncubePositionSanitized: " + CubePositionSanitized;
            output += "\r\nOffsetPositionRaw: " + OffsetPositionRaw;
            output += "\r\nOffsetPositionSanitized: " + OffsetPositionSanitized;
            cursorPosText.text = output;
        }



    }
}
