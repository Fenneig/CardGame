using CardGame.Logic;
using CardGame.Visual;
using UnityEngine;

namespace CardGame.Commands
{
    public class CreatureAttackCommand : Command
    {
        private int _targetUniqueID;
        private int _attackerUniqueID;
        private int _attackerHealthAfter;
        private int _targetHealthAfter;
        private int _damageTakenByAttacker;
        private int _damageTakenByTarget;

        public CreatureAttackCommand(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget,
            int attackerHealthAfter, int targetHealthAfter)
        {
            _targetUniqueID = targetID;
            _attackerUniqueID = attackerID;
            _damageTakenByAttacker = damageTakenByAttacker;
            _damageTakenByTarget = damageTakenByTarget;
            _attackerHealthAfter = attackerHealthAfter;
            _targetHealthAfter = targetHealthAfter;
        }

        public override void StartCommandExecution()
        {
            GameObject attacker = IDHolder.GetGameObjectWithID(_attackerUniqueID);

            attacker.GetComponent<CreatureAttackVisual>().AttackTarget(_targetUniqueID, _damageTakenByTarget,
                _damageTakenByAttacker, _attackerHealthAfter, _targetHealthAfter);
        }
    }
}