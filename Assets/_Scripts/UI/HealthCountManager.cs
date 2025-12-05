using DG.Tweening;
using TMPro;
using UnityEngine;

public class HealthCountManager : MonoBehaviour
{
    public static HealthCountManager Instance;

    private void Awake() => Instance = this;
    
    [SerializeField] private TextMeshProUGUI healthText;
    
    private void Update()
    {
        healthText.SetText($"{PlayerHealthManager.Instance.PlayerHealth}hp");
    }

    public void ShakeHealthText()
    {    
        transform.DOShakePosition(0.25f, 4f, 20);
    }
}
