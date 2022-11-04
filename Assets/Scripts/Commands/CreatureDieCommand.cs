using CardGame.Logic;

namespace CardGame.Commands
{
    public class CreatureDieCommand : Command
    {
        private Player _player;
        private int _deadCreatureID;

        public CreatureDieCommand(int deadCreatureID, Player player)
        {
            _player = player;
            _deadCreatureID = deadCreatureID;
        }

        public override void StartCommandExecution() => 
            _player.PlayerArea.TableVisual.RemoveCreatureWithID(_deadCreatureID);
    }
}