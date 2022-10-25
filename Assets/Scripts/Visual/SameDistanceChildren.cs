using UnityEngine;

public class SameDistanceChildren : MonoBehaviour
{
    public Transform[] _children;

    private void Awake()
    {
        Vector3 firstElementPos = _children[0].transform.position;
        Vector3 lastElementPos = _children[_children.Length - 1].transform.position;

        float distX = (lastElementPos.x - firstElementPos.x) / (float) (_children.Length - 1);
        float distY = (lastElementPos.y - firstElementPos.y) / (float) (_children.Length - 1);
        float distZ = (lastElementPos.z - firstElementPos.z) / (float) (_children.Length - 1);

        Vector3 dist = new Vector3(distX, distY, distZ);

        for (int i = 1; i < _children.Length; i++)
            _children[i].transform.position = _children[i - 1].transform.position + dist;
    }
}
