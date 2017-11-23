using UnityEngine;

namespace Game_Logic
{
    public class YearCounter : MonoBehaviour
    {
        private static bool IsPaused = false;
        private static bool TimeIsGoingBackwards = false;
        
        private static float _currentTimePassed = 0;
        private static int _secondsPerYear = 8;

        private static int _minimumYear = 1945;
        private static int _maximumYear = 2012;
        private static int _currentYear = _minimumYear;


        public static void Play()
        {
            IsPaused = false;
            TimeIsGoingBackwards = false;
        }

        public static void Pause()
        {
            IsPaused = true;
        }
        
        public static void FastForward()
        {
            TimeIsGoingBackwards = false;
            
            SpeedTimeUp(3);
        }

        public static void FastBackward()
        {
            TimeIsGoingBackwards = true;
            
            SpeedTimeUp(3);
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
            if (_currentYear < _maximumYear)
                _currentYear++;
        }

        private static void DecrementYear()
        {
            if (_currentYear > _minimumYear)
                _currentYear--;
        }
        
        private static void SlowTimeDown(int slowDownFactor)
        {
            _secondsPerYear = _secondsPerYear / slowDownFactor;
        }

        private static void SpeedTimeUp(int speedUpFactor)
        {
            _secondsPerYear = _secondsPerYear * speedUpFactor;
        }
    }
}