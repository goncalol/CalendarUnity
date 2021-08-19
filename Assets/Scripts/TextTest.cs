using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextTest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Camera pCamera;

    private string firstText;
    private bool mouseEnter;
    private TextMeshProUGUI pTextMeshPro;
    private int prevIndex = -1;

    void Awake()
    {
        pTextMeshPro = GetComponent<TextMeshProUGUI>();
        firstText = pTextMeshPro.text;
    }

    private void LateUpdate()
    {
        if (mouseEnter)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, Input.mousePosition, pCamera);

            if (linkIndex != -1)
            {
                if (prevIndex != linkIndex)
                {
                    var highlitedText = pTextMeshPro.textInfo.linkInfo[linkIndex].GetLinkText();
                    pTextMeshPro.SetText(firstText);
                    pTextMeshPro.SetText(pTextMeshPro.text.Replace(highlitedText, "<b>" + highlitedText + "</b>"));
                    prevIndex = linkIndex;
                   
                }
            }
            else
            {
                prevIndex = -1;
                pTextMeshPro.SetText(firstText);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseEnter = false;
        pTextMeshPro.SetText(firstText);
    }

}
