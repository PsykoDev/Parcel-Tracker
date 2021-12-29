using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

#pragma warning disable SYSLIB0014, CS8604, CS1998, NU1701

namespace Chronopost.Delivery
{
    public class Delivery
    {
        static CultureInfo provider = CultureInfo.InvariantCulture;

        public static async Task<string> Delivery_dateAsync(string url)
        {
            try
            {
                ChromeOptions options = new ChromeOptions();
                options.PageLoadStrategy = PageLoadStrategy.Normal;
                options.AddArguments("headless", "disable-logging", "silent-launch", "silent-debugger-extension-api");
                ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
                driverService.HideCommandPromptWindow = true;
                driverService.EnableVerboseLogging = false;
                driverService.SuppressInitialDiagnosticInformation = true;
                driverService.EnableAppendLog = false;

                ChromeDriver driver = new ChromeDriver(driverService, options);
                driver.Navigate().GoToUrl(url);
                string html = driver.PageSource;

                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                driver.Quit();
                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='ch-colis-information']"))
                {
                    string value = node.InnerText;
                    if(value.Contains("paration")) return "Unknown";
                    var reg = Regex.Replace(value, @"[A-Za-z\téà<br>']+", "");
                    var whitepsace = Regex.Replace(reg, @"\s+", " ").Trim();
                    var split = Split(whitepsace);
                    return split;
                }
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string Split(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            string[] subs = value.Split(' ');
            DateTime DT = DateTime.ParseExact(subs[0], new string[] { "dd/MM/yyyy" }, provider, DateTimeStyles.None);
            DateTime DT1 = DateTime.ParseExact(subs[1], new string[] { "H:mm" }, provider, DateTimeStyles.None);
            DateTime DT2 = DateTime.ParseExact(subs[2], new string[] { "H:mm" }, provider, DateTimeStyles.None);
            return $"{DT.ToString("d MMMM yyyy")} {DT1.ToString("H:mm")} -> {DT2.ToString("H:mm")}";
        }
    }
}

