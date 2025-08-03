using System.Collections.Generic;
using UnityEngine;
using Combat;
using System.Runtime.CompilerServices;

[CreateAssetMenu(menuName = "Registry/SkillRegistry")]
public class SkillRegistrySO : ScriptableObject
{
    public List<skillEntry> registeredSkills = new();
    // public Dictionary<string, SkillSO> skillRegistery = new();

    // public void Register(string skill)
    // {
    //     if (!registeredSkills.Contains(skill))
    //         registeredSkills.Add(skill);
    // }

    // public void Clear() => registeredSkills.Clear(); // Optional: for runtime cleanup
}
[System.Serializable]
public class skillEntry
{
    public string key;
    public SkillSO value;
}