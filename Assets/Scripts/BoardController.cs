using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardController : MonoBehaviour {
    private BoardData boardData;
    private AIOpponent ai;
    private bool playerPlaysWhite = true;

    [SerializeField] private GameObject whiteCellPrefab;
    [SerializeField] private GameObject blackCellPrefab;
    [SerializeField] private Figures whiteFigures;
    [SerializeField] private Figures blackFigures;
    [SerializeField] private GameObject highlightCellPrefab;
    private GameObject[] hightlightCells;

    private GameObject[] gameFigures;
    private Vector2Int[] savedPossibleMoves;

    [Header("Debug")]
    [SerializeField] private FigureType figureBoardToShow;

    private void Start() {
        boardData = new BoardData();
        ai = new AIOpponent(boardData, 4);
        CreateBoard();
        PopulateBoard();
    }

    private void CreateBoard() {
        Transform boardParent = new GameObject("Board Holder").transform;
        boardParent.parent = transform;
        int index = 0;
        for(int r = 0; r < boardData.BoardSize; r++) {
            index++;
            for(int c = 0; c < boardData.BoardSize; c++) {
                Instantiate(index % 2 == 0 ? whiteCellPrefab : blackCellPrefab, new Vector3(c, r, 1), Quaternion.identity, boardParent);
                index++;
            }
        }
    }

    private void PopulateBoard() {
        gameFigures = new GameObject[boardData.BoardSize * 4];
        int index = 0;

        Transform figureParent = new GameObject("Figure Holder").transform;
        figureParent.parent = transform;

        hightlightCells = new GameObject[boardData.BoardSizeSqr];
        Transform highlightParent = new GameObject("Highlight Holder").transform;
        highlightParent.parent = transform;
        for(int r = 0; r < boardData.BoardSize; r++) {
            for(int c = 0; c < boardData.BoardSize; c++) {
                Vector3 pos = new Vector3(c, r);

                //TODO: Check if I need to pass any data about highlight to BoardData
                hightlightCells[r * boardData.BoardSize + c] = Instantiate(highlightCellPrefab, pos, Quaternion.identity, highlightParent);
                hightlightCells[r * boardData.BoardSize + c].SetActive(false);

                if(boardData.IsCellOccupied(FigureType.White, c, r)) {
                    if(boardData.IsCellOccupied(FigureType.King, c, r)) {
                        gameFigures[index] = Instantiate(whiteFigures.king, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "W_King";
                        index++;
                        continue;
                    } else if(boardData.IsCellOccupied(FigureType.Queen, c, r)) {
                        gameFigures[index] = Instantiate(whiteFigures.queen, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "W_Queen";
                        index++;
                        continue;
                    } else if(boardData.IsCellOccupied(FigureType.Rook, c, r)) {
                        gameFigures[index] = Instantiate(whiteFigures.rook, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "W_Rook";
                        index++;
                        continue;
                    } else if(boardData.IsCellOccupied(FigureType.Bishop, c, r)) {
                        gameFigures[index] = Instantiate(whiteFigures.bishop, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "W_Bishop"; ;
                        index++;
                        continue;
                    } else if(boardData.IsCellOccupied(FigureType.Knight, c, r)) {
                        gameFigures[index] = Instantiate(whiteFigures.knight, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "W_Knight";
                        index++;
                        continue;
                    } else if(boardData.IsCellOccupied(FigureType.Pawn, c, r)) {
                        gameFigures[index] = Instantiate(whiteFigures.pawn, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "W_Pawn";
                        index++;
                        continue;
                    }
                } else if(boardData.IsCellOccupied(FigureType.Black, c, r)) {
                    if(boardData.IsCellOccupied(FigureType.King, c, r)) {
                        gameFigures[index] = Instantiate(blackFigures.king, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "B_King";
                        index++;
                    } else if(boardData.IsCellOccupied(FigureType.Queen, c, r)) {
                        gameFigures[index] = Instantiate(blackFigures.queen, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "B_Queen";
                        index++;
                    } else if(boardData.IsCellOccupied(FigureType.Rook, c, r)) {
                        gameFigures[index] = Instantiate(blackFigures.rook, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "B_Rook";
                        index++;
                    } else if(boardData.IsCellOccupied(FigureType.Bishop, c, r)) {
                        gameFigures[index] = Instantiate(blackFigures.bishop, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "B_Bishop"; ;
                        index++;
                    } else if(boardData.IsCellOccupied(FigureType.Knight, c, r)) {
                        gameFigures[index] = Instantiate(blackFigures.knight, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "B_Knight";
                        index++;
                    } else if(boardData.IsCellOccupied(FigureType.Pawn, c, r)) {
                        gameFigures[index] = Instantiate(blackFigures.pawn, pos, Quaternion.identity, figureParent);
                        gameFigures[index].name = "B_Pawn";
                        index++;
                    }
                }
            }
        }
    }

    public void AIMove() {
        ai.CalculateNextMove();
        MoveFigure(ai.GetChessPieceToMove(), ai.GetBestMove());
    }

    // TODO: Fix problem with finding chess piece by it's position
    public void MoveFigure(Vector3 oldPos, Vector2Int newPos) {
        boardData.MoveFigure(new Vector2Int((int)oldPos.x, (int)oldPos.y), newPos);

        Vector3 pos = new Vector3(newPos.x, newPos.y, oldPos.z);
        for(int i = 0; i < gameFigures.Length; i++) {
            if(gameFigures[i].transform.position == pos) {
                gameFigures[i].SetActive(false);
            }
        }

        for(int i = 0; i < gameFigures.Length; i++) {
            if(gameFigures[i].transform.position == oldPos) {
                gameFigures[i].transform.position = pos;
            }
        }
        UnhighlightPreviousMoves();
    }

    public void MoveFigure(Vector2Int oldPos, Vector2Int newPos) {
        MoveFigure(new Vector3(oldPos.x, oldPos.y), newPos);
    }

    public void HighlightPossibleMoves(Vector2Int pos) {
        UnhighlightPreviousMoves();
        FigureType figureType = boardData.GetFigureType(pos.x, pos.y);
        savedPossibleMoves = boardData.GetPossibleMoves(figureType, pos.x, pos.y);
        for(int i = 0; i < savedPossibleMoves.Length; i++) {
            HighlightCell(savedPossibleMoves[i]);
        }
    }

    private void HighlightCell(Vector2Int pos) {
        int index = pos.y * boardData.BoardSize + pos.x;
        hightlightCells[index].SetActive(true);
        boardData.HighlightCell(index);
    }

    private void UnhighlightPreviousMoves() {
        for(int i = 0; i < hightlightCells.Length; i++) {
            if(boardData.IsCellHighlighted(i)) {
                hightlightCells[i].SetActive(false);
                boardData.UnhighlightCell(i);
            }
        }
    }

    public bool IsLegalMove(Vector2Int pos) {
        if(boardData.IsWithinGameBoard(pos)) {
            for(int i = 0; i < savedPossibleMoves.Length; i++) {
                if(pos == savedPossibleMoves[i]) {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsCellOccupiedWithFiguresOfPlayerColor(Vector2Int pos) {
        return boardData.IsCellOccupied(playerPlaysWhite ? FigureType.White : FigureType.Black, pos.x, pos.y);
    }

    private void OnGUI() {
        if(figureBoardToShow == FigureType.Empty) {
            return;
        }

        long boardToShow = 0L;
        switch(figureBoardToShow) {
            case FigureType.White:
                boardToShow = boardData.WhiteFiguresBoard;
                break;
            case FigureType.Black:
                boardToShow = boardData.BlackFiguresBoard;
                break;
            case FigureType.Pawn:
                boardToShow = boardData.PawnsBoard;
                break;
            case FigureType.Knight:
                boardToShow = boardData.KnightsBoard;
                break;
            case FigureType.Bishop:
                boardToShow = boardData.BishopsBoard;
                break;
            case FigureType.Rook:
                boardToShow = boardData.RooksBoard;
                break;
            case FigureType.Queen:
                boardToShow = boardData.QueensBoard;
                break;
            case FigureType.King:
                boardToShow = boardData.KingsBoard;
                break;
            case FigureType.All:
                boardToShow = boardData.MainBoard;
                break;
            default:
                print("Check OnGUI. It is broken in switch.");
                break;
        }

        string toShow = Convert.ToString(boardToShow, 2).PadLeft(64, '0');
        int index = 0;
        for(int i = 1; i <= boardData.BoardSize; i++) {
            toShow = toShow.Insert(boardData.BoardSize * i + index, "\n");
            index++;
        }
        GUI.BeginGroup(new Rect(10, 10, 100, 200));
        GUI.Box(new Rect(0, 0, 60, 125), "");
        GUI.Label(new Rect(0, 0, 60, 125), toShow);
        GUI.EndGroup();
    }
}

[Serializable]
struct Figures {
    public GameObject king;
    public GameObject queen;
    public GameObject rook;
    public GameObject bishop;
    public GameObject knight;
    public GameObject pawn;
}