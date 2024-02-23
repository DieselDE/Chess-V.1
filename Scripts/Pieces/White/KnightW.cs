using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightW : BaseWhite
{
    public override bool IsViableMove(Vector2 newTile){

        int tempx = (int)Math.Abs(newTile.x - OccupiedTile.transform.position.x);
        int tempy = (int)Math.Abs(newTile.y - OccupiedTile.transform.position.y);

        if(tempx == 1 && tempy == 2 || tempx == 2 && tempy == 1){
            
            if(!PieceManager.Instance.IsTileOccupied(newTile)){

                return true;
            }

            Tile tile = GridManager.Instance.GetPieceSpawnTile((int)newTile.x, (int)newTile.y);
            BasePiece piece = tile.GetPiece();

            if(piece.ScriptablePiece.Team != Team.White){
                    
                return true;
            }
        }

        return false;
    }
}
