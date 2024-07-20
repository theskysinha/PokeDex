using UnityEngine;
using UnityEngine.UI;

public class ScrollDetector : MonoBehaviour
{
    public ScrollRect scrollRect;
    public PokemonAPIManager pokemonAPIManager;
    public RectTransform content;

    // void Update()
    // {
    //     if (scrollRect.verticalNormalizedPosition <= 0.1f)
    //     {
    //         pokemonAPIManager.LoadMorePokemon();
    //     }

    //     CheckOffScreenItems();
    // }

    // private void CheckOffScreenItems()
    // {
    //     float viewportHeight = scrollRect.viewport.rect.height;
    //     float contentYMin = content.anchoredPosition.y;
    //     float contentYMax = contentYMin + viewportHeight;

    //     for (int i = content.childCount - 1; i >= 0; i--)
    //     {
    //         Transform child = content.GetChild(i);
    //         RectTransform rectTransform = child.GetComponent<RectTransform>();
    //         float itemYMin = rectTransform.anchoredPosition.y;
    //         float itemYMax = itemYMin + rectTransform.rect.height;

    //         if (itemYMax < contentYMin || itemYMin > contentYMax)
    //         {
    //             pokemonAPIManager.ReturnItemToPool(child.gameObject);
    //         }
    //     }
    // }
}
