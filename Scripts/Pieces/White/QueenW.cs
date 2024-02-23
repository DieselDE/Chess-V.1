using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenW : BaseWhite
{
    public override bool IsViableMove(Vector2 newTile){

        int tempx = (int)Math.Abs(newTile.x - OccupiedTile.transform.position.x);
        int tempy = (int)Math.Abs(newTile.y - OccupiedTile.transform.position.y);

        if(newTile.x == OccupiedTile.transform.position.x || newTile.y == OccupiedTile.transform.position.y || tempx == tempy){

            int directionX = newTile.x > OccupiedTile.transform.position.x ? 1 : (newTile.x < OccupiedTile.transform.position.x ? -1 : 0);
            int directionY = newTile.y > OccupiedTile.transform.position.y ? 1 : (newTile.y < OccupiedTile.transform.position.y ? -1 : 0);

            for (int i = 1; i < Mathf.Max(tempx, tempy); i++){

                int checkX = (int)OccupiedTile.transform.position.x + i * directionX;
                int checkY = (int)OccupiedTile.transform.position.y + i * directionY;

                Vector2 checkTile = new Vector2(checkX, checkY);

                if (PieceManager.Instance.IsTileOccupied(checkTile)){
                    
                    return false;
                }
            }

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
