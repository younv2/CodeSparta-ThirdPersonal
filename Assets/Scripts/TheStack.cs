using System.Collections;
using System.Collections.Generic;
using TheStack;
using UnityEngine;
namespace TheStack
{
    public class TheStack : MonoBehaviour
    {
        private const float BoundSize = 3.5f;
        private const float MovingBoundSize = 3f;
        private const float StackMovingSpeed = 5.0f;
        private const float BlockMovingSpeed = 5.0f;
        private const float ErrorMargin = 0.1f;
        [SerializeField] private InputHandler inputHandler;
        public GameObject originBlock = null;

        private Vector3 prevBlockPosition;
        private Vector3 desiredPosition;
        private Vector3 stackBound = new Vector2(BoundSize, BoundSize);

        Transform lastBlock = null;
        float blockTransition = 0f;
        float secondaryPosition = 0f;

        int stackCount = -1;
        public int Score { get { return stackCount; } }
        int comboCount = 0;
        public int Combo { get { return comboCount; } }
        int maxCombo = 0;
        public int MaxCombo { get { return maxCombo; } }

        public Color prevColor;
        public Color nextColor;

        bool isMovingX = true;

        int bestScore = 0;
        public int BestScore { get => bestScore; }

        int bestCombo = 0;
        public int BestCombo { get => bestCombo; }

        private const string BestScoreKey = "BestScore";
        private const string BestComboKey = "BestCombo";

        // Start is called before the first frame update
        void Start()
        {
            if (originBlock == null)
            {
                Debug.Log("OriginBlock is Null");
                return;
            }

            bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
            bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

            prevBlockPosition = Vector3.down;

            prevColor = GetRandomColor();
            nextColor = GetRandomColor();
            SpawnBlock();
            SpawnBlock();
            inputHandler.onClick += ClickEvent;
        }
        void ClickEvent()
        {
            if (PlaceBlock())
            {
                SpawnBlock();
            }
            else
            {
                Debug.Log("Game Over");
                UpdateScore();
            }
        }
        // Update is called once per frame
        void Update()
        {

            MoveBlock();
            transform.position = Vector3.Lerp(transform.position, desiredPosition, StackMovingSpeed * Time.deltaTime);
        }



        bool SpawnBlock()
        {
            if (lastBlock != null)
            {
                prevBlockPosition = lastBlock.localPosition;
            }

            GameObject newBlock = null;
            Transform newTrans = null;

            newBlock = Instantiate(originBlock);

            if (newBlock == null)
            {
                Debug.Log("NewBlock Instantiate Failed");
                return false;
            }
            ColorChange(newBlock);
            newTrans = newBlock.transform;
            newTrans.parent = this.transform;
            newTrans.localPosition = prevBlockPosition + Vector3.up;
            newTrans.localRotation = Quaternion.identity;
            newTrans.localScale = new Vector3(stackBound.x, 1, stackBound.y);

            stackCount++;

            desiredPosition = Vector3.down * stackCount;
            blockTransition = 0f;

            lastBlock = newTrans;

            isMovingX = !isMovingX;

            return true;
        }

        Color GetRandomColor()
        {
            float r = Random.Range(100f, 250f) / 255f;
            float g = Random.Range(100f, 250f) / 255f;
            float b = Random.Range(100f, 250f) / 255f;

            return new Color(r, g, b);
        }
        void ColorChange(GameObject go)
        {
            Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

            Renderer rn = go.GetComponent<Renderer>();

            if (rn == null)
            {
                Debug.Log("Renderer is Null");
                return;
            }

            rn.material.color = applyColor;
            Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

            if (applyColor.Equals(nextColor))
            {
                prevColor = nextColor;
                nextColor = GetRandomColor();
            }
        }

        void MoveBlock()
        {
            blockTransition += Time.deltaTime * BlockMovingSpeed;

            float movePosition = Mathf.PingPong(blockTransition, BoundSize) - BoundSize / 2;

            if (isMovingX)
            {
                lastBlock.localPosition = new Vector3(movePosition * MovingBoundSize, stackCount, secondaryPosition);
            }
            else
            {
                lastBlock.localPosition = new Vector3(secondaryPosition, stackCount, movePosition * MovingBoundSize);
            }
        }

        bool PlaceBlock()
        {
            Vector3 lastPosition = lastBlock.localPosition;

            if (isMovingX)
            {
                float deltaX = prevBlockPosition.x - lastPosition.x;
                bool isNegativeNum = (deltaX < 0) ? true : false;

                deltaX = Mathf.Abs(deltaX);

                if (deltaX > ErrorMargin)
                {
                    stackBound.x -= deltaX;
                    if (stackBound.x <= 0)
                    {
                        return false;
                    }

                    float middle = (prevBlockPosition.x + lastPosition.x) / 2f;
                    lastBlock.localScale = new Vector3(stackBound.x, 1, stackBound.y);

                    Vector3 tempPosition = lastBlock.localPosition;
                    tempPosition.x = middle;
                    lastBlock.localPosition = lastPosition = tempPosition;
                    float rubbleHalfScale = deltaX / 2f;
                    CreateRubble(new Vector3(isNegativeNum ? lastPosition.x + stackBound.x / 2 + rubbleHalfScale : lastPosition.x - stackBound.x / 2 - rubbleHalfScale,
                        lastPosition.y, lastPosition.z), new Vector3(deltaX, 1, stackBound.y));
                    comboCount = 0;
                }
                else
                {
                    ComboCheck();
                    lastBlock.localPosition = prevBlockPosition + Vector3.up;
                }
            }
            else
            {
                float deltaZ = prevBlockPosition.z - lastPosition.z;
                bool isNegativeNum = (deltaZ < 0) ? true : false;
                deltaZ = Mathf.Abs(deltaZ);
                if (deltaZ > ErrorMargin)
                {
                    stackBound.y -= deltaZ;
                    if (stackBound.y <= 0)
                    {
                        return false;
                    }
                    float middle = (prevBlockPosition.z + lastPosition.z) / 2f;
                    lastBlock.localScale = new Vector3(stackBound.x, 1, stackBound.y);

                    Vector3 tempPosition = lastBlock.localPosition;
                    tempPosition.z = middle;
                    lastBlock.localPosition = lastPosition = tempPosition;

                    float rubbleHalfScale = deltaZ / 2f;
                    CreateRubble(new Vector3(lastPosition.x,
                        lastPosition.y, isNegativeNum ? lastPosition.z + stackBound.y / 2 + rubbleHalfScale : lastPosition.z - stackBound.y / 2 - rubbleHalfScale), new Vector3(stackBound.y, 1, deltaZ));
                    comboCount = 0;
                }
                else
                {
                    ComboCheck();
                    lastBlock.localPosition = prevBlockPosition + Vector3.up;
                }
            }

            secondaryPosition = (isMovingX ? lastBlock.localPosition.x : lastBlock.localPosition.z);

            return true;
        }
        void CreateRubble(Vector3 pos, Vector3 scale)
        {
            GameObject go = Instantiate(lastBlock.gameObject);
            go.transform.parent = this.transform;

            go.transform.localPosition = pos;
            go.transform.localScale = scale;
            go.transform.localRotation = Quaternion.identity;

            go.AddComponent<Rigidbody>();
            go.name = "Rubble";
        }

        void ComboCheck()
        {
            comboCount++;
            if (comboCount > maxCombo)
                maxCombo = comboCount;

            if ((comboCount % 5) == 0)
            {
                Debug.Log("5 Combo!");
                stackBound += new Vector3(0.5f, 0.5f);
                stackBound.x = (stackBound.x > BoundSize) ? BoundSize : stackBound.x;
                stackBound.y = (stackBound.y > BoundSize) ? BoundSize : stackBound.y;

            }
        }

        void UpdateScore()
        {
            if (bestScore < stackCount)
            {
                Debug.Log("최고 점수 갱신");

                bestScore = stackCount;
                bestCombo = maxCombo;

                PlayerPrefs.SetInt(BestScoreKey, bestScore);
                PlayerPrefs.SetInt(BestComboKey, bestCombo);
            }
        }
    }
}
