using UnityEngine;
using TMPro;
using TMPro.Examples;
using System.Collections;
public class SubtitleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private string[] lines;
    [SerializeField] private float textSpeed;

    private int index;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
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

    public void StartDialogue()
    {
        Time.timeScale = 0;
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
            textComponent.text = string.Empty;
            StartCoroutine(DisplayText());
        }
        else
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

}
