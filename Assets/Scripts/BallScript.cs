using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    float _counter;
    string selfTag;
    [SerializeField] float _timerAmt;
    void Start()
    {
        selfTag = gameObject.tag;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == selfTag && selfTag != "seventh"){
            Vector3 placement = gameObject.transform.position;
            Destroy(gameObject);
            Destroy(collisionInfo.gameObject);
            GameManager.instance.setFlag(selfTag, placement);
        }
    }

    void OnTriggerStay(Collider other)
    {
        _counter += Time.deltaTime;
        if(_counter > _timerAmt){
            GameManager.instance.EndGame();
        }
    }

    void OnTriggerExit(Collider other)
    {
        _counter = 0;
    }
}
