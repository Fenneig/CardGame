using UnityEngine;

public enum CharClass
{
    Elf,
    Monk,
    Warrior
}

namespace SOAssets
{
    [CreateAssetMenu(fileName = "Create", menuName = "Assets/Character")]

    public class CharacterAsset : ScriptableObject
    {
        public CharClass _class;
        public string _className;
        public int _maxHealth = 30;
        public string _heroPowerName;
        public Sprite _avatarImage;
        public Sprite _heroPowerIconImage;
        public Sprite _avatarBGImage;
        public Sprite _heroPowerBGImage;
        public Color32 _avatarBGTint;
        public Color32 _heroPowerBGTint;
        public Color32 _classCardTint;
        public Color32 _classRibbonsTint;
    }
}