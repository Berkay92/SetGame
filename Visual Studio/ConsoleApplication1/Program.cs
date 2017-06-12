using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO; 
using System.Threading;  

namespace set_game
{
    class Program
    {
        static string set_elements = "ABCDEFGH";  
        static bool timeflag = true;
        static int result = 0;
        static char first, middle, last;
        static bool vertical = false, horizontal = false;
        static int delete_position_x, delete_position_y;
        static int score = 0;
        static int round = 1;


        static void Main(string[] args)
        {
            //Console.ForegroundColor = ConsoleColor.White; gereksiz
            StreamReader file = File.OpenText("oyunAlani.txt");
            string screen_file = file.ReadToEnd();     
            int[,] sets = new int[8, 8];
            char[,] screen = new char[15, 25];
            int position_x, position_y;
            char[,] screen_wall = new char[15, 25]; // harfleri rastgele etrafı boşlukşeklinde atmaya çalıştığında hiç yer kalmamışsa ilk halini kullanmak için
            Console.CursorVisible = false;
            int screen_index = 0;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    screen[i, j] = screen_file[screen_index++];
                }
                screen_index = screen_index + 2;
            }
            screen_wall = screen; // harfleri rastgele etrafı boşlukşeklinde atmaya çalıştığında hiç yer kalmamışsa ilk halini kullanmak için
            Random rnd = new Random();
            int setcount = 0;
            char x = 'X';
            for (int i = 0; i < 8; i++)
            {
                setcount = rnd.Next(4, 8);               //kümelerin eleman sayılarını belirliyoruz 

                for (int j = 0; j < setcount; j++)
                {
                    int k = 0;
                    sets[i, j] = rnd.Next(1, 20);        // kümelerin içindeki elemanlarının sayılarını belirliyoruz
                    while ((k <= j) && (j != 0))
                    {

                        while ((sets[i, j] == sets[i, k]) && (k < j))  // aynı elemanlar olmaması için
                        {
                            sets[i, j] = rnd.Next(1, 20);
                        }
                        k++;

                    }
                }
            }
            string str = "AABBCCDDEEFFGGHH++--nnuu";
            for (int i = 0; i < 24; i++)
            {
                int count = 0;
                position_x = rnd.Next(0, 15);
                position_y = rnd.Next(0, 25);
                while (screen[position_x, position_y] != ' ' || screen[position_x + 1, position_y] != ' ' || screen[position_x - 1, position_y] != ' ' || screen[position_x, position_y + 1] != ' ' || screen[position_x, position_y - 1] != ' ' || screen[position_x + 1, position_y + 1] != ' ' || screen[position_x + 1, position_y - 1] != ' ' || screen[position_x - 1, position_y - 1] != ' ' || screen[position_x - 1, position_y + 1] != ' ')
                {
                    count++;
                    position_x = rnd.Next(0, 15);
                    position_y = rnd.Next(0, 25);
                    if (count > 2000) ///////////////////////////////////////
                    {
                        screen = screen_wall;
                        i = 0;
                        break; 
                    }
                }
                screen[position_x, position_y] = str[i];

            }
            position_x = rnd.Next(0, 15);
            position_y = rnd.Next(0, 25);//Xin rastgele screena yazdırılması

            while ((screen[position_x, position_y] != ' ') && (screen[position_x, position_y] == '#'))
            {
                position_x = rnd.Next(0, 15);
                position_y = rnd.Next(0, 25);

            }
            int Xposition_x = 0, Xposition_y = 0;
            Xposition_x = position_x; ////////////////////// Neden yeni değişkene atadık
            Xposition_y = position_y;
            screen[position_x, position_y] = x;

            System.Timers.Timer t = new System.Timers.Timer(1000);
            t.Elapsed += t_Elapsed;
            t.Start();

            timeflag = false;  //////////////////////////

            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (screen[i, j] == screen[Xposition_x, Xposition_y])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(screen[i, j]); 
                        //Console.ForegroundColor = ConsoleColor.White; ////////////////
                    }
                    else if (screen[i, j] == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(screen[i, j]);
                        //Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(screen[i, j]);
                        //Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(screen[i, j]);
                        //Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.Write("\n");
            }
            timeflag = true;  ///////////////////////////////

            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(27, 0);
            Console.Write("Sets");
            Console.SetCursorPosition(27, 1);
            Console.Write("---------------------------------");
            Console.SetCursorPosition(27, 12);
            Console.Write("Expressions:");
            Console.SetCursorPosition(27, 13);
            Console.Write("---------------------------------");
            Console.SetCursorPosition(0, 18);
            Console.Write("Remaining Time:");
            Console.SetCursorPosition(0, 19);
            Console.Write("Score:");
            Console.SetCursorPosition(0, 20);
            Console.Write("Round:");
            //Console.ForegroundColor = ConsoleColor.White; //////////////////////
            for (int i = 0; i < 16; i = i + 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(27, (i / 2) + 2);  // Expression altına A,B,C,D,E,F,G,H yazdırma
                Console.Write(str[i]);
                //Console.ForegroundColor = ConsoleColor.White;  /////////////////
            }
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(28, i + 2); // Expression A,B,C,D,E,F,G,H yanlarına "{" eklemek
                Console.Write("={");

                for (int j = 0; j < 8; j++)
                {
                    if (sets[i, j] != 0) // ----------->Virgül için
                    {
                        if (sets[i, j + 1] == 0) // ---> Bir sonraki satıra virgül getirmemek için
                        {
                            Console.Write(sets[i, j]);
                        }
                        else
                        {
                            Console.Write(sets[i, j] + ",");
                        }
                    }
                }
                Console.Write("}");
            }

            if (time == 0) //////////////////////// Burdaki if e tekrar nasıl dönüyor herhangi bir döngünün içinde değil
            {
                Console.SetCursorPosition(27, 12 + round);
                Console.Write("{0}.", round);
                round++;
                Console.SetCursorPosition(6, 19);
                Console.Write(score);

                timeflag = false;
                Console.SetCursorPosition(0, 0);
                //Console.Write(screen);
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        if (screen[i, j] == screen[Xposition_x, Xposition_y])
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(screen[i, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (screen[i, j] == '#')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(screen[i, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(screen[i, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(screen[i, j]);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    Console.Write("\n");
                }
                timeflag = true;

            }

          

            //////////////////////////////////////////////timeyeni//////////////////////
            /*
             TimeSpan fark = DateTime.Now - şimdiki_zaman_sabit;
             
            if (gerisayım > 0)
            {
                 fark = DateTime.Now - şimdiki_zaman_sabit;

                Console.SetCursorPosition(15, 18);

                gerisayım = 50 - fark.Seconds;
                Console.Write(gerisayım);
                Console.CursorVisible = false;

           ///////////////////////////////////////////////////////////////////////////////
            }*/



           /* if ((DateTime.Now - şimdiki_zaman_sabit).Seconds == 1)
                time_key = false;
            else time_key = true;*/

            bool exit = false;
            while (!exit)
            {

                /*if (gerisayım > 0)///////////////////// fonksiyon olmadan time ı kullandık yalnız zaman akışı düzgün ilerlemesine rağmen. program seri olarak aktığı için zamanı ekrana basman için hersefereinde cursor tuşuna basmamızı bekliyordu. paralel olarak ancak fonsiyonla gösterebildik.
                {

                    fark = DateTime.Now - şimdiki_zaman_sabit;
                    Console.SetCursorPosition(15, 18);

                    gerisayım = 50 - (DateTime.Now - şimdiki_zaman_sabit).Seconds;
                    Console.Write(gerisayım);

                    ///////////////////////////////////////////////////////////////////////////////
                }*/
                //////////////////////////////////////////////////////////////////////******CONTROL FONKSİYONUN İÇİ/////////////////////////////////////////////////////////////////////////////////
                //int x_pozition;// sınır 28e 373  
                for (position_x = 0; position_x < 15; position_x++)
                    for (position_y = 0; position_y < 25; position_y++)
                    {

                        if (((screen[position_x, position_y] == 'A' || screen[position_x, position_y] == 'B' || screen[position_x, position_y] == 'C' || screen[position_x, position_y] == 'D' || screen[position_x, position_y] == 'E' || screen[position_x, position_y] == 'F' || screen[position_x, position_y] == 'G' || screen[position_x, position_y] == 'H') && (screen[position_x, position_y + 1] == '+' || screen[position_x, position_y + 1] == '-' || screen[position_x, position_y + 1] == 'u' || screen[position_x, position_y + 1] == 'n') && (screen[position_x, position_y + 2] == 'A' || screen[position_x, position_y + 2] == 'B' || screen[position_x, position_y + 2] == 'C' || screen[position_x, position_y + 2] == 'D' || screen[position_x, position_y + 2] == 'E' || screen[position_x, position_y + 2] == 'F' || screen[position_x, position_y + 2] == 'G' || screen[position_x, position_y + 2] == 'H')) || ((screen[position_x, position_y] == 'A' || screen[position_x, position_y] == 'B' || screen[position_x, position_y] == 'C' || screen[position_x, position_y] == 'D' || screen[position_x, position_y] == 'E' || screen[position_x, position_y] == 'F' || screen[position_x, position_y] == 'G' || screen[position_x, position_y] == 'H') && (screen[position_x + 1, position_y] == '+' || screen[position_x + 1, position_y] == '-' || screen[position_x + 1, position_y] == 'u' || screen[position_x + 1, position_y] == 'n') && (screen[position_x + 2, position_y] == 'A' || screen[position_x + 2, position_y] == 'B' || screen[position_x + 2, position_y] == 'C' || screen[position_x + 2, position_y] == 'D' || screen[position_x + 2, position_y] == 'E' || screen[position_x + 2, position_y] == 'F' || screen[position_x + 2, position_y] == 'G' || screen[position_x + 2, position_y] == 'H')))
                        { // yukardaki satırda horizontal veya vertical 3lü varmı diye bakılıyor
                            /*
                            Console.SetCursorPosition(0, 0);
                            Console.Write(screen);*/

                            if ((position_x < 24) && (screen[position_x, position_y] == 'A' || screen[position_x, position_y] == 'B' || screen[position_x, position_y] == 'C' || screen[position_x, position_y] == 'D' || screen[position_x, position_y] == 'E' || screen[position_x, position_y] == 'F' || screen[position_x, position_y] == 'G' || screen[position_x, position_y] == 'H') && (screen[position_x, position_y + 1] == '+' || screen[position_x, position_y + 1] == '-' || screen[position_x, position_y + 1] == 'u' || screen[position_x, position_y + 1] == 'n') && (screen[position_x, position_y + 2] == 'A' || screen[position_x, position_y + 2] == 'B' || screen[position_x, position_y + 2] == 'C' || screen[position_x, position_y + 2] == 'D' || screen[position_x, position_y + 2] == 'E' || screen[position_x, position_y + 2] == 'F' || screen[position_x, position_y + 2] == 'G' || screen[position_x, position_y + 2] == 'H'))
                            { // horizontal olma koşulu

                                if (screen[position_x, position_y] != screen[position_x, position_y + 2]) // ağer iki aynı setle işlem yapılmak istenirse bu durumu önlemek için
                                {
                                    first = screen[position_x, position_y];
                                    middle = screen[position_x, position_y + 1];
                                    last = screen[position_x, position_y + 2];
                                    horizontal = true;
                                }
                            }

                            else // horizontal değilse 3lü olduğunu bildiğimiz için vertical 3lü vardır
                            {
                                if (screen[position_x, position_y] != screen[position_x + 2, position_y] && position_x < 14)
                                {
                                    first = screen[position_x, position_y];
                                    middle = screen[position_x + 1, position_y];
                                    last = screen[position_x + 2, position_y];
                                    vertical = true;
                                }
                            }

                            delete_position_x = position_x; // horizontal veya vertical oluşturduğumuz setlerin başlangıç noktalarını referans alarak sileceğiz
                            delete_position_y = position_y;

                            if (horizontal || vertical)
                            {
                                int[] union_set = new int[16];
                                int[] intersection_set = new int[16];
                                int[] difference_set = new int[16];
                                int[] sym_set = new int[16];
                                if (horizontal)
                                {
                                    screen[delete_position_x, delete_position_y] = ' ';
                                    screen[delete_position_x, delete_position_y + 1] = ' ';
                                    screen[delete_position_x, delete_position_y + 2] = ' ';
                                    timeflag = false;  ////////////////////////
                                    Console.SetCursorPosition(0, 0);

                                    for (int i = 0; i < 15; i++)
                                    {
                                        for (int j = 0; j < 25; j++)
                                        {
                                            if (screen[i, j] == screen[Xposition_x, Xposition_y])
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else if (screen[i, j] == '#')
                                            {
                                                Console.ForegroundColor = ConsoleColor.Blue;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                                            {
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                        }

                                        Console.Write("\n");
                                    }
                                    timeflag = true; ////////////////7
                                    horizontal = false;
                                }
                                if (vertical)
                                {
                                    screen[delete_position_x, delete_position_y] = ' ';
                                    screen[delete_position_x + 1, delete_position_y] = ' ';
                                    screen[delete_position_x + 2, delete_position_y] = ' ';
                                    timeflag = false; /////////////////////////
                                    Console.SetCursorPosition(0, 0);
                                    for (int i = 0; i < 15; i++)
                                    {
                                        for (int j = 0; j < 25; j++)
                                        {
                                            if (screen[i, j] == screen[Xposition_x, Xposition_y])
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else if (screen[i, j] == '#')
                                            {
                                                Console.ForegroundColor = ConsoleColor.Blue;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                                            {
                                                Console.ForegroundColor = ConsoleColor.White;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.Write(screen[i, j]);
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                        }
                                        Console.Write("\n");
                                    }
                                    timeflag = true;
                                    vertical = false;
                                }

                                if (middle == 'u')
                                {
                                    //union
                                    int index1 = 0, index2 = 0, index = 0, counter = 0;
                                    result = 0;
                                    
                                    for (int i = 0; i < set_elements.Length; i++)
                                    {
                                        if (first == set_elements[i])   // AuB gibi ifadede ilk A, son B olacak.
                                            index1 = i;    // S stringdeki aradığımız ilk indexi ( örnekte A nın indexi olan 0 ı index1 e atar)
                                        if (last == set_elements[i])
                                            index2 = i;   // S stringdeki aradığımız son indexi ( örnekte B nin indexi olan 1 i index2 ye atar)
                                    }
                                    for (int i = 0; i < 8; i++)
                                    {                            
                                        if (sets[index1, i] != 0) // AuB örneğinde bu satırda A nın içindeki elemanları teker teker oluşturduğum yeni kümeye ekliyorum
                                        {
                                            union_set[index++] = sets[index1, i];
                                            counter++;
                                        }
                                    }

                                    for (int i = 0; i < 8; i++)
                                    {
                                        bool union_flag = false;
                                        if (sets[index2, i] != 0) // AuB örneğinde Bnin içindeki elemanları aşağıdaki for döngüsü içerisinde   
                                        {
                                            for (int j = 0; j < counter; j++)
                                            {
                                                if (sets[index2, i] != union_set[j])  // ikinci kümedeki elemanlarla ilk kümeden yeni_küme_dizisine attığım tüm elemanları teker teker kontrol ediyor aynı olursa flag false olacak
                                                    union_flag = true;  //////// true olunca direkt döngüden çıkmıyor
                                                else
                                                {
                                                    union_flag = false;   
                                                    break;
                                                }
                                            }
                                       
                                            if (union_flag == true)
                                                union_set[index++] = sets[index2, i]; // yeni küme dizisine A yı atmıştık, kaldığımız yerden B yi atmaya devam edicek
                                            union_flag = false;
                                        }
                                    }

                                    if (round <10) 
                                    {

                                        Console.SetCursorPosition(27, 13 + round);
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("{0}.{1}u{2}=", round, first, last);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.SetCursorPosition(33, 13 + round);
                                        Console.Write("{");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (union_set[i] != 0 && union_set[i + 1] != 0)// son elemana kadar yazdırıyor//union kümeisnin screena yazdırılması
                                                Console.Write("{0},", union_set[i]);
                                            if (union_set[i] != 0 && union_set[i + 1] == 0)// son elemandan sonra virgül gelmemesi için özel durum 
                                                Console.Write("{0}", union_set[i]);
                                        }
                                        Console.Write("}");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (union_set[i] != 0) //iki kümenin union elamanlarının toplanması
                                                result += union_set[i];
                                        }
                                        Console.Write(" --> {0}", result);
                                        Console.SetCursorPosition(6, 19);
                                        Console.Write(score); 
                                        Console.SetCursorPosition(6, 20);
                                        Console.Write(round); 
                                        round++;
                                    }

                                    score += result + (time * 2);
                                    time = 50;
                                    Console.SetCursorPosition(6, 19);
                                    Console.Write(score);
                                }
                                    // intersection
                                else if (middle == 'n')
                                {

                                    int index1 = 0, index2 = 0, index = 0;
                                    result = 1;
                                    
                                    for (int i = 0; i < set_elements.Length; i++)
                                    {
                                        if (first == set_elements[i])  //union işleminde yaptığımız işlemin aynısı
                                            index1 = i;
                                        if (last == set_elements[i])
                                            index2 = i;
                                    }
                                    for (int i = 0; i < 8; i++)
                                        for (int j = 0; j < 8; j++)
                                        {
                                            if ((sets[index1, i] == sets[index2, j]) && (sets[index2, j] != 0)) //// kesişim elemanları belirleniyor
                                                intersection_set[index++] = sets[index2, j];
                                        }
                                    if (round < 10)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.SetCursorPosition(27, 13 + round);
                                        Console.Write("{0}.{1}n{2}=", round, first, last);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.SetCursorPosition(33, 13 + round);
                                        Console.Write("{");
                                        for (int i = 0; i < index; i++)
                                        {
                                            if (intersection_set[i] != 0 && intersection_set[i + 1] != 0)// intersection kümesinin screena yazdırılması (son eleman hariç)
                                                Console.Write("{0},", intersection_set[i]);
                                            if (intersection_set[i] != 0 && intersection_set[i + 1] == 0)// son elemandan sonra virgül gelmemesi için özel durum 
                                                Console.Write("{0}", intersection_set[i]);
                                        }
                                        Console.Write("}");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (intersection_set[0] == 0) // Bu kısım kesişimler yoksa sonucu 1 çıkarmasın diye --> direkt sıfır
                                            {
                                                result = 0;
                                                break;
                                            }
                                            else if (intersection_set[i] != 0) //iki kümenin kesişiminden çıkan sonuçların çarpılması
                                                result *= intersection_set[i];
                                        }
                                        Console.Write(" -->{0}", result);
                                        Console.SetCursorPosition(6, 19);
                                        Console.Write(score);
                                        Console.SetCursorPosition(6, 20);
                                        Console.Write(round);
                                        round++;
                                    }

                                    score += result + (time * 2);
                                    time = 50;
                                    Console.SetCursorPosition(6, 19);
                                    Console.Write(score);
                                }
                                //difference
                                else if (middle == '-')
                                {
                                    int index1 = 0, index2 = 0, index = 0;
                                    
                                    ///int[] intersect = Intersection(sets, ilk, son);
                                    for (int i = 0; i < set_elements.Length; i++)
                                    {
                                        if (first == set_elements[i])  //bu kısımlar yine aynı mantık
                                            index1 = i;
                                        if (last == set_elements[i])
                                            index2 = i;
                                    }
                                    bool yaz = false;
                                    //////////////////////////intersection////////////////
                                    for (int i = 0; i < 8; i++)
                                        for (int j = 0; j < 8; j++)
                                        {
                                            if ((sets[index1, i] == sets[index2, j]) && (sets[index2, j] != 0)) //// kesişim elemanları belirleniyor 
                                                intersection_set[index++] = sets[index2, j];
                                        }
                                    index = 0;
                                    ///////////////////////////////////////////////
                                    for (int i = 0; i < 8; i++)
                                    {
                                        for (int j = 0; j < 8; j++)
                                        {
                                            if ((sets[index1, i] != intersection_set[j]))   // Sadece A da olan elemanları alıcam o yüzden intersection sayılarını da çıkarıyorum 
                                                yaz = true;  //kağıttaki örneklerden anlaşılacağı gibi A={3,9,7,6} B={15,3,7,8,12} olduğunda AnB = {3,7} ise A-B = {9,6}

                                            else if (sets[index1, i] == intersection_set[j])
                                            {
                                                yaz = false;
                                                break;
                                            }
                                        }  

                                        if (yaz && (sets[index1, i] != 0))//////////////////////////////////
                                            difference_set[index++] = sets[index1, i]; // sadece A nın elemanları (baştaki set'in)

                                        result = 0;
                                    }
                                    if (round <10)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.SetCursorPosition(27, 13 + round);
                                        Console.Write("{0}.{1}-{2}=", round, first, last);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.SetCursorPosition(33, 13 + round);
                                        Console.Write("{");
                                        for (int i = 0; i < index; i++)
                                        {

                                            if (difference_set[i] != 0 && difference_set[i + 1] != 0)//difference kümesinin elemanlarının screena yazdırılması
                                                Console.Write("{0},", difference_set[i]);
                                            if (difference_set[i] != 0 && difference_set[i + 1] == 0)// son elemandan sonra virgül gelmemesi için özel durum 
                                                Console.Write("{0}", difference_set[i]);
                                        }
                                        Console.Write("}");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (difference_set[i] != 0)/////////////////////////////////
                                                result += difference_set[i];
                                        }
                                        Console.Write("-->{0}", result);
                                        Console.SetCursorPosition(6, 19);
                                        Console.Write(score);
                                        Console.SetCursorPosition(6, 20);
                                        Console.Write(round);
                                        round++;
                                    }
                                    score += result + (time * 2);
                                    time = 50;
                                    Console.SetCursorPosition(6, 19);
                                    Console.Write(score);
                                }
                                    //symmetric difference  ////////////////////////////////////
                                else if (middle == '+')
                                {
                                    int index1 = 0, index2 = 0, index = 0;

                                    
                                    for (int i = 0; i < set_elements.Length; i++)
                                    {
                                        if (first == set_elements[i])  // Aynı işlemler 
                                            index1 = i;
                                        if (last == set_elements[i])
                                            index2 = i;
                                    }

                                    ////////intersection///////////////////////////////////////////
                                    for (int i = 0; i < 8; i++)
                                        for (int j = 0; j < 8; j++)
                                        {
                                            if ((sets[index1, i] == sets[index2, j]) && (sets[index2, j] != 0)) // kesişim elemanları belirleniyor 
                                                intersection_set[index++] = sets[index2, j];
                                        }
                                    index = 0;
                                    //////////////intersection/////////////////////////////////
                                    /////////////////////yeni union//////////////////////////////
                                    for (int i = 0; i < 8; i++)
                                    {                             
                                        if (sets[index1, i] != 0) // AuB örneğinde bu satırda A nın içindeki elemanları teker teker oluşturduğum yeni kümeye ekliyorum
                                        {
                                            union_set[index++] = sets[index1, i]; // Sadece A DA OLAN ELEMANLAR
                                        }
                                    }
                                    int union_index = index;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        bool write_flag = false;
                                        for (int j = 0; j < union_index; j++)
                                        {
                                            if (union_set[j] != sets[index2, i])
                                                write_flag = true;
                                            else
                                            {
                                                write_flag = false;
                                                break;
                                            }
                                        }
                                        if (write_flag && (sets[index2, i] != 0))
                                            union_set[index++] = sets[index2, i];
                                    }

                                    //////////////////////yeni union bitiş////////////////////////
  
                                    index = 0;
                                    for (int i = 0; i < 16; i++)
                                    {
                                        bool write_flag = false;
                                        for (int j = 0; j < 16; j++)
                                        {
                                            if ((union_set[i] != intersection_set[j]))
                                                write_flag = true;
                                            else if (union_set[i] == intersection_set[j])
                                            {
                                                write_flag = false;
                                                break;
                                            }
                                        } //// yaz false olduktan sonra bikaç eleman daha eklenmesi gerekebilir... döngü anlaşılmadı?

                                        if (write_flag && (union_set[i] != 0))
                                            sym_set[index++] = union_set[i];
                                        result = 0;
                                    }
                                    if (round < 10)
                                    {
                                        Console.SetCursorPosition(27, 13 + round);
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write("{0}.{1}+{2}=", round, first, last);
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.SetCursorPosition(33, 13 + round);
                                        Console.Write("                                     ");
                                        Console.SetCursorPosition(33, 13 + round);
                                        Console.Write("{");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (sym_set[i] != 0 && sym_set[i + 1] != 0)//  symmetric differnce kümesinin screena yazdırılması
                                                Console.Write("{0},", sym_set[i]);
                                            if (sym_set[i] != 0 && sym_set[i + 1] == 0)//  son elemandan sonra virgül gelmemesi için özel durum 
                                                Console.Write("{0}", sym_set[i]);
                                        }
                                        Console.Write("}");
                                        for (int i = 0; i < 16; i++)
                                        {
                                            if (sym_set[i] != 0) //iki kümenin symmetric difference elamanlarının toplanması
                                                result += sym_set[i];
                                        }
                                        Console.Write("-->{0}", result);
                                        Console.SetCursorPosition(6, 19);
                                        Console.Write(score);
                                        Console.SetCursorPosition(6, 20);
                                        Console.Write(round);
                                        round++;
                                    }
                                    score += result + (time * 2);
                                    time = 50;
                                    Console.SetCursorPosition(6, 19);
                                    Console.Write(score);
                                }
                            }
                          //control_flag = true; // horizontal veya vertical üçlü bulduysa flag true oluyor 
                            break;
                        }
                    }
                if (round == 9)
                {
                    Console.SetCursorPosition(15, 24);
                    Console.Write("........GAME OVER.......");
                }
                //return control_flag;

                /////////////////////////////////////////////////////controlflaggg//////////////////////////////////////////////////////
                /*    ////////timeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee_alternatif////////////////////////////////////////////////////////////
                 if (gerisayım > 0)
                {
                    fark = DateTime.Now - şimdiki_zaman_sabit;

                    Console.SetCursorPosition(15, 18);

                    gerisayım = 50 - (DateTime.Now - şimdiki_zaman_sabit).Seconds;
                    Console.Write(gerisayım);
                }
                if ((DateTime.Now - şimdiki_zaman_sabit).Milliseconds%1000 ==0)
                     time_key=false;
                else time_key=true;
                ConsoleKeyInfo a = Console.ReadKey(time_key);  //
                time_key = false;
                if ((DateTime.Now - şimdiki_zaman_sabit).Milliseconds%1000==0)
                    time_key = false;
                else time_key = true;
                char key = a.KeyChar;                      //
                string keychar = Convert.ToString(a.Key);  //
                */
                ConsoleKeyInfo a = Console.ReadKey(true);  //
                char key = a.KeyChar;                      //
                string keychar = Convert.ToString(a.Key);  //
                switch (keychar)
                {
                    case "LeftArrow":
                        if (screen[Xposition_x, Xposition_y - 1] != '#')
                        {
                            if (screen[Xposition_x, Xposition_y - 1] == ' ')
                            {
                                screen[Xposition_x, Xposition_y - 1] = x;
                                screen[Xposition_x, Xposition_y] = ' ';
                                Xposition_y--;
                            }
                            else if (screen[Xposition_x, Xposition_y - 2] == ' ')
                            {
                                char temp_letter = screen[Xposition_x, Xposition_y - 1];
                                screen[Xposition_x, Xposition_y - 2] = temp_letter;
                                
                                screen[Xposition_x, Xposition_y - 1] = x;
                                
                                screen[Xposition_x, Xposition_y] = ' ';
                                Xposition_y--;

                            }

                            timeflag = false;  ///////////////////////////
                            Console.SetCursorPosition(0, 0);
                            //Console.Write(screen);
                            for (int i = 0; i < 15; i++)
                            {
                                for (int j = 0; j < 25; j++)
                                {
                                    if (screen[i, j] == screen[Xposition_x, Xposition_y])
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '#')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                Console.Write("\n");
                            }
                            timeflag = true;
                        }
                        break;
                    case "RightArrow":
                        if (screen[Xposition_x, Xposition_y + 1] != '#')
                        {
                            if (screen[Xposition_x, Xposition_y + 1] == ' ')
                            {

                                screen[Xposition_x, Xposition_y + 1] = x;
                                screen[Xposition_x, Xposition_y] = ' ';

                                Xposition_y++;

                            }
                            else if (screen[Xposition_x, Xposition_y + 2] == ' ')
                            {
                                char temp_letter = screen[Xposition_x, Xposition_y + 1];
                                screen[Xposition_x, Xposition_y + 2] = temp_letter;
                                screen[Xposition_x, Xposition_y + 1] = x;
                                screen[Xposition_x, Xposition_y] = ' ';
                                Xposition_y++;
                            }

                            timeflag = false;
                            Console.SetCursorPosition(0, 0);
                            //Console.Write(screen);
                            for (int i = 0; i < 15; i++)
                            {
                                for (int j = 0; j < 25; j++)
                                {
                                    if (screen[i, j] == screen[Xposition_x, Xposition_y])
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '#')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                Console.Write("\n");
                            }
                            timeflag = true;
                        }
                        break;
                    case "UpArrow":
                        if (screen[Xposition_x - 1, Xposition_y] != '#')
                        {
                            if (screen[Xposition_x - 1, Xposition_y] == ' ')//eğer üstü boşsa 
                            {
                                screen[Xposition_x, Xposition_y] = ' ';
                                screen[Xposition_x - 1, Xposition_y] = x;
                                Xposition_x--;
                            }
                            else if (screen[Xposition_x - 2, Xposition_y] == ' ')
                            {
                                char temp_letter = screen[Xposition_x - 1, Xposition_y];

                                screen[Xposition_x, Xposition_y] = ' ';
                                screen[Xposition_x - 1, Xposition_y] = x;
                                screen[Xposition_x - 2, Xposition_y] = temp_letter;
                                Xposition_x--;
                            }
                            timeflag = false;
                            Console.SetCursorPosition(0, 0);
                            //Console.Write(screen);
                            for (int i = 0; i < 15; i++)
                            {
                                for (int j = 0; j < 25; j++)
                                {
                                    if (screen[i, j] == screen[Xposition_x, Xposition_y])
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '#')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                Console.Write("\n");
                            }
                            timeflag = true;
                        }
                        break;
                    case "DownArrow":
                        if (screen[Xposition_x + 1, Xposition_y] != '#')
                        {
                            if (screen[Xposition_x + 1, Xposition_y] == ' ')//eğer altı boşsa 
                            {
                                screen[Xposition_x, Xposition_y] = ' ';
                                screen[Xposition_x + 1, Xposition_y] = x;
                                Xposition_x++;
                            }
                            else if (screen[Xposition_x + 2, Xposition_y] == ' ')
                            {
                                char temp_letter = screen[Xposition_x + 1, Xposition_y];

                                screen[Xposition_x, Xposition_y] = ' ';
                                screen[Xposition_x + 1, Xposition_y] = x;
                                screen[Xposition_x + 2, Xposition_y] = temp_letter;
                                Xposition_x++;
                            }
                            timeflag = false;
                            Console.SetCursorPosition(0, 0);
                            //Console.Write(screen);
                            for (int i = 0; i < 15; i++)
                            {
                                for (int j = 0; j < 25; j++)
                                {
                                    if (screen[i, j] == screen[Xposition_x, Xposition_y])
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '#')
                                    {
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else if (screen[i, j] == '+' || screen[i, j] == '-' || screen[i, j] == 'u' || screen[i, j] == 'n')
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.Write(screen[i, j]);
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                }
                                Console.Write("\n");
                            }
                            timeflag = true;
                        }

                        break;
                }
            }
            //////////////////////////////CONTROL FONKSİYONUN BİTİŞİ////////////////////////////////////////////////////////////////////////////////////

        }  // Main bitimi

        static int time = 50;
        static void t_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (time != 0 && round <10)
            {
                time--;
                if (timeflag)
                {
                    Console.SetCursorPosition(15, 18);
                    Console.Write("          ");    // Zamanı yazarken her zaman çift hane gibi gözükmemesi için (9 yerine 90 yazıyordu)
                    Console.SetCursorPosition(15, 18);
                    Console.Write(time);
                    Console.SetCursorPosition(0, 0);
                }
                if (round >= 9)
                {
                   
  
                        Console.Clear();
                        Console.SetCursorPosition(15, 15);
                        Console.Write("Your score:{0}", score);
                       

                }
            }
            else if (time == 0 && round < 10)
            {
                time = 50;
                Console.SetCursorPosition(27, 13 + round);  // zaman bittiğinde 
                
                Console.Write("{0}.", round);
                Console.SetCursorPosition(6, 20);
                Console.Write(round);
                round++;
                if (round >=9)
                {

                        Console.Clear();
                        Console.SetCursorPosition(15, 15);
                        Console.Write("oyun bitti scorunuz:{0}", score);

                }
            }

        }
    }
}
