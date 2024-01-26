using DG.Tweening;
using UnityEngine;
using TMPro;
using strange.extensions.signal.impl;

namespace Controllers
{
    public class PlayerTextController : MonoBehaviour
    {
        [Inject]
        public IPlayerTextService PlayerTextService { get; set; }

        [SerializeField] private TextMeshPro playerScoreText;

        public void UpdatePlayerScore(float totalScore)
        {
            PlayerTextService.UpdatePlayerScore(totalScore, playerScoreText);
        }

        public void CloseScoreText(bool isClosed)
        {
            PlayerTextService.CloseScoreText(isClosed, transform);
        }
    }

    public interface IPlayerTextService
    {
        void UpdatePlayerScore(float totalScore, TextMeshPro playerScoreText);
        void CloseScoreText(bool isClosed, Transform transform);
    }

    public class PlayerTextService : IPlayerTextService
    {
        public void UpdatePlayerScore(float totalScore, TextMeshPro playerScoreText)
        {
            playerScoreText.text = totalScore.ToString();
        }

        public void CloseScoreText(bool isClosed, Transform tr
