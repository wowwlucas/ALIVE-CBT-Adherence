﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodDiary : MonoBehaviour, IMoodCheckActivity_Generic
{

    public QuestionAndResponse[] situationQuestions;
    public QuestionAndResponse[] physicalSensationQuestions;
    public QuestionAndResponse[] unhelpfulThoughtsQuestions;
    public QuestionAndResponse[] challengeThoughtsQuestionsYes;
    public QuestionAndResponse[] challengeThoughtsQuestionsNo;

    public MoodCheckManager moodCheckManager;
    public Text question;
    public InputField answerInput;
    public GameObject YesNo;

    private QuestionType questionType;
    private int _challengeThoughtsQuestionsCount = 0;
    private int _unhelpfulQuestionsCount = 0;
    private MoodDiaryInfo _moodDiaryInfo;
    private bool _isYes = false;
    private bool _isQuestion = false;
    private QuestionAndResponse _currentQnR;

    // Use this for initialization
    void Start () {
        // Set the question type to the first type, which is situation
        questionType = QuestionType.Situation;

        // Setting up Mood Diary Info 
        _moodDiaryInfo = new MoodDiaryInfo();
        _moodDiaryInfo.Question_ChallengeThoughts = new string[challengeThoughtsQuestionsYes.Length + challengeThoughtsQuestionsNo.Length];
        _moodDiaryInfo.Answer_ChallengeThoughts = new string[challengeThoughtsQuestionsYes.Length + challengeThoughtsQuestionsNo.Length];

        _moodDiaryInfo.Question_UnhelpfulThoughts = new string[unhelpfulThoughtsQuestions.Length];
        _moodDiaryInfo.Answer_UnhelpfulThoughts = new string[unhelpfulThoughtsQuestions.Length];

        QuestionGenerator();
    }
	
	// Update is called once per frame
	void Update () {
        if(answerInput.text == "")
        {
            moodCheckManager.next.interactable = false;
        }
        else
        {
            moodCheckManager.next.interactable = true;
        }
	}

    public void QuestionGenerator()
    {
        int randNum;
        switch (questionType)
        {
            case QuestionType.Situation:
                randNum = Random.Range(0, situationQuestions.Length);
                _currentQnR = situationQuestions[randNum];
                if(_currentQnR.Question == "")
                {
                    _isQuestion = false;
                    answerInput.gameObject.SetActive(false);
                    ResponseGenerator();
                }
                else
                {
                    _isQuestion = true;
                    _moodDiaryInfo.Question_Situation = question.text = situationQuestions[randNum].Question;
                }
                break;
            case QuestionType.PhysicalSensations:
                randNum = Random.Range(0, physicalSensationQuestions.Length);
                _currentQnR = physicalSensationQuestions[randNum];
                if (_currentQnR.Question == "")
                {
                    _isQuestion = false;
                    answerInput.gameObject.SetActive(false);
                    ResponseGenerator();
                }
                else
                {
                    _isQuestion = true;
                    _moodDiaryInfo.Question_PhysicalSensation = question.text = physicalSensationQuestions[randNum].Question;
                }
                break;
            case QuestionType.UnhelpfulThoughts:
                _currentQnR = unhelpfulThoughtsQuestions[_unhelpfulQuestionsCount];
                if (_currentQnR.Question == "")
                {
                    _isQuestion = false;
                    answerInput.gameObject.SetActive(false);
                    ResponseGenerator();
                }
                else
                {
                    _isQuestion = true;
                    _moodDiaryInfo.Question_UnhelpfulThoughts[_unhelpfulQuestionsCount] = question.text = unhelpfulThoughtsQuestions[_unhelpfulQuestionsCount].Question;
                }
                break;
            case QuestionType.ChallengeThoughts:
                if (_isYes)
                {
                    if (_challengeThoughtsQuestionsCount < challengeThoughtsQuestionsYes.Length)
                    {
                        _currentQnR = challengeThoughtsQuestionsYes[_challengeThoughtsQuestionsCount];
                        if (_currentQnR.Question == "")
                        {
                            _isQuestion = false;
                            answerInput.gameObject.SetActive(false);
                            ResponseGenerator();
                        }
                        else
                        {
                            _isQuestion = true;
                            _moodDiaryInfo.Question_ChallengeThoughts[_challengeThoughtsQuestionsCount] = question.text
                             = challengeThoughtsQuestionsYes[_challengeThoughtsQuestionsCount].Question;
                            _challengeThoughtsQuestionsCount++;
                        }
                        
                    }
                }
                else
                {
                    if (_challengeThoughtsQuestionsCount < challengeThoughtsQuestionsNo.Length)
                    {
                        _currentQnR = challengeThoughtsQuestionsNo[_challengeThoughtsQuestionsCount];
                        if (_currentQnR.Question == "")
                        {
                            _isQuestion = false;
                            answerInput.gameObject.SetActive(false);
                            ResponseGenerator();
                        }
                        else
                        {
                            _isQuestion = true;
                            _moodDiaryInfo.Question_ChallengeThoughts[_challengeThoughtsQuestionsCount] = question.text
                            = challengeThoughtsQuestionsNo[_challengeThoughtsQuestionsCount].Question;
                            _challengeThoughtsQuestionsCount++;
                        }
                    }
                }
                break;
        }
    }

    public void ResponseGenerator()
    {
        _isQuestion = false;
        switch (questionType)
        {
            case QuestionType.Situation:
                question.text = _currentQnR.Response;
                break;
            case QuestionType.PhysicalSensations:
                question.text = _currentQnR.Response;
                break;
            case QuestionType.UnhelpfulThoughts:
                question.text = _currentQnR.Response;
                break;
            case QuestionType.ChallengeThoughts:
                question.text = _currentQnR.Response;
                break;
        }
    }

    public void Back()
    {
        switch (questionType)
        {
            case QuestionType.Situation:
                Reset();
                moodCheckManager.OpenActivitySelection();
                break;
            case QuestionType.PhysicalSensations:
                _moodDiaryInfo.Answer_PhysicalSensation = answerInput.text;
                answerInput.text = _moodDiaryInfo.Answer_Situation;
                questionType = QuestionType.Situation;
                QuestionGenerator();
                break;
            case QuestionType.UnhelpfulThoughts:
                _moodDiaryInfo.Answer_UnhelpfulThoughts[_unhelpfulQuestionsCount] = answerInput.text;
                answerInput.text = _moodDiaryInfo.Answer_PhysicalSensation;
                questionType = QuestionType.PhysicalSensations;
                QuestionGenerator();
                break;
            case QuestionType.ChallengeThoughts:
                if (_challengeThoughtsQuestionsCount == 1)
                {
                    _moodDiaryInfo.Answer_ChallengeThoughts[0] = answerInput.text;
                    answerInput.text = _moodDiaryInfo.Answer_UnhelpfulThoughts[_unhelpfulQuestionsCount];
                    questionType = QuestionType.UnhelpfulThoughts;
                    _challengeThoughtsQuestionsCount = 0;
                    QuestionGenerator();
                }
                else if (_challengeThoughtsQuestionsCount > 1)
                {
                    _challengeThoughtsQuestionsCount -= 2;
                    _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount + 1] = answerInput.text;
                    answerInput.text = _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount];
                    QuestionGenerator();
                }
                break;
        }
    }

    public void Next()
    {
        switch(questionType)
        {
            case QuestionType.Situation:
                if ((_currentQnR.Response == "") || !_isQuestion)
                {
                    // Set the input text as active
                    answerInput.gameObject.SetActive(true);
                    // Set next question type
                    questionType = QuestionType.PhysicalSensations;
                    // Add the player's input to the mood diary info
                    _moodDiaryInfo.Answer_Situation = answerInput.text;
                    // Set the input text as next question's answer
                    answerInput.text = _moodDiaryInfo.Answer_PhysicalSensation;
                    QuestionGenerator();
                }
                else
                {
                    // Set the input text as not active
                    answerInput.gameObject.SetActive(false);
                    ResponseGenerator();
                }
                break;
            case QuestionType.PhysicalSensations:
                if ((_currentQnR.Response == "") || !_isQuestion)
                {
                    // Set the input text as active
                    answerInput.gameObject.SetActive(true);
                    // Set next question type
                    questionType = QuestionType.UnhelpfulThoughts;
                    // Add the player's input to the mood diary info
                    _moodDiaryInfo.Answer_PhysicalSensation = answerInput.text;
                    // Set the input text as next question's answer
                    answerInput.text = _moodDiaryInfo.Answer_UnhelpfulThoughts[_unhelpfulQuestionsCount];
                    QuestionGenerator();
                }
                else
                {
                    // Set the input text as not active
                    answerInput.gameObject.SetActive(false);
                    ResponseGenerator();
                }
                break;
            case QuestionType.UnhelpfulThoughts:
                if (_unhelpfulQuestionsCount < unhelpfulThoughtsQuestions.Length)
                {
                    if(_unhelpfulQuestionsCount == 0)
                    {
                        _moodDiaryInfo.Answer_UnhelpfulThoughts[_unhelpfulQuestionsCount] = answerInput.text;
                        // Set the input text as not active
                        answerInput.gameObject.SetActive(false);
                        ResponseGenerator();
                        _unhelpfulQuestionsCount++;
                    }
                    else if(_unhelpfulQuestionsCount == 1)
                    {
                        if ((_currentQnR.Response == "") || !_isQuestion)
                        {
                            // Set the input text as active
                            answerInput.gameObject.SetActive(true);
                            // Set next question type
                            questionType = QuestionType.ChallengeThoughts;
                            // Add the player's input to the mood diary info
                            _moodDiaryInfo.Answer_UnhelpfulThoughts[_unhelpfulQuestionsCount] = answerInput.text;
                            // Set the input text as next question's answer
                            answerInput.text = _moodDiaryInfo.Answer_ChallengeThoughts[0];
                            QuestionGenerator();
                        }
                        else
                        {
                            // Set the input text as not active
                            answerInput.gameObject.SetActive(false);
                            ResponseGenerator();
                            _unhelpfulQuestionsCount++;
                        }
                    }                    
                }
                break;
            case QuestionType.ChallengeThoughts:
                if (_isYes)
                {
                    if (_challengeThoughtsQuestionsCount >= challengeThoughtsQuestionsYes.Length)
                    {
                        // Add the player's input to the mood diary info
                        _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount - 1] = answerInput.text;
                        Save();
                        moodCheckManager.OpenActivitySelection();
                        moodCheckManager.activitySelection.GetComponent<ActivitySelection>().RemoveActivity(0);
                    }
                    else
                    {
                        // Add the player's input to the mood diary info
                        _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount - 1] = answerInput.text;
                        // Set the input text as next question's answer
                        answerInput.text = _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount];
                        QuestionGenerator();
                    }

                }
                else
                {
                    if (_challengeThoughtsQuestionsCount >= challengeThoughtsQuestionsNo.Length)
                    {
                        // Add the player's input to the mood diary info
                        _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount - 1] = answerInput.text;
                        Save();
                        moodCheckManager.OpenActivitySelection();
                        moodCheckManager.activitySelection.GetComponent<ActivitySelection>().RemoveActivity(0);
                    }
                    else
                    {
                        // Add the player's input to the mood diary info
                        _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount - 1] = answerInput.text;
                        // Set the input text as next question's answer
                        answerInput.text = _moodDiaryInfo.Answer_ChallengeThoughts[_challengeThoughtsQuestionsCount];
                        QuestionGenerator();
                    }
                }
                break;
        }
    }

    public void Save()
    {
        moodCheckManager.moodCheckInfo.moodDiaryInfo = _moodDiaryInfo;
        moodCheckManager.moodCheckInfo.moodDiaryActive = true;
    }

    public void Reset()
    {
        _moodDiaryInfo.Answer_Situation = "";
        _moodDiaryInfo.Question_Situation = "";

        _moodDiaryInfo.Answer_PhysicalSensation = "";
        _moodDiaryInfo.Question_PhysicalSensation = "";
        
        for(int i = 0; i < _moodDiaryInfo.Question_UnhelpfulThoughts.Length; ++i)
        {
            _moodDiaryInfo.Answer_UnhelpfulThoughts[i] = "";
            _moodDiaryInfo.Question_UnhelpfulThoughts[i] = "";
        }

        for(int i = 0; i < _moodDiaryInfo.Question_ChallengeThoughts.Length; ++i)
        {
            _moodDiaryInfo.Answer_ChallengeThoughts[i] = "";
            _moodDiaryInfo.Question_ChallengeThoughts[i] = "";
        }

        questionType = QuestionType.Situation;
        _challengeThoughtsQuestionsCount = 0;
        _unhelpfulQuestionsCount = 0;
        answerInput.text = "";
    }

}