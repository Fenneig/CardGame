using CardGame.Logic;

namespace CardGame.Commands
{
    public class UpdateManaCrystalsCommand : Command
    {
        private Player _player;
        private int _totalMana;
        private int _availableMana;

        public UpdateManaCrystalsCommand(Player player, int totalMana, int availableMana)
        {
            _player = player;
            _totalMana = totalMana;
            _availableMana = availableMana;
        }

        public override void StartCommandExecution()
        {
            _player.PlayerArea.ManaBar.TotalCrystals = _totalMana;
            _player.PlayerArea.ManaBar.AvailableCrystals = _availableMana;
            CommandExecutionComplete();
        }
    }
}