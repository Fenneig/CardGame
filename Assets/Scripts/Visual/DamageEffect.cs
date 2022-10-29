using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CardGame.Visual
{
    public class DamageEffect : MonoBehaviour
    {
        [SerializeField] private Sprite[] _splashes;
        [SerializeField] private Image _damageImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Text _amountText;
        [Header("Fade details")]
        [SerializeField] private float _timeTillTextFade = 1f;
        [SerializeField] private float _fadeTickTime = 0.05f;
        [SerializeField] private float _fadeTickAmount = 0.05f;

        public Text AmountText
        {
            get => _amountText;
            set => _amountText = value;
        }

        private void Awake()
        {
            _damageImage.sprite = _splashes[Random.Range(0, _splashes.Length)];
        }

        private IEnumerator ShowDamageEffect()
        {
            _canvasGroup.alpha = 1f;

            yield return new WaitForSeconds(_timeTillTextFade);

            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= _fadeTickAmount;
                yield return new WaitForSeconds(_fadeTickTime);
            }

            Destroy(gameObject);
        }

        public static void CreateDamageEffect(Vector3 position, int amount)
        {
            GameObject newDamageEffect = Instantiate(GlobalSettings.Instance.DamageEffectPrefab, position, quaternion.identity);
            DamageEffect damageEffect = newDamageEffect.GetComponent<DamageEffect>();
            damageEffect.AmountText.text = $"-{amount.ToString()}";
            damageEffect.StartCoroutine(damageEffect.ShowDamageEffect());
        }
    }
}
