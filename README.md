# MarvelHeroSearchApp

MarvelHeroSearchApp is a web application built using ASP.NET Core MVC and .NET 7.0. It allows users to search for Marvel characters, view character details, and explore related comic books. The application uses the Marvel API to fetch character and comic book data.

## Overview

This project is a comprehensive example of using modern web development technologies to create a responsive and interactive web application. It demonstrates how to integrate third-party APIs, implement search functionality, and enhance user experience with features like autocomplete.

## Features

- Search for Marvel characters by name.
- View character details including description and thumbnail.
- Explore related comic books with cover images and titles.
- View detailed information about each comic book.
- Autocomplete suggestions for character names.

## Technologies Used

- ASP.NET Core MVC
- .NET 7.0
- Bootstrap 4.5.2
- jQuery 3.5.1
- jQuery UI 1.12.1
- Marvel API
- TempData for session management
- HttpClient for API requests

## Getting Started

### Prerequisites

- .NET 7.0 SDK
- Visual Studio 2022 or later
- Marvel API key

### Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/MarvelHeroSearchApp.git
    cd MarvelHeroSearchApp
    ```

2. Open the solution in Visual Studio.

3. Restore the NuGet packages:
    ```bash
    dotnet restore
    ```

4. Create a `appsettings.json` file in the root directory with your Marvel API keys:
    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "Marvel": {
        "PublicKey": "your_marvel_public_key",
        "PrivateKey": "your_marvel_private_key"
      }
    }
    ```

### Getting the Marvel API Key

1. Go to the [Marvel Developer Portal](https://developer.marvel.com/).
2. Sign up for a new account or log in if you already have one.
3. Go to the "My Developer Account" section.
4. Create a new application to get your API keys.
5. Copy the public and private keys and add them to your `appsettings.json` file as shown above.

### Running the Application

1. Build and run the application in Visual Studio or use the .NET CLI:
    ```bash
    dotnet run
    ```

2. Open your browser and navigate to `https://localhost:5001` or the URL specified in your launch settings.

## Usage

1. Enter the name of a Marvel character in the search bar.
2. Select a character from the autocomplete suggestions or press "Search".
3. View the character details and related comic books.
4. Click on a comic book to view its detailed information.

## Screenshots

_Add screenshots of your application here._

### Home Page
![Home Page](![image](https://github.com/karabasnejat/MarvelComicsExplorer/assets/62561906/10e17796-ec7a-4bf0-bb86-1074cbe4ba6f)
)

### Search Results
![Search Results](![image](https://github.com/karabasnejat/MarvelComicsExplorer/assets/62561906/dc3af69e-6ad9-4b65-8835-24f68ae750ec)
)

### Character Details
![Character Details](![image](https://github.com/karabasnejat/MarvelComicsExplorer/assets/62561906/b07b5894-7011-40bc-9a8d-21a4c33368ac)
)

### Comic Book Details
![Comic Book Details](![image](https://github.com/karabasnejat/MarvelComicsExplorer/assets/62561906/e8551527-bee2-49b4-8e11-4e702d396dc7)
)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details.

## Acknowledgements

- [Marvel Developer Portal](https://developer.marvel.com/) for providing the API.
- [Bootstrap](https://getbootstrap.com/) for the responsive UI framework.
- [jQuery](https://jquery.com/) and [jQuery UI](https://jqueryui.com/) for enhancing the user experience.
