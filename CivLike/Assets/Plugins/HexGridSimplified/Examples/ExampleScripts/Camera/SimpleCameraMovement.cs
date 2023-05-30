using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class SimpleCameraMovement : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            float vertical = 0;
            float horizontal = 0;
            Vector3 movement = Vector3.zero;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                vertical = 1;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                vertical = -1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontal = 1;
            }

            movement.z += vertical;
            movement.x += horizontal;
            transform.Translate(new Vector3(horizontal * Time.deltaTime * 4f, vertical * Time.deltaTime * 4f, 0));
        }
    }
}