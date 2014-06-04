using System;
using System.Linq;
using OpenQA.Selenium;

namespace PatientActivatorAcceptanceTests.pages.VendorComponents
{
    /// <summary>
    /// PageObject for interacting with Bootstrap-Datepicker dropdown.
    /// http://www.eyecon.ro/bootstrap-datepicker/
    /// </summary>
    public class BootstrapDatePickerDropDownPageComponent
    {
        private const string DropdownSelector = "div.datepicker.dropdown-menu";

        private readonly IWebElement _dropDownDivElement;
        
        private readonly IWebElement _previousMonthElement;
        private readonly IWebElement _nextMonthElement;
        private readonly IWebElement _monthYearHeaderElement;
        

        public BootstrapDatePickerDropDownPageComponent(ISearchContext driver)
        {
            try
            {
                var allDropDowns = driver.FindElements(By.CssSelector(DropdownSelector));

                //Find the visible dropdown.
                foreach (var dropdown in allDropDowns.Where(dropdown => dropdown.Displayed))
                {
                    _dropDownDivElement = dropdown;
                    break;
                }

                _monthYearHeaderElement = _dropDownDivElement.FindElement(By.XPath(".//th[contains(@class, 'switch')]"));
                _previousMonthElement = _dropDownDivElement.FindElement(By.CssSelector("th.prev"));
                _nextMonthElement = _dropDownDivElement.FindElement(By.CssSelector("th.next"));
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchWindowException("Date picker dropdown was not found or is not currently active.");
            }
        }

        /// <summary>
        /// Set the date picker to target date.
        /// </summary>
        /// <param name="date"></param>
        public void SetDate(DateTime date)
        {
            SetMonthYear(date.Month, date.Year);
            SetDay(date.Day);
            
        }

        /// <summary>
        /// Select the day view is visible, provided day is selected.
        /// </summary>
        /// <param name="day"></param>
        private void SetDay(int day)
        {
            _dropDownDivElement.FindElement(By.XPath(String.Format(".//td[.='{0}']", day))).Click();
        }

        
        private void SetMonthYear(int month, int year)
        {
            var targetMonthYear = new DateTime(year, month, 1);
            var i = 1000; // control value for preventing infinite loop.
            while ((i--) > 0)
            {
                var visibleMonthYear = DateTime.Parse(_monthYearHeaderElement.Text);
                if (visibleMonthYear.Year == year && visibleMonthYear.Month == month)
                    return;

                //Go forward or back a month.
                if( new DateTime(year, month, 1) < visibleMonthYear )
                    _previousMonthElement.Click();
                else
                    _nextMonthElement.Click();
            }

            throw new BootstrapDatePickerException("Unable to select the correct Month/Year");
        }

    }

    class BootstrapDatePickerException: Exception
    {
        public BootstrapDatePickerException(string message, Exception e=null) : base(message, e) { }
    }
}
