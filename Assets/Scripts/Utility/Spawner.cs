using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    protected int spawnChanceTotal = 100;

    protected int GetSpawn(int[] spawnChances)
    {
        int ticket = Random.Range(0, spawnChanceTotal);
        Vector2 winningRange = Vector2.zero;

        for (int i = 0; i < spawnChances.Length; i++)
        {
            winningRange = new Vector2(winningRange.y, winningRange.y + spawnChances[i]);

            if (ticket >= winningRange.x && ticket <= winningRange.y)
            {
                return i;
            }
        }

        return 0;
    }
}
