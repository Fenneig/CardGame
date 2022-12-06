using CardGame.Logic;
using CardGame.Visual;
using CardGame.Visual.CardVisual;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Dragging
{
    [RequireComponent(typeof(Draggable))]
    public class DragCreatureOnTable : DraggingActions
    {
        [SerializeField] private CardsAndCreaturesVisualState _state;
        [SerializeField] private OneCardManager _manager;

        private int _savedHandSlot;
        private IDHolder _idHolder;
        private VisualStates _tempState;

        public override bool CanDrag => true; // base.CanDrag && _manager.CanBePlayedNow;

        public override void OnStartDrag()
        {
            _savedHandSlot = _state.Slot;
            _tempState = _state.State;
            _state.State = VisualStates.Dragging;
            _state.BringToFront();
        }

        public override void OnEndDrag()
        {
            if (DragSuccessful())
            {
                int tablePosition = PlayerOwner.PlayerArea.TableVisual.TablePositionForNewCreature(Camera.main
                    .ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        Input.mousePosition.z - Camera.main.transform.position.z)).x);
                PlayerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().UniqueID, tablePosition);
            }
            else
            {
                _state.SetHandSortingOrder();
                _state.State = _tempState;
                HandVisual playerHand = PlayerOwner.PlayerArea.HandVisual;
                Vector3 oldCardPosition = playerHand.Slots.Children[_savedHandSlot].transform.localPosition;
                transform.DOLocalMove(oldCardPosition, 1f);

            }
        }

        public override void OnDraggingInUpdate()
        {
        }

        public override bool DragSuccessful()
        {
            bool tableNotFull = PlayerOwner.Table.CreaturesOnTable.Count < 7;

            return TableVisual.CursorOverSomeTable && tableNotFull;
        }
    }
}