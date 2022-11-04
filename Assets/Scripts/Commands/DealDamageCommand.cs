using CardGame.Logic;
using CardGame.Visual;
using CardGame.Visual.CardVisual;
using UnityEngine;

namespace CardGame.Commands
{
    public class DealDamageCommand : Command
    {
        private int _targetID;
        private int _amount;
        private int _healthAfter;

        public DealDamageCommand(int targetID, int amount, int healthAfter)
        {
            _targetID = targetID;
            _amount = amount;
            _healthAfter = healthAfter;
        }

        public override void StartCommandExecution()
        {
            GameObject target = IDHolder.GetGameObjectWithID(_targetID);
            if (_targetID == GlobalSettings.Instance.BottomPlayer.ID ||
                _targetID == GlobalSettings.Instance.TopPlayer.ID)
                target.GetComponent<PlayerPortraitVisual>().TakeDamage(_amount, _healthAfter);
            else target.GetComponent<OneCreatureManager>().TakeDamage(_amount, _healthAfter);
            
            CommandExecutionComplete();
        }
    }
}