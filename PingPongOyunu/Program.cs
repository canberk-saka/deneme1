//Canberk Saka
//B211200005
//Bilişim Sistemleri Mühendisliği
using System;

namespace PingPongOyunu
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(30, 30);
            Console.CursorVisible = false;

            var game = new PingPongGame();
            game.Init();
            game.Baslat();
        }
    }
}
