namespace CardGame.Logic.CreatureScripts
{
    public abstract class CreatureEffect
    {
        private Player _owner;
        private CreatureLogic _creature;
        private int _specialAmount;

        public CreatureEffect(Player owner, CreatureLogic creature, int specialAmount)
        {
            _creature = creature;
            _owner = owner;
            _specialAmount = specialAmount;
        }

        public abstract void RegisterEffect();
        public abstract void CauseEffect();
    }
}