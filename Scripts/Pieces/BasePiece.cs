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

    // All changes for formating BaseWhite/BaseBlack below

    private BasePiece draggedObject;
    private Tile oldTile;
    private Tile newTile;
    Vector2 difference = Vector2.zero;

    void Start(){

        PieceManager.Instance.PutInAllPiecesInDictionary();
    }

    void PutPieceBack(){
        
        draggedObject.transform.position = oldTile.transform.position;
    }

    void OnMouseDown(){

        draggedObject = this;

        if((GameManager.Instance.State == GameState.WhiteTurn && draggedObject.ScriptablePiece.Team != Team.White) || 
            (GameManager.Instance.State == GameState.BlackTurn && draggedObject.ScriptablePiece.Team != Team.Black)){

            return;
        }

        oldTile = OccupiedTile;
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
    }

    void OnMouseDrag(){

        if((GameManager.Instance.State == GameState.WhiteTurn && draggedObject.ScriptablePiece.Team != Team.White) || 
            (GameManager.Instance.State == GameState.BlackTurn && draggedObject.ScriptablePiece.Team != Team.Black)){

            return;
        }

        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
    }

    void OnMouseUp(){

        int temp = 0; // for the collider destruction

        if((GameManager.Instance.State == GameState.WhiteTurn && draggedObject.ScriptablePiece.Team != Team.White) || 
            (GameManager.Instance.State == GameState.BlackTurn && draggedObject.ScriptablePiece.Team != Team.Black)){

            return;
        }

        newTile = GridManager.Instance.GetTile(transform.position); // checking newTile for UIE
        if(newTile != null){

            Vector2 newTileCenter = newTile.transform.position;
            draggedObject.transform.position = new Vector2(newTileCenter.x, newTileCenter.y);
            
            if(newTile == oldTile){

                SoundManager.Instance.PlaySound(SoundEffect.illegalSoundEffect);
                GameManager.Instance.UpdateGameState(GameManager.Instance.State); // check later if needed

                return;
            }
        }

        if(!draggedObject.IsViableMove(newTile.transform.position) || // check if move is viable
            PieceManager.Instance.IsKingUnderAttack(draggedObject, oldTile, Team.White)){
            
            PutPieceBack();
            SoundManager.Instance.PlaySound(SoundEffect.illegalSoundEffect);
            GameManager.Instance.UpdateGameState(GameManager.Instance.State); // check later if needed

            return;
        }

        draggedObject.OccupiedTile = newTile;
/*
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
        */

        PieceManager.Instance.SetLastMovedPiece(draggedObject.transform.position); // Logik für en passant
        if(draggedObject.ScriptablePiece.Name != Name.Pawn){

            PieceManager.Instance.SetPawnMoved(0);
        }


    }
}