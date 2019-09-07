using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AR_EditorControls : MonoBehaviour
{
    public float speed = .05f;
    public float lookSpeed = .05f;

    public GameObject ARCamera;
    public GameObject ARMock;
    public GameObject EditorCamera;

    // Start is called before the first frame update
    void Start()
    {
        ARCamera = transform.Find("AR Camera").gameObject;
        ARMock = transform.Find("AR Mock").gameObject;
        EditorCamera = ARMock.transform.Find("Mock Camera").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    private Vector2 rotation = Vector2.zero;
    void CheckInputs()
    {
        //forward back
        if (Input.GetKey(KeyCode.W))
        {
            EditorCamera.transform.position += transform.forward * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            EditorCamera.transform.position += -transform.forward * speed;
        }
        //left right
        if (Input.GetKey(KeyCode.A))
        {
            EditorCamera.transform.position += -transform.right * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            EditorCamera.transform.position += transform.right * speed;
        }
        //strafe up/down
        if (Input.GetKey(KeyCode.Q))
        {
            EditorCamera.transform.position += -transform.up * speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            EditorCamera.transform.position += transform.up * speed;
        }

        if (Input.GetMouseButton(0))
        {
            float X, Y;

            if (Input.GetMouseButton(0))
            {
                transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * lookSpeed, -Input.GetAxis("Mouse X") * lookSpeed, 0));
                X = transform.rotation.eulerAngles.x;
                Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
            }
        }
    }
}
