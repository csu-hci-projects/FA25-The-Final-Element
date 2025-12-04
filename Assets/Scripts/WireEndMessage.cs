using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WireEndMessage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public WireGameLogic Game;
    public GameObject welldonemessage;
    public GameObject returnmessage;
    void Start()
    {
        welldonemessage.SetActive(false);
        returnmessage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        Game.ResetWires();
        welldonemessage.SetActive(false);
        returnmessage.SetActive(false);
        
    }
    
}
