using UnityEngine;
using UnityEngine.InputSystem;

public class CheckMouseCollision : MonoBehaviour
{
    private Camera cam;
    private Vector3 mouseStartPos;
    [HideInInspector] public GameObject selectedObject;
    private RaycastHit mouseTarget;
    private ObjectController _Objects;

    private void Start()
    {
        cam = Camera.main;
        _Objects = GetComponent<ObjectController>();
    }

    private void FixedUpdate()
    {
        var ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out mouseTarget, 60f))
        {
            if (MyInput.delete)
            {
                MyInput.delete = false;
                if (selectedObject != null)
                {
                    _Objects.objects.Remove(selectedObject);
                    Destroy(selectedObject);
                }
            }
            //Tries to set the selected object to the object you click
            if (MyInput.leftPressed)
            {
                MyInput.leftPressed = false;
                //Checks whether you clicked a selectable object
                if (mouseTarget.collider.CompareTag("Selectable"))
                {
                    selectedObject = mouseTarget.collider.gameObject;
                }
                else
                {
                    selectedObject = null;
                }
            }

            if (MyInput.leftHold && selectedObject != null)
            {
                selectedObject.transform.position = new Vector3(mouseTarget.point.x, mouseTarget.point.y, selectedObject.transform.position.z);
            }
        }
    }
}