using CardGame.Commands;
using CardGame.Logic;
using CardGame.Visual.CardVisual;
using DG.Tweening;
using UnityEngine;

namespace CardGame.Visual
{
    public class CreatureAttackVisual : MonoBehaviour
    {
        private OneCreatureManager _manager;
        private CardsAndCreaturesVisualState _cardState;

        private void Awake()
        {
            _manager = GetComponent<OneCreatureManager>();
            _cardState = GetComponent<CardsAndCreaturesVisualState>();
        }

        public void AttackTarget(int targetUniqueID, int DamageTakenByTarget, int damageTakenByAttacker,
            int AttackerHealthAfter, int targetHealthAfter)
        {
            _manager.CanAttack = false;
            GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);
            
            _cardState.BringToFront();
            VisualStates tempState = _cardState.State;
            _cardState.State = VisualStates.Transition;

            transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(
                () =>
                {
                    if (DamageTakenByTarget > 0)
                        DamageEffect.CreateDamageEffect(target.transform.position, DamageTakenByTarget);
                    if (damageTakenByAttacker > 0)
                        DamageEffect.CreateDamageEffect(transform.position, damageTakenByAttacker);

                    if (targetUniqueID == GlobalSettings.Instance.BottomPlayer.ID ||
                        targetUniqueID == GlobalSettings.Instance.TopPlayer.ID)
                        target.GetComponent<PlayerPortraitVisual>().HealthText.text = targetHealthAfter.ToString();
                    else 
                        target.GetComponent<OneCreatureManager>().HealthText.text = targetHealthAfter.ToString();
                    
                    _cardState.SetTableSortingOrder();
                    _cardState.State = tempState;

                    _manager.HealthText.text = AttackerHealthAfter.ToString();
                    Sequence sequence = DOTween.Sequence();
                    sequence.AppendInterval(1f);
                    sequence.OnComplete(Command.CommandExecutionComplete);
                });
        }
    }
}