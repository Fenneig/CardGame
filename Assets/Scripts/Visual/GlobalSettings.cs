using System.Collections.Generic;
using CardGame.Commands;
using CardGame.Logic;
using CardGame.SOAssets;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{
    public class GlobalSettings : MonoBehaviour
    {
        [Header("Player")] 
        [SerializeField] private Player _topPlayer;
        [SerializeField] private Player _bottomPlayer;

        [Header("Colors")] 
        [SerializeField] private Color32 _cardBodyStandardColor;
        [SerializeField] private Color32 _cardRibbonsStandardColor;
        [SerializeField] private Color32 _cardGlowColor;

        [Header("Numbers and Values")]
        [SerializeField] private float _cardPreviewTime = 1f;
        [SerializeField] private float _cardTransitionTime = 1f;
        [SerializeField] private float _cardPreviewTimeFast = 0.2f;
        [SerializeField] private float _cardTransitionTimeFast = 0.2f;

        [Header("Prefabs and Assets")] 
        [SerializeField] private GameObject _noTargetSpellCardPrefab;
        [SerializeField] private GameObject _targetedSpellCardPrefab;
        [SerializeField] private GameObject _creatureCardPrefab;
        [SerializeField] private GameObject _creaturePrefab;
        [SerializeField] private GameObject _damageEffectPrefab;
        [SerializeField] private GameObject _explosionPrefab;
        
        [Header("Other")] 
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private CardAsset _coinCard;
        [SerializeField] private GameObject _gameOverCanvas;

        public readonly Dictionary<AreaPosition, Player> Players = new Dictionary<AreaPosition, Player>();

        private static GlobalSettings _instance;
        public static GlobalSettings Instance => _instance;
        public GameObject DamageEffectPrefab => _damageEffectPrefab;
        public GameObject ExplosionPrefab => _explosionPrefab;

        public Player TopPlayer => _topPlayer;
        public Player BottomPlayer => _bottomPlayer;

        public float CardPreviewTime => _cardPreviewTime;
        public float CardTransitionTime => _cardTransitionTime;
        public float CardPreviewTimeFast => _cardPreviewTimeFast;
        public float CardTransitionTimeFast => _cardTransitionTimeFast;

        public GameObject NoTargetSpellCardPrefab => _noTargetSpellCardPrefab;
        public GameObject TargetedSpellCardPrefab => _targetedSpellCardPrefab;
        public GameObject CreatureCardPrefab => _creatureCardPrefab;
        public GameObject CreaturePrefab => _creaturePrefab;
        public CardAsset CoinCard => _coinCard;
        public GameObject GameOverCanvas => _gameOverCanvas;

        private void Awake()
        {
            Players.Add(AreaPosition.Top, _topPlayer);
            Players.Add(AreaPosition.Bottom, _bottomPlayer);
            _instance = this;
        }

        public bool CanControlThisPlayer(AreaPosition owner)
        {
            bool playersTurn = TurnManager.Instance.WhoseTurn == Players[owner];
            bool notDrawingAnyCards = !Command.CardDrawPending();
            return Players[owner].PlayerArea.AllowedToControlThisPlayer && Players[owner].PlayerArea.ControlsEnable &&
                   playersTurn && notDrawingAnyCards;
        }

        public bool CanControlThisPlayer(Player ownerPlayer)
        {
            bool playersTurn = (TurnManager.Instance.WhoseTurn == ownerPlayer);
            bool notDrawingAnyCards = !Command.CardDrawPending();
            return ownerPlayer.PlayerArea.AllowedToControlThisPlayer && ownerPlayer.PlayerArea.ControlsEnable &&
                   playersTurn && notDrawingAnyCards;
        }

        public void EnableEndTurnButtonOnStart(Player player)
        {
            _endTurnButton.interactable = player == BottomPlayer && CanControlThisPlayer(AreaPosition.Bottom) ||
                                         player == TopPlayer && CanControlThisPlayer(AreaPosition.Top);
        }
    }
}