using UnityEngine;
using UnityEngine.InputSystem;

public class MyInput : MonoBehaviour
{
    public static bool leftPressed;
    public static bool leftHold;
    public static bool rightHold;
    public static bool delete;
    public static bool rotateRight;
    public static bool rotateLeft;
    public static bool resetRotation;
    public static bool resetSize;
    public static bool changeLock;
    public static bool sans;
    public static bool moveItemToFront;
    public static bool flip;
    public static bool duplicate;
    public static bool exit;
    public static bool hideUI;

    public static Vector2 mousePos;
    public static Ray mouseInWorld;
    
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) exit = !exit;
        if (Keyboard.current.xKey.wasPressedThisFrame) changeLock = true;
        if (Keyboard.current.backspaceKey.wasPressedThisFrame) delete = true;
        if (Keyboard.current.sKey.wasPressedThisFrame) sans = !sans;
        if (Keyboard.current.wKey.wasPressedThisFrame) moveItemToFront = true;
        if (Keyboard.current.dKey.wasPressedThisFrame) duplicate = true;
        if (Keyboard.current.tabKey.wasPressedThisFrame) hideUI = !hideUI;
        
        if (Mouse.current.leftButton.wasPressedThisFrame) leftPressed = true;
        leftHold = Mouse.current.leftButton.isPressed;
        rightHold = Mouse.current.rightButton.isPressed;

        rotateRight = Keyboard.current.eKey.isPressed;
        rotateLeft = Keyboard.current.qKey.isPressed;
        if (Keyboard.current.rKey.wasPressedThisFrame) resetRotation = true;
        if (Keyboard.current.spaceKey.wasPressedThisFrame) resetSize = true;
        if (Keyboard.current.fKey.wasPressedThisFrame) flip = true;
        
        mousePos = Mouse.current.position.ReadValue();
        mouseInWorld = cam.ScreenPointToRay(mousePos);
    }
}