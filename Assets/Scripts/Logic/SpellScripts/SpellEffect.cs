using UnityEngine;

namespace CardGame.Logic.SpellScripts
{
    public abstract class SpellEffect
    {
        public virtual void ActivateEffect(int specialAmount = 0, ICharacter target = null)
        {
            Debug.Log("No spell effect with this name found! Check for typos in CardAsset");
        }
    }
}