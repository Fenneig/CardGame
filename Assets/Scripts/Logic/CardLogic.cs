using System;
using System.Collections.Generic;
using CardGame.Logic.SpellScripts;
using CardGame.SOAssets;
using UnityEngine;

namespace CardGame.Logic
{
    [Serializable]
    public class CardLogic : IIdentifiable
    {
        [SerializeField] private Player _owner;
        [SerializeField] private int _uniqueCardID;

        [SerializeField] private CardAsset _cardAsset;
        [SerializeField] private GameObject _visualRepresentation;

        [SerializeField] private int _maxCreaturesOnTable = 7;

        private int _baseManaCost;
        private SpellEffect _effect;
        
        public int ID => _uniqueCardID;
        public CardAsset CardAsset => _cardAsset;
        public SpellEffect Effect => _effect;
        public Player Owner
        {
            get => _owner;
            set => _owner = value;
        }
        
        public int CurrentManaCost { get; set; }

        public bool CanBePlayed
        {
            get
            {
                bool ownersTurn = TurnManager.Instance.WhoseTurn == _owner;
                bool fieldNotFull = true;
                if (_cardAsset.MaxHealth > 0) fieldNotFull = _owner.Table.CreaturesOnTable.Count < _maxCreaturesOnTable;
                return ownersTurn && fieldNotFull && CurrentManaCost <= _owner.ManaLeft;
            }
        }

        public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();

        public CardLogic(CardAsset cardAsset)
        {
            _cardAsset = cardAsset;
            _uniqueCardID = IDFactory.GetUniqueID();
            _baseManaCost = _cardAsset.ManaCost;
            ResetManaCost();
            if (!string.IsNullOrEmpty(cardAsset.SpellScriptName)) 
                _effect = Activator.CreateInstance(Type.GetType(cardAsset.SpellScriptName)) as SpellEffect;
            CardsCreatedThisGame.Add(_uniqueCardID, this);
        }

        public void ResetManaCost()
        {
            CurrentManaCost = _baseManaCost;
        }

    }
}