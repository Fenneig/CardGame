using UnityEngine;

namespace CardGame.Visual
{
    public class PlayerDeckVisual : MonoBehaviour
    {
        [SerializeField] private AreaPosition _ownerPosition;
        [SerializeField] private float _heightOfOneCard = 0.012f;

        private int _cardsInDeck = 0;
        public int CardsInDeck
        {
            get => _cardsInDeck;
            set
            {
                _cardsInDeck = value;
                transform.position = new Vector3(transform.position.x, transform.position.y, -_heightOfOneCard * value);
            }
        }

        private void Start()
        {
            CardsInDeck = GlobalSettings.Instance.Players[_ownerPosition].Deck.Cards.Count;
        }
    }
}