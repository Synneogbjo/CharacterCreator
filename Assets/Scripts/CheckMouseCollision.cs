using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheckMouseCollision : MonoBehaviour
{
    private Camera cam;
    private Vector3 mouseStartPos;
    public GameObject selectedObject;
    private RaycastHit mouseTarget;
    private ObjectController _Objects;
    private GameObject stretchAudio;
    private SelectUIItem _UIItem;

    [SerializeField] private Canvas _ExitCanvas;
    [SerializeField] private Canvas _ItemCanvas;
    
    public AudioSource m_Audio;
    [SerializeField] private AudioClip PickUp;
    public AudioClip Place;
    [SerializeField] private AudioClip Delete;
    [SerializeField] private AudioClip CameraSound;
    [SerializeField] private AudioClip rotateSound;
    [SerializeField] private AudioClip lockSound;
    [SerializeField] private AudioClip unlockSound;
    [SerializeField] private AudioClip undoSound;
    [SerializeField] private AudioClip flipSound;
    public AudioClip sansSound;

    [SerializeField] private float itemMinSize = 0.08f;
    private bool holding;
    private float rotateTimer;
    private float rotateMaxTimer = 0.05f;

    private Sprite spr;

    private void Start()
    {
        cam = Camera.main;
        m_Audio = GetComponent<AudioSource>();
        _Objects = GetComponent<ObjectController>();
        stretchAudio = GameObject.Find("stretchAudio");
        _UIItem = GetComponent<SelectUIItem>();
        _ItemCanvas.gameObject.SetActive(true);
        _ExitCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (MyInput.exit)
        {
            _ExitCanvas.gameObject.SetActive(true);
        }
        else _ExitCanvas.gameObject.SetActive(false);

        if (MyInput.hideUI)
        {
            _ItemCanvas.gameObject.SetActive(false);
        }
        else _ItemCanvas.gameObject.SetActive(true);
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
                        if(!MyInput.sans) m_Audio.PlayOneShot(rotateSound);
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
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    selectedObject.transform.rotation = Quaternion.identity;

                    if (!MyInput.sans)
                    {
                        m_Audio.PlayOneShot(undoSound);
                    }
                    else m_Audio.PlayOneShot(sansSound);
                }
            }
        }
        
        //Reset size of selected object
        if (MyInput.resetSize)
        {
            MyInput.resetSize = false;
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    selectedObject.transform.localScale = new Vector3(1f, 1f, selectedObject.transform.localScale.z);
                    
                    if (!MyInput.sans)
                    {
                        m_Audio.PlayOneShot(undoSound);
                    }
                    else m_Audio.PlayOneShot(sansSound);
                }
            }
        }
        
        //Flip item
        if (MyInput.flip)
        {
            MyInput.flip = false;
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    var scale = selectedObject.transform.localScale;
                    selectedObject.transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
                    selectedObject.transform.eulerAngles = -selectedObject.transform.eulerAngles;

                    if (!MyInput.sans)
                    {
                        m_Audio.PlayOneShot(flipSound);
                    }
                    else m_Audio.PlayOneShot(sansSound);
                }
            }
        }
        
        //Duplicate item
        if (MyInput.duplicate)
        {
            MyInput.duplicate = false;
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    _UIItem.AddItemExt(selectedObject.GetComponent<ItemInfo>().item, selectedObject.transform.localScale, selectedObject.transform.eulerAngles);
                }
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
                    if(!MyInput.sans) m_Audio.PlayOneShot(Delete);
                    else m_Audio.PlayOneShot(sansSound);
                }
            }
        }
        
        //Lock or Unlock Item
        if (MyInput.changeLock && selectedObject != null)
        {
            selectedObject.GetComponent<ItemInfo>().locked = !selectedObject.GetComponent<ItemInfo>().locked;
            MyInput.changeLock = false;
            if (!MyInput.sans)
            {
                if (selectedObject.GetComponent<ItemInfo>().locked)
                {
                    m_Audio.PlayOneShot(lockSound);
                }
                else m_Audio.PlayOneShot(unlockSound);
            }
            else m_Audio.PlayOneShot(sansSound);
        }

        if (MyInput.moveItemToFront)
        {
            MyInput.moveItemToFront = false;
            if (selectedObject != null)
            {
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    _Objects.objects.Remove(selectedObject);
                    _Objects.objects.Add(selectedObject);
                }
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
                    if(!MyInput.sans) m_Audio.PlayOneShot(PickUp);
                    else m_Audio.PlayOneShot(sansSound);

                    holding = true;
                }
                else
                {
                    selectedObject = null;
                }
            }

            if (MyInput.leftHold && selectedObject != null)
            {
                print("moving");
                if (!selectedObject.GetComponent<ItemInfo>().locked) selectedObject.transform.position = new Vector3(mouseTarget.point.x, mouseTarget.point.y, selectedObject.transform.position.z);
            }
            else if (holding)
            {
                holding = false;
                if (!MyInput.sans) m_Audio.PlayOneShot(Place);
                else m_Audio.PlayOneShot(sansSound);
            }

            if (MyInput.rightHold && selectedObject != null)
            {
                print("scaling");
                if (!selectedObject.GetComponent<ItemInfo>().locked)
                {
                    if (!stretchAudio.GetComponent<AudioSource>().isPlaying) stretchAudio.GetComponent<AudioSource>().Play();
                    
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
            else if (stretchAudio.GetComponent<AudioSource>().isPlaying) stretchAudio.GetComponent<AudioSource>().Stop();
        }
    }

    public void TakeScreenShot()
    {
        CameraSaveScreenshot.TakeHiResShot();
        if (!MyInput.sans) m_Audio.PlayOneShot(CameraSound);
        else m_Audio.PlayOneShot(sansSound);
    }
}