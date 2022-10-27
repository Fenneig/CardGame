using CardGame.SOAssets;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{
    public class OneCreatureManager : MonoBehaviour
    {
        [SerializeField] private CardAsset _cardAsset;
        [SerializeField] private OneCardManager _previewManager;
        [Header("Text Component References")]
        [SerializeField] private Text _healthText;
        [SerializeField] private Text _attackText;
        [Header("Image References")]
        [SerializeField] private Image _graphicImage;
        [SerializeField] private Image _glowImage;

        private bool _canAttack = false;

        public bool CanAttack
        {
            get => _canAttack;
            set
            {
                _canAttack = value;
                _glowImage.enabled = value;
            }
        }

        private void Awake()
        {
            if (_cardAsset != null) ReadCreatureFromAsset();
        }
        
        

        private void ReadCreatureFromAsset()
        {
            _graphicImage.sprite = _cardAsset.CardImage;

            _attackText.text = _cardAsset.Attack.ToString();
            _healthText.text = _cardAsset.MaxHealth.ToString();

            if (_previewManager != null)
            {
                _previewManager.CardAsset = _cardAsset;
                _previewManager.ReadCardFromAsset();
            }
        }

        public void TakeDamage(int amount, int healthAfter)
        {
            if (amount > 0)
            {
                // TODO DamageEffect.CreateDamageEffect(transform.position, amount);
                _healthText.text = healthAfter.ToString();
            }
        }
    }
}