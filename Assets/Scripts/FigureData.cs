using System.Collections.Generic;
using UnityEngine;

public class FigureData {
    private readonly int boardSize;
    private readonly BoardData boardData;
    
    public FigureData(BoardData board) {
        boardSize = board.BoardSize;
        boardData = board;
    }

    // TODO: Castling
    // TODO: En passant https://en.wikipedia.org/wiki/En_passant
    // TODO: Pawn promotion
    public Vector2Int[] GetPossibleMoves(FigureType type, int col, int row, bool whitesTurn = true) {
        switch(type) {
            case FigureType.Pawn:
                return GetPawnPossibleMoves(col, row, whitesTurn).ToArray();
            case FigureType.Rook:
                return GetRookPossibleMoves(col, row, whitesTurn).ToArray();
            case FigureType.Knight:
                return GetKnightPossibleMoves(col, row, whitesTurn).ToArray();
            case FigureType.Bishop:
                return GetBishopPossibleMoves(col, row, whitesTurn).ToArray();
            case FigureType.Queen:
                return GetQueenPossibleMoves(col, row, whitesTurn).ToArray();
            case FigureType.King:
                return GetKingPossibleMoves(col, row, whitesTurn).ToArray();
            default:
                Debug.Log("Illegal call to GetPossibleMoves. You can call this only on figures.");
                break;
        }
        return new Vector2Int[0];
    }

    private List<Vector2Int> GetPawnPossibleMoves(int col, int row, bool whitesTurn) {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int possibleMove = new Vector2Int(col, row + (whitesTurn ? 1 : -1));
        if(!boardData.IsCellOccupiedGlobal(possibleMove) && boardData.IsWithinGameBoard(possibleMove)) {
            moves.Add(possibleMove);
        }

        // Special first move for two squares forward
        if(row == (whitesTurn ? 1 : 6)) {
            Vector2Int cellInFront = new Vector2Int(col, row + (whitesTurn ? 1 : -1));
            possibleMove = new Vector2Int(col, row + (whitesTurn ? 2 : -2));
            if(!boardData.IsCellOccupiedGlobal(cellInFront) && boardData.IsWithinGameBoard(cellInFront) &&
               !boardData.IsCellOccupiedGlobal(possibleMove) && boardData.IsWithinGameBoard(possibleMove)) {
                moves.Add(possibleMove);
            }
        }

        // Attack
        possibleMove = new Vector2Int(col - 1, row + (whitesTurn ? 1 : -1));
        if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) && boardData.IsWithinGameBoard(possibleMove)) {
            moves.Add(possibleMove);
        }
        possibleMove = new Vector2Int(col + 1, row + (whitesTurn ? 1 : -1));
        if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) && boardData.IsWithinGameBoard(possibleMove)) {
            moves.Add(possibleMove);
        }
        return moves;
    }

    private List<Vector2Int> GetRookPossibleMoves(int col, int row, bool whitesTurn) {
        List<Vector2Int> moves = new List<Vector2Int>();
        bool[] breakLoop = new bool[4];
        int index = 1;
        for(int i = 0; i < boardSize; i++) {
            if(breakLoop[0] && breakLoop[1] && breakLoop[2] && breakLoop[3]) {
                break;
            }

            Vector2Int possibleMove = new Vector2Int(col - index, row);
            if(!breakLoop[0] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[0] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[0] = true;
                    moves.Remove(possibleMove);
                }
            }
            possibleMove = new Vector2Int(col + index, row);
            if(!breakLoop[1] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[1] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[1] = true;
                    moves.Remove(possibleMove);
                }
            }
            possibleMove = new Vector2Int(col, row - index);
            if(!breakLoop[2] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[2] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[2] = true;
                    moves.Remove(possibleMove);
                }
            }
            possibleMove = new Vector2Int(col, row + index);
            if(!breakLoop[3] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[3] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[3] = true;
                    moves.Remove(possibleMove);
                }
            }
            index++;
        }
        return moves;
    }

    private List<Vector2Int> GetKnightPossibleMoves(int col, int row, bool whitesTurn) {
        List<Vector2Int> moves = new List<Vector2Int>();
        for(int i = -1; i <= 1; i += 2) {
            Vector2Int possibleMove = new Vector2Int(col, row + 2);
            possibleMove.x += i;
            if(boardData.IsWithinGameBoard(possibleMove) && (boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) || !boardData.IsCellOccupiedGlobal(possibleMove))) {
                moves.Add(possibleMove);
            }

            possibleMove = new Vector2Int(col, row - 2);
            possibleMove.x += i;
            if(boardData.IsWithinGameBoard(possibleMove) && (boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) || !boardData.IsCellOccupiedGlobal(possibleMove))) {
                moves.Add(possibleMove);
            }

            possibleMove = new Vector2Int(col + 2, row);
            possibleMove.y += i;
            if(boardData.IsWithinGameBoard(possibleMove) && (boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) || !boardData.IsCellOccupiedGlobal(possibleMove))) {
                moves.Add(possibleMove);
            }

            possibleMove = new Vector2Int(col - 2, row);
            possibleMove.y += i;
            if(boardData.IsWithinGameBoard(possibleMove) && (boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) || !boardData.IsCellOccupiedGlobal(possibleMove))) {
                moves.Add(possibleMove);
            }
        }
        return moves;
    }

    private List<Vector2Int> GetBishopPossibleMoves(int col, int row, bool whitesTurn) {
        List<Vector2Int> moves = new List<Vector2Int>();
        bool[] breakLoop = new bool[4];
        int index = 1;
        for(int i = 0; i < boardSize; i++) {
            if(breakLoop[0] && breakLoop[1] && breakLoop[2] && breakLoop[3]) {
                break;
            }

            Vector2Int possibleMove = new Vector2Int(col - index, row - index);
            if(!breakLoop[0] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[0] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[0] = true;
                    moves.Remove(possibleMove);
                }
            }
            possibleMove = new Vector2Int(col + index, row + index);
            if(!breakLoop[1] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[1] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[1] = true;
                    moves.Remove(possibleMove);
                }
            }
            possibleMove = new Vector2Int(col + index, row - index);
            if(!breakLoop[2] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[2] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[2] = true;
                    moves.Remove(possibleMove);
                }
            }
            possibleMove = new Vector2Int(col - index, row + index);
            if(!breakLoop[3] && boardData.IsWithinGameBoard(possibleMove)) {
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove)) {
                    breakLoop[3] = true;
                }
                moves.Add(possibleMove);
                if(boardData.IsCellOccupied(whitesTurn ? FigureType.White : FigureType.Black, possibleMove)) {
                    breakLoop[3] = true;
                    moves.Remove(possibleMove);
                }
            }
            index++;
        }
        return moves;
    }

    private List<Vector2Int> GetQueenPossibleMoves(int col, int row, bool whitesTurn) {
        List<Vector2Int> bishopMoves = GetBishopPossibleMoves(col, row, whitesTurn);
        List<Vector2Int> rookMoves = GetRookPossibleMoves(col, row, whitesTurn);
        foreach(Vector2Int piece in rookMoves) {
            bishopMoves.Add(piece);
        }
        return bishopMoves;
    }

    private List<Vector2Int> GetKingPossibleMoves(int col, int row, bool whitesTurn) {
        List<Vector2Int> moves = new List<Vector2Int>();
        // TODO: Need to check that this cell is not under attack before moving there
        for(int x = -1; x < 2; x++) {
            for(int y = -1; y < 2; y++) {
                if(x == 0 && y == 0) {
                    continue;
                }
                Vector2Int possibleMove = new Vector2Int(col - x, row - y);
                if(boardData.IsWithinGameBoard(possibleMove) && (boardData.IsCellOccupied(whitesTurn ? FigureType.Black : FigureType.White, possibleMove) || !boardData.IsCellOccupiedGlobal(possibleMove))) {
                    moves.Add(possibleMove);
                }
            }
        }
        return moves;
    }
}

public enum FigureType { Pawn, Rook, Knight, Bishop, Queen, King, White, Black, Empty }