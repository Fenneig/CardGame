using CardGame.Logic;
using CardGame.Visual;
using CardGame.Visual.CardVisual;
using UnityEngine;

namespace CardGame.Dragging
{
    [RequireComponent(typeof(Draggable))]
    public class DragCreatureAttack : DraggingActions
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private CardsAndCreaturesVisualState _state;
        [SerializeField] private SpriteRenderer _triangleSpriteRenderer;
        [SerializeField] private OneCreatureManager _manager;


        [SerializeField] private float _distanceBetweenLineAndTarget;
        [SerializeField] private float _distanceBetweenTriangleAndTarget;

        private GameObject _target;

        public override bool CanDrag => base.CanDrag && _manager.CanAttack;

        public override void OnStartDrag()
        {
            _state.State = VisualStates.Dragging;
            _spriteRenderer.enabled = true;
            _lineRenderer.enabled = true;
        }

        public override void OnEndDrag()
        {
            _target = null;

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position,
                (-Camera.main.transform.position + transform.position).normalized, 30f);

            foreach (var hit in hits)
            {
                if (hit.transform.CompareTag("TopPlayer") && transform.CompareTag("BottomPlayer") ||
                    hit.transform.CompareTag("BottomPlayer") && transform.CompareTag("TopPlayer"))
                    _target = hit.transform.gameObject;

                else if (hit.transform.CompareTag("TopCreature") && transform.CompareTag("BottomCreature") ||
                         hit.transform.CompareTag("BottomCreature") && transform.CompareTag("TopCreature"))
                    _target = hit.transform.parent.gameObject;
            }

            bool validTarget = false;

            if (_target != null)
            {
                int targetID = _target.GetComponent<IDHolder>().UniqueID;
                if (targetID == GlobalSettings.Instance.BottomPlayer.ID ||
                    targetID == GlobalSettings.Instance.TopPlayer.ID)
                {
                    CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
                    validTarget = true;
                }
                else if (CreatureLogic.CreaturesCreatedThisGame[targetID] != null)
                {
                    CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID]
                        .AttackCreatureWithID(targetID);
                    validTarget = true;
                }
            }

            if (!validTarget)
            {
                _state.State = VisualStates.BottomTable;
                _state.SetTableSortingOrder();
            }

            transform.localPosition = Vector3.zero;
            _spriteRenderer.enabled = false;
            _lineRenderer.enabled = false;
            _triangleSpriteRenderer.enabled = false;
        }

        public override void OnDraggingInUpdate()
        {
            Vector3 currentPositionFromCardToTarget = transform.position - transform.parent.position;
            Vector3 direction = currentPositionFromCardToTarget.normalized;
            float distanceToTarget = (direction * _distanceBetweenLineAndTarget).magnitude;
            if (currentPositionFromCardToTarget.magnitude > distanceToTarget)
            {
                var lineRendererPositions = new[]
                    {transform.parent.position, transform.position - direction * _distanceBetweenLineAndTarget};
                _lineRenderer.SetPositions(lineRendererPositions);
                _lineRenderer.enabled = true;

                _triangleSpriteRenderer.enabled = true;
                _triangleSpriteRenderer.transform.position =
                    transform.position - direction * _distanceBetweenTriangleAndTarget;

                float rotationZ = Mathf.Atan2(currentPositionFromCardToTarget.y, currentPositionFromCardToTarget.x) *
                                  Mathf.Rad2Deg;
                _triangleSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            }
            else
            {
                _lineRenderer.enabled = false;
                _triangleSpriteRenderer.enabled = false;
            }
        }

        public override bool DragSuccess()
        {
            return true;
        }
    }
}