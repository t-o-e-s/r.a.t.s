using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.src.management.units;
using Library.src.units.control;
using UnityEngine.UI;

public class UI_Unit_Widget : MonoBehaviour
{
    Image bg;
    RectTransform bgPos;
    Vector3 startingPos;
    Vector3 targetPos;
    Button revealButton;
    bool isRevealed;
    
    void Awake()
    {
        bg = GetComponentInChildren<Image>();
        revealButton = GetComponentInChildren<Button>();
        isRevealed = false;

        bgPos = bg.GetComponent<RectTransform>();
        startingPos = new Vector3 (0f, -330f, 0f);
        targetPos = new Vector3(0f, -250f, 0f);

        bgPos.localPosition = startingPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RevealWidget()
    {
        if (isRevealed == false)
        {
            bgPos.localPosition = targetPos;
            isRevealed = true;
        }
        else if (isRevealed == true)
        {
            bgPos.localPosition = startingPos;
            isRevealed = false;
        }
    }
}
