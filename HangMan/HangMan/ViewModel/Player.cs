using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangMan.ViewModel
{
    class Player
    {
        public string username { get; set; }
        public int games { get; set; }
        public int gamesWon { get; set; }
        public  int lives { get; set; }
        public Player()
        {
            username = "";
            games = 0;
            gamesWon = 0;
        }
        public Player(string name, int gaWon, int game)
        {
            username = name;
            gamesWon = gaWon;
            games = game;
        }
       
    }
}
