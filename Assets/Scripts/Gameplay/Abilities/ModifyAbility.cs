using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Combat/Abilities/Modify")]
public class ModifyAbility : AbilityCard
{
    public float scaleAmount = 1.5f;
    public Color colorChange = Color.green;

    public override bool CanExecute(CubeControl user, CubeControl target)
    {
        return true;
    }

    public override IEnumerator Execute(CubeControl user, CubeControl target)
    {
        CubeControl cubeToModify = user;

        Debug.Log($"{user.name} modifies itself!");

        yield return cubeToModify.MoveTo(cubeToModify.transform.position + Vector3.forward * 0.5f);

        user.PlaySound(3);

        cubeToModify.Modify(scaleAmount, colorChange);

        yield return new WaitForSeconds(0.5f);
        yield return cubeToModify.ReturnToOriginalPosition();
    }
}