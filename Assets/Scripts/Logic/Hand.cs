using System.Collections.Generic;
using UnityEngine;

namespace CardGame.Logic
{
    public class Hand : MonoBehaviour
    {
        [SerializeField] private List<CardLogic> _cardsInHand = new List<CardLogic>();
        public List<CardLogic> CardsInHand => _cardsInHand;
    }
}