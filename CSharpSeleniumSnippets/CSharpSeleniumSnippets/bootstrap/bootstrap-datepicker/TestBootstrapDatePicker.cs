using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using PatientActivatorAcceptanceTests.pages.VendorComponents;


namespace CSharpSeleniumSnippets.bootstrap
{
    [TestFixture]
    public class TestBootstrapDatePicker
    {        

        [Test]
        public void ShouldSelectDate()
        {
            IWebDriver driver = null;
            try
            {
                Console.WriteLine("Opening base URL");
                driver = new FirefoxDriver();

                driver.Navigate().GoToUrl("http://www.eyecon.ro/bootstrap-datepicker/");
                var dateInput = driver.FindElement(By.CssSelector("input"));                 
                var actions = new Actions(driver);
                actions.MoveToElement(dateInput).MoveByOffset(3, 3).Click().Perform();                

                var bootstrapdatePickerDropdown =
                    PageFactory.InitElements<BootstrapDatePickerDropDownPageComponent>(driver);

                bootstrapdatePickerDropdown.SetDate(new DateTime(2010, 6, 21));

                Assert.That(dateInput.GetAttribute("value"), Is.EqualTo("06-21-2010"));
            }
            finally
            {
                if (driver != null) driver.Quit();
            }
        }

    }
}