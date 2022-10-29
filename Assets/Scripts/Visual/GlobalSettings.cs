using CardGame.Logic;
using UnityEngine;

namespace CardGame.Visual
{
    public class GlobalSettings : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Player _topPlayer;
        [SerializeField] private Player _lowPlayer;
        [Header("Colors")] 
        [SerializeField] private Color32 _cardBodyStandardColor;
        [SerializeField] private Color32 _cardRibbonsStandardColor;
        [SerializeField] private Color32 _cardGlowColor;
        [SerializeField] private GameObject _damageEffectPrefab;

        private static GlobalSettings _instance;
        public static GlobalSettings Instance => _instance;
        public GameObject DamageEffectPrefab => _damageEffectPrefab;

        private void Awake()
        {
            _instance = this;
        }
    }
}