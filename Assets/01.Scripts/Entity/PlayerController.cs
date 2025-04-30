using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private PlayerStat playerStat;
    private PlayerVisual playerVisual;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveDirection;

    private bool isInitialized = false;

    public void Init(PlayerStat playerStat, PlayerVisual visual)
    {
        this.playerStat = playerStat;
        playerVisual = visual;
        isInitialized = true;
    }

    public void FixedUpdate()
    {
        if(!isInitialized)
        {
            throw new System.Exception("PlayerController가 초기화되지 않았습니다.");
        }
        if (moveDirection == Vector2.zero)
        {
            playerVisual.SetMoving(false);
            return;
        }
        transform.Translate(moveDirection * playerStat.Stats[StatType.Speed].FinalValue * Time.deltaTime);
        playerVisual.SetDirection(moveDirection);
        playerVisual.SetMoving(true);
    }

    private void OnMove(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();

        if (input != null)
        {
            moveDirection = input.normalized;
        }
    }

    public void ResetInput() => moveDirection = new Vector2();
}
