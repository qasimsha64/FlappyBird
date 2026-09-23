using UnityEngine;

public class GroundScoller : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 2f;

    private Material material;
    private Vector2 offset;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if(GameManager.Instance.GameState == GameState.GameOver)
            return; 
            

        offset.x += scrollSpeed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }







}
