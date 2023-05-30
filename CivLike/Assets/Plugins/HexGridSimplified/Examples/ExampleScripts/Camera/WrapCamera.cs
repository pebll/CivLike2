using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wunderwunsch.HexGridSimplified
{
    public class WrapCamera : MonoBehaviour
    {
      
        [SerializeField]
        private Camera primaryCamera = null;
        private float mapWidth;
        // Use this for initialization
        public void Init()
        {
            mapWidth = Hex.MapSize.x * Constants.sqrt3;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            Vector3 primaryPos = primaryCamera.transform.position;
            Vector3 myPosition;
            if (primaryCamera.transform.position.x > mapWidth / 2f)
            {
                myPosition = primaryPos - new Vector3(mapWidth, 0, 0);
            }
            else
            {
                myPosition = primaryPos + new Vector3(mapWidth, 0, 0);
            }

            transform.position = myPosition;
        }
    }
}
