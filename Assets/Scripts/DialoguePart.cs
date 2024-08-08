using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "DialogueScriptableObject", menuName = "ScriptableObjects/DialogueScriptableObject", order = 1)]
public class DialoguePart : ScriptableObject
{
    public string npcName;
    public string dialogueSentence;
    public TMP_FontAsset NPCfontAsset;
    public TMP_FontAsset dialogueFontAsset;
    public bool isScary;
    public bool isChill;
    public int bgImageIndex;
    public string SFXeventName;
}