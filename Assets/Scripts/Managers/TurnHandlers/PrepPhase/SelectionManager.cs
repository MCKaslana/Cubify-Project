using System;

public class SelectionManager : Singleton<SelectionManager>
{
    public CubeControl SelectedCube { get; private set; }
    public CubeControl TargetCube { get; private set; }

    public event Action<CubeControl> OnSelectedCubeChanged;
    public event Action<CubeControl> OnTargetCubeChanged;

    public void SelectCube(CubeControl cube)
    {
        if (SelectedCube == cube)
            return;

        SelectedCube = cube;
        OnSelectedCubeChanged?.Invoke(cube);
    }

    public void SelectTarget(CubeControl cube)
    {
        if (TargetCube == cube)
            return;
        TargetCube = cube;
        OnTargetCubeChanged?.Invoke(cube);
    }
}
