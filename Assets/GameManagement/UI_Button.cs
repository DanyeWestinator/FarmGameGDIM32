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
        bg = GetComponent<Image>();
        Deselect();
    }
    
    /// <summary>
    /// Event function called when the UI system selects a button
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        //Deselect old
        if (currentlySelected != null && currentlySelected != this)
        {
            currentlySelected.Deselect();
        }

        currentlySelected = this;
        bg.color = selectedColor;
    }
    /// <summary>
    /// Callled when the mouse pointer enters the space
    /// </summary>
    /// <param name="pointerEventData"></param>
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        OnSelect(pointerEventData);
    }

    void Deselect()
    {
        bg.color = deselectedColor;
    }
}
