using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CubeSelectionButton : MonoBehaviour
{
    public CubeControl cube;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        var redirectAbility = AbilityManager.Instance.GetPendingRedirect();

        if (redirectAbility != null)
        {
            var allCubes = CubeSpawner.Instance.GetAllCubes();
            List<CubeControl> playerCubes = new List<CubeControl>();

            if (allCubes.Count == 0) return;

            foreach (var c in allCubes)
            {
                if (c.GetTeam() == Team.Player)
                {
                    playerCubes.Add(c);
                }
            }

            var user = playerCubes[0];

            if (!redirectAbility.CanExecute(user, cube))
            {
                Debug.Log("Cannot redirect to this cube");
                return;
            }

            StartCoroutine(redirectAbility.Execute(user, cube));

            AbilityManager.Instance.ClearRedirectMode();
            return;
        }

        Debug.Log("Selected Cube");
        SelectionManager.Instance.SelectCube(cube);
    }
}
