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

        private static HoverPreview _currentlyViewing;

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
        
        private bool _previewsAllowed = true;
        public bool PreviewsAllowed
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
        
        
        
        private readonly Vector3 _vectorOne = Vector3.one;
        private readonly Vector3 _vectorZero = Vector3.zero;

        private void Awake()
        {
            _thisPreviewEnabled = _activateInAwake;
        }

        private void OnMouseEnter()
        {
            OverCollider = true;
            if (PreviewsAllowed && _thisPreviewEnabled) ShowPreview();
        }

        private void OnMouseExit()
        {
            OverCollider = false;
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
            _previewGameObject.transform.localPosition = _vectorZero;
            _previewGameObject.transform.localScale = _vectorOne;

            _previewGameObject.transform.DOLocalMove(_targetPosition, 1f).SetEase(Ease.OutQuint);
            _previewGameObject.transform.DOScale(_targetScale, 1f).SetEase(Ease.OutQuint);
        }

        private void StopThisPreview()
        {
            PreviewGameObject.SetActive(false);
            PreviewGameObject.transform.localScale = _vectorOne;
            PreviewGameObject.transform.position = _vectorZero;
            if (TurnThisOffWhenPreviewing != null) TurnThisOffWhenPreviewing.SetActive(true);
        }

        private void StopAllPreviews()
        {
            if (_currentlyViewing != null)
            {
                _currentlyViewing.PreviewGameObject.SetActive(false);
                _currentlyViewing.PreviewGameObject.transform.localScale = _vectorOne;
                _currentlyViewing.PreviewGameObject.transform.position = _vectorZero;
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