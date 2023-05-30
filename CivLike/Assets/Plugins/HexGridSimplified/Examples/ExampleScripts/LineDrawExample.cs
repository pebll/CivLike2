using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Wunderwunsch.HexGridSimplified
{
    public class LineDrawExample : MonoBehaviour
    {
        [SerializeField]
        private Text uiText = null;
        [SerializeField]
        private Mouse mouse = null;
        [SerializeField]
        private GameObject originMarkerPrefab = null;
        [SerializeField]
        private GameObject lineMarkerPrefabL = null;
        [SerializeField]
        private GameObject lineMarkerPrefabR = null;
        private Vector3Int lineOrigin;
        private Vector3Int prevMousePosition;
        private List<GameObject> lineMarkers;

        public void Start()
        {
            lineMarkers = new List<GameObject>();
        }

        public void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                lineOrigin = mouse.CubePositionSanitized;
            }

            Vector3Int currentPos = mouse.CubePositionSanitized;
            if(prevMousePosition!= currentPos)
            {
                CleanUp();
                GameObject originMarker = GameObject.Instantiate(originMarkerPrefab);
                originMarker.transform.position = HexConverter.CubeCoordToWorldPosition(lineOrigin);
                lineMarkers.Add(originMarker);

                List<Vector3Int> lineL = Hex.GetLine(lineOrigin, currentPos, -0.0001f, 0, 0, true);
                List<Vector3Int> lineR = Hex.GetLine(lineOrigin, currentPos, +0.0001f, 0, 0, true);

                foreach(var l in lineL)
                {
                    GameObject marker = GameObject.Instantiate(lineMarkerPrefabL);
                    marker.transform.position = HexConverter.CubeCoordToWorldPosition(l) + new Vector3(-0.25f,0,0);
                    lineMarkers.Add(marker);
                }

                foreach (var r in lineR)
                {
                    GameObject marker = GameObject.Instantiate(lineMarkerPrefabR);
                    marker.transform.position = HexConverter.CubeCoordToWorldPosition(r) + new Vector3(+0.25f, 0, 0); ;
                    lineMarkers.Add(marker);
                }
            }
            int distance = Hex.Distance(lineOrigin, currentPos);
            uiText.text = "Distance: " + distance;
            prevMousePosition = currentPos;
        }

        private void CleanUp()
        {
            foreach (GameObject g in lineMarkers)
            {
                Destroy(g);
            }
            lineMarkers.Clear();
        }
    }

}
