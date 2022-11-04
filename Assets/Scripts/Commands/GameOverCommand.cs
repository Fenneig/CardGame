using CardGame.Logic;

namespace CardGame.Commands
{
    public class GameOverCommand : Command
    {
        private Player _looser;

        public GameOverCommand(Player looser)
        {
            _looser = looser;
        }
        public override void StartCommandExecution()
        {
            _looser.PlayerArea.Portrait.Explode();
        }
    }
}