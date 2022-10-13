using UnityEngine;
using UnityEngine.UI;

namespace CardGame.Visual
{
    public class ManaPoolVisual : MonoBehaviour
    {
        [SerializeField] private Image[] _crystals;
        [SerializeField] private Text _progressText;
        
        private int _totalCrystals;
        public int TotalCrystals
        {
            get => _totalCrystals;
            set
            {
                if (value > _crystals.Length) _totalCrystals = _crystals.Length;
                else if (value < 0) _totalCrystals = 0;
                else _totalCrystals = value;

                for (int i = 0; i < _crystals.Length; i++)
                {
                    if (i < _totalCrystals)
                    {
                        if (_crystals[i].color == Color.clear)
                            _crystals[i].color = Color.gray;
                    }
                    else
                    {
                        _crystals[i].color = Color.clear;
                    }
                }

                _progressText.text = $"{_availableCrystals}/{_totalCrystals}";
            }
        }

        private int _availableCrystals;
        public int AvailableCrystals
        {
            get => _availableCrystals;
            set
            {
                if (value > _totalCrystals) _availableCrystals = _totalCrystals;
                else if (value < 0) _availableCrystals = 0;
                else _availableCrystals = value;
                
                for (int i = 0; i < _totalCrystals; i++) 
                    _crystals[i].color = i < _availableCrystals ? Color.white : Color.gray;
                
                _progressText.text = $"{_availableCrystals}/{_totalCrystals}";
            }
        }
    }
}