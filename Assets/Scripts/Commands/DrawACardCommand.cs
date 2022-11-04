using CardGame.Logic;

namespace CardGame.Commands
{
    public class DrawACardCommand : Command
    {
        private Player _player;
        //TODO возможно поле не нужно
        private int _positionInHand;
        private CardLogic _cardLogic;
        private bool _fast;
        private int _id;
        private bool _fromDeck;

        public DrawACardCommand(CardLogic cardLogic, Player player, int positionInHand, bool fast, bool fromDeck)
        {
            _player = player;
            _positionInHand = positionInHand;
            _cardLogic = cardLogic;
            _fast = fast;
            _fromDeck = fromDeck;
        }

        public override void StartCommandExecution()
        {
            _player.PlayerArea.PlayerDeck.CardsInDeck--;
            _player.PlayerArea.HandVisual.GivePlayerACard(_cardLogic.CardAsset, _cardLogic.ID, _fast, _fromDeck);
        }
    }
}