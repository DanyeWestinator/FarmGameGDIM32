using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField]
    private float moveSpeed = 1f;

    public Vector3 display;

    /// <summary>
    /// The direction the player is moving
    /// </summary>
    private Vector2 dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    /// <summary>
    /// Update the player's move direction based on input
    /// </summary>
    /// <param name="value">The input value for movement</param>
    void OnMove(InputValue value)
    {
        //The current stick direction
        dir = value.Get<Vector2>();
        //Ignore small values from stick drift
        if (dir.magnitude <= 0.1f)
            dir = Vector2.zero;
    }
    
    /// <summary>
    /// Handles move logic
    /// </summary>
    void Move()
    {
        Vector3 direction = (Vector3)dir * Time.deltaTime * moveSpeed;
        display = direction;

        transform.position += direction;
    }
}
