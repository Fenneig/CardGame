using CardGame.Visual;
using UnityEngine;

namespace CardGame.Dragging
{
    [RequireComponent(typeof(DraggingActions))]
    public class DraggableObject : MonoBehaviour
    {
        [SerializeField] [Tooltip("With turn off this parameter mouse will stay on the center of the card")] private bool _usePointerDisplacement = true;
        //флаг показывающий в данный момент карта передвигается или нет
        private bool _isDragging = false;
        //расстояние от центра карты до курсора
        private Vector3 _pointerDisplacement = Vector3.zero;
        //расстояние от камеры то Z координаты мыши
        private float _zDisplacement;
        private DraggingActions _draggingActions;

        private void Awake()
        {
            _draggingActions = GetComponent<DraggingActions>();
        }

        private void OnMouseDown()
        {
            if (!_draggingActions.CanDrag) return;
            
            _isDragging = true;
            HoverPreview.PreviewsAllowed = false;
            _draggingActions.OnStartDrag();
            _zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            if (_usePointerDisplacement) _pointerDisplacement = -transform.position + MouseInWorldCoords();
            else _pointerDisplacement = Vector3.zero;
        }

        private void Update()
        {
            if (!_isDragging) return;
            
            var mousePos = MouseInWorldCoords();
            _draggingActions.OnDraggingInUpdate();
            transform.position = new Vector3(mousePos.x - _pointerDisplacement.x, mousePos.y - _pointerDisplacement.y,
                transform.position.z);
        }

        private void OnMouseUp()
        {
            if (!_isDragging) return;

            _isDragging = false;
            HoverPreview.PreviewsAllowed = true;
            _draggingActions.OnEndDrag();
        }

        //Возвращает координату мыши в мировых координатых для объекта
        private Vector3 MouseInWorldCoords()
        {
            var screenMousePos = Input.mousePosition;
            screenMousePos.z = _zDisplacement;
            return Camera.main.ScreenToWorldPoint(screenMousePos);
        }
    }
}