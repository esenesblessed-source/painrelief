using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public int windowWidth = 120;
    public int windowHeight = 60;
    float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(10, 10, windowWidth, windowHeight);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 14;
        style.normal.textColor = Color.white;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms\n{1:0.} fps", msec, fps);
        GUI.Label(rect, text, style);
    }
}
