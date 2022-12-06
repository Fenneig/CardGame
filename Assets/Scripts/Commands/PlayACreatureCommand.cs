using CardGame.Logic;
using CardGame.Visual;
using UnityEngine;

namespace CardGame.Commands
{
    public class PlayACreatureCommand : Command
    {
        private Player _player;
        private CardLogic _creature;
        private int _tablePosition;
        private int _id;

        public PlayACreatureCommand(CardLogic creatureCard, Player player, int tableTablePosition, int uniqueID)
        {
            _player = player;
            _creature = creatureCard;
            _tablePosition = tableTablePosition;
            _id = uniqueID;
        }

        public override void StartCommandExecution()
        {
            HandVisual playerHand = _player.PlayerArea.HandVisual;
            GameObject card = IDHolder.GetGameObjectWithID(_creature.ID);
            playerHand.RemoveCard(card);
            _player.PlayerArea.TableVisual.AddCreatureAtIndex(_creature.CardAsset, _id, _tablePosition);
            card.SetActive(false);
        }
    }
}