using System.Collections.Generic;
using CardGame.Commands;
using CardGame.Dragging;
using CardGame.Logic;
using CardGame.SOAssets;
using CardGame.Visual.CardVisual;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Visual
{
    public class HandVisual : MonoBehaviour
    {
        [SerializeField] private AreaPosition _positionOwner;
        [SerializeField] private bool _takeCardsOpenly = true;
        [SerializeField] private SameDistanceChildren _slots;

        [Header("Transform references")]
        [SerializeField] private Transform _drawPreviewSpot;
        [SerializeField] private Transform _deckTransform;
        [SerializeField] private Transform _otherCardDrawSourceTransform;
        [SerializeField] private Transform _playPreviewSpot;

        public SameDistanceChildren Slots => _slots;
        public Transform OtherCardDrawSourceTransform => _otherCardDrawSourceTransform;
        
        private List<GameObject> _cardsInHand = new List<GameObject>();

        public void AddCard(GameObject card)
        {
            _cardsInHand.Insert(0, card);

            card.transform.SetParent(_slots.transform);

            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
        }

        public void RemoveCard(GameObject card)
        {
            _cardsInHand.Remove(card);
            
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
        }

        public void RemoveCardAtIndex(int index)
        {
            _cardsInHand.RemoveAt(index);
            
            PlaceCardsOnNewSlots();
            UpdatePlacementOfSlots();
        }

        public GameObject GetCardAtIndex(int index) => _cardsInHand[index];

        public void GivePlayerACard(CardAsset cardAsset, int uniqueId, bool fast = false, bool fromDeck = true)
        {
            GameObject card = fromDeck
                ? CreateACardAtPosition(cardAsset, _deckTransform.position, new Vector3(0, -179f, 0))
                : CreateACardAtPosition(cardAsset, OtherCardDrawSourceTransform.position, new Vector3(0, -179f, 0));

            foreach (var transform in card.GetComponentsInChildren<Transform>())
                transform.tag = $"{_positionOwner}Card";

            AddCard(card);

            CardsAndCreaturesVisualState state = card.GetComponent<CardsAndCreaturesVisualState>();
            state.BringToFront();
            state.Slot = 0;

            IDHolder id = card.AddComponent<IDHolder>();
            id.UniqueID = uniqueId;

            Sequence sequence = DOTween.Sequence();
            if (!fast)
            {
                sequence.Append(card.transform.DOMove(_drawPreviewSpot.position,
                    GlobalSettings.Instance.CardTransitionTime));
                
                sequence.Insert(0f,
                    _takeCardsOpenly
                        ? card.transform.DORotate(Vector3.zero, GlobalSettings.Instance.CardTransitionTime)
                        : card.transform.DORotate(new Vector3(0, 179f, 0), GlobalSettings.Instance.CardTransitionTime));
                
                sequence.AppendInterval(GlobalSettings.Instance.CardPreviewTime);
                sequence.Append(card.transform.DOLocalMove(_slots.Children[0].transform.localPosition,
                    GlobalSettings.Instance.CardTransitionTime));
            }
            else
            {
                sequence.Append(card.transform.DOLocalMove(_slots.Children[0].transform.localPosition,
                    GlobalSettings.Instance.CardTransitionTime));
            }

            sequence.OnComplete(() => ChangeLastCardStatusToInHand(cardAsset, state));
        }

        public void PlayASpellFromHand(int cardID)
        {
            GameObject card = IDHolder.GetGameObjectWithID(cardID);
            PlayASpellFromHand(card);
        }

        public void PlayASpellFromHand(GameObject card)
        {
            Command.CommandExecutionComplete();
            card.GetComponent<CardsAndCreaturesVisualState>().State = VisualStates.Transition;
            RemoveCard(card);

            card.transform.SetParent(null);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(card.transform.DOMove(_playPreviewSpot.position, 1f));
            sequence.Insert(0f, card.transform.DORotate(Vector3.zero, 1f));
            sequence.AppendInterval(2f);
            sequence.OnComplete(() =>
            {
                Destroy(card);
            });
        }

        private void ChangeLastCardStatusToInHand(CardAsset cardAsset, CardsAndCreaturesVisualState state)
        {
            if (_positionOwner == AreaPosition.Bottom) state.State = VisualStates.BottomHand;
            else state.State = VisualStates.TopHand;
            
            state.SetHandSortingOrder();
            Command.CommandExecutionComplete();
        }

        private GameObject CreateACardAtPosition(CardAsset cardAsset, Vector3 position, Vector3 eulerAngles)
        {
            GameObject card;
            if (cardAsset.MaxHealth > 0)
            {
                card = Instantiate(GlobalSettings.Instance.CreatureCardPrefab, position, Quaternion.Euler(eulerAngles));
            }
            else
            {
                if (cardAsset.Targets == TargetingOptions.NoTarget)
                {
                    card = Instantiate(GlobalSettings.Instance.NoTargetSpellCardPrefab, position,
                        Quaternion.Euler(eulerAngles));
                }
                else
                {
                    card = Instantiate(GlobalSettings.Instance.TargetedSpellCardPrefab, position,
                        Quaternion.Euler(eulerAngles));
                    DragSpellOnTarget dragSpell = card.GetComponentInChildren<DragSpellOnTarget>();
                    dragSpell.Targets = cardAsset.Targets;
                }
            }

            return card;
        }
        
        private void PlaceCardsOnNewSlots()
        {
            foreach (var card in _cardsInHand)
            {
                card.transform.DOLocalMoveX(_slots.Children[_cardsInHand.IndexOf(card)].transform.localPosition.x, 0.3f);

                CardsAndCreaturesVisualState state = card.GetComponent<CardsAndCreaturesVisualState>();
                state.Slot = _cardsInHand.IndexOf(card);
                state.SetHandSortingOrder();
            }
        }
        private void UpdatePlacementOfSlots()
        {
            float posX;
            if (_cardsInHand.Count > 0)
                posX = (_slots.Children[0].transform.localPosition.x -
                        _slots.Children[_cardsInHand.Count - 1].transform.localPosition.x) / 2;
            else 
                posX = 0f;

            _slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);
        }

    }
}