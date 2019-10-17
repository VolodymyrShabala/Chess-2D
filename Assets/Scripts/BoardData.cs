using UnityEngine;
using System.Collections.Generic;

public class BoardData {
    //TODO: temp for AI to use
    public FigureData figureData;
    public int BoardSize => 8;
    public int BoardSizeSqr => 64;

    // TODO: Remove public accessors on project end
    public long MainBoard { get; private set; }
    public long WhiteFiguresBoard { get; private set; }
    public long BlackFiguresBoard { get; private set; }
    public long KingsBoard { get; private set; }
    public long QueensBoard { get; private set; }
    public long RooksBoard { get; private set; }
    public long BishopsBoard { get; private set; }
    public long KnightsBoard { get; private set; }
    public long PawnsBoard { get; private set; }
    private long highlightBoard = 0L;

    public int WhiteScore { get; private set; }
    public int BlackScore { get; private set; }

    #region Initialise Board Data
    public BoardData() {
        FillBoardData();
        figureData = new FigureData(BoardSize, this);
        SetBoardScore();
    }

    // From lower right to upper left 
    private void FillBoardData() {
        // Rooks
        SetCellOccupied(FigureType.Rook, new Vector2Int(0, 0));
        SetCellOccupied(FigureType.Rook, new Vector2Int(7, 0));
        SetCellOccupied(FigureType.Rook, new Vector2Int(0, 7));
        SetCellOccupied(FigureType.Rook, new Vector2Int(7, 7));

        // Knights
        SetCellOccupied(FigureType.Knight, new Vector2Int(1, 0));
        SetCellOccupied(FigureType.Knight, new Vector2Int(6, 0));
        SetCellOccupied(FigureType.Knight, new Vector2Int(1, 7));
        SetCellOccupied(FigureType.Knight, new Vector2Int(6, 7));

        // Bishops
        SetCellOccupied(FigureType.Bishop, new Vector2Int(2, 0));
        SetCellOccupied(FigureType.Bishop, new Vector2Int(5, 0));
        SetCellOccupied(FigureType.Bishop, new Vector2Int(2, 7));
        SetCellOccupied(FigureType.Bishop, new Vector2Int(5, 7));

        //Queens
        SetCellOccupied(FigureType.Queen, new Vector2Int(3, 0));
        SetCellOccupied(FigureType.Queen, new Vector2Int(3, 7));

        // Kings
        SetCellOccupied(FigureType.King, new Vector2Int(4, 0));
        SetCellOccupied(FigureType.King, new Vector2Int(4, 7));

        for(int c = 0; c < BoardSize; c++) {
            SetCellOccupied(FigureType.Pawn, new Vector2Int(c, 1));
            SetCellOccupied(FigureType.Pawn, new Vector2Int(c, 6));
        }

        for(int r = 0; r <= 1; r++) {
            for(int c = 0; c < BoardSize; c++) {
                SetCellOccupied(FigureType.White, new Vector2Int(c, r));
            }
        }
        for(int r = 6; r <= 7; r++) {
            for(int c = 0; c < BoardSize; c++) {
                SetCellOccupied(FigureType.Black, new Vector2Int(c, r));
            }
        }

        MainBoard |= RooksBoard | KnightsBoard | BishopsBoard | QueensBoard | KingsBoard | PawnsBoard;
    }

    private void SetBoardScore() {
        Vector2Int[] whites = GetAllChessPiecesByColor(true);
        foreach(Vector2Int chessPiece in whites) {
            WhiteScore += figureData.GetFigureValue(GetFigureType(chessPiece), true);
        }

        Vector2Int[] blacks = GetAllChessPiecesByColor(true);
        foreach(Vector2Int chessPiece in whites) {
            BlackScore += figureData.GetFigureValue(GetFigureType(chessPiece), false);
        }
    }
    #endregion

    public Vector2Int[] GetPossibleMoves(FigureType type, int col, int row, bool whitesTurn = true) {
        return figureData.GetPossibleMoves(type, col, row, whitesTurn);
    }

    public Vector2Int[] GetPossibleMoves(FigureType type, Vector2Int pos, bool whitesTurn = true) {
        return GetPossibleMoves(type, pos.x, pos.y, whitesTurn);
    }

    // TODO: Look into this function to make it prettier
    public void MoveFigure(Vector2Int from, Vector2Int to) {
        if(!IsCellOccupiedGlobal(from)) {
            Debug.Log("There is no chess piece on this coordinates.");
            return;
        }

        // Get figure type and free the corresponding board
        FigureType type = GetFigureType(from);
        SetCellFree(type, from);

        // Cell with that figure type is free. Now only color left. Get Color and remove if from corresponding board
        FigureType color = GetFigureType(from);
        SetCellFree(color, from);

        // Do the same but fo the new coordinates so we kill any chess piece there is
        FigureType oldType = GetFigureType(to);
        SetCellFree(oldType, to);

        FigureType oldColor = GetFigureType(to);
        SetCellFree(oldColor, to);

        // Set figure and it's color to a new position
        SetCellOccupied(type, to);
        SetCellOccupied(color, to);

        CheckWinConditions(oldType, oldColor);
    }

    private void CheckWinConditions(FigureType type, FigureType color) {
        if(type == FigureType.King) {
            if(color == FigureType.White) {
                Debug.Log("Black won!");
                return;
            }
            if(color == FigureType.Black) {
                Debug.Log("White won!");
            }
        }
    }

    public void HighlightCell(int i) {
        if(i > 63) {
            Debug.Log("Wrong index in HighlightCell");
            return;
        }
        highlightBoard |= 1L << i;
    }

    public void UnhighlightCell(int i) {
        if(i > 63) {
            Debug.Log("Wrong index in UnhighlightCell");
            return;
        }
        long mask = 1 << i;
        highlightBoard &= ~mask;
    }

    #region Helper Functions
    public Vector2Int[] GetAllChessPiecesByColor(bool white) {
        List<Vector2Int> moves = new List<Vector2Int>();

        for(int row = 0; row < BoardSize; row++) {
            for(int col = 0; col < BoardSize; col++) {
                Vector2Int pos = new Vector2Int(col, row);
                if(IsCellOccupied(white ? FigureType.White : FigureType.Black, pos)) {
                    moves.Add(pos);
                }
            }
        }

        return moves.ToArray();
    }

    public FigureType GetFigureType(int col, int row) {
        if(!IsWithinGameBoard(col, row)) {
            Debug.Log("Coordinates is not within range of 0-7.");
            return FigureType.Empty;
        }
        for(int i = 0; i <= (int)FigureType.Empty; i++) {
            if(IsCellOccupied((FigureType)i, col, row)) {
                return (FigureType)i;
            }
        }

        return FigureType.Empty;
    }

    public FigureType GetFigureType(Vector2Int pos) {
        return GetFigureType(pos.x, pos.y);
    }

    //TODO: I am doing the same thing in all three functions. Maybe there is a way to make it more abstract
    public bool IsCellOccupied(FigureType boardType, int col, int row) {
        int index = GetCellIndex(col, row);
        if(index == -1) {
            return true;
        }
        long mask = 1L << index;
        switch(boardType) {
            case FigureType.Pawn:
                return (PawnsBoard & mask) != 0;
            case FigureType.Rook:
                return (RooksBoard & mask) != 0;
            case FigureType.Knight:
                return (KnightsBoard & mask) != 0;
            case FigureType.Bishop:
                return (BishopsBoard & mask) != 0;
            case FigureType.Queen:
                return (QueensBoard & mask) != 0;
            case FigureType.King:
                return (KingsBoard & mask) != 0;
            case FigureType.White:
                return (WhiteFiguresBoard & mask) != 0;
            case FigureType.Black:
                return (BlackFiguresBoard & mask) != 0;
            case FigureType.All:
                return (MainBoard & mask) != 0;
            case FigureType.Empty:
                break;
            default:
                Debug.LogError("Something went wrong in switch in IsCellOccupied");
                break;
        }

        return false;
    }

    public bool IsCellOccupied(FigureType boardType, Vector2Int pos) {
        return IsCellOccupied(boardType, pos.x, pos.y);
    }

    public bool IsCellOccupiedGlobal(Vector2Int pos) {
        return IsCellOccupiedGLobal(pos.x, pos.y);
    }

    private bool IsCellOccupiedGLobal(int col, int row) {
        int index = GetCellIndex(col, row);
        if(index == -1) {
            return true;
        }
        long mask = 1L << index;
        return (MainBoard & mask) != 0;
    }

    //TODO: Need to call it twice because I need to set occupied color boards too but I do not know which ones just from the new position
    private void SetCellOccupied(FigureType boardType, int col, int row) {
        int index = GetCellIndex(col, row);
        if(index == -1) {
            return;
        }
        long mask = 1L << index;
        switch(boardType) {
            case FigureType.Pawn:
                PawnsBoard |= mask;
                break;
            case FigureType.Rook:
                RooksBoard |= mask;
                break;
            case FigureType.Knight:
                KnightsBoard |= mask;
                break;
            case FigureType.Bishop:
                BishopsBoard |= mask;
                break;
            case FigureType.Queen:
                QueensBoard |= mask;
                break;
            case FigureType.King:
                KingsBoard |= mask;
                break;
            case FigureType.White:
                WhiteFiguresBoard |= mask;
                break;
            case FigureType.Black:
                BlackFiguresBoard |= mask;
                break;
            case FigureType.All:
                Debug.Log("Illegal call to SetCellOccupied. You can't set cell to be occupied by all figures at once.");
                break;
            case FigureType.Empty:
                Debug.Log("No figure type was chosen.");
                break;
            default:
                Debug.LogError("Something went wrong in switch in IsCellOccupied");
                break;
        }

        MainBoard |= mask;
    }

    private void SetCellOccupied(FigureType boardType, Vector2Int pos) {
        SetCellOccupied(boardType, pos.x, pos.y);
    }

    // TODO: Need to call it twice to set color board free too. I could do it here myself. Not sure if I should however
    private void SetCellFree(FigureType boardType, int col, int row) {
        int index = GetCellIndex(col, row);
        if(index == -1) {
            return;
        }
        long mask = 1L << index;
        switch(boardType) {
            case FigureType.Pawn:
                PawnsBoard &= ~mask;
                break;
            case FigureType.Rook:
                RooksBoard &= ~mask;
                break;
            case FigureType.Knight:
                KnightsBoard &= ~mask;
                break;
            case FigureType.Bishop:
                BishopsBoard &= ~mask;
                break;
            case FigureType.Queen:
                QueensBoard &= ~mask;
                break;
            case FigureType.King:
                KingsBoard &= ~mask;
                break;
            case FigureType.White:
                WhiteFiguresBoard &= ~mask;
                break;
            case FigureType.Black:
                BlackFiguresBoard &= ~mask;
                break;
            case FigureType.All:
                Debug.Log("Illegal call to SetCellFree. You can't set cell free without specifying what figure was there before.");
                break;
            case FigureType.Empty:
                break;
            default:
                Debug.LogError("Something went wrong in switch statement in IsCellOccupied function");
                break;
        }
        MainBoard &= ~mask;
    }

    private void SetCellFree(FigureType boardType, Vector2Int pos) {
        SetCellFree(boardType, pos.x, pos.y);
    }

    public bool IsCellHighlighted(int i) {
        if(i > 63) {
            Debug.Log("Wrong index in IsCellHighlighted");
            return false;
        }
        long mask = 1L << i;
        return (highlightBoard | mask) != 0;
    }

    public bool IsWithinGameBoard(Vector2Int pos) {
        return IsWithinGameBoard(pos.x, pos.y);
    }

    private bool IsWithinGameBoard(int col, int row) {
        return col >= 0 && col < BoardSize && row >= 0 && row < BoardSize;
    }



    // Need to be careful. It is working for reversed boards on X axis
    private int GetCellIndex(int col, int row) {
        if(!IsWithinGameBoard(col, row)) {
            return -1;
        }
        return (1 + row) * BoardSize - col - 1;
        //return row * BoardSize + col;
    }
    #endregion
}