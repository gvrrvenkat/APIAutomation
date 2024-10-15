using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace APITesting
{
    public class Tests
    {
        IWebDriver driver;
        //Locators
        public By locSearchButton = By.CssSelector("button[aria-label='Search button']");
        public By locSearchArea = By.CssSelector("input[placeholder='Search for people, services or...']");
        public By locResults = By.CssSelector("div[class='result'] a");

        [SetUp]
        public void Setup()
        {          
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.britinsurance.com/ ");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [Test]
        public void VerifySearchResults()
        {
            Thread.Sleep(10);
            driver.FindElement(locSearchButton).Click();
            Thread.Sleep(10);
            driver.FindElement(locSearchArea).SendKeys("IFRS 17");
            Thread.Sleep(3);
            IReadOnlyCollection<IWebElement> lstResultselments = driver.FindElements(locResults);
            List<String> lstResults = new List<string>();
            foreach (IWebElement elment in lstResultselments)
            {
                lstResults.Add(elment.Text);
            }
           Assert.IsTrue(lstResults.Contains("Financials"));
           Assert.IsTrue(lstResults.Contains("Interim results for the six months ended 30 June 2022"));
            Assert.IsTrue(lstResults.Contains("Results for the year ended 31 December 2023"));
            Assert.IsTrue(lstResults.Contains("Interim Report 2023"));
            Assert.IsTrue(lstResults.Contains("Kirstin Simon"));
        }
        [Test]
        public void APIValidations()
        {
            string html;
            string url = "https://api.restful-api.dev/objects?id=3&id=5&id=10Response";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            IEnumerable<Data> result = JsonConvert.DeserializeObject<IEnumerable<Data>>(html);
        }

        public class SingleUserResponseObject
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public int id { get; set; }                       
        }

        [TearDown]
        public void DeleteWebDriver()
        {
            if (driver != null)
            {
                 driver.Close();
                driver.Quit();
            }
        }
    }
}