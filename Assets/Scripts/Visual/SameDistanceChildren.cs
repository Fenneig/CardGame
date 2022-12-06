using UnityEngine;

namespace Visual
{
    public class SameDistanceChildren : MonoBehaviour
    {
        [SerializeField] private Transform[] _children;

        public Transform[] Children => _children;
    
        private void Awake()
        {
            Vector3 firstElementPos = _children[0].transform.position;
            Vector3 lastElementPos = _children[_children.Length - 1].transform.position;

            float distX = (lastElementPos.x - firstElementPos.x) / (_children.Length - 1);
            float distY = (lastElementPos.y - firstElementPos.y) / (_children.Length - 1);
            float distZ = (lastElementPos.z - firstElementPos.z) / (_children.Length - 1);

            Vector3 dist = new Vector3(distX, distY, distZ);

            for (int i = 1; i < _children.Length; i++)
                _children[i].transform.position = _children[i - 1].transform.position + dist;
        }
    }
}
