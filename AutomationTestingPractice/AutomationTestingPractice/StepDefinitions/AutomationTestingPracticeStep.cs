using AutomationTestingPractice.Helper;
using AutomationTestingPractice.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace AutomationTestingPractice.StepDefinitions
{
    [Binding]
    public class AutomationTestingPracticeStep
    {
        private IWebDriver driver;
        private AutomationPracticePage practicePage;
        public AutomationTestingPracticeStep(IWebDriver driver, AutomationPracticePage practicePage)
        {
            this.driver = driver;
            this.practicePage = new AutomationPracticePage(driver);
        }

        [Given(@"Go To Automation Testing Practice Page")]
        public void GivenGoToAutomationTestingPracticePage()
        {
            driver.Navigate().GoToUrl("https://testautomationpractice.blogspot.com/"); 
            Thread.Sleep(1000);
        }

        [When(@"Enter Users Name, Email, Phone, Address, Gender and Days")]
        public void WhenEnterUsersNameEmailPhoneAddressGenderAndDays(Table table)
        {
            var dataSet = table.CreateSet<TestData>();
            foreach (var data in dataSet)
            {
                practicePage.SendText(practicePage.TxtName, data.Name);
                practicePage.SendText(practicePage.TxtEmail, data.Email);
                practicePage.SendText(practicePage.TxtPhone, data.Phone);
                practicePage.SendText(practicePage.TxtAddress, practicePage.GenerateDescription());
                Thread.Sleep(2000);
                practicePage.SelectGender(data.Gender);
                practicePage.SelectDays(data.Day1);
                practicePage.SelectDays(data.Day2);
                practicePage.SelectDays(data.Day3);
                Thread.Sleep(2000);

            }
        }

        [When(@"Select Country, Colors, Date and Click On link")]
        public void WhenSelectCountryColorsDateAndClickOnLink(Table table)
        {
            // Scroll To Country Label
            practicePage.ScrollToElement(practicePage.Countrylabel);
            Thread.Sleep(2000);

            var dataSet = table.CreateSet<TestData>();
            foreach (var data in dataSet)
            {
                // Select Country
                practicePage.SelectOneElementByText(practicePage.SelectCountry, data.Country);
                Thread.Sleep(2000);

                // Create Actions instance
                Actions actions = new Actions(driver);
                // Hold down the CTRL key to start multi-selecting
                actions.KeyDown(Keys.Control).Perform();
                // Select Multiple Colors
                practicePage.SelectOneElementByText(practicePage.MultiSelectColors, data.Color1);
                practicePage.SelectOneElementByText(practicePage.MultiSelectColors, data.Color2);
                Thread.Sleep(2000);
                // Scroll To Down inside multiselect element
                practicePage.ScrollElementDown(driver, practicePage.MultiSelectColors, 200);
                // Select Multiple Colors
                practicePage.SelectOneElementByText(practicePage.MultiSelectColors, data.Color3);
                // Release the CTRL key after selections
                actions.KeyUp(Keys.Control).Perform();
                Thread.Sleep(2000);

                // Click On Date
                practicePage.ClickElement(practicePage.Datepicker);
                Thread.Sleep(1000);
                // practicePage.SendText(practicePage.Datepicker, data.Date);
                practicePage.SelectTodaysDate();
                Thread.Sleep(2000);

                // Store the current window handle (original tab)
                string originalTab = driver.CurrentWindowHandle;
                // Click an element to open a new tab
                practicePage.ClickElement(practicePage.LnkDifferentTab);
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
                Thread.Sleep(3000);
            }

        }

        [Then(@"Is Total Price of Product Is '([^']*)'")]
        public void ThenIsTotalPriceOfProductIs(string expectedTotalPrice)
        {
            Assert.AreEqual(expectedTotalPrice, practicePage.TotalPrices().ToString());
        }

        [Then(@"Check pagination table that, Is '([^']*)' Products price is greater than '([^']*)'")]
        public void ThenCheckPaginationTableThatIsProductsPriceIsGreaterThan(string expectedProductCount, string lowestPriceOfProduct)
        {
            practicePage.ScrollToBottom();
            Thread.Sleep(2000);

            double number = practicePage.ConvertToDouble(lowestPriceOfProduct);
            int sum = 0;
            sum = sum + practicePage.PaginationTotalPrices(number);

            practicePage.ClickElement(practicePage.SecondPage);
            Thread.Sleep(2000);
            sum = sum + practicePage.PaginationTotalPrices(number);

            practicePage.ClickElement(practicePage.ThirdPage);
            Thread.Sleep(2000);
            sum = sum + practicePage.PaginationTotalPrices(number);

            practicePage.ClickElement(practicePage.ForthPage);
            Thread.Sleep(2000);
            sum = sum + practicePage.PaginationTotalPrices(number);

            Assert.AreEqual(double.Parse(expectedProductCount), sum);
        }




    }
}
