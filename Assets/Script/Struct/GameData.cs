using UnityEngine;
public struct GameData
{
    private int canDeletePuyoValue;
    private Vector2 _popPos;
    private Vector2 _nextPos;
    private FieldDataScript _fieldDataScript;

    public int CanDeletePuyoValue { get { return canDeletePuyoValue; } }

    public Vector2 NextPos { get { return _nextPos; } }

    public Vector2 PopPos { get { return _popPos; } }

    public FieldDataScript FieldDataScript { get { return _fieldDataScript; } }

    public GameData(int canDeletePuyoValue,Vector2 nextPos,Vector2 popPos,FieldDataScript fieldDataScript)
    {
        this._nextPos = nextPos;
        this._popPos = popPos;
        this._fieldDataScript = fieldDataScript;
        this.canDeletePuyoValue = canDeletePuyoValue;
    }
}
