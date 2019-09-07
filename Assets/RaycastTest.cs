using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RaycastTest : MonoBehaviour
{
    ARRaycastManager rcm;

    // Start is called before the first frame update
    void Start()
    {
        rcm = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        rcm.Raycast(screenCenter, hits, TrackableType.PlaneWithinBounds);

        if (hits.Count > 0)
        {
            print("Hit object");
        }
        else
        {
            print("Did not hit object");
        }
    }
}
