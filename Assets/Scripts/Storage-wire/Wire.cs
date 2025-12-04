using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Wire : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LineRenderer Line;
    public Transform correct_EndWire;
    public Transform incorrect_EndWire1;
    public Transform incorrect_EndWire2;
    public Transform incorrect_EndWire3;

    bool Dragging = false;
    bool connected = false;
    Vector3 OGPosition;

    Transform snappedEnd = null;
  
    void Start()
    {
        OGPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            
            Vector3 convertedMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            convertedMousePosition.z = 0;

            setPosition(convertedMousePosition);

            Vector3 C_EndWireDifference = convertedMousePosition - correct_EndWire.position;
            Vector3 I1_diff = convertedMousePosition - incorrect_EndWire1.position;
            Vector3 I2_diff = convertedMousePosition - incorrect_EndWire2.position;
            Vector3 I3_diff = convertedMousePosition - incorrect_EndWire3.position;
            if (C_EndWireDifference.magnitude < 0.5f)
            {
                snaptoend(correct_EndWire);
            }else if(I1_diff.magnitude < 0.5f)
            {
                snaptoend(incorrect_EndWire1);
            }else if(I2_diff.magnitude < 0.5f)
            {
                snaptoend(incorrect_EndWire2);
            }else if(I3_diff.magnitude < 0.5f)
            {
                snaptoend(incorrect_EndWire3);
            }
        }
    }
    
    void snaptoend(Transform endWire)
    {
        setPosition(endWire.position);
        snappedEnd = endWire;
        Dragging = false;
        connected = true;
    }
    void setPosition(Vector3 pPosition)
    {
        transform.position = pPosition;

        Vector3 positionDifference = pPosition - Line.transform.position;
        Line.SetPosition(2, positionDifference - new Vector3(.8f, 0, 0));
        Line.SetPosition(3, positionDifference - new Vector3(.15f, 0, 0));
    }

    private void OnMouseDown()
    {
        Dragging = true;

    }
    
    void ResetPosition()
    {
        snappedEnd = null;
        setPosition(OGPosition);
    }

    private void OnMouseUp()
    {
        Dragging = false;
        if (!connected)
        {
            ResetPosition();
        }

    }

    public bool IsConnected()
    {
        return connected;
    }
    
    public void SetConnected(bool pconnected)
    {
        connected = pconnected;
        if (!connected)
        {
            ResetPosition();
        }
    }

    public bool isCorrectConnection()
    {
        return snappedEnd == correct_EndWire;
    }
}