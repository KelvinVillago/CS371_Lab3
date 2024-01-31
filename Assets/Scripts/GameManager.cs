using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    public static GameManager instance;

    public GameObject player;
    bool hasObject = false;
    Vector3 spawnPosition;
    GameObject newObject;
    Rigidbody objectRB;

    void Awake()
    {
        instance = this;
    }
    
    void Start() => StartCoroutine(SpawnObject());  

    public void DropObject(){
        print("Dropped");
        if(newObject != null){
            FollowScript follow = newObject.GetComponent<FollowScript>();
            follow.enabled = false;
        }
        objectRB.useGravity = true;
        hasObject = false;
        
    }

    IEnumerator SpawnObject()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if(hasObject == false){
                int choice = Random.Range(0,4);
                
                spawnPosition = player.transform.position + new Vector3(0,5,0);
                newObject = Instantiate(gameObjects[choice], spawnPosition, Quaternion.identity, player.transform);
                objectRB = newObject.GetComponent<Rigidbody>();
                objectRB.useGravity = false;
                hasObject = true;
            }
        }
    }
}
