using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
public class Resource : ScriptableObject
{
    public string ResourceName;
    public string ResourceDescription;

    public int level;
}
