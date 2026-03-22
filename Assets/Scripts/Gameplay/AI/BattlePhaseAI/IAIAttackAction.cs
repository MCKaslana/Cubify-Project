using System.Collections;
using UnityEngine;

public interface IAIAttackAction
{
    bool CanExecute();
    IEnumerator Execute();
}
