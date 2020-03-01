using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

public class UniversalFunctions : MonoBehaviour
{
    public static void SetGlobalScale(Transform trans, Vector3 newScale)
    {
        print(trans.name+ "'s scale was modified");
        Transform parent = trans.parent;
        trans.parent = null;

        trans.localScale = newScale;

        trans.SetParent(parent);
    }
}
