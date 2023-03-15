using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Player manager
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// How fast the player moves
    /// </summary>
    [SerializeField]
    private float moveSpeed = 1f;
    /// <summary>
    /// All tools live as children objects. They are turned on/off to cycle
    /// </summary>
    [SerializeField] private Transform toolParent;
    [SerializeField] private Tool currentTool;
    /// <summary>
    /// The tile the player hit last frame
    /// </summary>
    private GameObject lastHit;
    /// <summary>
    /// The UI item for the pause panel
    /// </summary>
    public GameObject pausePanel;
    /// <summary>
    /// The Text displaying the current score
    /// </summary>
    public ScoreKeeper scoreKeeper;
    private static int currentScore = 0;
    /// <summary>
    /// What tile the player is currently standing on
    /// </summary>
    public FarmTile currentTile;
    /// <summary>
    /// The animator controller for the tools
    /// </summary>
    private Animator anim;
    /// <summary>
    /// Index in toolParent's children of the current tool
    /// </summary>
    private int tool_i = 0;
    /// <summary>
    /// The tools the player has. Used to spawn the prefabs as children of toolParent
    /// </summary>
    [SerializeField] private List<Tool> tools = new List<Tool>();

    /// <summary>
    /// The direction the player is moving
    /// </summary>
    private Vector2 dir;

    private bool _canMove = true;

    public static PlayerController player;
    public static HashSet<PlayerController> players = new HashSet<PlayerController>();

    private bool isPaused = false;
    

    // Start is called before the first frame update
    void Start()
    {
        //Singleton (kind of, doesn't check if another exists) of player
        player = this;
        if (players.Contains(this) == false)
        {
            players.Add(this);
        }
        
        //Spawn all the player's tools as children under toolParent
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
        AddScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    //Updating which tile the player is standing on
    public void OnHit(GameObject col)
    {
        //Turn off the last tile if exists
        if (lastHit != col && lastHit)
        {
            lastHit.GetComponent<FarmTile>().SetSelected(false);
        }
            
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

    void OnPause()
    {
         _canMove = isPaused;
                
        
        //Flip isPaused
        isPaused = !isPaused;
        GameStateManager.TogglePause();

    }

    void OnUse()
    {
        //Don't start using a tool if already using one
        if (frozenMovement != null || currentTool == null)
            return;
        
        
        
        //Start the tool animation
        if (currentTool.gameObject.name.Contains("Seeds") == false)
            anim.SetTrigger($"Start{currentTool.gameObject.name}");
        else
        {
            anim.SetTrigger("StartSeeds");
        }
        //Freeze the player's movement until animation finishes
        frozenMovement = freezeMovement();
        StartCoroutine(frozenMovement);
        
        
    }

    private IEnumerator frozenMovement = null;
    /// <summary>
    /// Freezes player movement until anim is done
    /// </summary>
    /// <returns>Void, returns WaitForSeconds</returns>
    IEnumerator freezeMovement()
    {
        //Freeze movement until done
        _canMove = false;
        
        //Get length of current animation
        float secs = anim.GetCurrentAnimatorStateInfo(0).length;
        
        //Wait n seconds
        yield return new WaitForSeconds(secs);
        //Use the current tool, after waiting
        currentTool.UseTool(currentTile.gameObject);
        //Allow movement again
        _canMove = true;
        frozenMovement = null;
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

    private int old_x_dir;
    /// <summary>
    /// Handles move logic
    /// </summary>
    void Move()
    {
        if (_canMove == false)
            return;
        
        Vector3 direction = (Vector3)dir * Time.deltaTime * moveSpeed;
        int x_dir = (int)math.sign(dir.x);
        if (old_x_dir != x_dir && x_dir != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = x_dir * -1;
            transform.localScale = scale;
        }
        transform.position += direction;
        old_x_dir = x_dir;
    }

    public void AddScore(int toAdd)
    {
        scoreKeeper.AddScore(toAdd);
        
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
