using System;
using UnityEngine;
using UnityEngine.Rendering;

public class SelectionManager : Singleton<SelectionManager>
{
    public CubeControl CurrentUser { get; private set; }
    public CubeControl CurrentTarget { get; private set; }

    public event Action OnSelectionChanged;

    private bool _hasSelectedUser = false;

    public void SelectCube(CubeControl cube)
    {
        cube = CurrentUser == cube ? null : cube;

        if (_hasSelectedUser)
        {
            cube = CurrentTarget == cube ? null : cube;
        }

        _hasSelectedUser = true;

        OnSelectionChanged?.Invoke();
    }

    public void ResetSelection()
    {
        CurrentUser = null;
        CurrentTarget = null;
        OnSelectionChanged?.Invoke();

        _hasSelectedUser = false;
    }
}
