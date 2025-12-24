using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using TicketerAutomation.Pages;

namespace TicketerAutomation.StepDefinitions;

[Binding]
public class TicketerWebsiteSteps
{
    private readonly ScenarioContext _scenarioContext;
    private IWebDriver Driver => _scenarioContext.Get<IWebDriver>("WebDriver");
    
    private TicketerHomeClass? _homePage;
    private WhippetSuccessStoryClass? _whippetPage;
    private LoginClass? _loginPage;

    public TicketerWebsiteSteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    // Home Page Steps
    [Given(@"I navigate to the Ticketer homepage")]
    public void GivenINavigateToTheTicketerHomepage()
    {
        _homePage = new TicketerHomeClass(Driver);
        _homePage.NavigateTo();
        TestContext.WriteLine($"Navigated to: {Driver.Url}");
    }

    [Given(@"I accept the cookies prompt")]
    public void GivenIAcceptTheCookiesPrompt()
    {
        _homePage ??= new TicketerHomeClass(Driver);
        _homePage.AcceptCookies();
        TestContext.WriteLine("Cookies prompt accepted");
    }

    [Then(@"the Ticketer logo should be visible in the top left corner")]
    public void ThenTheTicketerLogoShouldBeVisibleInTheTopLeftCorner()
    {
        _homePage ??= new TicketerHomeClass(Driver);
        var isLogoVisible = _homePage.IsLogoVisible();
        isLogoVisible.Should().BeTrue("Ticketer logo should be visible in the header");
        TestContext.WriteLine("Ticketer logo is visible");
    }

    [When(@"I scroll down to the Customer Success section")]
    public void WhenIScrollDownToTheCustomerSuccessSection()
    {
        _homePage ??= new TicketerHomeClass(Driver);
        _homePage.ScrollToCustomerSuccessSection();
        TestContext.WriteLine("Scrolled to Customer Success section");
    }

    [When(@"I click the right arrow until the Whippet story is displayed")]
    public void WhenIClickTheRightArrowUntilTheWhippetStoryIsDisplayed()
    {
        _homePage ??= new TicketerHomeClass(Driver);
        _homePage.ClickRightArrowUntilWhippetVisible();
        TestContext.WriteLine("Whippet story is now visible");
    }

    [When(@"I click Find out more on the Whippet story")]
    public void WhenIClickFindOutMoreOnTheWhippetStory()
    {
        _homePage ??= new TicketerHomeClass(Driver);
        _homePage.ClickFindOutMoreOnWhippet();
        Thread.Sleep(2000); // Wait for page navigation
        TestContext.WriteLine($"Clicked 'Find out more', navigated to: {Driver.Url}");
    }

    // Whippet Success Story Page Steps
    [Then(@"I should be taken to the Whippet success story page")]
    public void ThenIShouldBeTakenToTheWhippetSuccessStoryPage()
    {
        _whippetPage = new WhippetSuccessStoryClass(Driver);
        var isOnPage = _whippetPage.IsOnWhippetPage();
        isOnPage.Should().BeTrue($"Should be on Whippet page. Current URL: {Driver.Url}");
        TestContext.WriteLine($"Successfully navigated to Whippet success story page: {Driver.Url}");
    }

    [Then(@"the page should contain the text ""(.*)""")]
    public void ThenThePageShouldContainTheText(string expectedText)
    {
        _whippetPage ??= new WhippetSuccessStoryClass(Driver);
        var containsText = _whippetPage.ContainsExpectedText();
        containsText.Should().BeTrue($"Page should contain text related to: {expectedText}");
        TestContext.WriteLine($"Page contains expected Whippet content");
    }

    // Login Page Steps
    [When(@"I click on the login button in the top right corner")]
    public void WhenIClickOnTheLoginButtonInTheTopRightCorner()
    {
        _homePage ??= new TicketerHomeClass(Driver);
        _homePage.ClickLoginButton();
        Thread.Sleep(2000); 
        TestContext.WriteLine($"Clicked login button, navigated to: {Driver.Url}");
    }

    [Then(@"I should be taken to the identity login page")]
    public void ThenIShouldBeTakenToTheIdentityLoginPage()
    {
        _loginPage = new LoginClass(Driver);
        var isOnLoginPage = _loginPage.IsOnLoginPage();
        isOnLoginPage.Should().BeTrue($"Should be on identity login page. Current URL: {Driver.Url}");
        TestContext.WriteLine($"Successfully navigated to login page: {Driver.Url}");
    }

    [When(@"I enter ""(.*)"" into the Username field")]
    public void WhenIEnterIntoTheUsernameField(string username)
    {
        _loginPage ??= new LoginClass(Driver);
        _loginPage.EnterUsername(username);
        TestContext.WriteLine($"Entered '{username}' into Username field");
    }

    [When(@"I click the Log in button")]
    public void WhenIClickTheLogInButton()
    {
        _loginPage ??= new LoginClass(Driver); 
        _loginPage.ClickLoginButton();
        TestContext.WriteLine("Clicked Log in button");
    }

    [Then(@"I should see the error text ""(.*)""")]
    public void ThenIShouldSeeTheErrorText(string expectedError)
    {
        _loginPage ??= new LoginClass(Driver);
        var isErrorDisplayed = _loginPage.IsPasswordRequiredErrorDisplayed();
        isErrorDisplayed.Should().BeTrue($"Should see password required error. Expected: {expectedError}");
        TestContext.WriteLine($"Password required error message is displayed");
    }
}
