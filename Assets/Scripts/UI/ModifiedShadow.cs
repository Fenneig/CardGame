using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ModifiedShadow : Shadow
    {
        protected new void ApplyShadow(List<UIVertex> verts, Color32 color, int start, int end, float x, float y)
        {
            UIVertex vt;

            var neededCapacity = verts.Count + (end - start);
            if (verts.Capacity < neededCapacity)
                verts.Capacity = neededCapacity;

            for (int i = start; i < end; ++i)
            {
                vt = verts[i];
                verts.Add(vt);

                Vector3 v = vt.position;
                v.x += x;
                v.y += y;
                vt.position = v;
                var newColor = color;
                if (useGraphicAlpha)
                    newColor.a = (byte)((newColor.a * verts[i].color.a) / 255);
                vt.color = newColor;
                verts[i] = vt;
            }
        }

        public void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            var list = ListPool<UIVertex>.Get();
            vh.GetUIVertexStream(list);

            ModifyVertices(list);
            
            vh.AddUIVertexTriangleStream(list);
            ListPool<UIVertex>.Release(list);
        }

        public virtual void ModifyVertices(List<UIVertex> verts)
        {
        }

    }
}