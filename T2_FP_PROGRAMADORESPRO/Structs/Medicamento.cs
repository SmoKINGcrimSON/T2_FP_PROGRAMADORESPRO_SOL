using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2_FP_PROGRAMADORESPRO.Structs
{
    public struct Medicamento
    {
        private static Random p_rnd = new Random();
        //static fields
        private static List<string> p_registedCodes = new List<string>();
        public static List<string> P_registedCodes { get => p_registedCodes;}
        //Instations fields
        public string P_code { get; private set; }
        public string P_name { get; set; }
        public int P_count { get; set; }
        public float P_unitPrice { get; set; }
        public float P_investedAmount { get => P_count * P_unitPrice;}
        //Constructors
        public Medicamento(string p_name, int p_count, float p_unitprice)
        {
            P_name = p_name;
            P_count = p_count;
            P_unitPrice = p_unitprice;
            P_code = "";
            GenerateCode();
        }
        //Methods
        private bool ValidCode(string code) => !p_registedCodes.Exists(c => c == code);
        private void GenerateCode()
        {
            bool endRegist = false;
            StringBuilder pattern = new StringBuilder("qwertyuiopasdfghjklñzxcvbnmQWERTYUIOPASDFGHJKLÑZXCVBNM1234567890");
            StringBuilder newCode = new StringBuilder("");
            while (!endRegist)
            {
                for (int i = 0; i < 10; i++) newCode.Append(pattern[p_rnd.Next(0, pattern.Length - 1)]);
                endRegist = ValidCode(newCode.ToString());
            }
            P_code = newCode.ToString();
            p_registedCodes.Add(P_code);
        }
        public override string ToString() => P_name;
        public static void RemoveCode(string code)
        {
            p_registedCodes.Remove(code);
        }
    }
}
