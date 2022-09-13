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

namespace SOAssets
{
    [CreateAssetMenu(fileName = "Create", menuName = "Assets/Card")]
    public class CardAsset : ScriptableObject
    {
        [Header("General Info")] [Tooltip("If this is null, it's a neutral card")]
        public CharacterAsset _characterAsset;
        [TextArea(2, 3)] 
        public string _description;
        public Sprite _cardImage;
        public int _manaCost;

        [Header("Creature Info")] [Tooltip("If this parameter equals to 0 that means that this is spell card")]
        public int _maxHealth;
        public int _attack;
        public int _attacksForOneTurn = 1;
        public bool _taunt;
        public bool _charge;
        public string _creatureScriptName;
        public int _specialCreatureAmount;

        [Header("SpellInfo")] 
        public string _spellScriptName;
        public int _specialSpellAmount;
        public TargetingOptions _targets;

    }
}