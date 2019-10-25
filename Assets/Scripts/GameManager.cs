using UnityEngine;

public class GameManager : MonoBehaviour {
    private BoardController board;
    private Camera mainCamera;
    private Vector3 chosenFigurePosition;

    private void Start() {
        board = FindObjectOfType<BoardController>();
        mainCamera = Camera.main;
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, new Vector3(0, 0, 1), Mathf.Infinity);
            if(hit.collider != null) {
                Vector3 hitPos = hit.transform.position;
                Vector2Int pos2D = new Vector2Int((int)hitPos.x, (int)hitPos.y);
                Vector3 illegalPosition = new Vector3(-1, -1, -1);

                if(chosenFigurePosition != illegalPosition && !board.IsCellOccupiedWithFiguresOfPlayerColor(pos2D) && board.IsLegalMove(pos2D)) {
                    board.MoveFigure(chosenFigurePosition, pos2D);
                    chosenFigurePosition = illegalPosition;
                    board.AIMove();
                    return;
                }

                if(hit.collider.CompareTag("Figure") && board.IsCellOccupiedWithFiguresOfPlayerColor(pos2D)) {
                    chosenFigurePosition = hitPos;
                    board.HighlightPossibleMoves(pos2D);
                }
            }
        }
    }
}