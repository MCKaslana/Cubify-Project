using UnityEngine;

public enum CubeSize
{
    Small = 0,
    Medium = 1,
    Large = 2
}

[CreateAssetMenu(fileName = "CubeData", menuName = "Scriptable Objects/CubeData")]
public class CubeData : ScriptableObject
{
    public CubeSize cubeSize = CubeSize.Medium;
    public float sizeMultiplier = 1f;
    public Color color = Color.white;
}
