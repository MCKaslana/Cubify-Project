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
        if (cube == null) return;

        if (!_hasSelectedUser)
        {
            Debug.Log("Select user");

            // Toggle selection
            if (CurrentUser == cube)
                CurrentUser = null;
            else
                CurrentUser = cube;

            // If user was cleared, stay in user selection phase
            _hasSelectedUser = CurrentUser != null;
        }
        else
        {
            Debug.Log("Select target");

            // Toggle selection
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
