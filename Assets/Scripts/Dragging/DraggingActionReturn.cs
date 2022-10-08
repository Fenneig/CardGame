using DG.Tweening;
using UnityEngine;

namespace CardGame.Dragging
{
    public class DraggingActionReturn : DraggingActions
    {
        private Vector3 _savedPos;
        public override void OnStartDrag() => _savedPos = transform.position;

        public override void OnEndDrag() => transform.DOMove(_savedPos, 1f).SetEase(Ease.OutQuint);

        public override void OnDraggingInUpdate() {}

        public override bool OnDragSuccess() => true;
    }
}