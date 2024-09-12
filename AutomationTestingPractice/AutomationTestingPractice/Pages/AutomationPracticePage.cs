using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AutomationTestingPractice.Pages
{
    public class AutomationPracticePage
    {
        private IWebDriver driver;
        public AutomationPracticePage(IWebDriver driver)
        {
            this.driver = driver;
        }
        public IWebElement TxtName => driver.FindElement(By.XPath("//input[@id='name']"));
        public IWebElement TxtEmail => driver.FindElement(By.XPath("//input[@id='email']"));
        public IWebElement TxtPhone => driver.FindElement(By.XPath("//input[@id='phone']"));
        public IWebElement TxtAddress => driver.FindElement(By.XPath("//textarea[@id='textarea']"));
        public IWebElement GenderMale => driver.FindElement(By.XPath("//input[@id='male']"));
        public IWebElement GenderFemale => driver.FindElement(By.XPath("//input[@id='female']"));
        public IWebElement SunDay => driver.FindElement(By.XPath("//input[@id='sunday']"));
        public IWebElement MonDay => driver.FindElement(By.XPath("//input[@id='monday']"));
        public IWebElement ThursDay => driver.FindElement(By.XPath("//input[@id='thursday']"));
        public IWebElement Countrylabel => driver.FindElement(By.XPath("//label[normalize-space()='Country:']"));
        public IWebElement SelectCountry => driver.FindElement(By.XPath("//select[@id='country']"));
        public IWebElement MultiSelectColors => driver.FindElement(By.XPath("//select[@id='colors']"));
        public IWebElement Datepicker => driver.FindElement(By.XPath("//input[@id='datepicker']"));
        public IWebElement LnkDifferentTab => driver.FindElement(By.XPath("//a[normalize-space()='Posts (Atom)']"));
        public IWebElement SecondPage => driver.FindElement(By.XPath("//ul[@id='pagination']/li[2]"));
        public IWebElement ThirdPage => driver.FindElement(By.XPath("//ul[@id='pagination']/li[3]"));
        public IWebElement ForthPage => driver.FindElement(By.XPath("//ul[@id='pagination']/li[4]"));
        public IWebElement SearchField => driver.FindElement(By.XPath("//input[@id='Wikipedia1_wikipedia-search-input']"));
        public IWebElement FirstSearchResult => driver.FindElement(By.XPath("//div[@id='Wikipedia1_wikipedia-search-results']//div[1]//a"));
        public IWebElement BtnNewBrowserWindow => driver.FindElement(By.XPath("//button[normalize-space()='New Browser Window']"));
        public IWebElement BtnAlert => driver.FindElement(By.XPath("//button[normalize-space()='Alert']"));
        public IWebElement BtnConfirmBox => driver.FindElement(By.XPath("//button[normalize-space()='Confirm Box']"));
        public IWebElement BtnPrompt => driver.FindElement(By.XPath("//button[normalize-space()='Prompt']"));

        public void SendText(IWebElement element, string text)
        {
            if(element.Displayed)
            {
                element.Clear();
                element.SendKeys(text);
            } 
        }

        public void SelectOneElementByText(IWebElement element, string text)
        {
            if(element.Displayed) 
            {
                SelectElement selectElement = new SelectElement(element);
                selectElement.SelectByText(text);
            }
        }

        public string GenerateDescription()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] descriptionChars = new char[30];
            for (int i = 0; i < descriptionChars.Length; i++)
            {
                if (i % 5 == 0)
                    descriptionChars[i] = ' ';
                else
                    descriptionChars[i] = chars[new Random().Next(chars.Length)];
            }

            return new string(descriptionChars);
        }
        public void ClickElement(IWebElement element)
        {
            if (element.Displayed)
                element.Click();
        }
        public void SelectGender(string gender)
        {
            if (gender == "Male")
                ClickElement(GenderMale);
            else
                ClickElement(GenderFemale);
        }

        public void SelectDays(string day)
        {
            switch (day) 
            {
                case "Sunday":
                    ClickElement(SunDay);
                    break;
                case "Monday":
                    ClickElement(MonDay);
                    break;
                case "Thursday":
                    ClickElement(ThursDay);
                    break;
                default: 
                    throw new ArgumentException();
            }
        }
        public void ScrollToElement(IWebElement element) => ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        public void ScrollToBottom() => ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        public void ScrollToTop() => ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, 0);");
        public void ScrollElementDown(IWebDriver driver, IWebElement element, int pixels)
        {
            // JavaScript to scroll the element down by a specified number of pixels
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript($"arguments[0].scrollTop += {pixels};", element);
            Thread.Sleep(2000);
        }

        public void SelectTodaysDate() 
        {
            var days = driver.FindElements(By.XPath("//td[contains(@data-handler,'selectDay')]"));
            var today = days.First(day => day.GetAttribute("class").Contains("today"));
            ClickElement(today);
            
        }

        public int TotalPrices()
        {
            List<string> prices = new List<string>();
            int count = 0;
            int sum = 0;
            var allPrices = driver.FindElements(By.XPath("//tbody/tr/td[4]"));
            foreach ( var item in allPrices ) 
            {
                prices.Add(item.Text);
                count++;
                if (count > 5)
                    break;
            }
            foreach (var item in prices) 
            {
                sum = sum + int.Parse(item);
            }

            return sum;
        }

        public int SinglePageProductCount(double lowestPriceOfProducts)
        {
            // Count of product which is greater than the lowestPriceOfProducts 
            List<string> prices = new List<string>();
            int count = 0; 
            var allPrices = driver.FindElements(By.XPath("//tbody/tr/td[3]"));
            for (int i = 0; i < allPrices.Count; i++)
            {
                if(i > 5 && i < 11)
                {
                    prices.Add(allPrices[i].Text);
                }
            }
            foreach(var item in prices)
            {
                if (ConvertToDouble(item) > lowestPriceOfProducts)
                {
                    count++;
                }
            }
            
            return count;
        }
        public double MultiplePageProductCount(double lowestPriceOfProducts)
        {
            int sum = 0;
            for (int i = 1; i <= 4; i++)
            {
                var page = driver.FindElement(By.XPath($"//ul[@id='pagination']/li[{i}]"));
                ClickElement(page);
                Thread.Sleep(2000);
                
                sum = sum + SinglePageProductCount(lowestPriceOfProducts);
            }
            return sum;
        }

        public double ConvertToDouble(string priceString)
        {
            // Remove the dollar sign
            string cleanedPriceString = priceString.Replace("$", "");

            // Parse the cleaned string to double
            double price = double.Parse(cleanedPriceString, CultureInfo.InvariantCulture);

            return price;
        }

        public void SwitchBackToOrginalTab(IWebElement clickableElement)
        {
            // Store the current window handle (original tab)
            string originalTab = driver.CurrentWindowHandle;
            // Click an element to open a new tab
            ClickElement(clickableElement);
            Thread.Sleep(3000);
            // Get all open window handles
            var allTabs = driver.WindowHandles;
            // Switch to the new tab
            foreach (var tab in allTabs)
            {
                if (tab != originalTab)
                {
                    driver.SwitchTo().Window(tab);
                    break;
                }
            }
            // Switch back to the original tab
            driver.SwitchTo().Window(originalTab);
        }

        public void AlertConfirmBox(IWebElement element)
        {
            // Click the alert button
            element.Click();
            Thread.Sleep(2000);

            // Switch to the alert
            var alert = driver.SwitchTo().Alert();

            // Get the text from the alert
            string alertText = alert.Text;
            Console.WriteLine("Alert text: " + alertText);

            // Accept the alert (click OK)
            alert.Accept();

            // Optionally, you can also dismiss the alert (click Cancel) by:
            // alert.Dismiss();

        }

    }
}
