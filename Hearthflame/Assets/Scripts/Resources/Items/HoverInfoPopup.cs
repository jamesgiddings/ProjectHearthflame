using GramophoneUtils.Items;
using GramophoneUtils.Items.Hotbars;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hel.Items
{
    public class HoverInfoPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvasObject = null;
        [SerializeField] private RectTransform popupObject = null;
        [SerializeField] private TextMeshProUGUI infoText = null;
        //[SerializeField] private Vector3 offset = new Vector3(0f, 5000f, 0f);
        [SerializeField] private float padding = 25f;

        private Canvas popupCanvas = null;

        private void Start() => popupCanvas = popupCanvasObject.GetComponent<Canvas>();

        private void Update() => FollowCursor();

        public void HideInfo() => popupCanvasObject.SetActive(false);

        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }
          
            Vector3 newPos = new Vector3(Input.mousePosition.x + 220, Input.mousePosition.y - 250, 0f );
            newPos.z = 0f;

            // float rightEdgeToScreenEdgeDistance = Screen.width - (newPos.x + popupObject.rect.width * popupCanvas.scaleFactor / 2) - padding;
            // if (rightEdgeToScreenEdgeDistance < 0)
            // {
            //     newPos.x += rightEdgeToScreenEdgeDistance;
            // }
            // float leftEdgeToScreenEdgeDistance = 0 - (newPos.x - popupObject.rect.width * popupCanvas.scaleFactor / 2) + padding;
            // if (leftEdgeToScreenEdgeDistance > 0)
            // {
            //     newPos.x += leftEdgeToScreenEdgeDistance;
            // }
            // float topEdgeToScreenEdgeDistance = Screen.height - (newPos.y + popupObject.rect.height * popupCanvas.scaleFactor) - padding;
            // if (topEdgeToScreenEdgeDistance < 0)
            // {
            //     newPos.y += topEdgeToScreenEdgeDistance;
            // }
            popupObject.transform.position = newPos;
        }

        public void DisplayInfo(Resource infoResource)
        {
            if (popupCanvasObject.activeInHierarchy == true) { return; }
                Item infoItem = infoResource as Item;
                if (infoItem != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<size=35>").Append(infoItem.ColouredName).Append("</size>\n");
                    builder.Append(infoItem.GetInfoDisplayText());
                    infoText.text = builder.ToString();
                    popupCanvasObject.SetActive(true);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
                }
        }
    }
}
