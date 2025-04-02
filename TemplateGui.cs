using BepInEx;
using UnityEngine;
using GorillaLocomotion;

[BepInPlugin("com.Skittles.Gui", "Gui Mod", "1.1.1")]
public class TemplateGui : BaseUnityPlugin
{
    private float pullAmount = 0f;
    private float ArmAmount = 0f;
    private float SpeedAmount = 0f;
    private bool isTestOn = false;
    private bool isGuiVisible = true;

    private bool isDragging = false;
    private Vector2 offset;
    private Rect guiBoxRect;
    private Rect titleBarRect;

    public void Start()
    {
        guiBoxRect = new Rect((Screen.width / 2) - 250, (Screen.height / 2) - 300, 600f, 600f); // Dont Change Unless You Know What Your Doing
    }

    public void OnGUI()
    {
        titleBarRect = new Rect(guiBoxRect.x, guiBoxRect.y, guiBoxRect.width, 40f);

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.H) // KeyCode.H means H to toggle Gui Visibility Changable if you want
        {
            isGuiVisible = !isGuiVisible;
        }
        if (!isGuiVisible) return;

        if (Event.current.type == EventType.MouseDown && titleBarRect.Contains(Event.current.mousePosition))
        {
            isDragging = true;
            offset = Event.current.mousePosition - new Vector2(guiBoxRect.x, guiBoxRect.y);
            Event.current.Use();
        }
        else if (Event.current.type == EventType.MouseDrag && isDragging)
        {
            guiBoxRect.x = Event.current.mousePosition.x - offset.x;
            guiBoxRect.y = Event.current.mousePosition.y - offset.y;
            Event.current.Use();
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            isDragging = false;
        }

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.normal.background = CreateTexture(255, 182, 193, 255);
        boxStyle.normal.textColor = Color.gray;
        boxStyle.padding = new RectOffset(10, 10, 10, 10);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.normal.textColor = Color.gray;
        buttonStyle.normal.background = CreateTexture(255, 153, 174, 255);
        buttonStyle.border = new RectOffset(3, 3, 3, 3);
        buttonStyle.alignment = TextAnchor.MiddleCenter;

        buttonStyle.active.background = CreateTexture(50, 50, 50, 255);
        buttonStyle.focused.background = CreateTexture(100, 100, 100, 255);

        GUI.Box(guiBoxRect, "Closet UI", boxStyle); // Menu Name Here
        DrawBoxOutline(guiBoxRect, Color.white);

        float sliderY = guiBoxRect.y + 40f;

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 24;
        GUI.Label(new Rect(guiBoxRect.x + 20f, sliderY - 40f, 155f, 35f), "Settings:", labelStyle); // Built In Sliders By Skittles (PullSlider Is By Me And Inoxi) :D

        GUI.Label(new Rect(guiBoxRect.x + 20f, sliderY, 155f, 35f), "Pull Slider:");
        pullAmount = GUI.HorizontalSlider(new Rect(guiBoxRect.x + 200f, sliderY, 250f, 35f), pullAmount, 0f, 60f);

        sliderY += 40f;
        GUI.Label(new Rect(guiBoxRect.x + 20f, sliderY, 155f, 35f), "Arm Slider:");
        ArmAmount = GUI.HorizontalSlider(new Rect(guiBoxRect.x + 200f, sliderY, 250f, 35f), ArmAmount, 0f, 4f);

        sliderY += 40f;
        GUI.Label(new Rect(guiBoxRect.x + 20f, sliderY, 155f, 35f), "Speed Slider:");
        SpeedAmount = GUI.HorizontalSlider(new Rect(guiBoxRect.x + 200f, sliderY, 250f, 35f), SpeedAmount, 0f, 30f);

        float buttonY = sliderY + 40f;
        float buttonWidth = 150f;
        float buttonSpacing = 10f;

        if (GUI.Button(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f), "Set Pull", buttonStyle))
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GTPlayer.Instance.transform.forward * Time.deltaTime * pullAmount;
            }
        }
        DrawButtonOutline(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f));

        if (GUI.Button(new Rect(guiBoxRect.x + 20f + buttonWidth + buttonSpacing, buttonY, buttonWidth, 35f), "Reset Pull", buttonStyle))
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GTPlayer.Instance.transform.forward * Time.deltaTime * 0f;
            }
        }
        DrawButtonOutline(new Rect(guiBoxRect.x + 20f + buttonWidth + buttonSpacing, buttonY, buttonWidth, 35f));

        buttonY += 40f;
        if (GUI.Button(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f), "Set Arms", buttonStyle))
        {
            GTPlayer.Instance.transform.localScale = new Vector3(ArmAmount, ArmAmount, ArmAmount);
        }
        DrawButtonOutline(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f));

        if (GUI.Button(new Rect(guiBoxRect.x + 20f + buttonWidth + buttonSpacing, buttonY, buttonWidth, 35f), "Reset Arms", buttonStyle))
        {
            GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        DrawButtonOutline(new Rect(guiBoxRect.x + 20f + buttonWidth + buttonSpacing, buttonY, buttonWidth, 35f));

        buttonY += 40f;
        if (GUI.Button(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f), "Set Speed", buttonStyle))
        {
            GTPlayer.Instance.maxJumpSpeed = SpeedAmount;
            GTPlayer.Instance.jumpMultiplier = SpeedAmount;
            GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        DrawButtonOutline(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f));

        buttonY += 60f;
        GUI.Label(new Rect(guiBoxRect.x + 20f, buttonY, 155f, 35f), "Closets:", labelStyle);
        buttonY += 30f;

        string toggleText = isTestOn ? "WeakWallWalk | On" : "WeakWallWalk | Off"; // This Will be Your Toggle Button Which When Turned On Will Go To ToggleButton | On
        if (GUI.Button(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f), toggleText, buttonStyle))
        {
            isTestOn = !isTestOn;

            if (ControllerInputPoller.instance.rightGrab && ControllerInputPoller.instance.leftGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.forward * (Time.deltaTime * (2.5f / Time.deltaTime)), ForceMode.Acceleration);
            }
        }
        DrawButtonOutline(new Rect(guiBoxRect.x + 20f, buttonY, buttonWidth, 35f));
    }

    private void DrawBoxOutline(Rect rect, Color outlineColor)
    {
        Color originalColor = GUI.color; // outline code dont change if you want
        GUI.color = outlineColor;
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 2, rect.width, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y, 2, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x + rect.width - 2, rect.y, 2, rect.height), Texture2D.whiteTexture);
        GUI.color = originalColor;
    }

    private void DrawButtonOutline(Rect rect)
    {
        Color originalColor = GUI.color; // Outline Code dont change if you want
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - 2, rect.width, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x, rect.y, 2, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.x + rect.width - 2, rect.y, 2, rect.height), Texture2D.whiteTexture);
        GUI.color = originalColor;
    }

    private Texture2D CreateTexture(byte r, byte g, byte b, byte a)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color32(r, g, b, a));
        texture.Apply();
        return texture;
    }
}
