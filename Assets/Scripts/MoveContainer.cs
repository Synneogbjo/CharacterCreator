using UnityEngine;

public class MoveContainer : MonoBehaviour
{
    private RectTransform _Transform;
    private void Start()
    {
        _Transform = GetComponent<RectTransform>();
        _Transform.localPosition = new Vector3((_Transform.sizeDelta.x/2)-200f,_Transform.localPosition.y, _Transform.localPosition.z);
    }

    public void ChangeX(float movx)
    {
        var pos = _Transform.localPosition;
        var targetx = pos.x + movx;
        var width = _Transform.sizeDelta.x;
        
        if (targetx < -(width/2)+200f)
        {
            _Transform.localPosition = new Vector3((width/2)-200f, pos.y, pos.z);
        }
        else if (targetx > (width/2)-200f)
        {
            _Transform.localPosition = new Vector3(-(width/2)+200f, pos.y, pos.z);
        }
        else
        {
            _Transform.localPosition = new Vector3(targetx, pos.y, pos.z);
        }
    }
}
