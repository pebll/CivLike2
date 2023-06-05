using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleWithZoom : MonoBehaviour
{
    private new Camera camera;
    private new Transform transform;
    [SerializeField] private float factor;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    void Start()
    {      
        camera= Camera.main;
        transform = GetComponent<Transform>();
        float scale = Mathf.Clamp((camera.transform.position.z + 10) * factor, minScale, maxScale);

        transform.localScale = new Vector3(scale, scale, scale);
    }
}
