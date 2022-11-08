using CardGame.Logic;
using CardGame.Visual;
using CardGame.Visual.CardVisual;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Dragging
{    
    [RequireComponent(typeof(Draggable))]
    public class DragSpellNoTarget : DraggingActions
    {
        [SerializeField] private CardsAndCreaturesVisualState _state;
        private int _savedHandSlot;

        public override void OnStartDrag()
        {
            _savedHandSlot = _state.Slot;

            _state.State = VisualStates.Dragging;
            _state.BringToFront();
        }

        public override void OnEndDrag()
        {
            if (DragSuccess())
            {
                PlayerOwner.PlayASpellFromHand(GetComponent<IDHolder>().UniqueID, -1);
            }
            else
            {
                _state.Slot = _savedHandSlot;
                _state.State = VisualStates.BottomHand;
                HandVisual playerHand = TurnManager.Instance.WhoseTurn.PlayerArea.HandVisual;
                Vector3 oldCardPosition = playerHand.Slots.Children[_savedHandSlot].transform.localPosition;
                transform.DOLocalMove(oldCardPosition, 1f);
            }
        }

        public override void OnDraggingInUpdate()
        {
        }

        public override bool DragSuccess()
        {
            return TableVisual.CursorOverSomeTable;
        }
    }
}