using TMPro;
using UnityEngine;

public class HealthCountManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    
    private void Update()
    {
        healthText.SetText($"{PlayerHealthManager.Instance.PlayerHealth}hp");
    }
}
