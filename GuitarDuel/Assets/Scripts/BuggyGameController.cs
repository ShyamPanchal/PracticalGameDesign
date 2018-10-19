using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public float[] _comPlayCooldown;
    public int[] _playSequence;
    public int[] _comSequenceLength;
    public float _perfectPlay;
    public float _greatPlay;
    public float _nicePlay;
    public float _badPlay;

    private float _waitForPlayerToStartTime = 4.0f;
    private AudioSource[] _audioSources;
    private int _comPlayIndex = 0;
    private bool _played = false;
    private int _comSequenceLengthIndex = 0;
    private int _currentSequenceLength;
    private bool _playerTurn = false;
    public float[] _playerPlayCooldown;
    public int[] _playerPlaySequence;
    private IDictionary<string, int> _keyMapper = new Dictionary<string, int>();
    public float _playerWaitTime;
    private int _length;
    public int _playerAllowedKeys;
    public int _sectionScore = 0;
    public int _totalScore = 0;
    public int _sectionPossibleScore;
    public int _totalPossibleScore;
    public int _playerPlayIndex = 0;
    private bool _playerStarted = false;
    public float _playerNextPlayTime = 0.0f;
    private float _playerTimingDifference;
    private float _sectionScorePercentage;
    private int _currentArrayCopyIndex = 0;
    private bool _timeForFreestyle = false;
    private bool _freestyleStarted = false;
    private bool _crowdReactionFlag = false;
    private bool _yourTurnFlag = false;
    private bool _ratingFlag = false;
    private bool _finalCrowdReaction = false;
    private bool _finalWordFlag = false;
    private bool _endGameFlag = false;
    private bool _intro2Flag = false;
    private bool _intro3Flag = false;
    private bool _intro4Flag = false;
    private bool _intro5Flag = false;
    private bool _intro6Flag = false;
    private bool _tryDuration = false;
    private bool _gameStarted = false;
    private float _tryDurationTimer = 50.0f;
    private bool _finalWord;
    private float _countdownTimer = 0.0f;
    private int _noteSpammed = 0;
    private float _freestyleDuration = 10.0f;

    // Use this for initialization
    void Start()
    {
        _audioSources = GameObject.FindWithTag("AudiosHolder").GetComponents<AudioSource>();
        _currentSequenceLength = _comSequenceLength[_comSequenceLengthIndex];
        _playerPlayCooldown = new float[0];
        _playerWaitTime = 0.0f;
        _totalPossibleScore = 40 * _playSequence.Length + 40;
        _audioSources[20].Play();
        _countdownTimer = 8.5f;
        _intro2Flag = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (_endGameFlag)
        {
            Debug.Log("Proceed to next level");
        }
        else if (_countdownTimer > 0)
        {
            _countdownTimer -= Time.deltaTime;
            if (_countdownTimer <= 0)
            {
                if (_crowdReactionFlag)
                {
                    _playerTurn = false;
                    _crowdReactionFlag = false;
                }
                else if (_yourTurnFlag)
                {
                    _audioSources[8].Play();
                    _playerTurn = true;
                    _yourTurnFlag = false;
                }
                else if (_finalCrowdReaction)
                {
                    _audioSources[11].Play();
                    _countdownTimer = 2.0f;
                    _ratingFlag = true;
                    _finalCrowdReaction = false;
                }
                else if (_ratingFlag)
                {
                    float rating = (float)_totalScore / (float)_totalPossibleScore;
                    if (rating == 1)
                    {
                        _audioSources[12].Play();
                        _finalWord = true;
                    }
                    else if (rating >= 0.9)
                    {
                        _audioSources[13].Play();
                        _finalWord = true;
                    }
                    else if (rating >= 0.7)
                    {
                        _audioSources[14].Play();
                        _finalWord = true;
                    }
                    else if (rating >= 0.5)
                    {
                        _audioSources[15].Play();
                        _finalWord = false;
                    }
                    else if (rating >= 0.3)
                    {
                        _audioSources[16].Play();
                        _finalWord = false;
                    }
                    else
                    {
                        _audioSources[17].Play();
                        _finalWord = false;
                    }
                    _countdownTimer = 1.5f;
                    _finalWordFlag = true;
                    _ratingFlag = false;
                }
                else if (_finalWordFlag)
                {
                    if (_finalWord)
                    {
                        _audioSources[18].Play();
                    }
                    else
                    {
                        _audioSources[19].Play();
                    }
                    _finalWordFlag = false;
                    _endGameFlag = true;
                }
                else if (_intro2Flag)
                {
                    _audioSources[8].Play();
                    _countdownTimer = 1.0f;
                    _intro3Flag = true;
                    _intro2Flag = false;
                }
                else if (_intro3Flag)
                {
                    _audioSources[21].Play();
                    _countdownTimer = 11.5f;
                    _intro4Flag = true;
                    _intro3Flag = false;
                }
                else if (_intro4Flag)
                {
                    _audioSources[9].Play();
                    _countdownTimer = 1.5f;
                    _intro5Flag = true;
                    _intro4Flag = false;
                }
                else if (_intro5Flag)
                {
                    _audioSources[22].Play();
                    _countdownTimer = 5.0f;
                    _tryDuration = true;
                    _intro5Flag = false;
                }
                else if (_intro6Flag)
                {
                    _audioSources[23].Play();
                    _countdownTimer = 3.0f;
                    _gameStarted = true;
                    _intro6Flag = false;
                }
            }
        }
        else if (_tryDuration)
        {
            _tryDurationTimer -= Time.deltaTime;
            if (Input.GetAxis("Vertical") > 0)
            {
                _audioSources[4].Play();
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                _audioSources[5].Play();
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                _audioSources[6].Play();
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                _audioSources[7].Play();
            }
            if (_tryDurationTimer <= 0)
            {
                _countdownTimer = 0.5f;
                _intro6Flag = true;
                _tryDuration = false;
            }
        }
        // If freestyle event is triggered, start the freestyle
        else if (_freestyleStarted)
        {
            _freestyleDuration -= Time.deltaTime;
            if (Input.GetAxis("Vertical") > 0)
            {
                _audioSources[4].Play();
                _noteSpammed += 1;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                _audioSources[5].Play();
                _noteSpammed += 1;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                _audioSources[6].Play();
                _noteSpammed += 1;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                _audioSources[7].Play();
                _noteSpammed += 1;
            }
            if (_freestyleDuration <= 0)
            {
                _freestyleStarted = false;
                if (_noteSpammed >= 50)
                {
                    _totalScore += 40;
                    _sectionScorePercentage = 1.0f;
                }
                else if (_noteSpammed > 40)
                {
                    _totalScore += 30;
                    _sectionScorePercentage = 0.75f;
                }
                else if (_noteSpammed > 30)
                {
                    _totalScore += 20;
                    _sectionScorePercentage = 0.5f;
                }
                else if (_noteSpammed > 20)
                {
                    _totalScore += 10;
                    _sectionScorePercentage = 0.25f;
                }
                else
                {
                    _sectionScorePercentage = 0.1f;
                }
                _audioSources[10].volume = _sectionScorePercentage;
                _audioSources[10].Play();
                _countdownTimer = 6.0f;
                _finalCrowdReaction = true;
            }
        }
        // Check if it's player's turn
        else if (!_playerTurn && _gameStarted)
        {

            // Check if it's time for the final freestyle event
            if (_timeForFreestyle)
            {
                _timeForFreestyle = false;
                _audioSources[9].Play();
                _freestyleStarted = true;
            }
            // If it's not freestyle time, the player is still in regular sequence play mode
            else
            {
                // _playerPlayCooldown is an array holding the perfect note intervals in the current sequence
                // when its length is 0 it means the array has not been set to the current sequence
                if (_playerPlayCooldown.Length == 0)
                {
                    // _length is the current sequence length
                    _length = _comSequenceLength[_comSequenceLengthIndex];
                    _playerPlayCooldown = new float[_length];

                    // _playerPlaySequence is an array holding the correct notes that should be played by the player
                    // in the current sequence
                    _playerPlaySequence = new int[_length];
                    Array.Copy(_comPlayCooldown, _currentArrayCopyIndex, _playerPlayCooldown, 0, _length);
                    Array.Copy(_playSequence, _currentArrayCopyIndex, _playerPlaySequence, 0, _length);
                    _currentArrayCopyIndex += _length;

                    // Calculate the duration to wait for the player to finish playing the current sequence
                    for (int i = 1; i < _playerPlayCooldown.Length; i++)
                    {
                        _playerWaitTime += _playerPlayCooldown[i];
                    }
                    _playerWaitTime += _badPlay;

                    // Calculate the total number of keys allowed to be input by the player in the current sequence
                    _playerAllowedKeys = _playerPlayCooldown.Length;

                    // Calculate the highest possible score obtainable in the current sequence
                    _sectionPossibleScore = 40 * _playerAllowedKeys;
                }

                // _comPlayCooldown is an array holding the note intervals between each note, when the current interval
                // decreases to below 0, play the current note and start decreasing the next interval if it's not the player's turn
                // _played is a check to prevent the note from being played twice
                _comPlayCooldown[_comPlayIndex] -= Time.deltaTime;
                if (_comPlayCooldown[_comPlayIndex] <= 0 && !_played)
                {
                    _played = true;
                    _audioSources[_playSequence[_comPlayIndex]].Play();

                    // When current sequence length drops to 0, it means the computer's sequence has ended it the player's turn should start
                    _currentSequenceLength--;
                    if (_currentSequenceLength == 0)
                    {
                        _countdownTimer = 3.0f;
                        _yourTurnFlag = true;

                        // Check if there are more sequence to be played after player's turn, if no, freestyle event should be triggered after player's turn
                        if (_comSequenceLengthIndex < _comSequenceLength.Length - 1)
                        {
                            _comSequenceLengthIndex++;
                            _currentSequenceLength = _comSequenceLength[_comSequenceLengthIndex];
                        }
                        else
                        {
                            _timeForFreestyle = true;
                        }
                    }

                    // Check if there is more items in the cooldown array, can probably get rid of this check and combines this code with the code above
                    if (_comPlayIndex < _comPlayCooldown.Length - 1)
                    {
                        _comPlayIndex++;
                    }
                    _played = false;
                }
            }

        }
        // If it's player's turn
        else if (_playerTurn && _gameStarted)
        {
            // Player has _waitForPlayerToStartTime amount of time to play the first note in the sequence, if player plays the right note during before this
            // time runs out, the player will score a perfect for this note. The player is considered to have started playing the current sequence once the
            // player plays a note in this duration, or after the duration runs out 
            if (!_playerStarted)
            {
                _waitForPlayerToStartTime -= Time.deltaTime;
                if (Input.GetAxis("Vertical") > 0)
                {
                    _audioSources[4].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 0)
                    {
                        _sectionScore += 40;
                        _totalScore += 40;
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerStarted = true;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    _audioSources[5].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 1)
                    {
                        _sectionScore += 40;
                        _totalScore += 40;
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerStarted = true;
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    _audioSources[6].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 2)
                    {
                        _sectionScore += 40;
                        _totalScore += 40;
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerStarted = true;
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    _audioSources[7].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 3)
                    {
                        _sectionScore += 40;
                        _totalScore += 40;
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerStarted = true;
                }
                else if (_waitForPlayerToStartTime <= 0)
                {
                    _playerStarted = true;
                }
            }
            // Enter this conditional block after player has started playing the current sequence
            else
            {
                // The player is only allowed to play certain number of notes in the current turn, and the player is only allowed certain amount
                // of time to play them, if either runs out, the player's turn ends
                _playerWaitTime -= Time.deltaTime;

                // _playerNextPlayTime calculates the interval between 2 notes the player played. The interval is then used to compare with the supposedly
                // perfect interval to check how many score the player earns.
                _playerNextPlayTime += Time.deltaTime;
                if (Input.GetAxis("Vertical") > 0)
                {
                    _audioSources[4].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 0)
                    {
                        _playerTimingDifference = Math.Abs(_playerNextPlayTime - _playerPlayCooldown[_playerPlayIndex]);
                        if (_playerTimingDifference <= _perfectPlay)
                        {
                            _sectionScore += 40;
                            _totalScore += 40;
                        }
                        else if (_playerTimingDifference <= _greatPlay)
                        {
                            _sectionScore += 30;
                            _totalScore += 30;
                        }
                        else if (_playerTimingDifference <= _nicePlay)
                        {
                            _sectionScore += 20;
                            _totalScore += 20;
                        }
                        else if (_playerTimingDifference <= _badPlay)
                        {
                            _sectionScore += 10;
                            _totalScore += 10;
                        }
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerNextPlayTime = 0.0f;
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    _audioSources[5].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 1)
                    {
                        _playerTimingDifference = Math.Abs(_playerNextPlayTime - _playerPlayCooldown[_playerPlayIndex]);
                        if (_playerTimingDifference <= _perfectPlay)
                        {
                            _sectionScore += 40;
                            _totalScore += 40;
                        }
                        else if (_playerTimingDifference <= _greatPlay)
                        {
                            _sectionScore += 30;
                            _totalScore += 30;
                        }
                        else if (_playerTimingDifference <= _nicePlay)
                        {
                            _sectionScore += 20;
                            _totalScore += 20;
                        }
                        else if (_playerTimingDifference <= _badPlay)
                        {
                            _sectionScore += 10;
                            _totalScore += 10;
                        }
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerNextPlayTime = 0.0f;
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    _audioSources[6].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 2)
                    {
                        _playerTimingDifference = Math.Abs(_playerNextPlayTime - _playerPlayCooldown[_playerPlayIndex]);
                        if (_playerTimingDifference <= _perfectPlay)
                        {
                            _sectionScore += 40;
                            _totalScore += 40;
                        }
                        else if (_playerTimingDifference <= _greatPlay)
                        {
                            _sectionScore += 30;
                            _totalScore += 30;
                        }
                        else if (_playerTimingDifference <= _nicePlay)
                        {
                            _sectionScore += 20;
                            _totalScore += 20;
                        }
                        else if (_playerTimingDifference <= _badPlay)
                        {
                            _sectionScore += 10;
                            _totalScore += 10;
                        }
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerNextPlayTime = 0.0f;
                }
                else if (Input.GetAxis("Horizontal") > 0)
                {
                    _audioSources[7].Play();
                    if (_playerPlaySequence[_playerPlayIndex] == 3)
                    {
                        _playerTimingDifference = Math.Abs(_playerNextPlayTime - _playerPlayCooldown[_playerPlayIndex]);
                        if (_playerTimingDifference <= _perfectPlay)
                        {
                            _sectionScore += 40;
                            _totalScore += 40;
                        }
                        else if (_playerTimingDifference <= _greatPlay)
                        {
                            _sectionScore += 30;
                            _totalScore += 30;
                        }
                        else if (_playerTimingDifference <= _nicePlay)
                        {
                            _sectionScore += 20;
                            _totalScore += 20;
                        }
                        else if (_playerTimingDifference <= _badPlay)
                        {
                            _sectionScore += 10;
                            _totalScore += 10;
                        }
                    }
                    _playerAllowedKeys--;
                    _playerPlayIndex++;
                    _playerNextPlayTime = 0.0f;
                }

                // Check if the player's turn should end
                if (_playerWaitTime <= 0 || _playerAllowedKeys <= 0)
                {
                    // Upon ending player's turn, reset everything for the current player sequence
                    _playerPlayCooldown = new float[0];
                    _sectionScorePercentage = (float)_sectionScore / (float)_sectionPossibleScore;
                    _sectionScore = 0;
                    _playerStarted = false;
                    _waitForPlayerToStartTime = 4.0f;
                    _playerPlayIndex = 0;
                    if (_sectionScorePercentage == 0)
                    {
                        _sectionScorePercentage = 0.1f;
                    }
                    _audioSources[10].volume = _sectionScorePercentage;
                    _audioSources[10].Play();
                    _crowdReactionFlag = true;
                    _countdownTimer = 6.0f;
                }
            }
        }
    }
}
