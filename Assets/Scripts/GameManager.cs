using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    string[] tags = {"first", "second", "third", "fourth", "fifth", "sixth", "seventh"};
    
    //references
    public static GameManager instance;
    public GameObject player;
    
    [SerializeField] GameObject RestartUI;
    [SerializeField] TextMeshProUGUI scoreText;

    bool hasObject = false;
    int _score = 0;

    //spawning new objects
    bool flag = false;
    Vector3 spawnPosition;
    GameObject newObject;
    Rigidbody objectRB;
    Collider objectCollider;

    //combination variables
    string combineTag;
    Vector3 combinePos;

    void Awake()
    {
        instance = this;
        RestartUI.SetActive(false);
    }
    
    void Start() => StartCoroutine(SpawnObject());  

    void Update()
    {
        if(flag){
            Combine(combineTag, combinePos);
            flag = false;
        }
    }

    public void DropObject()
    {
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

    public void Combine(string tag, Vector3 location)
    {
        StopCoroutine(SpawnObject());
        int ind = 0;
        for(int i = 0; i < tags.Length; i++){
            if(tags[i] == tag){
                ind = i;
                break;
            }
        }
        if(ind < tags.Length - 1 && flag == true){
            GameObject combinedObj = Instantiate(gameObjects[ind+1], location, Quaternion.identity);
            combinedObj.transform.position = location;
            FollowScript newObjFollow = combinedObj.GetComponent<FollowScript>();
            newObjFollow.enabled = false;
        }
        _score += (ind + 1) * 100;
        scoreText.text = "Score: " + _score.ToString();

        StartCoroutine(SpawnObject());
    }

    public void setFlag(string tag, Vector3 location)
    {
        flag = true;
        combineTag = tag;
        combinePos = location;
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        RestartUI.SetActive(true);
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
