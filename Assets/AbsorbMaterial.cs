using UnityEngine;
using System.Collections;

public enum AbrsorbType
{
    nessuno,
    ossigeno,
    silice,
    titanio,
    amnios,
    solvente1,
    solvente2
}

public class AbsorbMaterial : MonoBehaviour
{
    public AbrsorbType material;
}
