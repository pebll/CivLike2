using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wunderwunsch.HexGridSimplified
{
    public class Selection : MonoBehaviour
    {        

        public GameObject SelectedObject { get; private set; }
        [SerializeField]
        private Material selectedMaterial = null;
        private Material originalMaterial = null;
        [SerializeField]
        private Mouse mouse = null;
        [SerializeField]
        Text text = null;

        public void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                GameObject hitObject;
                RaycastHit hit;
                Ray selectionRay = mouse.SelectionRayWrapped;
                Physics.Raycast(selectionRay, out hit);
                if(hit.collider != null)
                {
                    hitObject = hit.collider.gameObject;
                    SelectObject(hitObject);
                }                
            }

            if(Input.GetMouseButtonDown(1))
            {
                if(SelectedObject!= null)
                {
                    Vector3Int mouseCubePos = mouse.CubePositionSanitized;
                    if(mouse.OffsetPositionRaw.y >= 0 && mouse.OffsetPositionRaw.y < Hex.MapSize.y)
                    {
                        Vector3 centerOfHex = HexConverter.CubeCoordToWorldPosition(mouseCubePos);
                        SelectedObject.transform.position = centerOfHex;
                    }                  
                }
            }

            if(Input.GetKeyDown(KeyCode.A))
            {
                if(SelectedObject.GetComponent<SimpleConeVisionProvider>() != null)
                {
                    SelectedObject.GetComponent<SimpleConeVisionProvider>().RotateCounterClockwise();
                }
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (SelectedObject.GetComponent<SimpleConeVisionProvider>() != null)
                {
                    SelectedObject.GetComponent<SimpleConeVisionProvider>().RotateClockwise();
                }
            }
        }

        public void SelectObject(GameObject target)
        {
            if(SelectedObject!= null)
            {
                SelectedObject.GetComponent<Renderer>().material = originalMaterial;
            }
            SelectedObject = target;
            originalMaterial = SelectedObject.GetComponent<Renderer>().material;
            SelectedObject.GetComponent<Renderer>().material = selectedMaterial;
            SetText();
        }

        public void SetText()
        {
            if (text == null) return;
            text.text = "Selected Object: " +SelectedObject.name; 
        }
    }
}
