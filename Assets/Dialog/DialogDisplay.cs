using UnityEngine;

public class DialogDisplay : MonoBehaviour
{
    public Conversation conversation;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    RectTransform[] dialogObjects;

    SpeakerUI speakerUILeft;
    SpeakerUI speakerUIRight;

    public bool convoCompleted;

    int activeLineIndex = 0;

    void Awake()
    {
        //this covo starts upon game start. Temp code whilst there is only 1 level and no start sequence. Update when we aren't booting to level directly.
        convoCompleted = false;

        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;

        dialogObjects = GetComponentsInChildren<RectTransform>();
        SetDisplay(false);
    }

    void Update()
    {
        //add more button mappings here
        if (Input.GetMouseButtonDown(0))
            AdvanceConversation();
    }

    void AdvanceConversation()
    {
        if ((activeLineIndex < conversation.lines.Length) && !convoCompleted)
        {
            if (activeLineIndex == 0) SetDisplay(true);
            DisplayLine();
            activeLineIndex += 1;
        }
        else
        {
            speakerUILeft.Hide();
            speakerUIRight.Hide();
            activeLineIndex = 0;
            convoCompleted = true;
            SetDisplay(false);
        }
    }

    // Assumptions: Only two characters
    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
    }

    void SetDialog(
        SpeakerUI activeSpeakerUI,
        SpeakerUI inactiveSpeakerUI,
        string text
    )
    {
        activeSpeakerUI.Dialog = text;
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();
    }

    void SetDisplay(bool display)
    {
        foreach(RectTransform rect in dialogObjects)
        {
            if (rect.gameObject.Equals(gameObject)) continue;
            rect.gameObject.SetActive(display);
        }
    }
}
