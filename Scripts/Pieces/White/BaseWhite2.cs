using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWhite2 : BasePiece
{
    void Start(){

        PieceManager.Instance.PutInAllPiecesInDictionary();
    }
}
