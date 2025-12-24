@csv
Feature: Generate Test Data CSV
    As a test automation engineer
    I want to generate a CSV file with dummy test data
    So that I can use it for testing purposes

Scenario: Generate CSV file with 10 rows of dummy data
    Given I want to create a test data CSV file
    When I generate the CSV file with 10 rows
    Then the CSV file should be created successfully
    And the CSV file should contain 4 columns: Name, Age, Email, Phone
    And the CSV file should contain 10 rows of data
    And each row should contain different randomly generated data
