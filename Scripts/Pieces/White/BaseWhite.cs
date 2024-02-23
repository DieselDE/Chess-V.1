using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWhite : BasePiece
{
    private BasePiece draggedObject;
    private Tile oldTile;
    private Tile newTile;
    Vector2 difference = Vector2.zero;

    void Start(){
        
        PieceManager.Instance.PutInAllPiecesInDictionary();
    }

    void OnMouseDown(){

        if(GameManager.Instance.State != GameState.WhiteTurn)
            return;
        
        oldTile = OccupiedTile;
        
        draggedObject = this;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }

    void OnMouseDrag(){

        if(GameManager.Instance.State != GameState.WhiteTurn)
            return;

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    void OnMouseUp(){

        int temp = 0;

        if(GameManager.Instance.State != GameState.WhiteTurn)
            return;

        newTile = GridManager.Instance.GetTile(transform.position);

        if(newTile != null){

            Vector2 newTileCenter = newTile.transform.position;
            draggedObject.transform.position = new Vector2(newTileCenter.x, newTileCenter.y);

            if(newTile == oldTile){

                SoundManager.Instance.PlaySound(SoundEffect.illegalSoundEffect);
                GameManager.Instance.UpdateGameState(GameState.WhiteTurn);

                return;
            }
        }

        if(!draggedObject.IsViableMove(newTile.transform.position)){

            if(draggedObject != null){

                PutPieceBackWhite();
            }

            SoundManager.Instance.PlaySound(SoundEffect.illegalSoundEffect);
            GameManager.Instance.UpdateGameState(GameState.WhiteTurn);

            return;
        }

        draggedObject.OccupiedTile = newTile;

        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);

        foreach(Collider2D collider in colliders){

            BasePiece basePiece = collider.gameObject.GetComponent<BasePiece>();

            if(basePiece != null && basePiece.ScriptablePiece != null && basePiece.ScriptablePiece.Team == Team.Black){

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

        Vector2 tempVec = PieceManager.Instance.FindPieceVec(Name.King, Team.Black);
        Tile tempTile = GridManager.Instance.GetTile(tempVec);
        // BasePiece tempPiece = PieceManager.Instance.FindPieceBase(new Vector2(4, 7));
        // Debug.Log($"{tempTile} {tempVec}{PieceManager.Instance.IsTileUnderAttack(tempTile)} {tempPiece}");

        if(tempTile == null){
            Debug.LogWarning("King's tile not found!");
            return;
        }

        if(draggedObject.ScriptablePiece.Name == Name.Pawn && ((int)newTile.transform.position.y == 7 || (int)newTile.transform.position.y == 0)){

            Destroy(draggedObject.gameObject);
            draggedObject = PieceManager.Instance.PawnToQueen<BaseWhite>(newTile, this);
            PieceManager.Instance.PutInAllPiecesInDictionary();
        }

        if(PieceManager.Instance.IsTileUnderAttack(tempTile, Team.Black)){

            SoundManager.Instance.PlaySound(SoundEffect.moveCheckSoundEffect);

            if(PieceManager.Instance.IsWinConditionMet(Team.Black)){
                
                Debug.Log("Game Won");
                SoundManager.Instance.PlaySound(SoundEffect.gameEndSoundEffect);
                return;
            }

            Debug.Log("Black King is under Attack");
        }

        GameManager.Instance.UpdateGameState(GameState.BlackTurn);
    }

    void PutPieceBackWhite(){
        
        draggedObject.transform.position = oldTile.transform.position;
    }
}