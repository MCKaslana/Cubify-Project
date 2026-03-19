using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public Transform[] playerSlots = new Transform[3];
    public Transform[] aiSlots = new Transform[3];

    public Transform GetRandomAISlot(ref List<int> available)
    {
        int index = Random.Range(0, available.Count);
        int slotIndex = available[index];
        available.RemoveAt(index);
        return aiSlots[slotIndex];
    }
}
