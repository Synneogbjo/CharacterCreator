using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();

    private void FixedUpdate()
    {
        for (var i = 0; i < objects.Count; i++)
        {
            var pos = objects[i].transform.position;
            var zpos = 50f - (0.1f * (i + 1));

            if (zpos <= 0f)
            {
                objects.RemoveAt(0);
            }
            objects[i].transform.position = new Vector3(pos.x, pos.y, zpos);
        }
    }
}
