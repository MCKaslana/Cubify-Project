using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using static CubeControl;
using UnityEngine.Splines.ExtrusionShapes;

public class CubeSpawner : Singleton<CubeSpawner>
{
    protected override bool IsPersistent => false;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameBoard _board;

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
            Transform slot = _board.GetRandomAISlot(ref availableSlots);

            GameObject cubeObj = Instantiate(_cubePrefab, slot.position, Quaternion.identity);
            CubeControl cube = cubeObj.GetComponent<CubeControl>();

            cube.SetTeam(Team.Enemy);
            cube.SetCubeSize(size);
            cube.SetLane((Lane)System.Array.IndexOf(_board.aiSlots, slot));
        }
    }

    public GameObject SpawnPlayerCubePreview(CubeSize size)
    {
        GameObject cubeObj = Instantiate(_cubePrefab);
        CubeControl cube = cubeObj.GetComponent<CubeControl>();

        cube.SetTeam(Team.Player);
        cube.SetCubeSize(size);

        return cubeObj;
    }

    public void PlacePlayerCube(GameObject cubeObj, Transform slot, int lane)
    {
        cubeObj.transform.position = slot.position;

        CubeControl cube = cubeObj.GetComponent<CubeControl>();
        cube.SetLane((Lane)lane);
    }
}
