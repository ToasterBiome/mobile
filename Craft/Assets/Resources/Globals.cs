using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Globals")]
public class Globals : ScriptableObject
{
    private static Globals instance;
    public static Globals Instance { get { return instance; } }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        instance = Resources.LoadAll<Globals>("/")[0];
    }

    //Do stuff below this line

    public Color normalHitColor;
    public Color criticalHitColor;
    public Color missHitColor;
    public Gradient healthGradient;

    public Sprite lightningIcon;
    public RawImage lightningIconI;


}
