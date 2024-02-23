using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingB : BaseBlack
{
    public override bool IsViableMove(Vector2 newTile){

        int tempx = (int)Math.Abs(newTile.x - OccupiedTile.transform.position.x);
        int tempy = (int)Math.Abs(newTile.y - OccupiedTile.transform.position.y);

        if(tempx <= 1 && tempy <= 1){

            Tile tempTile = GridManager.Instance.GetTile(newTile);

            if(!PieceManager.Instance.IsTileOccupied(newTile) && !PieceManager.Instance.IsTileUnderAttack(tempTile, Team.Black)){

                return true;
            }

            Tile tile = GridManager.Instance.GetPieceSpawnTile((int)newTile.x, (int)newTile.y);
            BasePiece piece = tile.GetPiece();

            if(piece.ScriptablePiece.Team != Team.Black){
                    
                return true;
            }
        }

        else if(tempx == 2 && tempy == 0){

            int directionX = newTile.x > OccupiedTile.transform.position.x ? 1 : -1;

            for(int i = 1; i <= tempx; i++){

                int checkX = (int)OccupiedTile.transform.position.x + i * directionX;
                Tile checkTile = GridManager.Instance.GetTile(new Vector2(checkX, (int)OccupiedTile.transform.position.y));

                if(PieceManager.Instance.IsTileOccupied(checkTile.transform.position) || PieceManager.Instance.IsTileUnderAttack(checkTile, Team.Black)){
                    
                    return false;
                }
            }

            if(PieceManager.Instance.IsCastleConditionMet(Team.Black, newTile)){

                Vector2 rookStartPos = (newTile.x > 4) ? new Vector2(7, (int)OccupiedTile.transform.position.y) : new Vector2(0, (int)OccupiedTile.transform.position.y);
                Vector2 rookEndPos = Player ? ((newTile.x > 4) ? new Vector2(5, (int)OccupiedTile.transform.position.y) : new Vector2(3, (int)OccupiedTile.transform.position.y))
                                            : ((newTile.x > 4) ? new Vector2(4, (int)OccupiedTile.transform.position.y) : new Vector2(2, (int)OccupiedTile.transform.position.y));

                Tile oldRookTile = GridManager.Instance.GetTile(rookStartPos);
                Tile newRookTile = GridManager.Instance.GetTile(rookEndPos);

                BasePiece tempPiece = PieceManager.Instance.FindPieceBase(rookStartPos);
                tempPiece.transform.position = (Vector2)rookEndPos;
                tempPiece.OccupiedTile = newRookTile;
                PieceManager.Instance.MovePiece(rookEndPos, tempPiece);
                oldRookTile.SetPiece(null);
                newRookTile.SetPiece(tempPiece);

                return true;
            }
        }

        return false;
    }
}
