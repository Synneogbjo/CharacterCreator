using UnityEngine;
using UnityEngine.InputSystem;

public class CheckMouseCollision : MonoBehaviour
{
    private Camera cam;
    private Vector3 mouseStartPos;
    public GameObject selectedObject;
    private RaycastHit mouseTarget;
    [SerializeField] public AudioSource m_Audio;
    [SerializeField] public AudioClip PickUp;
    [SerializeField] public AudioClip Place;


    private void Start()
    {
        cam = Camera.main;
        m_Audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        print(Mouse.current.position.ReadValue());
        
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out mouseTarget, 60f))
        {
            //Tries to set the selected object to the object you click
            if (MyInput.leftPressed)
            {
                MyInput.leftPressed = false;
                //Checks whether you clicked a selectable object
                if (mouseTarget.collider.CompareTag("Selectable"))
                {
                    selectedObject = mouseTarget.collider.gameObject;
                    m_Audio.PlayOneShot(PickUp);
                }
                else
                {
                    selectedObject = null;
                }
            }

            if (MyInput.leftHold && selectedObject != null)
            {
                selectedObject.transform.position = new Vector3(mouseTarget.point.x, mouseTarget.point.y, selectedObject.transform.position.z);
                m_Audio.PlayOneShot(Place);
            }
        }
    }
}