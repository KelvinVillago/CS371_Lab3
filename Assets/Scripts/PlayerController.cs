using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    PlayerInput _playerInput;
    InputAction _moveAction;
    InputAction _interaction;
    [SerializeField] float speed;
    [SerializeField] float boundary;
    Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Movement"];
        _interaction = _playerInput.actions["Interaction"];
        
    }

    void Start()
    {
        _interaction.started += Action;
    }

    void Update(){

    }

    void FixedUpdate()
    {
        Vector2 moveVector =_moveAction.ReadValue<Vector2>();

        float moveSpeed = speed * Time.fixedDeltaTime;
        float boundedZ = Mathf.Clamp(transform.position.z + moveVector.y*speed, -boundary, boundary);
        float boundedX = Mathf.Clamp(transform.position.x + moveVector.x*speed, -boundary, boundary);

        Vector3 movement = new Vector3(boundedX, transform.position.y, boundedZ);

        _rigidbody.Move(movement, Quaternion.identity);

    }

    void Action(InputAction.CallbackContext context){
        GameManager.instance.DropObject();
    }
}
