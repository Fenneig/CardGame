using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardGame.Logic
{
    public class RopeTimer : MonoBehaviour, IEventSystemHandler
    {
        [SerializeField] private GameObject _ropeGameObject;
        [SerializeField] private Slider _ropeSlider;
        [SerializeField] private float _timeForOneTurn;
        [SerializeField] private float _ropeBurnTime;
        [SerializeField] private Text _timerText;
        [SerializeField] private UnityEvent _timerExpired = new UnityEvent();

        private float _timeTillZero;
        private bool _counting;
        private bool _ropeIsBurning;

        private void Awake()
        {
            if (_ropeGameObject != null)
            {
                _ropeSlider.minValue = 0;
                _ropeSlider.maxValue = _ropeBurnTime;
                _ropeGameObject.SetActive(false);
            }
        }

        public void StartTimer()
        {
            _timeTillZero = _timeForOneTurn;
            _counting = true;
            _ropeIsBurning = false;
            if (_ropeGameObject != null) _ropeGameObject.SetActive(false);
        }

        public void StopTimer()
        {
            _counting = false;
        }

        private void Update()
        {
            if (!_counting) return;

            _timeTillZero -= Time.deltaTime;
            if (_timerText != null) _timerText.text = ToString();

            if (_ropeGameObject != null)
            {
                if (_timeTillZero <= _ropeBurnTime && !_ropeIsBurning)
                {
                    _ropeIsBurning = true;
                    _ropeGameObject.SetActive(true);
                }

                if (_ropeIsBurning)
                {
                    _ropeSlider.value = _timeTillZero;
                }
            }

            if (_timeTillZero <= 0)
            {
                _counting = false;
                _timerExpired?.Invoke();
            }

        }

        public override string ToString()
        {
            int inSeconds = Mathf.RoundToInt(_timeTillZero);
            string justSeconds = (inSeconds % 60).ToString();
            if (justSeconds.Length == 1)
                justSeconds = 0 + justSeconds;
            string justMinutes = (inSeconds / 60).ToString();
            if (justMinutes.Length == 1)
                justMinutes = "0" + justMinutes;

            return string.Format($"{justMinutes}:{justSeconds}");
        }
    }
}