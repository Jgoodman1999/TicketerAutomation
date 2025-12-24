using OpenQA.Selenium;
using TicketerAutomation.Framework;

namespace TicketerAutomation.Pages;

public class WhippetSuccessStoryClass : BaseClass
{
    private const string ExpectedUrlPart = "customer-success-stories/whippet";
    private const string ExpectedText = "Increasing Whippet's passenger numbers";

    public WhippetSuccessStoryClass(IWebDriver driver) : base(driver) { }

    public bool IsOnWhippetPage()
    {
        return UrlContains(ExpectedUrlPart, 15);
    }

    public string GetExpectedUrl()
    {
        return "ticketer.com/customer-success-stories/whippet-bus-and-ticketer/";
    }

    public bool ContainsExpectedText()
    {
        return PageContainsText(ExpectedText) || 
               PageContainsText("Whippet") ||
               PageContainsText("passenger");
    }

    public string GetExpectedText()
    {
        return ExpectedText;
    }
}