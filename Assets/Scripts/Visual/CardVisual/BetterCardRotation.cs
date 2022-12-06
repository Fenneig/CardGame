using UnityEngine;

namespace CardGame.Visual.CardVisual
{
//Скрипт прикрепляется к GO карте для корректного отображения ротации карты 
    [ExecuteInEditMode]
    public class BetterCardRotation : MonoBehaviour
    {
        //родительский объект со всей графикой лица карты
        [SerializeField] private RectTransform _cardFront;

        //родительский объект со всей графикой рубашки карты
        [SerializeField] private RectTransform _cardBack;

        //пустой GO расположенный перед лицом карты строго по центру карты
        [SerializeField] private Transform _targetFacePoint;

        //3д коллайдер присоеденненый к карте (2д коллайдер не сработает)
        [SerializeField] private Collider _collider;

        //Буля показывающая смотрит ли игрок на рубашку карты
        private bool _showingBack;


        private void FixedUpdate()
        {
            //кидаем луч от камеры к точке перед лицом карты, если луч проходит через коллайдер значит карта находится рубашкой "вверх"
            RaycastHit[] hits;

            var pointOfView = Camera.main.transform.position;
            hits = Physics.RaycastAll(pointOfView, 
                (-pointOfView + _targetFacePoint.position).normalized,
                (-pointOfView + _targetFacePoint.position).magnitude);
            
            bool passedThroughColliderOnCard = false;
            foreach (var hit in hits)
            {
                if (hit.collider == _collider)
                    passedThroughColliderOnCard = true;
            }

            if (passedThroughColliderOnCard == _showingBack) return;
            _showingBack = passedThroughColliderOnCard;
            if (_showingBack)
            {
                _cardFront.gameObject.SetActive(false);
                _cardBack.gameObject.SetActive(true);
            }
            else
            {
                _cardFront.gameObject.SetActive(true);
                _cardBack.gameObject.SetActive(false);
            }
        }
    }
}