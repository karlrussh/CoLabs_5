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

    private int index;
    private char seperator = '\n';
    private bool dialogueActive = false;

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && dialogueActive)
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
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
        foreach(char c in lines[index].ToCharArray() )
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
