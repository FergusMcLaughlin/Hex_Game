using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseZoom : MonoBehaviour
{
    private Camera cam;
    private float targetZoom;
    private float zoomfactor = 3f;
    private float zoomspeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollimp;
        scrollimp = Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log(scrollimp);

        targetZoom = targetZoom - scrollimp * zoomfactor;
        //fixes the inverting thing
        targetZoom = Mathf.Clamp(targetZoom, 2f, 8f);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomspeed);
    }
}
