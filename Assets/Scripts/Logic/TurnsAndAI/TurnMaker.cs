using UnityEngine;

namespace CardGame.Logic.TurnsAndAI
{
    public abstract class TurnMaker : MonoBehaviour
    {
        [SerializeField] private Player _player;
        protected Player Player => _player;

        public virtual void OnTurnStart()
        {
            Player.OnTurnStart();
        }
    }
}