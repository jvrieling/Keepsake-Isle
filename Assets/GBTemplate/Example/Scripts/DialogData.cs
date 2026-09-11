using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GBTemplate/DialogData")]
public class DialogData : ScriptableObject
{
    public List<DialogSentence> Sentences;
}

[System.Serializable]
public class DialogSentence
{
    [TextArea(3, 10)]
    public string Text;
    public AudioClip TypeSound;
}
