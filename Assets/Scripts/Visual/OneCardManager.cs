﻿using CardGame.SOAssets;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{

    public class OneCardManager : MonoBehaviour
    {
        public CardAsset CardAsset;
        public OneCardManager PreviewManager;
        [Header("Text Component References")] 
        public Text NameText;
        public Text ManaCostText;
        public Text DescriptionText;
        public Text HealthText;
        public Text AttackText;
        [Header("Image References")]
        public Image CardTopRibbonImage;
        public Image CardLowRibbonImage;
        public Image CardGraphicImage;
        public Image CardBodyImage;
        public Image CardFaceFrameImage;
        public Image CardFaceGlowImage;
        public Image CardBackGlowImage;

        private void Awake()
        {
            if (CardAsset != null)
                ReadCardFromAsset();
        }

        private bool _canBePlayedNow = false;

        public bool CanBePlayedNow
        {
            get => _canBePlayedNow;
            set
            {
                _canBePlayedNow = value;
                CardFaceGlowImage.enabled = value;
            }
        }

        private void ReadCardFromAsset()
        {
            if (CardAsset.CharacterAsset != null)
            {
                CardBodyImage.color = CardAsset.CharacterAsset.ClassCardTint;
                CardFaceFrameImage.color = CardAsset.CharacterAsset.ClassCardTint;
                CardTopRibbonImage.color = CardAsset.CharacterAsset.ClassRibbonsTint;
                CardLowRibbonImage.color = CardAsset.CharacterAsset.ClassRibbonsTint;
            }

            NameText.text = CardAsset.name;
            ManaCostText.text = CardAsset.ManaCost.ToString();
            DescriptionText.text = CardAsset.Description;
            CardGraphicImage.sprite = CardAsset.CardImage;

            if (CardAsset.MaxHealth != 0)
            {
                AttackText.text = CardAsset.Attack.ToString();
                HealthText.text = CardAsset.MaxHealth.ToString();
            }

            if (PreviewManager != null)
            {
                PreviewManager.CardAsset = CardAsset;
                PreviewManager.ReadCardFromAsset();
                
            }
        }

    }
}