//Canberk Saka
//B211200005
//Bilişim Sistemleri Mühendisliği
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace PingPongOyunu
{
    public class PingPongGame
    {
        
        const int KEY_STATE = 0x8000;

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]

        public static extern short GetAsyncKeyState(char vKey);

        static private bool IsKey(char key)
        {
            return (GetAsyncKeyState(key) & KEY_STATE) != 0;
        }

        static private bool IsKey(ConsoleKey key)
        {

            return IsKey((char)key);      
        }

        

        

        const int GolFPS = 1000 / 30;
        const int CubukGuncelle = 2;
        const int cubukHizi = 2;
            
        const int SahaUzunlugu = 30;
        const int SahaGenisligi = 55;

        Random random;
        int loopTicks;

        int sCubukX, sCubukY, sCubukSonY;
        int saCubukX, saCubukY, saCubukSonY;

        sbyte SolCubukDegis, SagCubukDegis;
        
        short lScore, rScore;

        bool topBaslatılsınMı;
        int topX, topY;
        int topVelx, topVely;
        int topSonX, topSonY;

        bool CalısıyorMu;

       

        public void Init()
        {
            random = new Random(DateTime.Now.Millisecond);

            Console.CursorVisible = false;
            Console.WindowHeight = SahaUzunlugu;
            Console.WindowWidth = SahaGenisligi * 2 + 5;
            CalısıyorMu = true;
            loopTicks = 0;

            sCubukX =  2;
            sCubukY = sCubukSonY = SahaUzunlugu / 2;

            saCubukX = SahaGenisligi - 2;
            saCubukY = saCubukSonY = SahaUzunlugu / 2;

            topBaslatılsınMı = true;
            TopuYenidenBaslat();
            topX = SahaGenisligi / 2;
            topY = SahaUzunlugu / 2;
            topVelx = topVely = 1;


        }
        public void Baslat ()
        {
            while (CalısıyorMu)
            {
                DegerAl();
                SahayıYaz();
                CubugkHaraket();
                CubukSınırları();

                TopHaraket();
                TopCıkma();
                TopYansıma();
                TopuYenidenBaslat();

                Render();

                Thread.Sleep(GolFPS);
                loopTicks++;
            }

        }

        public void SahayıYaz()
        {
            for (int i = 0; i <= SahaGenisligi * 2; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("-");
            }
            for (int i = 0; i <= SahaGenisligi * 2; i++)
            {
                Console.SetCursorPosition(i, SahaUzunlugu);
                Console.Write("-");
            }
            for (int i = 0; i < SahaUzunlugu; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("|");
            }
            for (int i = 0; i < SahaUzunlugu; i++)
            {
                Console.SetCursorPosition(SahaGenisligi * 2 + 1, i);
                Console.Write("|");
            }

        }
       

        private void DegerAl()
        {
            if (IsKey(ConsoleKey.Escape))
            {
                CalısıyorMu = false;
            }
            if (IsKey(ConsoleKey.W))
            {
                SolCubukDegis--;
            }
            if (IsKey(ConsoleKey.S))
            {
                SolCubukDegis++;
            }
            if (IsKey(ConsoleKey.UpArrow))
            {
                SagCubukDegis--;
            }
            if (IsKey(ConsoleKey.DownArrow))
            {
                SagCubukDegis++;
            }
        }
       
        private void CubugkHaraket()
        {
            if (loopTicks % CubukGuncelle == 0)
            {
                if (SolCubukDegis != 0)
                {
                    sCubukY += SolCubukDegis < 0 ? -cubukHizi : cubukHizi;
                    SolCubukDegis = 0;
                }
                if (SagCubukDegis != 0)
                {
                    saCubukY += SagCubukDegis < 0 ? -cubukHizi : cubukHizi;
                    SagCubukDegis = 0;
                }
            }
        }
       
        private void CubukSınırları()
        {

            void Belirle(ref int cubukY)
            {

                if (cubukY - 2 <= 0)
                {
                    cubukY = 2;
                }
                if (cubukY - 2 >= SahaUzunlugu)
                {
                    cubukY = SahaUzunlugu - 3;

                }
                Belirle(ref sCubukY);
                Belirle(ref saCubukY);
            }

            
        }
        private void TopHaraket()
        {

            topX += topVelx;
            topY += topVely;

        }
        private void TopYansıma()
        {

            bool CubugaCarma(int x, int y)
            {

                return topX == x && topY >= y - 2 && topY <= y + 2;
            }
            if (topX < 1 || CubugaCarma(sCubukX, sCubukY))  topVelx = 1;
            if (topX > SahaGenisligi || CubugaCarma(saCubukX, saCubukY))  topVelx = -1;

            if (topY < 1) topVely = 1;
            if (topY >= SahaUzunlugu - 1) topVely = -1;

        }
        private void TopCıkma()
        {
            if (topX == 0)
            {
                topBaslatılsınMı = true;
                rScore++;
            }
            if (topX == SahaGenisligi)
            {
                topBaslatılsınMı = true;
                lScore++;
            }

        }
        private void TopuYenidenBaslat()
        {
            if (!topBaslatılsınMı) return;
            topBaslatılsınMı = false;

            topX = SahaGenisligi / 2;
            topY = SahaUzunlugu / 2;

            topVelx = random.NextDouble() < 0.5 ? -1 : 1;
            topVely = random.NextDouble() < 0.5 ? -1 : 1;


        }
        private void Render()
        {

            void CubukCiz(int x, int y)
            {


                y -= 2;
                x *= 2;

                Console.SetCursorPosition(x, y++); Console.WriteLine("|"); 
                Console.SetCursorPosition(x, y++); Console.WriteLine("|"); 
                Console.SetCursorPosition(x, y++); Console.WriteLine("|"); 
                Console.SetCursorPosition(x, y++); Console.WriteLine("|"); 
                Console.SetCursorPosition(x, y);   Console.WriteLine("|"); 

            }

            void CubukSil(int x, int y, ref int sonY)
            {
                
                
                if (y != sonY)
                {
                    x *= 2;
                                                                                                            
                   
                    int a = sonY < y ? -1 : 1;

                    Console.SetCursorPosition(x, sonY + a); Console.Write(" ");
                    Console.SetCursorPosition(x, sonY + a * 2); Console.Write(" ");


                    sonY = y;
                }

            }

            CubukSil(sCubukX, sCubukY, ref sCubukSonY);
            CubukSil(saCubukX, saCubukY, ref saCubukSonY);

            Console.SetCursorPosition(topSonX * 2, topSonY); Console.Write(" ");
            topSonX = topX; topSonY = topY;

            CubukCiz(sCubukX, sCubukY);
            CubukCiz(saCubukX, saCubukY);

            Console.CursorVisible = false;
            Console.SetCursorPosition(topX * 2, topY); Console.Write("O");
        }
        
        
    }
}

   
