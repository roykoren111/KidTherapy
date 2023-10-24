using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable objects/InnerTalk data", fileName = "InnerTalk data")]
public class InnerTalkData : ScriptableObject
{
    public InnerTalkWord[] Words;
}
