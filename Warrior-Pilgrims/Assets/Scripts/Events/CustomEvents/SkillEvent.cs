using GramophoneUtils.Items;
using UnityEngine;

namespace GramophoneUtils.Events.CustomEvents
{
    [CreateAssetMenu(fileName = "New Skill Event", menuName = "Game Events/Skill Event")]
    public class SkillEvent : BaseGameEvent<ISkill> { }
}
