# 🐵 MonkeyFinder

MonkeyFinder is a simple but powerful .NET MAUI application built with .NET 8 and use MVVM architecture, 
designed to showcase data about various monkeys. It uses cross-platform .NET MAUI and .NET Community Toolkit features, along with custom implementations for handling HTTP requests, navigation, and platform services.

## 📱 Features

- **HTTP Client**: Custom `MonkeyHttpClient` for API calls to get monkey data.
- **Command Implementation**: Uses `RelayCommand` for easy and maintainable command binding in the MVVM pattern.
- **Navigation**: Simplified navigation across pages within the app.
- **Platform Services**: Integrates with platform-specific features through interfaces:
  - `IConnectivity`: Check internet connectivity.
  - `IGeolocation`: Get closest' monkey.
  - `IMap`: Open map with selected monkey.
- **Dark/Light Mode**: Automatically adjusts to system theme settings.
- **.NET Community Toolkit for MVVM**: Utilizes source generators to simplify ViewModel creation.
- **MVVM** architecture
