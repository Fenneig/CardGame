using CardGame.UI;
using UnityEngine;

namespace CardGame.Visual
{
    public enum AreaPosition
    {
        Top,
        Bottom
    }

    public class PlayerArea : MonoBehaviour
    {
        [SerializeField] private AreaPosition _ownerAreaPosition;
        [SerializeField] private bool _controlsEnable = true;
        [SerializeField] private PlayerDeckVisual _playerDeck;
        [SerializeField] private ManaPoolVisual _manaBar;
        [SerializeField] private HandVisual _handVisual;
        [SerializeField] private PlayerPortraitVisual _portrait;
        [SerializeField] private HeroPowerButton _heroPower;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private TableVisual _tableVisual;
        [SerializeField] private Transform _portraitPosition;
        [SerializeField] private bool _allowedToControlThisPlayer;

        public bool ControlsEnable
        {
            get => _controlsEnable;
            set => _controlsEnable = value;
        }

        public PlayerDeckVisual PlayerDeck => _playerDeck;
        public ManaPoolVisual ManaBar => _manaBar;
        public HandVisual HandVisual => _handVisual;
        public PlayerPortraitVisual Portrait => _portrait;
        public HeroPowerButton HeroPower => _heroPower;
        public TableVisual TableVisual => _tableVisual;
        public Transform PortraitPosition => _portraitPosition;

        public bool AllowedToControlThisPlayer { get; set; }
    }
}