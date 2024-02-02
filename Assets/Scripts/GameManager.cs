using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random=UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    string[] tags = {"first", "second", "third", "fourth", "fifth", "sixth", "seventh"};
    
    //references
    public static GameManager instance;
    public GameObject player;
    
    [SerializeField] GameObject RestartUI;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI _highScoresText;

    bool hasObject = false;
    int _score;
    int[] _highScores = {0,0,0};

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
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        RestartUI.SetActive(false);
        _score = 0;
    }
    
    void Start()
    {        
        _highScoresText.text = "1: " + _highScores[0] + "\n" + "2: " + _highScores[1] + "\n" + "3: " + _highScores[2] + "\n";
        StartCoroutine(SpawnObject());  
    }

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
        for(int i = 0; i < _highScores.Length; i++){
            if(_score > _highScores[i]){
                _highScores[i] = _score;
                break;
            }
        }
        Array.Sort(_highScores);

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
                print("Spawn pos: " + spawnPosition);
                newObject = Instantiate(gameObjects[choice], spawnPosition, Quaternion.identity, player.transform);
                newObject.transform.localPosition = new Vector3(0,5,0);

                objectRB = newObject.GetComponent<Rigidbody>();
                objectRB.useGravity = false;

                objectCollider = newObject.GetComponent<Collider>();
                objectCollider.enabled = false;

                hasObject = true;
            }
        }
    }
}
