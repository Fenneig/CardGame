using System.Collections.Generic;
using CardGame.Commands;
using CardGame.Logic.CreatureScripts;
using CardGame.SOAssets;
using UnityEngine;

namespace CardGame.Logic
{
    public class CreatureLogic : ICharacter
    {
        [SerializeField] private Player _owner;
        [SerializeField] private CardAsset _cardAsset;
        [SerializeField] private CreatureEffect _effect;
        [SerializeField] private int _uniqueCreatureID;
        [SerializeField] private bool _frozen = false;

        private int _health;
        private int _attacksForOneTurn = 1;
        private int _attacksLeftThisTurn;

        public int ID => _uniqueCreatureID;
        public int AttacksLeftThisTurn => _attacksLeftThisTurn;
        private int MaxHealth { get; }
        private int Attack { get; }
        

        public bool CanAttack
        {
            get
            {
                bool ownersTurn = TurnManager.Instance.WhoseTurn == _owner;
                return ownersTurn && _attacksLeftThisTurn > 0 && !_frozen;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                if (value > MaxHealth) _health = MaxHealth;
                else if (value <= 0) Die();
                else _health = value;
            }
        }

        public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame =
            new Dictionary<int, CreatureLogic>();

        public CreatureLogic(Player owner, CardAsset cardAsset)
        {
            _cardAsset = cardAsset;
            MaxHealth = cardAsset.MaxHealth;
            _health = cardAsset.MaxHealth;
            Attack = cardAsset.Attack;
            _attacksForOneTurn = cardAsset.AttacksForOneTurn;
            if (cardAsset.Charge) _attacksLeftThisTurn = _attacksForOneTurn;
            _owner = owner;
            _uniqueCreatureID = IDFactory.GetUniqueID();
            if (!string.IsNullOrEmpty(cardAsset.CreatureScriptName))
            {
                _effect = System.Activator.CreateInstance(System.Type.GetType(cardAsset.CreatureScriptName),
                    new object[] {owner, this, cardAsset.SpecialCreatureAmount}) as CreatureEffect;
                _effect?.RegisterEffect();
            }

            CreaturesCreatedThisGame.Add(_uniqueCreatureID, this);
        }

        public void OnTurnStart()
        {
            _attacksLeftThisTurn = _attacksForOneTurn;
        }

        public void Die()
        {
            _owner.Table.CreaturesOnTable.Remove(this);
            new CreatureDieCommand(_uniqueCreatureID, _owner).AddToQueue();
        }

        public void GoFace()
        {
            _attacksLeftThisTurn--;
            int targetHealthAfter = _owner.OtherPlayer.Health - Attack;
            new CreatureAttackCommand(_owner.OtherPlayer.ID, _uniqueCreatureID, 0, Attack, Health, targetHealthAfter)
                .AddToQueue();
            _owner.OtherPlayer.Health -= Attack;
        }

        public void AttackCreature(CreatureLogic target)
        {
            _attacksLeftThisTurn--;
            int targetHealthAfter = target.Health - Attack;
            int attackerHealthAfter = Health - target.Attack;
            new CreatureAttackCommand(target.ID, ID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter)
                .AddToQueue();
            target.Health -= Attack;
            Health -= target.Attack;
        }

        public void AttackCreatureWithID(int uniqueCreatureID)
        {
            CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
            AttackCreature(target);
        }
    }
}