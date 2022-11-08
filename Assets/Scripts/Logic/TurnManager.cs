using CardGame.Commands;
using CardGame.Logic.TurnsAndAI;
using CardGame.Visual;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardGame.Logic
{
    public class TurnManager : MonoBehaviour
    {
        [SerializeField] private RopeTimer _timer;

        public static TurnManager Instance;

        private Player _whoseTurn;

        public Player WhoseTurn
        {
            get => _whoseTurn;
            set
            {
                _whoseTurn = value;
                _timer.StartTimer();
                
                GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

                TurnMaker turnMaker = WhoseTurn.GetComponent<TurnMaker>();

                turnMaker.OnTurnStart();
                if (turnMaker is PlayerTurnMaker) WhoseTurn.HighlightPlayableCards();

                WhoseTurn.OtherPlayer.HighlightPlayableCards(true);
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            OnGameStart();
        }

        private void OnGameStart()
        {
            CardLogic.CardsCreatedThisGame.Clear();
            CreatureLogic.CreaturesCreatedThisGame.Clear();

            foreach (var player in Player.Players)
            {
                player.ManaThisTurn = 0;
                player.ManaLeft = 0;
                player.LoadCharacterInfoFromAsset();
                player.TransmitInfoAboutPlayerToVisual();
                player.PlayerArea.PlayerDeck.CardsInDeck = player.Deck.Cards.Count;
                player.PlayerArea.Portrait.transform.position = player.PlayerArea.HandVisual.OtherCardDrawSourceTransform.position;
            }

            Sequence sequence = DOTween.Sequence();

            sequence.Append(Player.Players[0].PlayerArea.Portrait.transform
                .DOMove(Player.Players[0].PlayerArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
            
            sequence.Insert(0, Player.Players[1].PlayerArea.Portrait.transform
                .DOMove(Player.Players[1].PlayerArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));

            sequence.PrependInterval(3f);
            sequence.OnComplete(() =>
            {
                int random = Random.Range(0, 2);
                Player whoGoesFirst = Player.Players[random];
                Player whoGoesSecond = whoGoesFirst.OtherPlayer;
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {
                    whoGoesFirst.DrawACard(true);
                    whoGoesSecond.DrawACard(true);
                }
                whoGoesSecond.DrawACard(true);
                whoGoesSecond.DrawACoin();
                new StartATurnCommand(whoGoesFirst).AddToQueue();
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) EndTurn();
        }

        public void EndTurn()
        {
            StopTheTimer();
            _whoseTurn.OnTurnEnd();
            
            new StartATurnCommand(_whoseTurn.OtherPlayer).AddToQueue();
        }

        public void StopTheTimer() => _timer.StopTimer();
    }
}