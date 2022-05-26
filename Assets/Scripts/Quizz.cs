using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quizz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO _question;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    bool hasAnsweredEarly;

    [Header("Buttons color")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    
    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        GetNextQuestion();
    }

    private void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        } else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
    }

    public void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == _question.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct !";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
        else
        {
            int correctAnswerIndex = _question.GetCorrectAnswerIndex();
            string correctAnswer = _question.GetAnswer(correctAnswerIndex);
            questionText.text = $"Sorry the correct answer was : \n {correctAnswer}";
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
    }

    void DisplayQuestion()
    {
        questionText.text = _question.GetQuestion();

        for (var index = 0; index < answerButtons.Length; index++)
        {
            var answerButton = answerButtons[index];
            TextMeshProUGUI buttonText = answerButton.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = _question.GetAnswer(index);
        }
    }

    void SetButtonState(bool state)
    {
        for (var index = 0; index < answerButtons.Length; index++)
        {
            var button = answerButtons[index].GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        for (var index = 0; index < answerButtons.Length; index++)
        {
            var buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
