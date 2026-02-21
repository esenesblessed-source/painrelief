using UnityEngine;

public class MobileInput : MonoBehaviour
{
    public static MobileInput Instance { get; private set; }

    [Header("References")]
    public OnScreenJoystick moveJoystick;

    [HideInInspector] public Vector2 move = Vector2.zero;
    [HideInInspector] public bool fire = false;
    [HideInInspector] public bool aim = false;
    [HideInInspector] public bool jump = false;
    [HideInInspector] public bool run = false;
    [HideInInspector] public bool switchWeapon = false;
    [HideInInspector] public bool enterExit = false;

    void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);
    }

    void Update()
    {
        if (moveJoystick != null)
        {
            move = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        }
    }

    // These methods are called by UI buttons (OnPointerDown/OnPointerUp or OnClick)
    public void ButtonDown(string name)
    {
        switch (name)
        {
            case "Fire": fire = true; break;
            case "Aim": aim = true; break;
            case "Jump": jump = true; break;
            case "Run": run = true; break;
            case "Switch": switchWeapon = true; break;
            case "EnterExit": enterExit = true; break;
        }
    }

    public void ButtonUp(string name)
    {
        switch (name)
        {
            case "Fire": fire = false; break;
            case "Aim": aim = false; break;
            case "Jump": jump = false; break;
            case "Run": run = false; break;
            case "Switch": switchWeapon = false; break;
            case "EnterExit": enterExit = false; break;
        }
    }

    // Convenience one-shot for click buttons
    public void ButtonClick(string name)
    {
        if (name == "Switch") switchWeapon = true;
        if (name == "EnterExit") enterExit = true;
    }
}
