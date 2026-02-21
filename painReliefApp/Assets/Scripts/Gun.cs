using UnityEngine;
using System.Collections;

public class Gun : WeaponBase
{
    [Header("Gun Stats")]
    public float damagePercent = 20f; // applied to IDamageable
    public float fireRate = 3f; // shots per second
    public int magazineSize = 12;
    public int currentMagazine = 12;
    public int reserveAmmo = 60;
    public float reloadTime = 2f;
    public float range = 200f;
    public bool isAutomatic = false;

    [Header("Aim")]
    public float aimFOV = 40f;
    public float hipFOV = 60f;
    public float aimSensitivityMultiplier = 0.6f;

    private float lastShotTime = -999f;
    private bool isReloading = false;
    private WeaponManager manager;

    void Start()
    {
        currentMagazine = Mathf.Clamp(currentMagazine, 0, magazineSize);
    }

    public override void Initialize(WeaponManager mgr)
    {
        manager = mgr;
    }

    public override bool CanShoot()
    {
        if (isReloading) return false;
        if (currentMagazine <= 0) return false;
        return Time.time - lastShotTime >= 1f / Mathf.Max(0.0001f, fireRate);
    }

    public override void Shoot(Transform origin)
    {
        if (!CanShoot()) return;
        lastShotTime = Time.time;
        currentMagazine = Mathf.Max(0, currentMagazine - 1);

        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out hit, range))
        {
            var dmg = hit.collider.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.ApplyDamage(damagePercent);
            }
        }
        // make noise so enemies can hear the gunshot
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.MakeSound(origin.position, 40f);
        }
        // play audio if available
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGunshot(origin.position);
        }
    }

    public override void StartReload()
    {
        if (isReloading) return;
        if (currentMagazine >= magazineSize) return;
        if (reserveAmmo <= 0) return;
        if (manager != null)
            manager.StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        int needed = magazineSize - currentMagazine;
        int taken = Mathf.Min(needed, reserveAmmo);
        currentMagazine += taken;
        reserveAmmo -= taken;
        isReloading = false;
    }

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
    }
}
