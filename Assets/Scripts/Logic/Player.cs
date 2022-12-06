using System;
using CardGame.Commands;
using CardGame.Logic.SpellScripts;
using CardGame.Logic.TurnsAndAI;
using CardGame.SOAssets;
using CardGame.Visual;
using CardGame.Visual.CardVisual;
using UnityEngine;

namespace CardGame.Logic
{
    public class Player : MonoBehaviour, ICharacter
    {
        [SerializeField] private int _playerID;
        [SerializeField] private CharacterAsset _characterAsset;
        [SerializeField] private PlayerArea _playerArea;
        [SerializeField] private SpellEffect _heroPowerEffect;

        [SerializeField] private Deck _deck;
        [SerializeField] private Hand _hand;
        [SerializeField] private Table _table;

        private int _bonusManaThisTurn = 0;
        private bool _usedHeroPowerThisTurn = false;

        private static Player[] _players;
        private TurnMaker _turnMaker;


        public int ID => _playerID;
        public PlayerArea PlayerArea => _playerArea;
        public Deck Deck => _deck;
        public Table Table => _table;
        public static Player[] Players => _players;
        public int Health { get; set; }
        public Player OtherPlayer => _players[0] == this ? _players[1] : _players[0];

        private int _manaThisTurn;

        public int ManaThisTurn
        {
            get => _manaThisTurn;
            set
            {
                _manaThisTurn = value;
                new UpdateManaCrystalsCommand(this, _manaThisTurn, _manaLeft).AddToQueue();
            }
        }

        private int _manaLeft;

        public int ManaLeft
        {
            get => _manaLeft;
            set
            {
                _manaLeft = value;
                new UpdateManaCrystalsCommand(this, _manaThisTurn, _manaLeft).AddToQueue();

                if (TurnManager.Instance.WhoseTurn == this) HighlightPlayableCards();
            }
        }

        public delegate void VoidWithNoArguments();

        public event VoidWithNoArguments EndTurnEvent;


        private void Awake()
        {
            _players = FindObjectsOfType<Player>();
            _playerID = IDFactory.GetUniqueID();
            //TODO если сериализация на авейке работает то поменять на это
            //_turnMaker = GetComponent<TurnMaker>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.D)) DrawACard(true);
        }

        public void OnTurnStart()
        {
            Debug.Log($"Turn start for {gameObject.name}");
            _usedHeroPowerThisTurn = false;
            _manaThisTurn++;
            ManaLeft = _manaThisTurn;
            foreach (CreatureLogic creatures in _table.CreaturesOnTable)
                creatures.OnTurnStart();
            PlayerArea.HeroPower.WasUsed = false;
        }

        public void GetBonusMana(int amount)
        {
            _bonusManaThisTurn += amount;
            _manaThisTurn += amount;
            _manaLeft += amount;
        }

        public void OnTurnEnd()
        {
            EndTurnEvent?.Invoke();
            _manaThisTurn -= _bonusManaThisTurn;
            _bonusManaThisTurn = 0;
            GetComponent<TurnMaker>().StopAllCoroutines();
        }

        public void DrawACard(bool fast = false)
        {
            if (_deck.Cards.Count > 0)
            {
                if (_hand.CardsInHand.Count < _playerArea.HandVisual.Slots.Children.Length)
                {
                    int indexToPlaceACard = _hand.CardsInHand.Count;
                    CardLogic newCard = new CardLogic(_deck.Cards[0]) {Owner = this};
                    _hand.CardsInHand.Add(newCard);
                    _deck.Cards.RemoveAt(0);
                    new DrawACardCommand(newCard, this, indexToPlaceACard, fast, true)
                        .AddToQueue();
                }
            }
        }

        public void DrawACoin()
        {
            if (_hand.CardsInHand.Count >= _playerArea.HandVisual.Slots.Children.Length) return;

            CardLogic coinCard = new CardLogic(GlobalSettings.Instance.CoinCard);
            coinCard.Owner = this;
            _hand.CardsInHand.Add(coinCard);
            new DrawACardCommand(_hand.CardsInHand[_hand.CardsInHand.Count - 1], this, _hand.CardsInHand.Count - 1,
                true, false).AddToQueue();
        }

        public void PlayASpellFromHand(int spellCardUniqueID, int targetUniqueID)
        {
            if (targetUniqueID < 0) PlayASpellFromHand(CardLogic.CardsCreatedThisGame[spellCardUniqueID], null);
            else if (targetUniqueID == ID) PlayASpellFromHand(CardLogic.CardsCreatedThisGame[spellCardUniqueID], this);
            else if (targetUniqueID == OtherPlayer.ID)
                PlayASpellFromHand(CardLogic.CardsCreatedThisGame[spellCardUniqueID], OtherPlayer);
            else
                PlayASpellFromHand(CardLogic.CardsCreatedThisGame[spellCardUniqueID],
                    CreatureLogic.CreaturesCreatedThisGame[targetUniqueID]);
        }

        public void PlayASpellFromHand(CardLogic playedCard, ICharacter targetUniqueID)
        {
            _manaLeft -= playedCard.CurrentManaCost;

            if (playedCard.Effect != null)
                playedCard.Effect.ActivateEffect(playedCard.CardAsset.SpecialSpellAmount, targetUniqueID);
            else Debug.LogWarning($"No effect found on card {playedCard.CardAsset.name}");

            new PlayASpellCommand(this, playedCard).AddToQueue();
            _hand.CardsInHand.Remove(playedCard);
        }

        public void PlayACreatureFromHand(int uniqueID, int tablePosition)
        {
            PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[uniqueID], tablePosition);
        }

        public void PlayACreatureFromHand(CardLogic playedCard, int tablePosition)
        {
            _manaLeft -= playedCard.CurrentManaCost;
            CreatureLogic newCreature = new CreatureLogic(this, playedCard.CardAsset);
            _table.CreaturesOnTable.Insert(tablePosition, newCreature);
            new PlayACreatureCommand(playedCard, this, tablePosition, newCreature.ID).AddToQueue();
            _hand.CardsInHand.Remove(playedCard);
            HighlightPlayableCards();
        }

        public void Die()
        {
            _playerArea.ControlsEnable = false;
            OtherPlayer.PlayerArea.ControlsEnable = false;
            TurnManager.Instance.StopTheTimer();
            new GameOverCommand(this).AddToQueue();
        }

        public void HighlightPlayableCards(bool removeAllHighlights = false)
        {
            foreach (var card in _hand.CardsInHand)
            {
                GameObject cardManager = IDHolder.GetGameObjectWithID(card.ID);
                if (cardManager != null)
                    cardManager.GetComponent<OneCardManager>().CanBePlayedNow =
                        card.CurrentManaCost <= _manaLeft && !removeAllHighlights;
            }

            foreach (var creature in _table.CreaturesOnTable)
            {
                GameObject creatureManager = IDHolder.GetGameObjectWithID(creature.ID);
                if (creatureManager != null)
                    creatureManager.GetComponent<OneCreatureManager>().CanAttack =
                        creature.AttacksLeftThisTurn > 0 && !removeAllHighlights;
            }

            _playerArea.HeroPower.Highlighted = !_usedHeroPowerThisTurn && _manaLeft >= 2 && !removeAllHighlights;
        }

        public void LoadCharacterInfoFromAsset()
        {
            Health = _characterAsset.MaxHealth;
            _playerArea.Portrait.CharacterAsset = _characterAsset;
            _playerArea.Portrait.ApplyLookFromAsset();
            if (!String.IsNullOrEmpty(_characterAsset.HeroPowerName))
                _heroPowerEffect = Activator.CreateInstance(Type.GetType(_characterAsset.HeroPowerName)) as SpellEffect;
            else Debug.LogWarning($"Check Hero power name for character {_characterAsset.ClassName}");
        }

        public void TransmitInfoAboutPlayerToVisual()
        {
            _playerArea.Portrait.GetComponent<IDHolder>().UniqueID = _playerID;
            _playerArea.AllowedToControlThisPlayer = !(GetComponent<TurnMaker>() is AITurnMaker);
        }

        public void UseHeroPower()
        {
            _manaLeft -= 2;
            _usedHeroPowerThisTurn = true;
            _heroPowerEffect.ActivateEffect();
        }
    }
}