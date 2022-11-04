﻿using CardGame.Visual;
using UnityEngine;

namespace CardGame.Dragging
{
    public class DragHeroPowerAction : DraggingActions
    {
        [SerializeField] private TargetingOptions _options;
        public override void OnStartDrag()
        {
            
        }

        public override void OnEndDrag()
        {
            
        }

        public override void OnDraggingInUpdate()
        {
            
        }

        public override bool DragSuccess()
        {
            return TableVisual.CursorOverSomeTable;
        }
    }
}