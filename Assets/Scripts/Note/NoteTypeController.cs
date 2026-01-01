//根据NoteType显示对应的外观
using UnityEngine;

public class NoteTypeController : MonoBehaviour
{
    [SerializeField] private GameObject tap;
    [SerializeField] private GameObject flick;
    [SerializeField] private GameObject hold;
    public void Init(NoteData note)
    {
        tap.SetActive(note.type == NoteType.Tap);
        flick.SetActive(note.type == NoteType.Flick);
        hold.SetActive(note.type == NoteType.Hold);
    }
}
