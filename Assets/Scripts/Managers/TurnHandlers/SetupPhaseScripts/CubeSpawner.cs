using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static CubeControl;

public class CubeSpawner : Singleton<CubeSpawner>
{
    protected override bool IsPersistent => false;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameBoard _board;

    [SerializeField] private CubeData _defaultData;
    [SerializeField] private CubeData _smallData;
    [SerializeField] private CubeData _mediumData;
    [SerializeField] private CubeData _largeData;

    public void SpawnAICubes()
    {
        List<CubeSize> sizes = new()
        {
            CubeSize.Small,
            CubeSize.Medium,
            CubeSize.Large
        };

        List<int> availableSlots = new() { 0, 1, 2 };

        foreach (var size in sizes)
        {
            CubeData data = GetDataForSize(size);
            Transform slot = _board.GetRandomAISlot(ref availableSlots);

            GameObject cubeObj = Instantiate(_cubePrefab, slot.position, Quaternion.identity);
            CubeControl cube = cubeObj.GetComponent<CubeControl>();

            cube.SetTeam(Team.Enemy);
            cube.InitializeCube(data);
            cube.SetLane((Lane)System.Array.IndexOf(_board.aiSlots, slot));
        }
    }

    public GameObject SpawnPlayerCubePreview(CubeSize size)
    {
        Debug.Log("Spawned player cube preview");
        CubeData data = GetDataForSize(size);

        GameObject cubeObj = Instantiate(_cubePrefab);
        CubeControl cube = cubeObj.GetComponent<CubeControl>();

        cube.SetTeam(Team.Player);
        cube.InitializeCube(data);

        return cubeObj;
    }

    public void PlacePlayerCube(GameObject cubeObj, Transform slot, int lane)
    {
        Debug.Log("Placed player cube in slot " + lane);
        cubeObj.transform.position = slot.position;

        CubeControl cube = cubeObj.GetComponent<CubeControl>();
        cube.SetLane((Lane)lane);

        cube.SetOriginalPosition(slot.position);
    }

    private CubeData GetDataForSize(CubeSize size)
    {
        return size switch
        {
            CubeSize.Small => _smallData,
            CubeSize.Medium => _mediumData,
            CubeSize.Large => _largeData,
            _ => _defaultData
        };
    }
}
