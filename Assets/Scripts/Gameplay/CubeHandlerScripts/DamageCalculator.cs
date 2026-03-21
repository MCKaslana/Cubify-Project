using UnityEngine;

public static class DamageCalculator
{
    public static int CalculateDamage(CubeControl attacker, CubeControl defender)
    {
        int sizeDiff = (int)attacker.GetCubeData().cubeSize -
                   (int)defender.GetCubeData().cubeSize;

        if (sizeDiff > 0)
            return 2; // bigger

        if (sizeDiff < 0)
            return 0; // smaller

        return 1; // equal
    }
}
