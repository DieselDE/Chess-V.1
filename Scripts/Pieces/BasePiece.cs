using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePiece : PieceManager
{
    public Tile OccupiedTile;
    public ScriptablePiece ScriptablePiece;

    public void SetScriptablePiece(ScriptablePiece scriptablePiece){

        ScriptablePiece = scriptablePiece;
    }

    public virtual bool IsViableMove(Vector2 newTile){

        return false;
    }
}