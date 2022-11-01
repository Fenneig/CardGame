using UnityEngine;

namespace CardGame.Dragging
{
    public class DragOnTarget : DraggingActions
    { 
        //public TargetingOptions Targets = TargetingOptions.AllCharacters;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private SpriteRenderer _triangleSpriteRenderer;

        [SerializeField] private float _distanceBetweenLineAndTarget;
        [SerializeField] private float _distanceBetweenTriangleAndTarget;

        private readonly Vector3 _originalTransformPosition = new Vector3(0, 0, 0.1f);

        public override void OnStartDrag()
        {
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
                //рисует линию от карты до цели
                _lineRenderer.SetPositions(new[]
                    {transform.parent.position, transform.position - direction * _distanceBetweenLineAndTarget});
                _lineRenderer.enabled = true;
                
                //рисует триугольник-указатель около цели
                _triangleSpriteRenderer.enabled = true;
                _triangleSpriteRenderer.transform.position = transform.position - direction * _distanceBetweenTriangleAndTarget;
                
                //Создает правильно вращение стрелки к цели
                float rotationZ = Mathf.Atan2(currentPositionFromCardToTarget.y, currentPositionFromCardToTarget.x) * Mathf.Rad2Deg;
                _triangleSpriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90);
            }
            else
            {
                _lineRenderer.enabled = false;
                _triangleSpriteRenderer.enabled = false;
            }
        }

        public override void OnEndDrag()
        {
            transform.localPosition = _originalTransformPosition;
            _spriteRenderer.enabled = false;
            _lineRenderer.enabled = false;
            _triangleSpriteRenderer.enabled = false;
        }


        public override bool OnDragSuccess()
        {
            return true;
        }
    }
}