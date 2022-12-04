using UnityEngine;
using XNode;

namespace GramophoneUtils.Maps
{
    public abstract class MapBaseNode : Node
    {
        [Input(backingValue = ShowBackingValue.Never)] public MapBaseNode input;
        [Output(backingValue = ShowBackingValue.Never)] public MapBaseNode output;

        abstract public void Trigger();

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}


