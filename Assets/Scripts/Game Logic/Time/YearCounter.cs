using UnityEngine;

namespace Game_Logic
{
    public class YearCounter : MonoBehaviour
    {
        private static bool IsPaused;
        private static bool TimeIsGoingBackwards ;
        
        private static float _currentTimePassed = 0;
        private static float _defaultSecondsPerYear = 1.5f;        
        private static float _secondsPerYear = _defaultSecondsPerYear;

        private static int _minimumYear = 1945;
        private static int _maximumYear = 2012;
        private static int _currentYear = _minimumYear;


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
                    Debug.Log("_secondsPerYear = " + _secondsPerYear);

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
        

        private static void SpeedTimeUp()
        {
            //Time is sped up, so we spend less seconds per year. 
            _secondsPerYear = _secondsPerYear / 2;
        }
    }
}