using System.Collections.Generic;
using CardGame.SOAssets;
using UnityEngine;
using Utils;

namespace CardGame.Logic
{
    public class Deck : MonoBehaviour
    {
        [SerializeField] private List<CardAsset> _cards = new List<CardAsset>();
        public List<CardAsset> Cards => _cards;

        private void Awake()
        {
            Cards.Shuffle();
        }
    }
}