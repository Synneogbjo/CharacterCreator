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

    public static Vector2 mousePos;
    public static Ray mouseInWorld;
    
    private Camera cam;
    [SerializeField] private CameraSaveScreenshot _Screenshot;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            _Screenshot.TakeHiResShot();
        }
        if(Keyboard.current.backspaceKey.wasPressedThisFrame) delete = true;
        
        if(Mouse.current.leftButton.wasPressedThisFrame) leftPressed = true;
        leftHold = Mouse.current.leftButton.isPressed;
        rightHold = Mouse.current.rightButton.isPressed;

        rotateRight = Keyboard.current.eKey.isPressed;
        rotateLeft = Keyboard.current.qKey.isPressed;
        if (Keyboard.current.rKey.wasPressedThisFrame) resetRotation = true;
        
        mousePos = Mouse.current.position.ReadValue();
        mouseInWorld = cam.ScreenPointToRay(mousePos);
    }
}