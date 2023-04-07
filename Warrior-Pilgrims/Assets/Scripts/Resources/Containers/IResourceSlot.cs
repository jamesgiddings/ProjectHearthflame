using System;
using UnityEngine;

namespace GramophoneUtils.Containers
{
    [Serializable]
    public struct IResourceSlot
    {
        private IResource _resource;
        public IResource Resource
        {
            get
            {
                return _resource;
            }
            set
            {
                _resource = value;
            }
        }

        [MinAttribute(1)]
        public int _quantity;
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
            }
        }

        public IResourceSlot(IResource resource, int quantity)
        {
            _resource = resource;
            _quantity = quantity;
        }
    }
}

