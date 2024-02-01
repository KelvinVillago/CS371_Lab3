using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

    string selfTag;
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
            GameManager.instance.Combine(selfTag, placement);
            print("Self: "+placement);
        }
        else if(collisionInfo.gameObject.tag == "Untagged"){
            this.enabled = false;
        }
    }
}
