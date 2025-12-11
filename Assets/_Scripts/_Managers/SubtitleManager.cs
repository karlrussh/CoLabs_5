using UnityEngine;
using TMPro;
using TMPro.Examples;
using System.Collections;
using System.IO;
public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;
    [SerializeField] private GameObject subtitles;
    [SerializeField] private PhoneScript phone;
    [SerializeField] public string callerID;

    private int index;
    private char seperator = '\n';
    private bool dialogueActive = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && dialogueActive)
        {
            if(textComponent.text == "<b>" + callerID + ":</b> " + lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = "<b>" + callerID + ":</b> " + lines[index];
            }
        }
    }
    public void SetText(TextAsset subtitleFile)
    {
        lines = subtitleFile.text.Split(seperator);
    }
    public void StartDialogue()
    {
        subtitles.SetActive(true);
        dialogueActive = true;
        index = 0;
        StartCoroutine(DisplayText());
    }
    
    private IEnumerator DisplayText()
    {
        textComponent.text = ("<b>" + callerID + ":</b> ");
        foreach (char c in lines[index].ToCharArray() )
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length -1)
        {
            index++;
            textComponent.text = null;
            StartCoroutine(DisplayText());
        }
        else
        {
            subtitles.SetActive(false);
            lines = null;
            dialogueActive = false;
            textComponent.text = null;
            phone.EndPhoneCall();
        }
    }

}
