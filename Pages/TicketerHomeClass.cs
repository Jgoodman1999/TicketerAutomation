using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using TicketerAutomation.Framework;

namespace TicketerAutomation.Pages;

public class TicketerHomeClass : BaseClass
{
    private readonly By _cookieAcceptButton = By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll");
    private readonly By _ticketerLogo = By.XPath("//img[contains(@src,'Ticketer-logo')]");
    private readonly By _customerSuccessHeading = By.XPath("//h2[contains(text(),'Partnering for success')]"); 
    private readonly By _carouselNextButton = By.CssSelector("button.flickity-button.next");
    private readonly By _whippetSlide = By.XPath("//a[contains(@href,'whippet-bus-and-ticketer')]");
    private readonly By _whippetFindOutMore = By.CssSelector("a[href*='whippet-bus-and-ticketer']");
    
    public TicketerHomeClass(IWebDriver driver) : base(driver) { }

    public void NavigateTo()
    {
        Driver.Navigate().GoToUrl("https://www.ticketer.com/");
    }

    public void AcceptCookies()
    {
        try
        {
            var acceptButton = Wait.Until(ExpectedConditions.ElementToBeClickable(_cookieAcceptButton));
            ClickElement(acceptButton);
            Thread.Sleep(500);
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Cookie prompt not displayed or already accepted");
        }
    }

    public bool IsLogoVisible()
    {
        try
        {
            var logo = Wait.Until(ExpectedConditions.ElementIsVisible(_ticketerLogo));
            return logo.Displayed;
        }
        catch
        {
            return false;
        }
    }

    public void ScrollToCustomerSuccessSection()
    {
        var section = Wait.Until(ExpectedConditions.ElementExists(_customerSuccessHeading));
        ScrollToElement(section);
        Thread.Sleep(500);
    }

    public void ClickRightArrowUntilWhippetVisible()
    {
        int maxAttempts = 10;
        
        for (int i = 0; i < maxAttempts; i++)
        {
            var whippetSlides = Driver.FindElements(_whippetSlide);
            if (whippetSlides.Any(e => e.Displayed))
            {
                return;
            }

            var nextButton = Wait.Until(ExpectedConditions.ElementToBeClickable(_carouselNextButton));
            ClickElement(nextButton);
            Thread.Sleep(800);
        }

        throw new Exception("Could not find Whippet story after maximum attempts");
    }

    public void ClickFindOutMoreOnWhippet()
    {
        var findOutMoreLink = Wait.Until(ExpectedConditions.ElementToBeClickable(_whippetFindOutMore));
        ScrollToElement(findOutMoreLink);
        Thread.Sleep(300);
        ClickElement(findOutMoreLink);
    }
    
}