﻿using UnityEngine;

namespace Game_Logic
{
    public class YearCounter : MonoBehaviour
    {
        private static bool IsPaused;
        private static bool TimeIsGoingBackwards ;
        
        private static float _currentTimePassed = 0;
        private static float _defaultSecondsPerYear = 1.5f;        
        private static float _secondsPerYear = _defaultSecondsPerYear;

        public static readonly int MinimumYear = 1945;
        public static readonly int MaximumYear = 2012;
        private static int _currentYear = MinimumYear;


        public static void Play()
        {
            IsPaused = false;
            _secondsPerYear = _defaultSecondsPerYear;
            TimeIsGoingBackwards = false;
        }

        public static void Pause()
        {
            IsPaused = true;
        }
        
        public static void FastForward()
        {
            TimeIsGoingBackwards = false;
            
            SpeedTimeUp();
        }

        public static void FastBackward()
        {
            TimeIsGoingBackwards = true;
            
            SpeedTimeUp();
        }
        
        //Only a getter, we do not want the year variable to be changed from outside this script.
        public static int GetCurrentYear()
        {
            return _currentYear;
        }
           
        
        
        private void Update()
        {
            if (!IsPaused)
            {
                _currentTimePassed += Time.deltaTime;

                if (_currentTimePassed >= _secondsPerYear)
                {
                    _currentTimePassed = 0.0f;

                    if(TimeIsGoingBackwards)
                        DecrementYear();
                    else
                        IncrementYear();
                }
            }
        }

        private static void IncrementYear()
        {
            if (_currentYear < MaximumYear)
                _currentYear++;
        }

        private static void DecrementYear()
        {
            if (_currentYear > MinimumYear)
                _currentYear--;
        }
        

        private static void SpeedTimeUp()
        {
            //Time is sped up, so we spend less seconds per year. 
            _secondsPerYear = _secondsPerYear / 2;
        }
    }
}