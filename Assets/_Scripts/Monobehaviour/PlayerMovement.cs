using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float inputValue;
    
    [SerializeField] private float moveSpeed;
    
    [Header("MOVEMENT BOUNDS")]
    [SerializeField] private float topBound;
    [SerializeField] private float bottomBound;
    
    
    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
       float yValue = transform.position.y;
       
       yValue += inputValue * moveSpeed * Time.deltaTime;
       
       if (yValue >= topBound)
           yValue = topBound;
       
       else if (yValue <= bottomBound)
           yValue = bottomBound;
       
       transform.position = new Vector3(transform.position.x, yValue, transform.position.z);
    }


    public void HandlePlayerMove(InputAction.CallbackContext context)
    {
        inputValue = context.ReadValue<float>();
    }
}
