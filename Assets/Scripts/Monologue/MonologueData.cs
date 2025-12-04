using UnityEngine;

[CreateAssetMenu(fileName = "New Monologue", menuName = "The Final Element/Monologue Data")]
public class MonologueData : ScriptableObject
{
    [Header("Monologue Content")]
    [TextArea(3, 10)]
    public string monologueText;
    
    [Header("Display Settings")]
    [Tooltip("How fast the text types out (characters per second). 0 = instant")]
    public float typingSpeed = 30f;
    
    [Tooltip("How long to display the text after typing finishes (seconds)")]
    public float displayDuration = 5f;
    
    [Tooltip("Can the player skip the monologue by pressing a key?")]
    public bool canSkip = true;
}