@ui
Feature: Ticketer Website Navigation and Login
    As a user of the Ticketer website
    I want to navigate through the site and access the login page
    So that I can verify the website functionality

Scenario: Navigate through Ticketer website to Whippet success story and login page
    Given I navigate to the Ticketer homepage
    And I accept the cookies prompt
    Then the Ticketer logo should be visible in the top left corner
    
    When I scroll down to the Customer Success section
    And I click the right arrow until the Whippet story is displayed
    And I click Find out more on the Whippet story
    Then I should be taken to the Whippet success story page
    And the page should contain the text "Increasing Whippet's passenger numbers"
    
    When I click on the login button in the top right corner
    Then I should be taken to the identity login page
    
    When I enter "Test" into the Username field
    And I click the Log in button
    Then I should see the error text "The Password field is required."
