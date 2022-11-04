using UnityEngine;

namespace CardGame.Visual.CardVisual
{
    public enum VisualStates
    {
        Transition,
        BottomHand,
        TopHand,
        BottomTable,
        TopTable,
        Dragging
    }

    public class CardsAndCreaturesVisualState : MonoBehaviour
    {
        private HoverPreview _hover;
        private Canvas _canvas;
        private int _topSortingOrder = 500;
        private const string TopSortingLayer = "AboveEverything";
        private const string CreaturesSortingLayer = "Creatures";
        private const string CardsSortingLayer = "Cards";

        private int _slot = -1;

        public int Slot
        {
            get => _slot;
            set => _slot = value;
        }

        private VisualStates _state;

        public VisualStates State
        {
            get => _state;
            set
            {
                _state = value;
                _hover.ThisPreviewEnabled = _state == VisualStates.BottomHand ||
                                            _state == VisualStates.BottomTable ||
                                            _state == VisualStates.TopTable;
            }
        }

        private void Awake()
        {
            //TODO Возможно лучше серелиализовать из инспектора
            _hover = GetComponent<HoverPreview>();
            if (_hover == null) _hover = GetComponentInChildren<HoverPreview>();
            _canvas = GetComponentInChildren<Canvas>();
        }

        public void BringToFront()
        {
            _canvas.sortingOrder = _topSortingOrder;
            _canvas.sortingLayerName = TopSortingLayer;
        }

        public void SetTableSortingOrder()
        {
            _canvas.sortingOrder = 0;
            _canvas.sortingLayerName = CreaturesSortingLayer;
        }

        public int HandSortingOrder(int placeInHand) => (placeInHand + 1) * 10;

        public void SetHandSortingOrder()
        {
            if (_slot != -1) _canvas.sortingOrder = HandSortingOrder(_slot);
            _canvas.sortingLayerName = CardsSortingLayer;
        }
    }
}