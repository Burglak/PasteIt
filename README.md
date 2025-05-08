# PasteIt

**PasteIt** is a simple pastebin-like web application that allows users to create, view, and share text snippets.

![image](https://github.com/user-attachments/assets/43624b34-c377-4f8a-831c-b6837b468d80)

![image](https://github.com/user-attachments/assets/a564bfb8-3349-4790-9663-76a23fd959f0)


## Features

- Create text snippets with optional titles
- Set snippet expiration time: 1 hour, 1 day, 1 week, or 1 month
- View snippet content, view count, and time remaining until expiration
- Copy snippet content or share link to clipboard
- Minimal, clean UI using Bootstrap

## Technologies

- ASP.NET Core MVC
- Entity Framework Core (Code-First)
- Bootstrap 5

## Getting Started

### Prerequisites

- [.NET 8 SDK]
- [SQL Server Express / LocalDB]

### Installation

1. Clone the repository:
   
   git clone https://github.com/Burglak/PasteIt.git
   cd PasteIt

2. Restore NuGet packages: dotnet restore

3. Apply EF Core migrations: dotnet ef database update

4. Run the application: dotnet run
   
