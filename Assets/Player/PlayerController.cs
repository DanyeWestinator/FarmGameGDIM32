using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private int tool_i = 0;
    /// <summary>
    /// The tools the player has
    /// </summary>
    [SerializeField] private List<Tool> tools = new List<Tool>();

    /// <summary>
    /// The direction the player is moving
    /// </summary>
    private Vector2 dir;
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

    void OnUse()
    {
        currentTool.UseTool();
        StartCoroutine(StartConsole());
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
        Vector3 direction = (Vector3)dir * Time.deltaTime * moveSpeed;

        transform.position += direction;
    }
}
