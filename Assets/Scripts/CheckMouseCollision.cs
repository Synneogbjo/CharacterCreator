using UnityEngine;
using UnityEngine.InputSystem;

public class CheckMouseCollision : MonoBehaviour
{
    private Camera cam;
    private Vector3 mouseStartPos;
    public GameObject selectedObject;
    private RaycastHit mouseTarget;
    private ObjectController _Objects;
    [SerializeField] private AudioSource m_Audio;
    [SerializeField] private AudioClip PickUp;
    [SerializeField] private AudioClip Place;
    [SerializeField] private AudioClip Delete;
    [SerializeField] private AudioClip Stretch;
<<<<<<< Updated upstream
    [SerializeField] private AudioClip CameraSound;
    private bool holding;

    private Sprite spr;
=======
    
    /* USED FOR TESTING MOVEMENT, REMOVE */
    public TMP_Text MouseX;
    public TMP_Text MouseY;
    public TMP_Text ImageX;
    public TMP_Text ImageY;
    public TMP_Text Width;
    public TMP_Text Height;
>>>>>>> Stashed changes

    private void Start()
    {
        cam = Camera.main;
        m_Audio = GetComponent<AudioSource>();
        _Objects = GetComponent<ObjectController>();
    }

    private void FixedUpdate()
    {
        //Rotate selected object
        if (MyInput.rotateRight || MyInput.rotateLeft)
        {
            if (MyInput.rotateRight)
            {
                selectedObject.transform.Rotate(0f,0f,-2f,Space.Self);
            }
            else selectedObject.transform.Rotate(0f, 0f, 2f, Space.Self);
        }

        //Reset rotation of selected object
        if (MyInput.resetRotation)
        {
            MyInput.resetRotation = false;
            if (selectedObject != null)
            {
                selectedObject.transform.rotation = Quaternion.identity;
            }
        }
        
        //Delete selected object
        if (MyInput.delete)
        {
            MyInput.delete = false;
            if (selectedObject != null)
            {
                Destroy(selectedObject);
                _Objects.objects.Remove(selectedObject);
                m_Audio.PlayOneShot(Delete);
            }
        }
        
        //Variable for the selected object's sprite
        if (selectedObject != null) spr = selectedObject.GetComponent<SpriteRenderer>().sprite;
        
        //Check Mouse Position and Clicking
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
                    _Objects.objects.Remove(selectedObject);
                    _Objects.objects.Add(selectedObject);
                    holding = true;
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
            else if (holding)
            {
                holding = false;
                m_Audio.PlayOneShot(Place);
            }

            if (MyInput.rightHold && selectedObject != null)
            {
                selectedObject.transform.localScale =
                    new Vector3((MyInput.mouseInWorld.origin.x - selectedObject.transform.position.x) * 2 * spr.pixelsPerUnit / spr.texture.width,
                                (MyInput.mouseInWorld.origin.y - selectedObject.transform.position.y) * 2 * spr.pixelsPerUnit / spr.texture.height,
                                  selectedObject.transform.localScale.z);
            }
        }
    }

    public void TakeScreenShot()
    {
        CameraSaveScreenshot.TakeHiResShot();
    }
}