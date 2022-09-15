using CardGame.Logic;
using UnityEngine;

namespace CardGame
{
    public class GlobalSettings : MonoBehaviour
    {
        [Header("Player")]
        public Player TopPlayer;
        public Player LowPlayer;
        [Header("Colors")] 
        public Color32 CardBodyStandardColor;
        public Color32 CardRibbonsStandardColor;
        public Color32 CardGlowColor;
        
        

        private static GlobalSettings _instance;
        public static GlobalSettings Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }
    }
}