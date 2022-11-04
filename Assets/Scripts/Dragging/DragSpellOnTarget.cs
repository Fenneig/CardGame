using CardGame.Logic;
using CardGame.Visual;
using CardGame.Visual.CardVisual;
using UnityEngine;

namespace CardGame.Dragging
{
    public class DragSpellOnTarget : DraggingActions
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private CardsAndCreaturesVisualState _cardState;
        [SerializeField] private SpriteRenderer _triangleSpriteRenderer;

        [SerializeField] private float _distanceBetweenLineAndTarget;
        [SerializeField] private float _distanceBetweenTriangleAndTarget;
        
        private TargetingOptions _targets = TargetingOptions.AllCharacters;
        private VisualStates _tempVisualState;
        private GameObject _target;
        
        public TargetingOptions Targets
        {
            get => _targets;
            set => _targets = value;
        }
        
        private readonly Vector3 _originalTransformPosition = new Vector3(0, 0, 0.4f);

        public override void OnStartDrag()
        {
            _tempVisualState = _cardState.State;
            _cardState.State = VisualStates.Dragging;
            _spriteRenderer.enabled = true;
            _lineRenderer.enabled = true;
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

        public override void OnEndDrag()
        {
            _target = null;
            RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position,
                (-Camera.main.transform.position + transform.position).normalized, 30f);

            foreach (var hit in hits)
            {
                if (hit.transform.tag.Contains("Player"))
                    _target = hit.transform.gameObject;
                
                else if (hit.transform.tag.Contains("Creature"))
                    _target = hit.transform.parent.gameObject;
                
            }

            bool validTarget = false;

            if (_target != null)
            {
                Player owner = tag.Contains("Bottom") ? 
                    GlobalSettings.Instance.BottomPlayer : 
                    GlobalSettings.Instance.TopPlayer;

                int targetID = _target.GetComponent<IDHolder>().UniqueID;
                switch (Targets)
                {
                    case TargetingOptions.AllCharacters:
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        validTarget = true;
                        break;
                    
                    case TargetingOptions.AllCreatures:
                        if (!_target.tag.Contains("Creature")) break;
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        validTarget = true;
                        break;
                    
                    case TargetingOptions.EnemyCharacters:
                        if (!_target.tag.Contains("Creature") && !_target.tag.Contains("Player")) break;
                        if (!CheckIfAimingACard()) break;
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        validTarget = true;
                        break;
                    
                    case TargetingOptions.EnemyCreatures:
                        if (!_target.tag.Contains("Creature")) break;
                        if (!CheckIfAimingACard()) break;
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        validTarget = true;
                        break;
                    
                    case TargetingOptions.YourCharacters:
                        if (!_target.tag.Contains("Creature") && !_target.tag.Contains("Player")) break;
                        if (!CheckIfAimingACard()) break;
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        validTarget = true;
                        break;
                    
                    case TargetingOptions.YourCreatures:
                        if (!_target.tag.Contains("Creature")) break;
                        if (!CheckIfAimingACard()) break;
                        owner.PlayASpellFromHand(GetComponentInParent<IDHolder>().UniqueID, targetID);
                        validTarget = true;
                        break;
                    
                    default:
                        Debug.LogWarning("Reached default case in DragSpellOnTarget! Suspicious behavior!");
                        break;
                }

                if (!validTarget)
                {
                    _cardState.State = _tempVisualState;
                    _cardState.SetHandSortingOrder();
                }
            }
            
            transform.localPosition = _originalTransformPosition;
            _spriteRenderer.enabled = false;
            _lineRenderer.enabled = false;
            _triangleSpriteRenderer.enabled = false;
        }

        public override bool DragSuccess()
        {
            return true;
        }

        private bool CheckIfAimingACard()
        {
            return tag.Contains("Bottom") && _target.tag.Contains("Bottom") ||
                     tag.Contains("Top") && _target.tag.Contains("Top");
        }
    }
}