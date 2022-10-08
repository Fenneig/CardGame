using UnityEngine;

namespace CardGame.Dragging
{
    public abstract class DraggingActions : MonoBehaviour
    {
        public abstract void OnStartDrag();
        public abstract void OnEndDrag();
        public abstract void OnDraggingInUpdate();
        
        public virtual bool CanDrag => true;
        public abstract bool OnDragSuccess();

    }
}