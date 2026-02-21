using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    public WeaponBase slotSmall; // pistol
    public WeaponBase slotBig;   // rifle
    public int activeSlot = 0; // 0 = small, 1 = big

    [Header("Input Settings")]
    public KeyCode switchKey = KeyCode.Q;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode aimKey = KeyCode.Mouse1;

    private WeaponBase activeWeapon;
    private Camera cam;
    private float defaultFOV = 60f;

    void Start()
    {
        cam = Camera.main;
        if (cam != null) defaultFOV = cam.fieldOfView;
        Equip(activeSlot);
    }

    void Update()
    {
        HandleSwitch();
        HandleAim();
        HandleFireAndReload();
    }

    void HandleSwitch()
    {
        if (Input.GetKeyDown(switchKey))
        {
            activeSlot = (activeSlot == 0) ? 1 : 0;
            Equip(activeSlot);
        }
    }

    void HandleAim()
    {
        if (activeWeapon is Gun g && cam != null)
        {
            if (Input.GetKey(aimKey))
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, g.aimFOV, Time.deltaTime * 12f);
            }
            else
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime * 8f);
            }
        }
    }

    void HandleFireAndReload()
    {
        if (activeWeapon == null) return;

        // Support mobile input
        bool fire = Input.GetButton("Fire1");
        bool aimKeyActive = Input.GetKey(aimKey);
        if (MobileInput.Instance != null)
        {
            fire = MobileInput.Instance.fire;
            aimKeyActive = MobileInput.Instance.aim;
            // handle one-shot switch button
            if (MobileInput.Instance.switchWeapon)
            {
                activeSlot = (activeSlot == 0) ? 1 : 0;
                Equip(activeSlot);
                MobileInput.Instance.switchWeapon = false;
            }
        }

        // Fire
        if (activeWeapon is Gun g)
        {
            if (g.isAutomatic)
            {
                if (fire) g.Shoot(cam.transform);
            }
            else
            {
                if (Input.GetButtonDown("Fire1") || (MobileInput.Instance != null && MobileInput.Instance.fire)) g.Shoot(cam.transform);
            }
        }

        // Reload
        if (Input.GetKeyDown(reloadKey))
        {
            activeWeapon.StartReload();
        }
    }

    public void Equip(int slot)
    {
        activeSlot = slot;
        if (slot == 0) activeWeapon = slotSmall;
        else activeWeapon = slotBig;

        if (slotSmall != null) slotSmall.Initialize(this);
        if (slotBig != null) slotBig.Initialize(this);
    }

    public void AddAmmoToAll(int amount)
    {
        if (slotSmall is Gun gs) gs.AddAmmo(amount);
        if (slotBig is Gun gb) gb.AddAmmo(amount);
    }
}
