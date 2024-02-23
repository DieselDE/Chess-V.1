using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnB : BaseBlack
{  
    public override bool IsViableMove(Vector2 newTile){

        int direction = PieceManager.Player ? -1 : 1;

        int tempx = (int)Math.Abs(newTile.x - OccupiedTile.transform.position.x);
        int tempy = direction * (int)(newTile.y - OccupiedTile.transform.position.y);

        if(tempx == 0 && tempy == 1){
            
            if(!PieceManager.Instance.IsTileOccupied(newTile)){
                
                PieceManager.Instance.SetPawnMoved(1);
                return true;
            }
        }
        else if(tempx == 0 && tempy == 2 && ((int)OccupiedTile.transform.position.y == 6 || (int)OccupiedTile.transform.position.y == 1)){

            if(!PieceManager.Instance.IsTileOccupied(newTile) && !PieceManager.Instance.IsTileOccupied(new Vector2(newTile.x, newTile.y - direction))){
                
                PieceManager.Instance.SetPawnMoved(2);
                return true;
            }
        }
        else if(tempx == 1 && tempy == 1){

            if(PieceManager.Instance.IsTileOccupied(newTile)){
                
                return true;
            }

            else if(!PieceManager.Instance.IsTileOccupied(newTile) && PieceManager.Instance.PawnMoved == 2){
                
                BasePiece piece = PieceManager.Instance.FindPieceBase(PieceManager.Instance.GetLastMovedPieceCoord());

                if(piece.ScriptablePiece.Name == Name.Pawn && PieceManager.Instance.GetLastMovedPieceCoord() == new Vector2(newTile.x, (int)OccupiedTile.transform.position.y)){
                    
                    PieceManager.Instance.SetLastMovedPiece(newTile);
                    PieceManager.Instance.SetPawnMoved(1);

                    Tile PawnTile = GridManager.Instance.GetTile(new Vector2(newTile.x, (int)OccupiedTile.transform.position.y));

                    PieceManager.Instance.RemovePiece(new Vector2(newTile.x, (int)OccupiedTile.transform.position.y));
                    PawnTile.SetPiece(null);
                    Destroy(piece.gameObject);
                    
                    return true;
                }
            }
        }
        
        return false;
    }
}
