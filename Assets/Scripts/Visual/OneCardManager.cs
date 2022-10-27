using CardGame.SOAssets;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{
    public class OneCardManager : MonoBehaviour
    {
        [SerializeField] private CardAsset _cardAsset;
        [SerializeField] private OneCardManager _previewManager;

        [Header("Text Component References")]
        [SerializeField] private Text _nameText;

        [SerializeField] private Text _manaCostText;
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Text _healthText;
        [SerializeField] private Text _attackText;

        [Header("Image References")]
        [SerializeField] private Image _cardTopRibbonImage;

        [SerializeField] private Image _cardLowRibbonImage;
        [SerializeField] private Image _cardGraphicImage;
        [SerializeField] private Image _cardBodyImage;
        [SerializeField] private Image _cardFaceFrameImage;
        [SerializeField] private Image _cardFaceGlowImage;
        [SerializeField] private Image _cardBackGlowImage;

        public CardAsset CardAsset
        {
            get => _cardAsset;
            set => _cardAsset = value;
        }


        private void Awake()
        {
            if (_cardAsset != null)
                ReadCardFromAsset();
        }

        private bool _canBePlayedNow = false;

        public bool CanBePlayedNow
        {
            get => _canBePlayedNow;
            set
            {
                _canBePlayedNow = value;
                _cardFaceGlowImage.enabled = value;
            }
        }

        public void ReadCardFromAsset()
        {
            if (_cardAsset.CharacterAsset != null)
            {
                _cardBodyImage.color = _cardAsset.CharacterAsset.ClassCardTint;
                _cardFaceFrameImage.color = _cardAsset.CharacterAsset.ClassCardTint;
                _cardTopRibbonImage.color = _cardAsset.CharacterAsset.ClassRibbonsTint;
                _cardLowRibbonImage.color = _cardAsset.CharacterAsset.ClassRibbonsTint;
            }

            _nameText.text = _cardAsset.name;
            _manaCostText.text = _cardAsset.ManaCost.ToString();
            _descriptionText.text = _cardAsset.Description;
            _cardGraphicImage.sprite = _cardAsset.CardImage;

            if (_cardAsset.MaxHealth != 0)
            {
                _attackText.text = _cardAsset.Attack.ToString();
                _healthText.text = _cardAsset.MaxHealth.ToString();
            }

            if (_previewManager != null)
            {
                _previewManager._cardAsset = _cardAsset;
                _previewManager.ReadCardFromAsset();
            }
        }
    }
}