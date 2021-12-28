﻿using System.Globalization;
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

                foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='ch-colis-information']"))
                {
                    string value = node.InnerText;
                    if(value.Contains("paration")) return "Unknown";
                    var reg = Regex.Replace(value, @"[A-Za-z\téà<br>']+", "");
                    var whitepsace = Regex.Replace(reg, @"\s+", " ").Trim();
                    DateTime DT = DateTime.ParseExact(whitepsace, new string[] { "dd.MM.yyyy H:mm", "dd-MM-yyyy H:mm", "dd/MM/yyyy H:mm" }, provider, DateTimeStyles.None);
                    return DT.ToString("d MMMM yyyy H:mm");
                }
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}

