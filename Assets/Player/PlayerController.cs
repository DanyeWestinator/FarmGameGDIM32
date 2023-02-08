using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField]
    private float moveSpeed = 1f;

    [SerializeField] private Transform toolParent;
    [SerializeField] private Tool currentTool;
    [SerializeField] private TextMeshProUGUI console;
    [SerializeField] private float consoleTime = 1.2f;
    [SerializeField] private GameObject lastHit;
    public FarmTile currentTile;

    private Animator anim;
    private int tool_i = 0;
    /// <summary>
    /// The tools the player has
    /// </summary>
    [SerializeField] private List<Tool> tools = new List<Tool>();

    /// <summary>
    /// The direction the player is moving
    /// </summary>
    private Vector2 dir;

    private bool _canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        print("Test changes!");
        foreach (Tool t in tools)
        {
            GameObject go = Instantiate(t.gameObject, toolParent);
            go.name = t.gameObject.name;
            go.SetActive(false);
        }

        currentTool = toolParent.GetChild(0).GetComponent<Tool>();
        currentTool.gameObject.SetActive(true);
        tool_i = 0;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void OnHit(GameObject col)
    {
        if (lastHit != col && lastHit)
            lastHit.GetComponent<FarmTile>().SetSelected(false);
        lastHit = col;
        currentTile = lastHit.GetComponent<FarmTile>();
        currentTile.SetSelected(true);
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

    void OnUse()
    {
        //Don't start using a tool if already using one
        if (frozenMovement != null)
            return;
        
        //Use the current tool
        currentTool.UseTool(currentTile.gameObject);
        
        //Start the tool animation
        anim.SetTrigger($"Start{currentTool.gameObject.name}");
        
        //Freeze the player's movement until animation finishes
        frozenMovement = freezeMovement();
        StartCoroutine(frozenMovement);
        
        
    }

    private IEnumerator frozenMovement = null;
    /// <summary>
    /// Coroutine
    /// </summary>
    /// <returns>Just waits</returns>
    IEnumerator freezeMovement()
    {
        //Freeze movement until done
        _canMove = false;
        
        //Get length of current animation
        float secs = anim.GetCurrentAnimatorStateInfo(0).length;
        
        //Wait n seconds
        yield return new WaitForSeconds(secs);
        if (currentTile)
            
        //Allow movement again
        _canMove = true;
        frozenMovement = null;
    }

    IEnumerator StartConsole()
    {
        console.text = $"Using {currentTool.gameObject.name}!";
        console.gameObject.SetActive(true);
        yield return new WaitForSeconds(consoleTime);
        console.gameObject.SetActive(false);
        
    }

    void OnNextTool()
    {
        //Turn off current tool
        currentTool.gameObject.SetActive(false);
        tool_i++;
        //Wrap if tool_i exceeds the number of tools
        if (tool_i >= toolParent.childCount)
            tool_i = 0;
        currentTool = toolParent.GetChild(tool_i).GetComponent<Tool>();
        currentTool.gameObject.SetActive(true);
    }
    void OnPrevTool()
    {
        //Turn off current tool
        currentTool.gameObject.SetActive(false);
        tool_i--;
        //Wrap if tool_i exceeds the number of tools
        if (tool_i < 0)
            tool_i = toolParent.childCount - 1;
        currentTool = toolParent.GetChild(tool_i).GetComponent<Tool>();
        currentTool.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Handles move logic
    /// </summary>
    void Move()
    {
        if (_canMove == false)
            return;
        
        Vector3 direction = (Vector3)dir * Time.deltaTime * moveSpeed;
        
        transform.position += direction;
    }
}
