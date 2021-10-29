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
    [SerializeField] private AudioClip CameraSound;
    [SerializeField] private AudioClip rotateSound;
    [SerializeField] private AudioClip sansSound;

    [SerializeField] private float itemMinSize = 0.08f;
    private bool holding;
    private float rotateTimer;
    private float rotateMaxTimer = 0.05f;
    private bool sans;
    
    private Sprite spr;

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
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    if (MyInput.rotateRight)
                    {
                        selectedObject.transform.Rotate(0f, 0f, -2f, Space.Self);
                    }
                    else selectedObject.transform.Rotate(0f, 0f, 2f, Space.Self);

                    if (rotateTimer <= 0f)
                    {
                        if(!sans) m_Audio.PlayOneShot(rotateSound);
                        else m_Audio.PlayOneShot(sansSound);
                        rotateTimer = rotateMaxTimer;
                    }
                    else rotateTimer -= Time.fixedDeltaTime;
                }
            }
        }
        else rotateTimer = 0f;

        //Reset rotation of selected object
        if (MyInput.resetRotation)
        {
            MyInput.resetRotation = false;
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked) selectedObject.transform.rotation = Quaternion.identity;
            }
        }
        
        //Delete selected object
        if (MyInput.delete)
        {
            MyInput.delete = false;
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    Destroy(selectedObject);
                    _Objects.objects.Remove(selectedObject);
                    m_Audio.PlayOneShot(Delete);
                }
            }
        }
        
        //Lock or Unlock Item
        if (MyInput.changeLock && selectedObject != null)
        {
            selectedObject.GetComponent<ItemInfo>().locked = !selectedObject.GetComponent<ItemInfo>().locked;
            MyInput.changeLock = false;
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
                    if (!selectedObject.GetComponent<ItemInfo>().locked)
                    {
                        _Objects.objects.Remove(selectedObject);
                        _Objects.objects.Add(selectedObject);
                    }

                    holding = true;
                }
                else
                {
                    selectedObject = null;
                }
            }

            if (MyInput.leftHold && selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked) selectedObject.transform.position = new Vector3(mouseTarget.point.x, mouseTarget.point.y, selectedObject.transform.position.z);
            }
            else if (holding)
            {
                holding = false;
                m_Audio.PlayOneShot(Place);
            }

            if (MyInput.rightHold && selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    selectedObject.transform.localScale =
                        new Vector3(
                            (MyInput.mouseInWorld.origin.x - selectedObject.transform.position.x) * 2 *
                            spr.pixelsPerUnit / spr.texture.width,
                            (MyInput.mouseInWorld.origin.y - selectedObject.transform.position.y) * 2 *
                            spr.pixelsPerUnit / spr.texture.height,
                            selectedObject.transform.localScale.z);
                    var scale = selectedObject.transform.localScale;
                    if (scale.x < itemMinSize && scale.x >= 0f) scale = new Vector3(itemMinSize, scale.y, scale.z);
                    if (scale.x > -itemMinSize && scale.x <= 0f) scale = new Vector3(-itemMinSize, scale.y, scale.z);
                    if (scale.y < itemMinSize && scale.y >= 0f) scale = new Vector3(scale.x, itemMinSize, scale.z);
                    if (scale.y > -itemMinSize && scale.y <= 0f) scale = new Vector3(scale.x, -itemMinSize, scale.z);
                    selectedObject.transform.localScale = scale;
                }
            }
        }
    }

    public void TakeScreenShot()
    {
        CameraSaveScreenshot.TakeHiResShot();
        m_Audio.PlayOneShot(CameraSound);
    }
}