# Todo Exercise

This exercise demonstrates a typical workflow/sprint for an existing API application. 

Candidates will need the following:
1. A [GitHub](https://github.com/) account
2. An IDE such as [Visual Studio Code](https://code.visualstudio.com/download)

This exercise provides candidates the opportunity to demonstrate a working knowledge of the following subjects:
- Git
- ASP.NET WebApi (.NET 7)
- Unit testing

> Note: Candidates are assessed on commit behaviour as much as the code changes.

# Getting started

## Create a new repository

1. Create a new public repository at [github.com](https://github.com/)
2. Clone the repository to your development machine
   ```ps
   git clone https://github.com/[your-account]/[your-repo]
   ```
3. Copy the exercise files into your new repository and commit them
6. Complete the scenario tasks below

# Scenario
A back-end API services a front-end application that allows users to create and manage Todo items. The front-end is managed by a different team, and is not part of this exercise. However, the tasks below will require you to make changes to the API to support new features and bug fixes.

# Tasks
The following user stories have been split into a typical workflow of Features, Bug-fixes and Technical tasks. 
- They have been created off the back of a sprint planning session.
- Most of the tasks are independent of each other.
- They don't need to be completed in any particular order.

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

# Submitting your work
When you have finished the exercise, please push your changes and provide a link to your new repository.

# Further information

## Todo Item Example
```json
{
   "id": "504c4345-121b-4523-865d-ce76416c16fd",
   "text": "Feed the cat",
   "created": "2022-03-23T16:41:29.9809001Z",
   "completed": "2022-03-23T20:01:10.1265001Z"
}
```
