using CardGame.Commands;

namespace CardGame.Logic.TurnsAndAI
{
    public class PlayerTurnMaker : TurnMaker
    {
        public override void OnTurnStart()
        {
            base.OnTurnStart();
            new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
            Player.DrawACard();
        }
    }
}