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
        var xpos = MyInput.mouseInWorld.origin.x;
        var ypos = MyInput.mouseInWorld.origin.y;
        _MouseCollision.selectedObject = Instantiate(itemPrefab, new Vector3(xpos, ypos, 49f), Quaternion.identity);
        _MouseCollision.selectedObject.GetComponent<SpriteRenderer>().sprite = _Image.Images[item];
        _Objects.objects.Add(_MouseCollision.selectedObject);
    }
}
