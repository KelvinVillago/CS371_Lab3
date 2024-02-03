using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Random=UnityEngine.Random;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public GameObject[] gameObjects;
    public ParticleSystem particleEffect;
    public AudioClip popSound;
    public AudioClip endSound;
    private AudioSource _audioSource;

    string[] tags = {"first", "second", "third", "fourth", "fifth", "sixth", "seventh"};
    
    PlayerInput _playerInput;
    InputAction _rotationAction;
    InputAction _resetCam;
    
    
    //references
    public static GameManager instance;
    public GameObject player;
    [SerializeField] float speed;
    
    [SerializeField] GameObject RestartUI;
    [SerializeField] TextMeshProUGUI _scoreText;
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
    GameObject _camera;

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
        
        _playerInput = GetComponent<PlayerInput>();
        _rotationAction = _playerInput.actions["Rotation"];
        _resetCam = _playerInput.actions["ResetCamera"];
        
        _audioSource = GetComponent<AudioSource>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "Main")
        {
            player = GameObject.Find("Player");
            GameObject mainCanvas = GameObject.Find("Canvas");
            TextMeshProUGUI[] texts = mainCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            if(texts[0] != null)
            {
                _scoreText = texts[0];
            }
            if(texts[1] != null)
            {
                _highScoresText = texts[1];

            }
            RestartUI = mainCanvas.transform.GetChild(2).gameObject;
            _camera = gameObject.transform.GetChild(0).gameObject;
            if(_camera.GetComponent<AudioListener>() == null)
            {
                _camera.AddComponent<AudioListener>();
            }
        }
        else
        {
            _camera = gameObject.transform.GetChild(0).gameObject;
            Destroy(_camera.GetComponent<AudioListener>());
        }
        _resetCam.started += ResetCam;
    }
    
    void Start()
    {   
        Time.timeScale = 1f;
        StartCoroutine(SpawnObject());  
        _resetCam.started += ResetCam;
    }

    void Update()
    {
        StartCoroutine(UpdateScore());

        if(flag){
            Combine(combineTag, combinePos);
            flag = false;
        }
        if(player != null && player.transform.childCount == 0){
            hasObject = false;
        }

        Vector2 motion = _rotationAction.ReadValue<Vector2>();
        float spinY = motion.x * speed;
        float spinX = motion.y * speed;
        float xRotation = transform.rotation.eulerAngles.x;
        print("X rotation: " + xRotation);
        if(xRotation > 180){
            xRotation -= 360;
        }
        print("X rotation: "+xRotation);
        if (xRotation < -60 && spinX < 0)
        {
            transform.Rotate(new Vector3(0,spinY,0));
        }
        else if(xRotation > 30 && spinX > 0)
        {
            transform.Rotate(new Vector3(0,spinY,0));
        }
        else{
            transform.Rotate(new Vector3(spinX,spinY,0));
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
        StartCoroutine(SpawnObject());
    }

    public void Combine(string tag, Vector3 location)
    {
        int ind = 0;
        _audioSource.PlayOneShot(popSound);
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
            Instantiate(particleEffect, combinedObj.transform.position, combinedObj.transform.rotation);

        }
        _score += (ind + 1) * 100;
        _scoreText.text = "Score: " + _score.ToString();

    }

    public void setFlag(string tag, Vector3 location)
    {
        flag = true;
        combineTag = tag;
        combinePos = location;
    }

    public void EndGame()
    {
        
        _audioSource.PlayOneShot(endSound);
        for(int i = _highScores.Length-1; i > 0; i--){
            if(_score > _highScores[i]){
                _highScores[i] = _score;
                print("Score: " + _score);
                print("High scores"+_highScores);
                break;
            }
        }
        Array.Sort(_highScores);
        Array.Reverse(_highScores);
        _score = 0;

        Time.timeScale = 0;
        RestartUI.SetActive(true);
    }

    IEnumerator SpawnObject()
    {
        // while(true)
        // {
            
            yield return new WaitForSeconds(2.0f);
            if(hasObject == false){
                int choice = Random.Range(0,4);
                
                spawnPosition = player.transform.position + new Vector3(0,10,0);
                // print("Spawn pos: " + spawnPosition);
                newObject = Instantiate(gameObjects[choice], spawnPosition, Quaternion.identity, player.transform);
                newObject.transform.localPosition = new Vector3(0,5,0);

                objectRB = newObject.GetComponent<Rigidbody>();
                objectRB.useGravity = false;

                objectCollider = newObject.GetComponent<Collider>();
                objectCollider.enabled = false;

                hasObject = true;
            }
        // }
    }

    IEnumerator UpdateScore()
    {
        if(_highScoresText != null){
            _highScoresText.text = "1: " + _highScores[0] + "\n" + "2: " + _highScores[1] + "\n" + "3: " + _highScores[2] + "\n";
        }
        yield return null;
    }

    void ResetCam(InputAction.CallbackContext context)
    {
        gameObject.transform.rotation = Quaternion.Euler(0,45,0);
    }

}
