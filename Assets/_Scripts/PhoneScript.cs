using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhoneScript : MonoBehaviour
{
    [SerializeField] private GameObject Phone;
    [SerializeField] private GameObject startPos, endPos;
    private bool moveUp, MoveDown;
    [SerializeField] private GameObject ImgTop, ImgBottom;
    [SerializeField] private TextMeshProUGUI CallerID;
    [SerializeField] private AudioSource PhoneAudio;
    private AudioClip phoneAudioClip;
    [SerializeField] private Animator hinge;
    [SerializeField] private PhoneSO call;
    [SerializeField] private SubtitleManager subtitles;
    [SerializeField] private GameManager manager;
    private GameState beforeDialogue;

    private float startTime, journeyLength;
    private float speed = 200;
    public void PhoneCall(PhoneSO phoneData)
    {
        subtitles.SetText(phoneData.subtitleFile);
        MoveDown = false;
        moveUp = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos.transform.position, endPos.transform.position);
        ImgTop.GetComponent<Image>().sprite = phoneData.callerPfpTop;
        ImgBottom.GetComponent<Image>().sprite = phoneData.callerPfpBottom;
        CallerID.text = phoneData.callerID;
        PhoneAudio.clip = phoneData.ringTone;
        PhoneAudio.Play();
        phoneAudioClip = phoneData.phoneCall;
    }

    public void AcceptCall()
    {
        PhoneAudio.clip = phoneAudioClip;
        PhoneAudio.Play();
        beforeDialogue = manager.State;
        manager.State = GameState.Dialogue;
        subtitles.StartDialogue();
        hinge.SetBool("Bobbing", true);
    }
    public void EndPhoneCall()
    {
        moveUp = false;
        MoveDown = true;
        startTime = Time.time;
        manager.State = beforeDialogue;
        PhoneAudio.Stop();
        hinge.SetBool("Bobbing", false);
    }

    private void Update()
    {
        if(moveUp)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            Phone.transform.position = Vector3.Lerp(startPos.transform.position, endPos.transform.position, fractionOfJourney);
        }
        if (MoveDown)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;
            Phone.transform.position = Vector3.Lerp(endPos.transform.position, startPos.transform.position, fractionOfJourney);
        }
        if (Input.GetKeyDown("1"))
        {
            PhoneCall(call);
        }
        if (Input.GetKeyDown("2"))
        {
            AcceptCall();
        }
    }

    private void Start()
    {
        manager = FindAnyObjectByType<GameManager>();
    }
}
