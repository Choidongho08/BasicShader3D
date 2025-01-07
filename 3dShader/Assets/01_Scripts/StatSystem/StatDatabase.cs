using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatDatabase", menuName = "SO/StatDatabase")]
public class StatDatabase : ScriptableObject
{
    public string assetPath = "Assets/System";
    public List<StatSO> table;
}
