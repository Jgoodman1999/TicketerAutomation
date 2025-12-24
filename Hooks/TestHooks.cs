using NUnit.Framework;
using OpenQA.Selenium;
using Reqnroll;
using TicketerAutomation.Drivers;

namespace TicketerAutomation.Hooks;

[Binding]
public class TestHooks
{
    private readonly ScenarioContext _scenarioContext;

    public TestHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario("@ui")]
    public void BeforeUIScenario()
    {
        var driver = WebDriverFactory.CreateDriver();
        _scenarioContext.Set(driver, "WebDriver");
    }

    [AfterScenario("@ui")]
    public void AfterUIScenario()
    {
        if (_scenarioContext.TryGetValue("WebDriver", out IWebDriver driver))
        {
            driver.Quit();
            driver.Dispose();
        }
    }

    [AfterScenario]
    public void TakeScreenshotOnFailure()
    {
        if (_scenarioContext.TestError != null)
        {
            if (_scenarioContext.TryGetValue("WebDriver", out IWebDriver driver))
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var fileName = $"Error_{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);
                screenshot.SaveAsFile(filePath);
                TestContext.WriteLine($"Screenshot saved to: {filePath}");
            }
        }
    }
}
