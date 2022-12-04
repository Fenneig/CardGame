using CardGame.Visual;
using UnityEngine;

namespace CardGame.Dragging
{
    [RequireComponent(typeof(Draggable))]
    public class DragHeroPowerAction : DraggingActions
    {
        [SerializeField] private TargetingOptions _options;
        public override void OnStartDrag()
        {
            
        }

        public override void OnEndDrag()
        {
            
        }

        public override void OnDraggingInUpdate()
        {
            
        }

        public override bool DragSuccessful()
        {
            return TableVisual.CursorOverSomeTable;
        }
    }
}