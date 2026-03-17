using UnityEngine;

public enum CubeSize
{
    Small,
    Medium,
    Large
}

[CreateAssetMenu(fileName = "CubeData", menuName = "Scriptable Objects/CubeData")]
public class CubeData : ScriptableObject
{
    public CubeSize cubeSize = CubeSize.Medium;
    public float sizeMultiplier = 1f;
    public Color color = Color.white;
}
