﻿Feature: Automation Testing Practice

Practice Some Functionality 

@TestingPractice
Scenario: Exploring Automation Testing Practice Page 
	Given Go To Automation Testing Practice Page 
	When Enter Users Name, Email, Phone, Address, Gender and Days
	| Name        | Email           | Phone       | Address | Gender | Day1   | Day2   | Day3     |
	| Mahir Afsar | Mahir@gmail.com | 01302309325 |         | Male   | Sunday | Monday | Thursday |
	#And Select Country, Colors, Date and Click On link
	#| Country | Color1 | Color2 | Color3 | Date       |
	#| Japan   | Red    | Green  | White  | 09/10/2024 |
	#Then Is Total Price of Product Is '7100'
	#Then Check pagination table that, Is '10' Products price is greater than '$15'
	#When Search <SearchKey> on search field
	#| SearchKey |
	#| Messi     |
	#And Go To New Browser Window and Come Back
	And Click on Alert, Confirm Box and Prompt
