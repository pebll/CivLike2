using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{    
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Camera primaryCamera = null;
        [SerializeField]
        private Camera wrapCamera = null;
        [SerializeField]
        private MapGenerator mapGenerator = null;
        public Map Map { get; private set; }
        

        // Use this for initialization
        void Start()
        {
            InitMap();
            Debug.Log("HexMapSize: " + Hex.MapSize);
            InitCameras();
        }

        private void InitMap()
        {
            Map m = mapGenerator.GenerateMap();
            m.InitVisualisation();
            Map = m;
        }

        private void InitCameras()
        {
            primaryCamera.GetComponent<MainCameraWrap>().Init();
            wrapCamera.GetComponent<WrapCamera>().Init();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
