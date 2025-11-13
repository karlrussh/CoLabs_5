using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PhoneSO", menuName = "ScriptableObject/PhoneSO")]

public class PhoneSO : ScriptableObject
{
    [field: SerializeField] public string callerID { get; private set; }
    [field: SerializeField] public AudioClip phoneCall { get; private set; }
    [field: SerializeField] public Sprite callerPfpTop { get; private set; }
    [field: SerializeField] public Sprite callerPfpBottom { get; private set; }
    [field: SerializeField] public AudioClip ringTone { get; private set; }

}
