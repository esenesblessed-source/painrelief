using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    public string weaponName = "Weapon";

    public abstract void Initialize(WeaponManager manager);
    public abstract bool CanShoot();
    public abstract void Shoot(Transform origin);
    public abstract void StartReload();
}
