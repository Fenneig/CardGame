using DG.Tweening;
using UnityEngine;

namespace CardGame.Visual
{
    public class HoverPreview : MonoBehaviour
    {
        [SerializeField] private GameObject _turnThisOffWhenPreviewing;
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _targetScale;
        [SerializeField] private GameObject _previewGameObject;
        [SerializeField] private bool _activateInAwake;
        [SerializeField] private int _rotatePower = 10;

        private static HoverPreview _currentlyViewing;

        private bool _canRotatePreview;
        
        private bool _thisPreviewEnabled;
        public bool ThisPreviewEnabled
        {
            get => _thisPreviewEnabled;
            set
            {
                _thisPreviewEnabled = value;
                if (!_thisPreviewEnabled) StopThisPreview();
            }
        }
        
        private static bool _previewsAllowed = true;
        public static bool PreviewsAllowed
        {
            get => _previewsAllowed;
            set
            {
                _previewsAllowed = value;
                if (!_previewsAllowed) StopAllPreviews();
            }
        }
        
        public bool OverCollider { get; set; }
        private GameObject TurnThisOffWhenPreviewing => _turnThisOffWhenPreviewing;
        private GameObject PreviewGameObject => _previewGameObject;
        
        
        
        private static readonly Vector3 VectorOne = Vector3.one;
        private static readonly Vector3 VectorZero = Vector3.zero;

        private void Awake()
        {
            _thisPreviewEnabled = _activateInAwake;
        }

        private void OnMouseEnter()
        {
            OverCollider = true;
            if (PreviewsAllowed && _thisPreviewEnabled) ShowPreview();
        }

        private void OnMouseOver()
        {
            if (!_canRotatePreview) return;
            
            var displacementZ = -Camera.main.transform.position.z + transform.position.z;
            var screenMousePos = Input.mousePosition;
            screenMousePos.z = displacementZ;
            var pointerDisplacement = -transform.position + Camera.main.ScreenToWorldPoint(screenMousePos);
            transform.rotation = Quaternion.Euler(pointerDisplacement.y * _rotatePower, -pointerDisplacement.x * _rotatePower, pointerDisplacement.z);
        }

        private void OnMouseExit()
        {
            _canRotatePreview = false;
            OverCollider = false;
            transform.rotation = Quaternion.identity;
            if (!PreviewingSomeCard()) StopAllPreviews();
        }

        private void ShowPreview()
        {
            //во-первых отключаем все включеные превью
            StopAllPreviews();
            //сохраняем эту превью как текущую
            _currentlyViewing = this;
            //включаем превью
            _previewGameObject.SetActive(true);
            //отключаем если есть что отключать
            if (_turnThisOffWhenPreviewing != null) _turnThisOffWhenPreviewing.SetActive(false);
            //передвегаем в целевую позицию
            _previewGameObject.transform.localPosition = VectorZero;
            _previewGameObject.transform.localScale = VectorOne;

            _previewGameObject.transform.DOLocalMove(_targetPosition, 1f).SetEase(Ease.OutQuint)
                .OnComplete(() => _canRotatePreview = true);
            _previewGameObject.transform.DOScale(_targetScale, 1f).SetEase(Ease.OutQuint);
        }

        private void StopThisPreview()
        {
            PreviewGameObject.SetActive(false);
            PreviewGameObject.transform.localScale = VectorOne;
            PreviewGameObject.transform.position = VectorZero;
            if (TurnThisOffWhenPreviewing != null) TurnThisOffWhenPreviewing.SetActive(true);
        }

        private static void StopAllPreviews()
        {
            if (_currentlyViewing != null)
            {
                _currentlyViewing.PreviewGameObject.SetActive(false);
                _currentlyViewing.PreviewGameObject.transform.localScale = VectorOne;
                _currentlyViewing.PreviewGameObject.transform.position = VectorZero;
                if (_currentlyViewing.TurnThisOffWhenPreviewing != null) _currentlyViewing.TurnThisOffWhenPreviewing.SetActive(true);
            }
        }

        private bool PreviewingSomeCard()
        {
            if (!PreviewsAllowed) return false;
            HoverPreview[] allHoverBlowups = FindObjectsOfType<HoverPreview>();
            
            foreach (var hoverBlowup in allHoverBlowups)
                if (hoverBlowup.OverCollider && hoverBlowup.ThisPreviewEnabled) return true;
            
            return false;
        }
    }
}