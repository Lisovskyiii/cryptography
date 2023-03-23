
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace лаба_3
{
    class Program
    {
        const int num = 20;
        static void Main(string[] args)
        {
            try
            {
                List<string> list = new List<string>();
                ReadFile(list);
                int[] positions = new int[num];
                string[] new_positions = list[1].Split((' '), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < new_positions.Length; i++)
                {
                    positions[i] = Convert.ToInt32(new_positions[i]);
                }

                int[] numbers = new int[num];
                Array.Copy(positions, numbers, num);
                int all_symbols = Convert.ToInt32(list[3]);
                string[] text = new string[list.Count - 5];
                for (int stroka = 5, i = 0; stroka <list.Count; stroka++, i++)
                {
                    text[i] += list[stroka] + '\r';
                }
                
                string line = Line(text);
                string new_text = Coding(line, positions, all_symbols);
                Console.WriteLine("\n\n");
                int[] new_key = New_key(numbers);
                Output(new_text, all_symbols, new_key);
            }
            catch (Exception e)
            {
                Error(e);
            }
        }
        public static void Error(Exception e)
        {
            FileStream f = new FileStream("OutPut.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(f, Encoding.Default);
            writer.WriteLine(e);
            f.Close();
        }
        static public List<string> ReadFile(List<string> list)
        { 
            File.Delete("OutPut.txt");
            FileStream f = new FileStream("3.Scrambled.txt", FileMode.Open);
            StreamReader reader = new StreamReader(f, Encoding.Default);
            string stroka = reader.ReadLine();
            list.Add(stroka);
            while(!stroka.Trim().Contains("// Text:")) 
            {
                stroka = reader.ReadLine();
                if (stroka!= "") { list.Add(stroka); }
            }
            while (!reader.EndOfStream)
            {
                list.Add(reader.ReadLine());
            }
            list.Remove("");
            return list;
        }
        static string Line(string[] text)
        {
            string line = "";
            for (int str = 0; str < text.Length; str++)
            {
                line = line + text[str]; 
            }
            return line;
        }
        public static string Multiple_Length(string linetext,int[] key,ref string test,int stroka)
        {
            for (; stroka < linetext.Length; stroka += num)
            {
                for (int k = 0; k < num; k++)
                {
                    test += linetext[key[k] + stroka];
                    Console.WriteLine(test);
                }
            }
            return test;
        }
        public static string Other_Length(string line, int[] key, ref string new_text,int last_stroka, int stroka)
        {
            for (; stroka < line.Length - last_stroka; stroka += num)
            {
                for (int k = 0; k < num; k++)
                {
                    new_text += line[key[k] + stroka];
                }
            }
            for (int i = 0; i < last_stroka; i++)
            {
                if (key[i] >= last_stroka)
                {
                    while (key[i] >= last_stroka)
                    {
                        key[i]=(int)key.GetValue(key[i]);
                    }
                }
                new_text += line[key[i] + stroka];
            }
            return new_text;
        }
        static string Coding(string line, int[] key, int allsymbols)
        {
            string new_text = "";
            int last_stroka = (line.Length - (line.Length / num) * num);
            int stroka= 0;
            if (line.Length % num == 0)
            {
                Multiple_Length(line, key, ref new_text,stroka);
                return new_text;
            }
            else
            {
                Other_Length(line, key, ref new_text, last_stroka, stroka);
                return new_text;
            }
        }
        static int[] New_key(int[] key)
        {
            int[] new_key = new int[num];
            for (int i = 0; i < num; i++)
            {
                for (int k = 0; k < num; k++)
                {
                    if (key[k] == i) { new_key[i] = k; break; }
                }
            }
            return new_key;
        }
        static void Output(string new_text, int allsymbols, int[] key)
        {
            FileStream f = new FileStream("OutPut.txt", FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(f, Encoding.Default);
            writer.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            writer.WriteLine($"Decrypting {allsymbols} characters");
            writer.Write("Using:  ");

            for (int i = 0; i < num; i++) 
            { 
                writer.Write("{0,2} ",i); 
            }
            writer.Write("\n\t");
            for (int i = 0; i < num; i++)
            {
                writer.Write("{0,2} ",key[i]);
            }
            writer.WriteLine("\n");
            writer.WriteLine(new_text);
            writer.WriteLine("\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            writer.Close();
        }
    }
}
