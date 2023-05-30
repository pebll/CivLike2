using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wunderwunsch.HexGridSimplified
{
    public class AllinRangeAndRings : MonoBehaviour
    {
        [SerializeField]
        private Toggle removeInvalidToggle = null;
        [SerializeField]
        private Dropdown dropdown = null;
        [SerializeField]
        private Slider rangeSlider = null;
        [SerializeField]
        private Slider thicknessSlider = null;
        [SerializeField]
        private Mouse mouse = null;
        [SerializeField]
        private GameObject tileMarkerPrefab = null;
        private Vector3Int prevMousePosition;
        private List<GameObject> markers;
        

        public void Start()
        {
            markers = new List<GameObject>();
        }

        public void Update()
        {
            Vector3Int currentPos = mouse.CubePositionSanitized;
            if(currentPos != prevMousePosition)
            {
                if(dropdown.value == 0)
                {
                    ShowAllInRange(currentPos);
                }
                else
                {
                    ShowRing(currentPos);
                }
            }

            prevMousePosition = currentPos;
        }        

        private void ShowRing(Vector3Int center)
        {
            CleanUp();
            HashSet<Vector3Int> result = Hex.GetRing(center, (int)rangeSlider.value, (int)thicknessSlider.value, removeInvalidToggle.isOn);
            PlaceMarkers(result);

        }

        private void ShowAllInRange(Vector3Int center)
        {
            CleanUp();
            HashSet<Vector3Int> result = Hex.GetAllWithinManhattanRange(center, (int)rangeSlider.value, true, removeInvalidToggle.isOn);
            PlaceMarkers(result);

        }

        private void PlaceMarkers(HashSet<Vector3Int> result)
        {
            foreach (var r in result)
            {
                GameObject g = Instantiate(tileMarkerPrefab);
                g.transform.position = HexConverter.CubeCoordToWorldPosition(r);
                g.name = "TileMarker";
                markers.Add(g);

            }
        }

        private void CleanUp()
        {
            foreach(GameObject g in markers)
            {
                Destroy(g);
            }
            markers.Clear();
        }
    }

}
