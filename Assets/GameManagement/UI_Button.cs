using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Handles OnSelect and OnDeselect for UI elements
/// </summary>
public class UI_Button : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [Tooltip("Whether the UI item is the first item selected")]
    [SerializeField] private bool IsFirstSelected = false;
    [SerializeField]
    private Color selectedColor = Color.red;
    [SerializeField]
    private Color deselectedColor = Color.cyan;
    
    /// <summary>
    /// The button that's currently selected.
    /// Need to deselect ourselves when someone new is selected
    /// </summary>
    private static UI_Button currentlySelected = null;
    
    

    private Image bg;

    private void Awake()
    {
        //Get our attached image
        bg = GetComponent<Image>();
    }
    
    private void OnEnable()
    {
        //Always deselect ourselves, to flush
        Deselect();
        //If we're the default, select ourselves
        if (IsFirstSelected)
        {
            OnSelect(null);
            StartCoroutine(_setSelected(gameObject));
        }
    }
    
    /// <summary>
    /// Called when the UI system selects a button, or a mouse enters the button
    /// </summary>
    public void OnSelect(BaseEventData eventData)
    {
        //Deselect old
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.Deselect();
        }
        //Set ourselves as currently selected, and change our color
        currentlySelected = this;
        bg.color = selectedColor;
    }
    /// <summary>
    /// Callled when the mouse pointer enters the space
    /// </summary>
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        OnSelect(pointerEventData);
    }
    
    /// <summary>
    /// Deselects ourselves after a new UI item is selected
    /// </summary>
    void Deselect()
    {
        bg.color = deselectedColor;
    }
    
    /// <summary>
    /// Helper coroutine to wait in line to set ourselves as the selected object, to get controller input
    /// </summary>
    /// <returns>Waits until it successfully selects</returns>
    private static IEnumerator _setSelected(GameObject go)
    {
        //Slight hardcoding
        float timeToWait = 1f;
        float timeWaited = 0f;
        while (true)
        {
            if (timeWaited >= timeToWait)
                break;
            yield return new WaitForEndOfFrame();
            if (EventSystem.current.alreadySelecting == false)
            {
                EventSystem.current.SetSelectedGameObject(go);
                break;
            }

            timeWaited += Time.deltaTime;
        }
    }
}
