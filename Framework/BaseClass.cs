using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace TicketerAutomation.Framework;

public abstract class BaseClass
{
    public readonly IWebDriver Driver;
    public readonly WebDriverWait Wait;

    public BaseClass(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    public void ClickElement(IWebElement element)
    {
        try
        {
            WaitForClickable(element).Click();
        }
        catch (Exception ex) when (ex is ElementClickInterceptedException or ElementNotInteractableException)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }

    public IWebElement WaitForElement(IWebElement element)
    {
        return Wait.Until(d => element.Displayed ? element : null);
    }

    public IWebElement WaitForClickable(IWebElement element)
    {
        return Wait.Until(ExpectedConditions.ElementToBeClickable(element));
    }

    public bool IsElementDisplayed(IWebElement element)
    {
        try
        {
            return WaitForElement(element).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public void ScrollToElement(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", element);
        Thread.Sleep(500);
    }

    public void EnterText(IWebElement element, string text)
    {
        var el = WaitForElement(element);
        el.Clear();
        el.SendKeys(text);
    }

    public string GetCurrentUrl()
    {
        return Driver.Url;
    }

    public bool UrlContains(string expectedUrlPart, int timeoutSeconds = 10)
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

    public bool PageContainsText(string text)
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
    
    public void ScrollToTop()
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 0);");
    }
}