using GramophoneUtils.Characters;
using System.Collections.Generic;

namespace GramophoneUtils
{
    public interface IUsable
    {
        void Use(List<Character> characterTargets, Character originator);
    }
}
