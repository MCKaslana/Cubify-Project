using System;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    [SerializeField] private GameObject SelectCubeMenu;
    [SerializeField] private GameObject SelectTargetMenu;

    public CubeControl SelectedCube { get; private set; }
    public CubeControl TargetCube { get; private set; }

    public event Action<CubeControl> OnSelectedCubeChanged;
    public event Action<CubeControl> OnTargetCubeChanged;

    public override void Awake()
    {
        base.Awake();
        SelectCubeMenu.SetActive(false);
        SelectTargetMenu.SetActive(false);
    }

    public void ToggleSelectCubeMenu(bool isActive)
    {
        SelectCubeMenu.SetActive(isActive);
    }

    public void ToggleSelectTargetMenu(bool isActive)
    {
        SelectTargetMenu.SetActive(isActive);
    }

    public void SelectCube(CubeControl cube)
    {
        if (cube == null) return;
        if (SelectedCube == cube)
            return;

        SelectedCube = cube;
        OnSelectedCubeChanged?.Invoke(cube);

        ToggleSelectCubeMenu(false);
        ToggleSelectTargetMenu(true);
    }

    public void SelectTarget(CubeControl cube)
    {
        if (TargetCube == cube)
            return;
        TargetCube = cube;
        OnTargetCubeChanged?.Invoke(cube);

        ToggleSelectTargetMenu(false);
    }
}
