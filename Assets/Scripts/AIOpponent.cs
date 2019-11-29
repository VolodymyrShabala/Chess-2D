using UnityEngine;

public class AIOpponent{
    private readonly BoardData boardData;
    private readonly CellValue cellValue;
    private readonly int difficulty;
    private FakeMove trueMove;
    
    public AIOpponent(BoardData board, int computerDifficulty, BoardController newController){
        boardData = board;
        difficulty = computerDifficulty;
        cellValue = new CellValue(board);
    }

    public void CalculateNextMove(bool white){
        trueMove = new FakeMove();
        // AlphaBetaPruning(new Vector2Int(0, 0), difficulty, white);
        Negamax(new Vector2Int(0, 0), difficulty, white);
        Debug.Log("Move from X: " + trueMove.from.x + ", Y: " + trueMove.from.y + ". Move to X: " + trueMove.to.x + ", Y: " + trueMove.to.y);

        // for(int i = difficulty; i > 0; i--) {
        //     BlackMoves();
        //     WhiteMoves();
        //     
        //     DoFakeMove(blackFakeMove);
        //     DoFakeMove(whiteFakeMove);
        //     
        //     fakeMoves.Push(blackFakeMove);
        //     fakeMoves.Push(whiteFakeMove);
        // }
        //
        // int fakeMovesLength = fakeMoves.Count;
        // for(int i = 0; i < fakeMovesLength; i++) {
        //     FakeMove fakeMove = fakeMoves.Pop();
        //     
        //     if(i == fakeMovesLength - (white ? 2 : 1)) {
        //         figureToMove = fakeMove.from;
        //         bestMove = fakeMove.to;
        //     }
        //     
        //     UndoFakeMove(fakeMove);
        // }
    }

    private int Negamax(Vector2Int cell, int depth, bool white){
        if(depth == 0 || (boardData.IsCellOccupied(FigureType.King, cell) &&
                          boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
            return Evaluate(boardData.GetFigureType(cell), cell);
        }

        int evaluation = int.MinValue;
        Vector2Int[] whitePieces = boardData.GetAllChessPiecesByColor(white);
        foreach(Vector2Int piece in whitePieces) {
            FigureType type = boardData.GetFigureType(piece);
            Vector2Int[] possibleMoves = boardData.GetPossibleMoves(type, piece, white);
            foreach(Vector2Int position in possibleMoves) {
                int i = -Negamax(position, depth - 1, !white);
                
                if(i > evaluation) {
                    evaluation = i;
                    trueMove.from = piece;
                    trueMove.to = position;
                }
            }
        }

        return evaluation;
    }

    // private int AlphaBetaPruning(Vector2Int cell, int depth, bool white, int alpha = -int.MaxValue,
    //                              int beta = int.MaxValue){
    //     // Debug.Log(white);
    //     if(depth == 0 || (boardData.IsCellOccupied(FigureType.King, cell) && boardData.IsCellOccupied(white ? FigureType.Black : FigureType.White, cell))) {
    //         return Evaluate(boardData.GetFigureType(cell), cell);
    //     }
    //
    //     if(white) {
    //         Vector2Int[] whitePieces = boardData.GetAllChessPiecesByColor(white);
    //         int maxEvaluation = int.MinValue;
    //         if(whitePieces.Length == 0) {
    //             maxEvaluation = 0;
    //         }
    //         foreach(Vector2Int piece in whitePieces) {
    //             FigureType type = boardData.GetFigureType(piece);
    //             Vector2Int[] possibleMoves = boardData.GetPossibleMoves(type, piece, white);
    //             foreach(Vector2Int position in possibleMoves) {
    //                 FakeMove fakeMove = new FakeMove(type, piece, position, white);
    //                 DoFakeMove(fakeMove);
    //
    //                 int evaluation = AlphaBetaPruning(position, depth - 1, !white, alpha, beta);
    //
    //                 UndoFakeMove(fakeMove);
    //
    //                 if(evaluation > maxEvaluation) {
    //                     // Debug.Log("White. Value: " + evaluation + ". MaxEvaluation: " + maxEvaluation);
    //                     maxEvaluation = evaluation;
    //                     alpha = maxEvaluation;
    //                     trueMove.from = piece;
    //                     trueMove.to = position;
    //                 }
    //
    //                 if(alpha >= beta) {
    //                     break;
    //                 }
    //             }
    //
    //         }
    //
    //         return maxEvaluation;
    //     }
    //
    //     Vector2Int[] blackPieces = boardData.GetAllChessPiecesByColor(white);
    //     int minEvaluation = int.MaxValue;
    //     if(blackPieces.Length == 0) {
    //         minEvaluation = 0;
    //     }
    //     foreach(Vector2Int piece in blackPieces) {
    //         FigureType type = boardData.GetFigureType(piece);
    //         Vector2Int[] possibleMoves = boardData.GetPossibleMoves(type, piece, white);
    //         foreach(Vector2Int position in possibleMoves) {
    //             FakeMove fakeMove = new FakeMove(type, piece, position, white);
    //             DoFakeMove(fakeMove);
    //
    //             int evaluation = AlphaBetaPruning(position, depth - 1, !white, alpha, beta);
    //
    //             UndoFakeMove(fakeMove);
    //
    //             if(evaluation < minEvaluation) {
    //                 // Debug.Log("Black. Value: " + evaluation + ". MaxEvaluation: " + minEvaluation);
    //                 minEvaluation = evaluation;
    //                 beta = minEvaluation;
    //                 trueMove.from = piece;
    //                 trueMove.to = position;
    //             }
    //
    //             if(alpha >= beta) {
    //                 break;
    //             }
    //         }
    //
    //     }
    //
    //     return minEvaluation;
    // }

    // private void BlackMoves(){
    //     int minEvaluation = int.MaxValue;
    //     Vector2Int[] blackPieces = boardData.GetAllChessPiecesByColor(false);
    //     foreach(Vector2Int piece in blackPieces) {
    //         FigureType type = boardData.GetFigureType(piece);
    //         Vector2Int[] possiblePositions = boardData.GetPossibleMoves(type, piece, false);
    //         foreach(Vector2Int position in possiblePositions) {
    //             FakeMove fakeMove = new FakeMove(type, piece, position, false);
    //             DoFakeMove(fakeMove);
    //             
    //             int evaluation = Evaluate(type, piece);
    //             
    //             UndoFakeMove(fakeMove);
    //             
    //             if(evaluation < minEvaluation) {
    //                 beta = evaluation;
    //                 minEvaluation = evaluation;
    //                 blackFakeMove = fakeMove;
    //             }
    //
    //             if(beta <= alpha) {
    //                 break;
    //             }
    //         }
    //     }
    // }
    //
    // private void WhiteMoves(){
    //     int maxEvaluation = int.MinValue;
    //     Vector2Int[] whitePieces = boardData.GetAllChessPiecesByColor(true);
    //     foreach(Vector2Int piece in whitePieces) {
    //         FigureType type = boardData.GetFigureType(piece);
    //         Vector2Int[] possiblePositions = boardData.GetPossibleMoves(type, piece, true);
    //         foreach(Vector2Int position in possiblePositions) {
    //             FakeMove fakeMove = new FakeMove(type, piece, position, true);
    //             DoFakeMove(fakeMove);
    //             
    //             int evaluation = Evaluate(type, piece);
    //
    //             UndoFakeMove(fakeMove);
    //             
    //             if(evaluation > maxEvaluation) {
    //                 alpha = evaluation;
    //                 maxEvaluation = evaluation;
    //                 whiteFakeMove = fakeMove;
    //             }
    //
    //             if(beta <= alpha) {
    //                 break;
    //             }
    //         }
    //     }
    // }

    // TODO: Sometimes returns infinity
    // private int AlphaBetaPruning(Vector2Int cell, int depth, int alpha = -int.MaxValue, int beta = int.MaxValue, bool maximizingPlayer = false) {
    //     if(depth == 0 || (boardData.IsCellOccupied(FigureType.King, cell) &&
    //                            boardData.IsCellOccupied(maximizingPlayer ? FigureType.Black : FigureType.White, cell))) {
    //         return Evaluate(cell);
    //     }
    //
    //     if(maximizingPlayer) {
    //         int maxEvaluation = -int.MaxValue;
    //         Vector2Int[] possiblePositions = boardData.GetPossibleMoves(figureType, cell, true);
    //         Debug.Log("Length: " + possiblePositions.Length + ". Figure type: " + figureType + ". Pos: " + cell);
    //         foreach(Vector2Int v in possiblePositions) {
    //             Debug.Log("Position: " + v);
    //         }
    //         foreach(Vector2Int position in possiblePositions) {
    //             if(!boardData.IsCellOccupied(figureType, position) || !boardData.IsWithinGameBoard(position)) {
    //                 // Debug.Log("Returning");
    //                 break;
    //             }
    //             FakeMove fakeMove = new FakeMove(figureType, cell, position, maximizingPlayer);
    //             fakeMoves.Push(fakeMove);
    //             DoFakeMove(fakeMove);
    //             // controller.MoveFigure(new Vector3(cell.x, cell.y), position);
    //             int evaluation = AlphaBetaPruning(position, depth - 1, alpha, beta, false);
    //             // controller.UndoMove();
    //             UndoFakeMove(fakeMove);
    //                             
    //             // Debug.Log(evaluation);
    //             if(evaluation > maxEvaluation) {
    //                 alpha = evaluation;
    //                 maxEvaluation = evaluation;
    //                 tempBestMove = position;
    //             }
    //             
    //             if(beta <= alpha) {
    //                 break;
    //             }
    //         }
    //         
    //         // Debug.Log("Returning max: " + maxEvaluation + ". Beta is: " + beta);
    //         return maxEvaluation;
    //     } else {
    //         int minEvaluation = int.MaxValue;
    //         Vector2Int[] possiblePositions = boardData.GetPossibleMoves(figureType, cell, false);
    //         Debug.Log("Length: " + possiblePositions.Length + ". Figure type: " + figureType + ". Pos: " + cell);
    //         foreach(Vector2Int v in possiblePositions) {
    //             Debug.Log("Position: " + v);
    //         }
    //         foreach(Vector2Int position in possiblePositions) {
    //             if(!boardData.IsCellOccupied(figureType, position) || !boardData.IsWithinGameBoard(position)) {
    //                 // Debug.Log("Returning");
    //                 break;
    //             }
    //             FakeMove fakeMove = new FakeMove(figureType, cell, position, maximizingPlayer);
    //             fakeMoves.Push(fakeMove);
    //             DoFakeMove(fakeMove);
    //             // controller.MoveFigure(new Vector3(cell.x, cell.y), position);
    //             int evaluation = AlphaBetaPruning(position, depth - 1, alpha, beta, true);
    //             // controller.UndoMove();
    //             UndoFakeMove(fakeMove);
    //             
    //             // Debug.Log(evaluation);
    //             if(evaluation < minEvaluation) {
    //                 beta = evaluation;
    //                 minEvaluation = evaluation;
    //                 tempBestMove = position;
    //             }
    //             
    //             if(beta <= alpha) {
    //                 break;
    //             }
    //         }
    //         
    //         // Debug.Log("Returning min: " + minEvaluation + ". Alpha is: " + alpha);
    //         return minEvaluation;
    //     }
    // }

    private void DoFakeMove(FakeMove fakeMove){
        boardData.SetCellFree(fakeMove.figureType, fakeMove.from);
        boardData.SetCellFree(fakeMove.whiteTurn ? FigureType.White : FigureType.Black, fakeMove.from);

        FigureType killedFigureType = boardData.GetFigureType(fakeMove.to);
        if(killedFigureType != FigureType.Empty) {
            boardData.SetCellFree(killedFigureType, fakeMove.to);
            boardData.SetCellFree(fakeMove.whiteTurn ? FigureType.Black : FigureType.White, fakeMove.to);
            fakeMove.killedFigure = killedFigureType;
        }

        boardData.SetCellOccupied(fakeMove.figureType, fakeMove.to);
        boardData.SetCellOccupied(fakeMove.whiteTurn ? FigureType.White : FigureType.Black, fakeMove.to);
    }

    private void UndoFakeMove(FakeMove fakeMove){
        boardData.SetCellFree(fakeMove.figureType, fakeMove.to);
        boardData.SetCellFree(fakeMove.whiteTurn ? FigureType.White : FigureType.Black, fakeMove.to);

        if(fakeMove.killedFigure != FigureType.Empty) {
            boardData.SetCellOccupied(fakeMove.killedFigure, fakeMove.to);
            boardData.SetCellOccupied(fakeMove.whiteTurn ? FigureType.Black : FigureType.White, fakeMove.to);
        }

        boardData.SetCellOccupied(fakeMove.figureType, fakeMove.from);
        boardData.SetCellOccupied(fakeMove.whiteTurn ? FigureType.White : FigureType.Black, fakeMove.from);
    }

    private int Evaluate(FigureType type, Vector2Int cell) {
        return cellValue.GetCellValue(type, cell);
    }
    
    public Vector2Int GetChessPieceToMove() {
        return trueMove.from;
    }

    public Vector2Int GetBestMove() {
        return trueMove.to;
    }
}

public struct FakeMove{
    public readonly FigureType figureType;
    public FigureType killedFigure;
    public Vector2Int from;
    public Vector2Int to;
    public readonly bool whiteTurn;
    
    public FakeMove(FigureType figure, Vector2Int moveFrom, Vector2Int moveTo, bool white){
        figureType = figure;
        from = moveFrom;
        to = moveTo;
        whiteTurn = white;
        killedFigure = FigureType.Empty;
    }
    
    public FakeMove(FigureType figure, Vector2Int moveFrom, Vector2Int moveTo, FigureType killedFigureType, bool white){
        figureType = figure;
        from = moveFrom;
        to = moveTo;
        killedFigure = killedFigureType;
        whiteTurn = white;
    }
}