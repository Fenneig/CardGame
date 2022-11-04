using DG.Tweening;

namespace CardGame.Commands
{
    public class DelayCommand : Command
    {
        private float _delay;

        public DelayCommand(float delay)
        {
            _delay = delay;
        }

        public override void StartCommandExecution()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.PrependInterval(_delay);
            sequence.OnComplete(CommandExecutionComplete);
        }
    }
}