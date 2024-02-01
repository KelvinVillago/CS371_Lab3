using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    string[] tags = {"first", "second", "third", "fourth", "fifth", "sixth", "seventh"};
    public static GameManager instance;

    public GameObject player;
    bool hasObject = false;
    Vector3 spawnPosition;
    GameObject newObject;
    Rigidbody objectRB;
    Collider objectCollider;

    void Awake()
    {
        instance = this;
    }
    
    void Start() => StartCoroutine(SpawnObject());  

    public void DropObject(){
        // print("Dropped");
        if(newObject != null){
            FollowScript follow = newObject.GetComponent<FollowScript>();
            follow.enabled = false;

            newObject.transform.parent = null;
            objectRB.useGravity = true;

            objectCollider = newObject.GetComponent<Collider>();
            objectCollider.enabled = true;
        }
        hasObject = false;
    }

    public void Combine(string tag, Vector3 location){
        StopCoroutine(SpawnObject());
        int ind = 0;
        print("Received: " + location);
        for(int i = 0; i < tags.Length; i++){
            if(tags[i] == tag){
                ind = i;
                break;
            }
        }
        if(ind < tags.Length - 1){
            GameObject combinedObj = Instantiate(gameObjects[ind+1], location, Quaternion.identity);
            combinedObj.transform.position = location;
            FollowScript newObjFollow = combinedObj.GetComponent<FollowScript>();
            newObjFollow.enabled = false;
            
            print("Create");
        }
        StartCoroutine(SpawnObject());

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

                objectCollider = newObject.GetComponent<Collider>();
                objectCollider.enabled = false;

                hasObject = true;
            }
        }
    }
}
