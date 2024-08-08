using UnityEngine;


public class FoodLetter : MonoBehaviour
{
    public LetterSprite sprite;
    public string letter;
    public SnakeGameController controller;

    public void setSprite(int charIndex, SnakeGameController con)
    {
        controller = con;

        LetterSprite s = Instantiate(sprite, gameObject.transform);
        s.GetComponent<SpriteRenderer>().sprite = controller.letterSprites[charIndex];
        s.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
