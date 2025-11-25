using UnityEngine;

[CreateAssetMenu(fileName = "New Note", menuName = "Game/Note")]
public class NoteData : ScriptableObject
{
        public string noteID; //Unique identifier
        public string noteTitle;
        [TextArea(5,15)]
        public string noteContent;
        public Sprite noteImage; 
        public NoteCategory category;
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
