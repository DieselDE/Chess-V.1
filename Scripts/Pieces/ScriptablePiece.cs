using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Piece", menuName = "Scriptable Piece")]
public class ScriptablePiece : ScriptableObject
{
    public Team Team;
    public Name Name;
    public BasePiece PiecePrefab;
    
}

public enum Team {
    White = 0,
    Black = 1
}

public enum Name {
    Pawn = 0,
    Knight = 1,
    Bishop = 2,
    Rook = 3,
    Queen = 4,
    King = 5
}