using System.Collections;

namespace HighScoreAndNameStruct
{
    [System.Serializable]
    public struct HighScoreAndName
    {
        public int score;
        public string name;

        public HighScoreAndName(int _score, string _name)
        {
            score = _score;
            name = _name;
        }

        public void SetScore(int _score)
        {
            score = _score;
        }

        public void AddScore(int amount)
        {
            score += amount;
        }

        public void SetName(string _name)
        {
            name = _name;
        }
    }
}
