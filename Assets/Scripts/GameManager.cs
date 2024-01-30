using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void DropObject(){
        print("Dropped");
    }
}
