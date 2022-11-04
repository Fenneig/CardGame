using CardGame.Logic;

namespace CardGame.Commands
{
    public class PlayASpellCommand : Command
    {
        private CardLogic _cardLogic;
        private Player _player;

        public PlayASpellCommand(Player player, CardLogic cardLogic)
        {
            _cardLogic = cardLogic;
            _player = player;
        }

        public override void StartCommandExecution()
        {
            _player.PlayerArea.HandVisual.PlayASpellFromHand(_cardLogic.ID);
        }
    }
}