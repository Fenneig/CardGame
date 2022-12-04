using CardGame.Logic;
using CardGame.Visual;
using UnityEngine;

namespace CardGame.Dragging
{
    public abstract class DraggingActions : MonoBehaviour
    {
        public abstract void OnStartDrag();
        public abstract void OnEndDrag();
        public abstract void OnDraggingInUpdate();
        public virtual bool CanDrag => GlobalSettings.Instance.CanControlThisPlayer(PlayerOwner);
        public abstract bool DragSuccessful();

        protected virtual Player PlayerOwner
        {
            get
            {
                if (tag.Contains("Bottom"))
                    return GlobalSettings.Instance.BottomPlayer;
                if (tag.Contains("Top"))
                    return GlobalSettings.Instance.TopPlayer;
                Debug.LogError($"Untagged card or creature {transform.parent.name}");
                return null;
            }
        }
    }
}