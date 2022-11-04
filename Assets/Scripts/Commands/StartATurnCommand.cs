using CardGame.Logic;

namespace CardGame.Commands
{
    public class StartATurnCommand : Command
    {
        private Player _player;

        public StartATurnCommand(Player player)
        {
            _player = player;
        }

        public override void StartCommandExecution()
        {
            TurnManager.Instance.WhoseTurn = _player;
            CommandExecutionComplete();
        }
    }
}