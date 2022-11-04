using CardGame.Logic;
using UnityEngine;

namespace CardGame.UI
{
    public class EndTurnButton : MonoBehaviour
    {
        public void OnClick()
        {
            TurnManager.Instance.EndTurn();
        }
    }
}