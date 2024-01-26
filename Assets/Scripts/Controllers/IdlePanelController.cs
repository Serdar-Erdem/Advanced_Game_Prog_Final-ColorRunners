using TMPro;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Controllers
{
    public class IdlePanelController : View
    {
        [SerializeField] private TextMeshProUGUI playerScoreText;

        public void SetScoreText(int value)
        {
            playerScoreText.text = value.ToString();
        }
    }
}
