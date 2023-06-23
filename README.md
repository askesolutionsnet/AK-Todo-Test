# Todo Exercise

This exercise demonstrates a typical workflow/sprint for an existing API application. 

# Getting started

# Scenario
The tasks below will require you to make changes to the API to support new features and bug fixes.

# Tasks
1. Bug fix: 'Wrong times'
   > Bug: Users in the UK are reporting that the `Created` field is off by one hour sometimes. Please investigate and fix.

2. New feature: 'Mark as completed'
   > As a user, I want the ability to mark my todo items as completed

3. New feature: 'Show/hide completed items'
   > As a user, I want the ability to show/hide Todo items that I have completed
   
4. New Feature: 'Form validation`
   > As a user, I should not be able to create an empty Todo item
   
5. New Feature: 'Item sorting'
   > As a user, I want to see the most recently created items at the top
   
6. New Feature: 'Make all items uppercase'
   > As a user, I want my todo item text to automatically convert to uppercase _after_ I've submitted it

7. Technical Story
   > Add unit tests for _new_ API logic

## Todo Item Example
{
   "id": "504c4345-121b-4523-865d-ce76416c16fd",
   "text": "Feed the cat",
   "created": "2022-03-23T16:41:29.9809001Z",
   "completed": "2022-03-23T20:01:10.1265001Z"
}
