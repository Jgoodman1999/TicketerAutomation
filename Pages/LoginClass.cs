using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using TicketerAutomation.Framework;

namespace TicketerAutomation.Pages;

public class LoginClass : BaseClass
{
    private const string ExpectedUrlPart = "identity.ticketer";
    
    private readonly By _usernameField = By.Id("Input_Username");
    private readonly By _passwordField = By.Id("Input_Password");
    private readonly By _loginButton = By.CssSelector("button[name='Input.Button']");
    private readonly By _passwordRequiredError = By.XPath("//*[contains(text(),'The Password field is required')]");

    public LoginClass(IWebDriver driver) : base(driver) { }

    public bool IsOnLoginPage()
    {
        return UrlContains(ExpectedUrlPart, 15);
    }

    public void EnterUsername(string username)
    {
        var field = Wait.Until(ExpectedConditions.ElementIsVisible(_usernameField));
        field.Clear();
        field.SendKeys(username);
    }

    public void ClickLoginButton()
    {
        var loginButton = Wait.Until(ExpectedConditions.ElementToBeClickable(_loginButton));
        ClickElement(loginButton);
        Thread.Sleep(500);
    }

    public bool IsPasswordRequiredErrorDisplayed()
    {
        Thread.Sleep(500);
        
        try
        {
            var error = Wait.Until(ExpectedConditions.ElementIsVisible(_passwordRequiredError));
            return error.Displayed;
        }
        catch
        {
            return Driver.PageSource.Contains("The Password field is required");
        }
    }
}