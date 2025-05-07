using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStat playerStat;
    private PlayerVisual playerVisual;
    private Vector2 moveDirection;
    private InputHandler inputHandler;

    private bool isInitialized = false;

    public void Init(PlayerStat playerStat, PlayerVisual visual)
    {
        this.playerStat = playerStat;
        playerVisual = visual;
        inputHandler = InputHandler.Instance;
        inputHandler.OnInteractInput += Interact;
        isInitialized = true;
    }

    public void FixedUpdate()
    {
        if (!isInitialized)
        {
            throw new System.Exception("PlayerController가 초기화되지 않았습니다.");
        }
        moveDirection = inputHandler.MoveInput;
        if (moveDirection == Vector2.zero)
        {
            playerVisual.SetMoving(false);
            return;
        }
        transform.Translate(moveDirection * playerStat.Stats[StatType.Speed].FinalValue * Time.deltaTime);
        playerVisual.SetDirection(moveDirection);
        playerVisual.SetMoving(true);
    }

    public void Interact()
    {
        InteractionManager.Instance.Interactive(transform.position);
    }

    public void ResetInput() => moveDirection = new Vector2();
}
