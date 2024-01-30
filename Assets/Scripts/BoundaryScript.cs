using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        print("Hit collider");
    }
    
}
