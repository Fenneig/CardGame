using CardGame.SOAssets;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{
    public class PlayerPortraitVisual : MonoBehaviour
    {
        [SerializeField] private CharacterAsset _characterAsset;

        [Header("TextComponentReferences")]
        [SerializeField] private Text _healthText;

        [Header("Image References")]
        [SerializeField] private Image _heroPowerIconImage;

        [SerializeField] private Image _heroPowerBackgroundImage;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private Image _portraitBackgroundImage;

        public Text HealthText
        {
            get => _healthText;
            set => _healthText = value;
        }

        public CharacterAsset CharacterAsset
        {
            get => _characterAsset;
            set => _characterAsset = value;
        }

        private void Awake()
        {
            if (_characterAsset != null) ApplyLookFromAsset();
        }

        public void ApplyLookFromAsset()
        {
            _healthText.text = _characterAsset.MaxHealth.ToString();
            _heroPowerIconImage.sprite = _characterAsset.HeroPowerIconImage;
            _heroPowerBackgroundImage.sprite = _characterAsset.HeroPowerBGImage;
            _portraitImage.sprite = _characterAsset.AvatarImage;
            _portraitBackgroundImage.sprite = _characterAsset.AvatarBGImage;

            _heroPowerBackgroundImage.color = _characterAsset.HeroPowerBGTint;
            _portraitBackgroundImage.color = _characterAsset.AvatarBGTint;
        }

        public void TakeDamage(int amount, int healthAfter)
        {
            if (amount > 0)
            {
               DamageEffect.CreateDamageEffect(transform.position, amount);
                _healthText.text = healthAfter.ToString();
            }
        }

        public void Explode()
        {
             Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
             Sequence sequence = DOTween.Sequence();
             sequence.PrependInterval(2f);
             sequence.OnComplete(() => GlobalSettings.Instance.GameOverCanvas.SetActive(true));
        }
    }
}