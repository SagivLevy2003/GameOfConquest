using Unity.VisualScripting;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] private Texture2D _baseCursorSprite;

    private void Start()
    {
        Cursor.SetCursor(_baseCursorSprite, Vector2.zero, CursorMode.Auto);
    }

    public void OnCommandCandidateChanged(CommandCandidate candidate)
    {
        if (candidate != null && candidate.VisualData != null && candidate.VisualData.CursorTexture != null)
        {
            Cursor.SetCursor(candidate.VisualData.CursorTexture, Vector2.zero, CursorMode.Auto);
            return;
        }

        Cursor.SetCursor(_baseCursorSprite, Vector2.zero, CursorMode.Auto);
    }
}