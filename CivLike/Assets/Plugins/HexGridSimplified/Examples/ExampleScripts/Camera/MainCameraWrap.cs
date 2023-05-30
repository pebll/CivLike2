using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class MainCameraWrap : MonoBehaviour
    {
        [SerializeField]
        private GameObject wrapCameraRenderQuad = null;
        [SerializeField]
        private float negativeJumpThreshold; //we jump JumpLength to the right if we are more negative than that
        [SerializeField]
        private float positiveJumpThreshold; //we jump JumpLength to the left if we are more positive than that        
        private float jumpLength;

        void LateUpdate()
        {
            if (transform.position.x < negativeJumpThreshold)
            {
                transform.position += new Vector3(jumpLength, 0, 0);
            }

            else if (transform.position.x > positiveJumpThreshold)
            {
                transform.position -= new Vector3(jumpLength, 0, 0);
            }
        }

        public void Init()
        {
            Camera c = GetComponent<Camera>();
            float aspect = c.aspect;
            float orthographicWidth = c.orthographicSize; //half-size

            float mapWidth = Hex.MapSize.x;
            Vector3 renderQuadScale = new Vector3
            {
                y = orthographicWidth * 2,
                x = orthographicWidth * 2 * aspect,
                z = 1
            };
            wrapCameraRenderQuad.transform.localScale = renderQuadScale;

            negativeJumpThreshold = -(aspect * 2 * orthographicWidth * 0.8f);
            positiveJumpThreshold = (mapWidth * Constants.sqrt3) + (aspect * 2 * orthographicWidth * 0.8f);//1.1 just to have some margin, might not be needed
            jumpLength = mapWidth * Constants.sqrt3;
        }
    }
}
