using System;
using System.Collections.Generic;
using System.Text;

namespace YatzyLibrary
{
    public class Player
    {
        public int[] _scoreTable { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }

        public Player()
        {
            _scoreTable = new int[18];

            //Lägg till random data i tabellen
            //DummyData();
        }

        private void DummyData()
        {
            Random random = new Random();
            for (int i = 0; i < _scoreTable.Length; i++)
            {
                _scoreTable[i] = random.Next(1, 20);
            }
        }
    }
}
