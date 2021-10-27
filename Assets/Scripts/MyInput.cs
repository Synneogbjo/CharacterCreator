using UnityEngine;
using UnityEngine.InputSystem;

public class MyInput : MonoBehaviour
{
    public static bool takeScreenshot;
    public static bool leftPressed;
    public static bool leftHold;

    // Update is called once per frame
    void Update()
    {
        takeScreenshot = Keyboard.current.kKey.wasPressedThisFrame;
        if(Mouse.current.leftButton.wasPressedThisFrame) leftPressed = true;
        leftHold = Mouse.current.leftButton.isPressed;
    }
}