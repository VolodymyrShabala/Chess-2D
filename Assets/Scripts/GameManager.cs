using UnityEngine;

public class GameManager : MonoBehaviour {
    private BoardController board;
    private bool playerPlaysWhite = true;

    public Transform chosenFigure;
    private bool playersTurn = true;

    private void Start() {
        board = FindObjectOfType<BoardController>();
        //board.Init(playerPlaysWhite);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, new Vector3(0, 0, 1), Mathf.Infinity);
            if(hit.collider != null) {
                Vector2Int pos = new Vector2Int((int)hit.transform.position.x, (int)hit.transform.position.y);

                if(chosenFigure != null && !board.IsCellOccupiedWithFiguresOfPlayerColor(pos) && board.IsLegalMove(pos)) {
                    board.MoveFigure(chosenFigure.position, pos);
                    chosenFigure = null;
                    playersTurn = false;
                    return;
                }

                // Maybe can get figure type from the beggining and check if it is not "Empty"
                if(hit.collider.CompareTag("Figure") && board.IsCellOccupiedWithFiguresOfPlayerColor(pos)) {
                    chosenFigure = hit.collider.transform;
                    board.HighlightPossibleMoves(pos);
                }
            }
        }

        //TODO: Temp
        if(!playersTurn) {
            board.AIMove();
            playersTurn = true;
        }
    }
}