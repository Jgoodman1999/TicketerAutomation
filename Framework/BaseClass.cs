using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TicketerAutomation.Framework;

public abstract class BaseClass
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BaseClass(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }
    protected void ClickElement(IWebElement element)
    {
        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
        catch (ElementNotInteractableException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }
    protected IWebElement WaitForElement(By locator)
    {
        return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
    }

    protected IWebElement WaitForClickable(By locator)
    {
        return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
    }

    protected bool IsElementDisplayed(By locator)
    {
        try
        {
            return WaitForElement(locator).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    protected void ScrollToElement(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
        Thread.Sleep(500);
    }

    protected void EnterText(By locator, string text)
    {
        var element = WaitForElement(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string GetCurrentUrl()
    {
        return Driver.Url;
    }

    protected bool UrlContains(string expectedUrlPart, int timeoutSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
        try
        {
            return wait.Until(d => d.Url.Contains(expectedUrlPart));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    protected bool PageContainsText(string text)
    {
        try
        {
            Wait.Until(d => d.PageSource.Contains(text));
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
    
    protected void ScrollToTop()
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 0);");
    }
}
