using System.Collections.Generic;
using UnityEngine;

public class PlayerUnlockManager : MonoBehaviour
{
    public static PlayerUnlockManager instance;

    public enum Unlockable { Sprint, DoubleJump, Dash } // Add other unlockables here

    private HashSet<Unlockable> unlockedAbilities = new HashSet<Unlockable>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockAbility(Unlockable ability)
    {
        if (!unlockedAbilities.Contains(ability))
        {
            unlockedAbilities.Add(ability);
            Debug.Log(ability + " unlocked!");
        }
    }

    public bool IsAbilityUnlocked(Unlockable ability)
    {
        return unlockedAbilities.Contains(ability);
    }
}
