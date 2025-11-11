using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;

    public float AmmoCount { get; private set; }
    [SerializeField] public float MaxAmmo { get; private set; } = 100f;

    private void Awake() 
    {
        Instance = this;
        AmmoCount = MaxAmmo;
    }

    public void TakeAmmo(float ammo)
    { 
        AmmoCount -= ammo;
        if (AmmoCount <= 0f)
        {
            AmmoCount = 0f;
        }

        Debug.Log($"Remaining Ammo: {AmmoCount}");
    }

    public void AddAmmo(float ammo)
    { 
        AmmoCount += ammo;
        if (AmmoCount >= MaxAmmo)
        {
            AmmoCount = MaxAmmo;
        }

        Debug.Log($"Ammo: {AmmoCount}");
    }
}
