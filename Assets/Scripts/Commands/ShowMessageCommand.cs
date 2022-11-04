using CardGame.Visual;

namespace CardGame.Commands
{
    public class ShowMessageCommand : Command
    {
        private string _message;
        private float _duration;

        public ShowMessageCommand(string message, float duration)
        {
            _message = message;
            _duration = duration;
        }

        public override void StartCommandExecution()
        {
            MessageManager.Instance.ShowMessage(_message, _duration, this);
        }
    }
}