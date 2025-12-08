using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Game/Note")]
public class NoteData : ScriptableObject
{
    public string noteID;
    public string noteTitle;

    [TextArea(5,15)]
    public string noteContent;

    public Sprite noteImage;
    public NoteCategory category;

    [Header("Optional Inner Dialogue After Closing")]
    [TextArea(2,5)]
    public string innerDialogueAfterClose;   // <--- NEW FIELD
}

public enum NoteCategory
{
    ResearchLog,
    Observation,
    ExperimentData,
    PersonalDiary,
    Warning,
    Map
}
