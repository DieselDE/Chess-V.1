using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBlack : BasePiece
{
    private BasePiece draggedObject;
    private Tile oldTile;
    private Tile newTile;
    Vector2 difference = Vector2.zero;

    void OnMouseDown(){

        if(GameManager.Instance.State != GameState.BlackTurn)
            return;

        oldTile = OccupiedTile;
        
        draggedObject = this;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }

    void OnMouseDrag(){

        if(GameManager.Instance.State != GameState.BlackTurn)
            return;

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    void OnMouseUp(){

        int temp = 0;

        if(GameManager.Instance.State != GameState.BlackTurn)
            return;

        newTile = GridManager.Instance.GetTile(transform.position);

        if(newTile != null){

            Vector2 newTileCenter = newTile.transform.position;
            draggedObject.transform.position = new Vector2(newTileCenter.x, newTileCenter.y);

            if(newTile == oldTile){

                SoundManager.Instance.PlaySound(SoundEffect.illegalSoundEffect);
                GameManager.Instance.UpdateGameState(GameState.BlackTurn);

                return;
            }
        }

        if(!draggedObject.IsViableMove(newTile.transform.position) || PieceManager.Instance.IsKingUnderAttack(draggedObject, oldTile, Team.Black)){
            
            PutPieceBackBlack();
            SoundManager.Instance.PlaySound(SoundEffect.illegalSoundEffect);
            GameManager.Instance.UpdateGameState(GameState.BlackTurn);

            return;
        }

        draggedObject.OccupiedTile = newTile;

        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

        foreach(Collider2D collider in colliders){

            BasePiece basePiece = collider.gameObject.GetComponent<BasePiece>();
            
            if(basePiece != null && basePiece.ScriptablePiece != null && basePiece.ScriptablePiece.Team == Team.White){

                if(collider.gameObject != draggedObject){

                    temp++;
                    Destroy(collider.gameObject);
                }
            }
        }

        if(temp > 0){

            SoundManager.Instance.PlaySound(SoundEffect.captureSoundEffect);
        }
        else{

            SoundManager.Instance.PlaySound(SoundEffect.moveSoundEffect);
        }
        
        PieceManager.Instance.SetLastMovedPiece(draggedObject.transform.position);
        
        if(draggedObject.ScriptablePiece.Name != Name.Pawn){

            PieceManager.Instance.SetPawnMoved(0);
        }

        PieceManager.Instance.MovePiece(newTile.transform.position, draggedObject);
        oldTile.SetPiece(null);
        newTile.SetPiece(draggedObject);
        
        Vector2 kingVec = PieceManager.Instance.FindPieceVec(Name.King, Team.White);
        Tile kingTile = GridManager.Instance.GetTile(kingVec);
        // Debug.Log($"{kingTile} {kingVec}");

        if (draggedObject.ScriptablePiece.Name == Name.Pawn && ((int)newTile.transform.position.y == 7 || (int)newTile.transform.position.y == 0)){

            Destroy(draggedObject.gameObject);
            draggedObject = PieceManager.Instance.PawnToQueen<BaseBlack>(newTile, this);
            PieceManager.Instance.PutInAllPiecesInDictionary();
        }


        if(PieceManager.Instance.IsTileUnderAttack(kingTile, Team.White)){

            SoundManager.Instance.PlaySound(SoundEffect.moveCheckSoundEffect);

            if(PieceManager.Instance.IsWinConditionMet(Team.White)){
                
                Debug.Log("Game Won");
                SoundManager.Instance.PlaySound(SoundEffect.gameEndSoundEffect);
                return;
            }

            Debug.Log("White King is under Attack");
        }

        GameManager.Instance.UpdateGameState(GameState.WhiteTurn);
    }

    void PutPieceBackBlack(){
        
        draggedObject.transform.position = oldTile.transform.position;
    }
}