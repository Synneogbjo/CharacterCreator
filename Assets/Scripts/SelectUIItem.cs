using System;
using UnityEngine;

public class SelectUIItem : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    private CheckMouseCollision _MouseCollision;
    private ImageController _Image;
    private ObjectController _Objects;

    private void Start()
    {
        _MouseCollision = GetComponent<CheckMouseCollision>();
        _Image = GetComponent<ImageController>();
        _Objects = GetComponent<ObjectController>();
    }

    public void AddItem(int item)
    {
        _MouseCollision.selectedObject = Instantiate(itemPrefab, new Vector3(0, 0, 49f), Quaternion.identity);
        _MouseCollision.selectedObject.GetComponent<SpriteRenderer>().sprite = _Image.Images[item];
        _Objects.objects.Add(_MouseCollision.selectedObject);
        if (!MyInput.sans) _MouseCollision.m_Audio.PlayOneShot(_MouseCollision.Place);
        else _MouseCollision.m_Audio.PlayOneShot(_MouseCollision.sansSound);
        
        //Set correct box collider size
        var spr = _MouseCollision.selectedObject.GetComponent<SpriteRenderer>().sprite;
        _MouseCollision.selectedObject.GetComponent<BoxCollider>().size = new Vector3(Math.Abs(_MouseCollision.selectedObject.transform.localScale.x) * spr.texture.width / spr.pixelsPerUnit, Math.Abs(_MouseCollision.selectedObject.transform.localScale.y) * spr.texture.height / spr.pixelsPerUnit, 0.05f);
        _MouseCollision.selectedObject.GetComponent<ItemInfo>().item = item;
    }

    public void AddItemExt(int item, Vector3 size, Vector3 rotation)
    {
        _MouseCollision.selectedObject = Instantiate(itemPrefab, new Vector3(0, 0, 49f), Quaternion.identity);
        _MouseCollision.selectedObject.GetComponent<SpriteRenderer>().sprite = _Image.Images[item];
        _Objects.objects.Add(_MouseCollision.selectedObject);
        if (!MyInput.sans) _MouseCollision.m_Audio.PlayOneShot(_MouseCollision.Place);
        else _MouseCollision.m_Audio.PlayOneShot(_MouseCollision.sansSound);
        
        //Set correct box collider size
        var spr = _MouseCollision.selectedObject.GetComponent<SpriteRenderer>().sprite;
        _MouseCollision.selectedObject.GetComponent<BoxCollider>().size = new Vector3(Math.Abs(_MouseCollision.selectedObject.transform.localScale.x) * spr.texture.width / spr.pixelsPerUnit, Math.Abs(_MouseCollision.selectedObject.transform.localScale.y) * spr.texture.height / spr.pixelsPerUnit, 0.05f);
        _MouseCollision.selectedObject.GetComponent<ItemInfo>().item = item;

        _MouseCollision.selectedObject.transform.localScale = size;
        _MouseCollision.selectedObject.transform.eulerAngles = rotation;
    }
}
