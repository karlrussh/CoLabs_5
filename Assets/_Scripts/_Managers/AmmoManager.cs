using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager Instance;

    public float AmmoCount { get; private set; }
    [SerializeField] private float MaxAmmo;

    private void Awake() => Instance = this;

    public void TakeAmmo(float ammo)
    { 
        AmmoCount -= ammo;
        if (AmmoCount <= MaxAmmo)
        {
            AmmoCount = 0f;
        }
    }

    public void AddAmmo(float ammo)
    { 
        AmmoCount += ammo;
        if (AmmoCount >= MaxAmmo)
        {
            AmmoCount = MaxAmmo;
        }
    }
}
