using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Splines.ExtrusionShapes;

public class CubePlacement : Singleton<CubePlacement>
{
    protected override bool IsPersistent => false;

    private GameObject _currentCube;

    public event Action OnCubesPlaced;

    [SerializeField] private GameBoard _board;

    private List<CubeSize> _remainingSizes = new()
    {
        CubeSize.Small,
        CubeSize.Medium,
        CubeSize.Large
    };

    private int _placedCount = 0;

    public void StartPlacement()
    {
        Debug.Log("Player placement started");
        SpawnNextCube();
    }
    
    private void SpawnNextCube()
    {
        if (_remainingSizes.Count == 0) return;

        var size = _remainingSizes[0];
        _remainingSizes.RemoveAt(0);

        _currentCube = CubeSpawner.Instance.SpawnPlayerCubePreview(size);
    }

    public void PlaceCube(GameObject cube, int slotIndex)
    {
        Transform slot = _board.playerSlots[slotIndex];

        CubeSpawner.Instance.PlacePlayerCube(cube, slot, slotIndex);

        _placedCount++;

        if (_placedCount >= 3)
        {
            Debug.Log("Player finished placement");
            OnCubesPlaced?.Invoke();
        }
        else
        {
            SpawnNextCube();
        }
    }

    public void PlaceCurrentCube(int slotIndex)
    {
        if (_currentCube == null) return;

        Transform slot = _board.playerSlots[slotIndex];

        CubeSpawner.Instance.PlacePlayerCube(_currentCube, slot, slotIndex);

        _placedCount++;
        _currentCube = null;

        if (_placedCount >= 3)
        {
            Debug.Log("Player finished placement");
            OnCubesPlaced?.Invoke();
        }
        else
        {
            SpawnNextCube();
        }
    }
}
