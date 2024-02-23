using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Color _mainColor, _offColor;
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] GameObject _highlight;

    private BasePiece piece;

    public void Init(bool isOffset){

        _renderer.color = isOffset ? _mainColor : _offColor;
    }

    public void SetPiece(BasePiece newPiece){

        piece = newPiece;
    }

    public BasePiece GetPiece(){

        return piece;
    }

    void OnMouseEnter(){

        _highlight.SetActive(true);
    }

    void OnMouseExit(){
        _highlight.SetActive(false);
    }
}