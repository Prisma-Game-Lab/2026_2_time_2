using System.Collections.Generic;
using UnityEngine;

public class AdPopupManager : MonoBehaviour
{
    [Header("Instanciação")]
    [SerializeField] private Transform windowsContainer;
    [SerializeField] private GameObject popupPrefab;

    [Header("Propagandas")]
    [SerializeField] private List<Sprite> adSprites = new List<Sprite>();

    [Header("Limites de Spawn (Offsets em Relação ao Centro)")]
    [SerializeField] private Vector2 minPosition = new Vector2(-650f, -250f);
    [SerializeField] private Vector2 maxPosition = new Vector2(650f, 250f);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnAd();
        }
    }

    public void SpawnAd()
    {
        if (popupPrefab == null || windowsContainer == null) return;

        GameObject popupObj = Instantiate(popupPrefab, windowsContainer);
        popupObj.transform.SetAsLastSibling();

        RectTransform rect = popupObj.GetComponent<RectTransform>();
        if (rect != null)
        {
            float posX = Random.Range(minPosition.x, maxPosition.x);
            float posY = Random.Range(minPosition.y, maxPosition.y);
            rect.anchoredPosition = new Vector2(posX, posY);
        }

        PopUpAdUI adUI = popupObj.GetComponent<PopUpAdUI>();
        if (adUI != null && adSprites.Count > 0)
        {
            Sprite chosenSprite = adSprites[Random.Range(0, adSprites.Count)];
            adUI.Setup(chosenSprite);
        }
    }
}