
# Reddit Data Pooler Project

Programming Assignment:     Reddit, much like other social media platforms, provides a way for users to communicate their interests etc. For this exercise, we would like to see you build an application that listens to your choice of subreddits (best to choose one with a good amount of posts). You can use this link to help identify one that interests you.  We'd like to see this as a .NET 6/7 application, and you are free to use any 3rd party libraries you would like.     Your app should consume the posts from your chosen subreddit in near real time and keep track of the following statistics between the time your application starts until it ends:  Posts with most up votes Users with most posts    Your app should also provide some way to report these values to a user (periodically log to terminal, return from RESTful web service, etc.). If there are other interesting statistics you’d like to collect, that would be great. There is no need to store this data in a database; keeping everything in-memory is fine. That said, you should think about how you would persist data if that was a requirement.     To acquire near real time statistics from Reddit, you will need to continuously request data from Reddit's rest APIs.  Reddit implements rate limiting and provides details regarding rate limit used, rate limit remaining, and rate limit reset period via response headers.  Your application should use these values to control throughput in an even and consistent manner while utilizing a high percentage of the available request rate.     It’s very important that the various application processes do not block each other as Reddit can have a high volume on many of their subreddits.  The app should process posts as concurrently as possible to take advantage of available computing resources. While we are only asking to track a single subreddit, you should be thinking about his you could scale up your app to handle multiple subreddits.     While designing and developing this application, you should keep SOLID principles in mind. Although this is a code challenge, we are looking for patterns that could scale and are loosely coupled to external systems / dependencies. In that same theme, there should be some level of error handling and unit testing. The submission should contain code that you would consider production ready.     When you're finished, please put your project in a repository on either GitHub or Bitbucket and send us a link. Please be sure to provide guidance as to where the Reddit API Token values are located so that the team reviewing the code can replace/configure the value. After review, we may follow-up with an interview session with questions for you about your code and the choices made in design/implementation.     While the coding exercise is intended to be an interesting and fun challenge, we are interested in seeing your best work - aspects that go beyond merely functional code, that demonstrate professionalism and pride in your work.  We look forward to your submission!     Accessing the Reddit API     To get the API, register here  Additional documentation can be found here.


## Overview
This project is a .NET 8.0 application designed to interact with the Reddit API. It uses Entity Framework 8.0 for an in-memory database to store and manage data. 
The application includes a .NET API with Swagger documentation for easy navigation and testing of the endpoints.

![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/c1469a57-ce42-415e-8561-5444107c0957)

### Features
- **Subreddit Management:** Add, remove, and list subreddits that users are interested in.
  User can add multiple subreddits to track
  ![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/9dd8206b-b779-4c86-92c5-1eaad5b68765)
  ![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/c98d42c9-0efe-49f0-bd4b-b2029e5b92b1)
  ![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/051727ed-5ca5-4e74-92b0-a1d4b324434e)

User can  count cached posts
![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/0a10902c-9320-425b-a61b-781cba81295f)
![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/2a181820-5812-4718-9c3b-45c7e4e0f1a4)

User can see top posts
![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/b8cf8f6d-9c44-4aab-a1b3-8b54bab5f5f8)

User can see top rated users
![image](https://github.com/vmudinas/SubRedditScooper/assets/5769233/85443847-b0d8-48a6-a6dd-ec7ee9292b9f)



- **Statistical Data Endpoints:** Retrieve statistical data such as users with the most posts or posts with the highest scores.
- **Background Service:** A continuous background service that pools data from the Reddit API, checks for rate limmits

## Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Any IDE that supports .NET development (e.g., [Visual Studio](https://visualstudio.microsoft.com/))

### Installation
1. Clone the repository
2. Run in visual studio

 ### Authentication/Authorization
 Was not required for those endpoints.
 
  ### Third party libraries
 Communication with reddit was done using Reddit.Net  
 
