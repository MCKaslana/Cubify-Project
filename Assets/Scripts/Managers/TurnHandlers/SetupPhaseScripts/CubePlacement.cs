using System;
using UnityEngine;
using System.Collections.Generic;

public class CubePlacement : Singleton<CubePlacement>
{
    protected override bool IsPersistent => false;

    private GameObject _currentCube;

    public event Action OnCubesPlaced;

    [SerializeField] private GameObject _previewHighlight;
    private List<GameObject> _previewList = new();

    [SerializeField] private GameBoard _board;

    private bool _isPlacing = false;

    private List<CubeSize> _remainingSizes = new()
    {
        CubeSize.Small,
        CubeSize.Medium,
        CubeSize.Large
    };

    private int _placedCount = 0;

    public void StartPlacement()
    {
        if (_isPlacing) return;

        _isPlacing = true;

        Debug.Log("Player placement started");
        SpawnNextCube();
    }
    
    private void SpawnNextCube()
    {
        if (_currentCube != null) Destroy(_currentCube);

        if (_remainingSizes.Count == 0) return;

        var size = _remainingSizes[0];
        _remainingSizes.RemoveAt(0);

        _currentCube = CubeSpawner.Instance.SpawnPlayerCubePreview(size);
        GameObject highlight = Instantiate(_previewHighlight, _currentCube.transform.position, Quaternion.identity, _currentCube.transform);
        _previewList.Add(highlight);
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

    public void ClearAllHighlights()
    {
        foreach (var highlight in _previewList)
        {
            if (highlight != null)
                Destroy(highlight);
        }
        _previewList.Clear();
    }
}
