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

    private List<CubeControl> _playerCubes = new();
    private List<CubeControl> _aiCubes = new();

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
            cube.SetOriginalPosition(slot.position);
            cube.InitializeCube(data);
            cube.SetLane((Lane)System.Array.IndexOf(_board.aiSlots, slot));

            _aiCubes.Add(cube);
        }
    }

    public GameObject SpawnPlayerCubePreview(CubeSize size)
    {
        CubeData data = GetDataForSize(size);

        GameObject cubeObj = Instantiate(_cubePrefab);
        CubeControl cube = cubeObj.GetComponent<CubeControl>();

        cube.SetTeam(Team.Player);
        cube.InitializeCube(data);

        return cubeObj;
    }

    public void PlacePlayerCube(GameObject cubeObj, Transform slot, int lane)
    {
        cubeObj.transform.position = slot.position;

        CubeControl cube = cubeObj.GetComponent<CubeControl>();
        cube.SetLane((Lane)lane);

        cube.SetOriginalPosition(slot.position);

        _playerCubes.Add(cube);
    }

    public List<CubeControl> GetAllCubes()
    {
        List<CubeControl> allCubes = new();
        allCubes.AddRange(_playerCubes);
        allCubes.AddRange(_aiCubes);
        return allCubes;
    }

    public List<CubeControl> GetAllSpawnedCubes(Team team)
    {
        List<CubeControl> allCubes = new();

        List<CubeControl> sortedPlayerCubes = new();
        List<CubeControl> sortedAICubes = new();

        allCubes.AddRange(_aiCubes);
        allCubes.AddRange(_playerCubes);

        foreach (var cube in allCubes)
        {
            if (!cube.IsSelectable) continue;

            if (cube.GetTeam() == Team.Player)
                sortedPlayerCubes.Add(cube);
            else if (cube.GetTeam() == Team.Enemy)
                sortedAICubes.Add(cube);
        }

        if (team == Team.Player)
            return sortedPlayerCubes;
        else
            return sortedAICubes;
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
