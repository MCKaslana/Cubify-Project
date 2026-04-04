using System;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public CubeControl CurrentUser { get; private set; }
    public CubeControl CurrentTarget { get; private set; }

    public event Action OnSelectionChanged;

    private bool _hasSelectedUser = false;

    public void SelectCube(CubeControl cube)
    {
        if (cube == null) return;

        if (!_hasSelectedUser)
        {
            if (CurrentUser == cube)
                CurrentUser = null;
            else
                CurrentUser = cube;

            _hasSelectedUser = CurrentUser != null;
        }
        else
        {
            if (CurrentTarget == cube)
                CurrentTarget = null;
            else
                CurrentTarget = cube;
        }

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
