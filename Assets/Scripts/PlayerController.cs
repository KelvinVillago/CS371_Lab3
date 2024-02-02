using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    PlayerInput _playerInput;
    InputAction _moveAction;
    InputAction _interaction;
    Rigidbody _rigidbody;

    //changeable fields
    [SerializeField] float speed;
    [SerializeField] float boundary;

    //Camera components for relative movement
    float cameraY;
    
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

    void FixedUpdate()
    {
        Vector2 moveVector =_moveAction.ReadValue<Vector2>();

        //camera relative movement
        Vector3 cameraFwd = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraFwd.y = 0;
        cameraRight.y = 0;

        Vector3 forwardRelative = moveVector.y * cameraFwd;
        Vector3 rightRelative = moveVector.x * cameraRight;

        Vector3 moveDir = forwardRelative + rightRelative;

        float moveSpeed = speed * Time.fixedDeltaTime;
        float boundedZ = Mathf.Clamp(transform.position.z + moveDir.z*speed, -boundary, boundary);
        float boundedX = Mathf.Clamp(transform.position.x + moveDir.x*speed, -boundary, boundary);

        Vector3 movement = new Vector3(boundedX, transform.position.y, boundedZ);

        _rigidbody.Move(movement, Quaternion.identity);

    }

    void Action(InputAction.CallbackContext context){
        GameManager.instance.DropObject();
    }
}
