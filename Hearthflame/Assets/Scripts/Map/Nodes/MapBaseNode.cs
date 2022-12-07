using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    [NodeWidth(500)]
    public abstract class MapBaseNode : Node
    {
        [Input(backingValue = ShowBackingValue.Never)] public MapBaseNode input;

        abstract public void Trigger();

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}