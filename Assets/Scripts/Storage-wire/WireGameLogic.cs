using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WireGameLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Wire> wires;
    public GameObject endMessage;
    public GameObject endMessage2;


    
    void Start()
    {
        ShuffleWires();
    }

    void Update()
    {
        int connectedCount = 0;
        bool allCorrect = true;


        foreach (Wire w in wires)
        {
            if (w.IsConnected())
                connectedCount++;

            if (!w.isCorrectConnection())
                allCorrect = false;
        }

        // If all wires are connected
        if (connectedCount == wires.Count)
        {
            if (allCorrect)
            {
                // WIN
                endMessage.SetActive(true);
                endMessage2.SetActive(true);
            }
            else
            {
                // WRONG → reset & shuffle
                ResetWires();
            }
        }
    }

    public void ResetWires()
    {
        foreach (Wire w in wires)
        {
            w.SetConnected(false);
        }

        ShuffleWires();
    }

   void ShuffleWires()
    {
        // Collect positions of ONLY the correct end wires
        List<Vector3> correctPositions = new List<Vector3>();
        foreach (Wire w in wires)
        {
            correctPositions.Add(w.correct_EndWire.position);
        }

        // Shuffle only the correct endpoints
        foreach (Wire w in wires)
        {
            int randomIndex = Random.Range(0, correctPositions.Count);
            w.correct_EndWire.position = correctPositions[randomIndex];
            correctPositions.RemoveAt(randomIndex);
        }
     }
}