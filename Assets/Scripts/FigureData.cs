using System.Collections.Generic;
using UnityEngine;

public class FigureData {
    private int boardSize;
    private BoardData boardData;

    private int[] pawnBoardValues = new int[] {
                 0,  0,  0,  0,  0,  0,  0,  0,
                50, 50, 50, 50, 50, 50, 50, 50,
                10, 10, 20, 30, 30, 20, 10, 10,
                 5,  5, 10, 25, 25, 10,  5,  5,
                 0,  0,  0, 20, 20,  0,  0,  0,
                 5, -5,-10,  0,  0,-10, -5,  5,
                 5, 10, 10,-20,-20, 10, 10,  5,
                 0,  0,  0,  0,  0,  0,  0,  0
    };

    private int[] pawnBoardMirroredValues = new int[] {
                0,  0,  0,  0,  0,  0,  0,  0,
                5, 10, 10,-20,-20, 10, 10,  5,
                5, -5,-10,  0,  0,-10, -5,  5,
                0,  0,  0, 20, 20,  0,  0,  0,
                5,  5, 10, 25, 25, 10,  5,  5,
                10, 10, 20, 30, 30, 20, 10, 10,
                50, 50, 50, 50, 50, 50, 50, 50,
                0,  0,  0,  0,  0,  0,  0,  0
    };

    private int[] knightBoardValues = new int[] {
                -50,-40,-30,-30,-30,-30,-40,-50,
                -40,-20,  0,  0,  0,  0,-20,-40,
                -30,  0, 10, 15, 15, 10,  0,-30,
                -30,  5, 15, 20, 20, 15,  5,-30,
                -30,  0, 15, 20, 20, 15,  0,-30,
                -30,  5, 10, 15, 15, 10,  5,-30,
                -40,-20,  0,  5,  5,  0,-20,-40,
                -50,-40,-30,-30,-30,-30,-40,-50,
    };

    private int[] knightBoardMirroredValues = new int[] {
                -50,-40,-30,-30,-30,-30,-40,-50,
                -40,-20,  0,  5,  5,  0,-20,-40,
                -30,  5, 10, 15, 15, 10,  5,-30,
                -30,  0, 15, 20, 20, 15,  0,-30,
                -30,  5, 15, 20, 20, 15,  5,-30,
                -30,  0, 10, 15, 15, 10,  0,-30,
                -40,-20,  0,  0,  0,  0,-20,-40,
                -50,-40,-30,-30,-30,-30,-40,-50
    };

    private int[] bishopBoardValues = new int[] {
                -20,-10,-10,-10,-10,-10,-10,-20,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -10,  0,  5, 10, 10,  5,  0,-10,
                -10,  5,  5, 10, 10,  5,  5,-10,
                -10,  0, 10, 10, 10, 10,  0,-10,
                -10, 10, 10, 10, 10, 10, 10,-10,
                -10,  5,  0,  0,  0,  0,  5,-10,
                -20,-10,-10,-10,-10,-10,-10,-20,
    };

    private int[] bishopBoardMirroredValues = new int[] {
                -20,-10,-10,-10,-10,-10,-10,-20,
                -10,  5,  0,  0,  0,  0,  5,-10,
                -10, 10, 10, 10, 10, 10, 10,-10,
                -10,  0, 10, 10, 10, 10,  0,-10,
                -10,  5,  5, 10, 10,  5,  5,-10,
                -10,  0,  5, 10, 10,  5,  0,-10,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -20,-10,-10,-10,-10,-10,-10,-20
    };

    private int[] rookBoardValues = new int[] {
                  0,  0,  0,  0,  0,  0,  0,  0,
                  5, 10, 10, 10, 10, 10, 10,  5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                 -5,  0,  0,  0,  0,  0,  0, -5,
                  0,  0,  0,  5,  5,  0,  0,  0
    };

    private int[] rookBoardMirroredValues = new int[] {
                0,  0,  0,  5,  5,  0,  0,  0,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                -5,  0,  0,  0,  0,  0,  0, -5,
                5, 10, 10, 10, 10, 10, 10,  5,
                0,  0,  0,  0,  0,  0,  0,  0
    };

    private int[] queenBoardValues = new int[] {
                -20,-10,-10, -5, -5,-10,-10,-20,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -10,  0,  5,  5,  5,  5,  0,-10,
                 -5,  0,  5,  5,  5,  5,  0, -5,
                  0,  0,  5,  5,  5,  5,  0, -5,
                -10,  5,  5,  5,  5,  5,  0,-10,
                -10,  0,  5,  0,  0,  0,  0,-10,
                -20,-10,-10, -5, -5,-10,-10,-20
    };

    private int[] queenBoardMirroredValues = new int[] {
                -20,-10,-10, -5, -5,-10,-10,-20,
                -10,  0,  5,  0,  0,  0,  0,-10,
                -10,  5,  5,  5,  5,  5,  0,-10,
                 0,  0,  5,  5,  5,  5,  0, -5,
                -5,  0,  5,  5,  5,  5,  0, -5,
                -10,  0,  5,  5,  5,  5,  0,-10,
                -10,  0,  0,  0,  0,  0,  0,-10,
                -20,-10,-10, -5, -5,-10,-10,-20
    };

    private int[] kingBoardMiddleGameValues = new int[] {
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -20,-30,-30,-40,-40,-30,-30,-20,
                -10,-20,-20,-20,-20,-20,-20,-10,
                 20, 20,  0,  0,  0,  0, 20, 20,
                 20, 30, 10,  0,  0, 10, 30, 20
    };

    private int[] kingBoardMirroredMiddleGameValues = new int[] {
                 20, 30, 10,  0,  0, 10, 30, 20,
                 20, 20,  0,  0,  0,  0, 20, 20,
                -10,-20,-20,-20,-20,-20,-20,-10,
                -20,-30,-30,-40,-40,-30,-30,-20,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30,
                -30,-40,-40,-50,-50,-40,-40,-30
    };

    //private int[] kingBoardLateGameValues = new int[] {
    //            -50,-40,-30,-20,-20,-30,-40,-50,
    //            -30,-20,-10,  0,  0,-10,-20,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-30,  0,  0,  0,  0,-30,-30,
    //            -50,-30,-30,-30,-30,-30,-30,-50
    //};

    //private int[] kingBoardMirroredLateGameValues = new int[] {
    //            -50,-30,-30,-30,-30,-30,-30,-50,
    //            -30,-30,  0,  0,  0,  0,-30,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 30, 40, 40, 30,-10,-30,
    //            -30,-10, 20, 30, 30, 20,-10,-30,
    //            -30,-20,-10,  0,  0,-10,-20,-30,
    //            -50,-40,-30,-20,-20,-30,-40,-50
    //};

    public FigureData(int sizeOfBoard) {
        boardSize = sizeOfBoard;
    }

    public FigureData(int sizeOfBoard, BoardData board) {
        boardSize = sizeOfBoard;
        boardData = board;
    }

    // TODO: Oh boy...
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
                Debug.LogError("Illegal call to GetPossibleMoves. You can call this only on figures.");
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

    public int GetFigureValue(FigureType type, bool whitesTurn) {
        switch(type) {
            case FigureType.Pawn:
                return whitesTurn ? -10 : 10;
            case FigureType.Rook:
                return whitesTurn ? -30 : 30;
            case FigureType.Knight:
                return whitesTurn ? -30 : 30;
            case FigureType.Bishop:
                return whitesTurn ? -50 : 50;
            case FigureType.Queen:
                return whitesTurn ? -90 : 90;
            case FigureType.King:
                return whitesTurn ? -900 : 900;
            case FigureType.White:
                break;
            case FigureType.Black:
                break;
            case FigureType.All:
                break;
            case FigureType.Empty:
                break;
            default:
                Debug.LogError("Something went wrong in switch in GetFigureValue");
                break;
        }

        return 0;
    }

    public int GetCellValue(FigureType figureType, Vector2Int cell, bool white = true) {
        switch(figureType) {
            case FigureType.Pawn:
                //int value = white ? pawnBoardValues[cell.y * boardSize + cell.x] : pawnBoardMirroredValues[cell.y * boardSize + cell.x];
                //if(boardData.IsCellOccupied(boardData.GetFigureType(cell), cell) && (boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
                //    value += 30 * (white ? 1 : -1);
                //}
                //return value;
                return white ? pawnBoardValues[cell.y * boardSize + cell.x] : pawnBoardMirroredValues[cell.y * boardSize + cell.x];
            case FigureType.Rook:
                return white ? rookBoardValues[cell.y * boardSize + cell.x] : rookBoardMirroredValues[cell.y * boardSize + cell.x];
            case FigureType.Knight:
                return white ? knightBoardValues[cell.y * boardSize + cell.x] : knightBoardMirroredValues[cell.y * boardSize + cell.x];
            case FigureType.Bishop:
                return white ? bishopBoardValues[cell.y * boardSize + cell.x] : bishopBoardMirroredValues[cell.y * boardSize + cell.x];
            case FigureType.Queen:
                return white ? queenBoardValues[cell.y * boardSize + cell.x] : queenBoardMirroredValues[cell.y * boardSize + cell.x];
            case FigureType.King:
                return white ? kingBoardMiddleGameValues[cell.y * boardSize + cell.x] : kingBoardMirroredMiddleGameValues[cell.y * boardSize + cell.x];
            default:
                Debug.LogError("Illegal call to GetCellValue. You can call this only on figures.");
                break;
        }

        return 0;
    }
}

public enum FigureType { Pawn, Rook, Knight, Bishop, Queen, King, White, Black, All, Empty }