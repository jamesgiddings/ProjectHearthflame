using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GramophoneUtils.UI
{
    public class HoverInfoPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvasObject = null;
        [SerializeField] private RectTransform popupObject = null;
        [SerializeField] private TextMeshProUGUI infoText = null;
        [SerializeField] private float padding = 25f;
        [SerializeField] private float offsetX = 220f;
        [SerializeField] private float offsetY = -250f;
        [SerializeField] private float threshold = 10f;

        private Vector3 lastPosition;
        

        private Canvas popupCanvas = null;

        private void Start() => popupCanvas = popupCanvasObject.GetComponent<Canvas>();

        private void LateUpdate() => FollowCursor();

        public void HideInfo() => popupCanvasObject.SetActive(false);



        private void FollowCursor()
        {
            if (!popupCanvasObject.activeSelf) { return; }

            Vector2 newPos = Input.mousePosition + new Vector3(offsetX, offsetY, 0f);

            newPos = ClampToScreen(newPos, popupObject.rect.size);

            popupObject.transform.position = newPos;
        }

        private Vector2 ClampToScreen(Vector2 pos, Vector2 size)
        {
            pos.x = Mathf.Clamp(pos.x, size.x / 2 + padding, Screen.width - size.x / 2 - padding);

            pos.y = Mathf.Clamp(pos.y, size.y + padding, Screen.height - padding);

            return pos;
        }

        public void DisplayInfo(IResource infoResource)
        {
            if (popupCanvasObject.activeInHierarchy == true) { return; }

            infoText.text = infoResource.GetInfoDisplayText();
            popupCanvasObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(popupObject);
        }
    }
}
