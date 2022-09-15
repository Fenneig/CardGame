using UnityEngine;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures,
    EnemyCreatures,
    YourCreatures,
    AllCharacters,
    EnemyCharacters,
    YourCharacters
}

namespace CardGame.SOAssets
{
    [CreateAssetMenu(fileName = "Create", menuName = "Assets/Card")]
    public class CardAsset : ScriptableObject
    {
        [Header("General Info")] [Tooltip("If this is null, it's a neutral card")]
        public CharacterAsset CharacterAsset;
        [TextArea(2, 3)] 
        public string Description;
        public Sprite CardImage;
        public int ManaCost;

        [Header("Creature Info")] [Tooltip("If this parameter equals to 0 that means that this is spell card")]
        public int MaxHealth;
        public int Attack;
        public int AttacksForOneTurn = 1;
        public bool Taunt;
        public bool Charge;
        public string CreatureScriptName;
        public int SpecialCreatureAmount;

        [Header("SpellInfo")] 
        public string SpellScriptName;
        public int SpecialSpellAmount;
        public TargetingOptions Targets;

    }
}