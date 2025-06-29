# 📱 Contact Manager

A modern, feature-rich Contact Management System built with **ASP.NET Core 8.0** and **Entity Framework Core**. Manage your contacts with style and efficiency!

![Dashboard Preview](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue) ![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-green) ![Bootstrap](https://img.shields.io/badge/Bootstrap-5-purple) ![SQLite](https://img.shields.io/badge/SQLite-Database-orange)

## ✨ Features

### 🏠 **Dashboard**
- **Modern, responsive dashboard** with real-time statistics
- **Quick overview cards** showing total contacts, favorites, and monthly growth
- **Recent contacts** display with profile photos
- **Favorite contacts** quick access
- **Quick action buttons** for import/export operations

### 👥 **Contact Management**
- ✅ **Create, Read, Update, Delete** (CRUD) operations
- 📸 **Profile photo upload** and management
- ⭐ **Favorite contacts** marking system
- 🔍 **Advanced search and filtering**
- 📄 **Pagination** for large contact lists
- 📊 **Sorting** by name, date created, etc.

### 🔒 **Authentication & Security**
- 🔐 **ASP.NET Core Identity** integration
- 👤 **User registration and login**
- 🛡️ **Role-based access control**
- 🔑 **Password reset functionality**
- 🚪 **Secure logout**

### 📥📤 **Import/Export**
- 📄 **CSV export** for spreadsheet compatibility
- 🔗 **XML export** for structured data
- 📥 **XML import** with duplicate detection
- 🔄 **Bulk operations** support

### 🎨 **Modern UI/UX**
- 📱 **Fully responsive design** (mobile-first)
- 🎨 **Modern Bootstrap 5** styling
- ✨ **Smooth animations** and hover effects
- 🌈 **Beautiful gradient backgrounds**
- 📱 **Mobile-optimized** interface
- 🎯 **Intuitive navigation**

## 🛠️ Technologies Used

- **Backend**: ASP.NET Core 8.0 (C#)
- **Frontend**: Razor Pages, Bootstrap 5, JavaScript
- **Database**: SQLite (Entity Framework Core)
- **Authentication**: ASP.NET Core Identity
- **Styling**: Custom CSS with Bootstrap 5
- **Icons**: Font Awesome
- **File Processing**: CsvHelper for CSV operations

## 📋 Prerequisites

Before running this application, make sure you have the following installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQLite](https://www.sqlite.org/) (included with .NET)

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd Contact
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Update Database
```bash
dotnet ef database update
```

### 4. Run the Application
```bash
dotnet run
```

### 5. Open in Browser
Navigate to `https://localhost:5021` or `http://localhost:5020`

## 📁 Project Structure

```
ContactManager/
├── 📁 Areas/Identity/          # ASP.NET Core Identity UI
├── 📁 Controllers/             # MVC Controllers
│   ├── 🎮 ContactsController.cs
│   └── 🏠 HomeController.cs
├── 📁 Data/                    # Database Context
│   └── 🗃️ ApplicationDbContext.cs
├── 📁 Models/                  # Data Models
│   ├── 👤 Contact.cs
│   └── ❌ ErrorViewModel.cs
├── 📁 ViewModels/              # View Models
│   └── 📋 ContactViewModels.cs
├── 📁 Views/                   # Razor Views
│   ├── 📁 Contacts/
│   ├── 📁 Home/
│   └── 📁 Shared/
├── 📁 wwwroot/                 # Static Files
│   ├── 🎨 css/
│   ├── 📜 js/
│   └── 🖼️ uploads/
├── 📁 Migrations/              # EF Core Migrations
└── 📄 Program.cs               # Application Entry Point
```

## 🎯 Key Features Explained

### Dashboard Analytics
- **Total Contacts**: Real-time count of all contacts
- **Favorites**: Count of marked favorite contacts  
- **Monthly Growth**: Contacts added in current month
- **Growth Rate**: Percentage calculation of monthly growth

### Contact Operations
- **Search**: Find contacts by name, email, or phone
- **Filter**: Show all contacts or favorites only
- **Pagination**: Handle large datasets efficiently
- **Profile Photos**: Upload and display contact images

### Data Management
- **CSV Export**: Export all contacts to spreadsheet format
- **XML Export**: Structured data export for backups
- **XML Import**: Bulk import with duplicate detection
- **Data Validation**: Comprehensive input validation

## 🗄️ Database Schema

### Contacts Table
| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary Key |
| Name | string(100) | Contact Name |
| Email | string(150) | Email Address |
| Phone | string(20) | Phone Number |
| Address | string(500) | Physical Address |
| ProfilePhotoPath | string | Profile Image Path |
| IsFavorite | bool | Favorite Status |
| DateCreated | DateTime | Creation Timestamp |
| UserId | string | Foreign Key to AspNetUsers |

## 🚀 Deployment Options

### 1. 🚫 **Vercel** (Not Recommended)
**Vercel is primarily designed for frontend frameworks and serverless functions, not for ASP.NET Core applications.** While technically possible with workarounds, it's not the ideal choice.

### 2. ✅ **Recommended Deployment Platforms**

#### **Azure App Service** (Best for .NET)
```bash
# Install Azure CLI
# Create App Service Plan
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku B1

# Create Web App
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name myContactManager --runtime "DOTNETCORE|8.0"

# Deploy
dotnet publish -c Release
# Upload to Azure via Visual Studio or Azure CLI
```

#### **Railway** (Simple & Affordable)
1. Create account at [Railway.app](https://railway.app)
2. Connect GitHub repository
3. Railway automatically detects .NET and deploys
4. Add SQLite or upgrade to PostgreSQL

#### **Heroku** (Free Tier Available)
```bash
# Install Heroku CLI
heroku create your-contact-manager
heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack
git push heroku main
```

#### **DigitalOcean App Platform**
1. Create account at [DigitalOcean](https://www.digitalocean.com)
2. Use App Platform
3. Connect GitHub repository
4. Automatic deployment with .NET detection

#### **Self-Hosted (VPS/Dedicated Server)**
```bash
# On Ubuntu/Debian server
sudo apt update
sudo apt install -y aspnetcore-runtime-8.0

# Upload your published app
dotnet YourApp.dll
```

### 3. 🐳 **Docker Deployment**

Create `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ContactManager.csproj", "."]
RUN dotnet restore "./ContactManager.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ContactManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContactManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactManager.dll"]
```

Deploy with Docker:
```bash
docker build -t contact-manager .
docker run -p 8080:80 contact-manager
```

## 📊 Performance Considerations

- **Database**: SQLite for development, consider PostgreSQL/SQL Server for production
- **File Storage**: Local storage for development, cloud storage (Azure Blob, AWS S3) for production
- **Caching**: Consider adding Redis for session management in production
- **CDN**: Use CDN for static assets in production

## 🔧 Configuration

### Production Settings
Update `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-production-connection-string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Environment Variables
- `ASPNETCORE_ENVIRONMENT=Production`
- `ConnectionStrings__DefaultConnection=your-db-connection`

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Support

If you have any questions or need support, please:
- Open an issue on GitHub
- Contact: your-email@example.com

## 🙏 Acknowledgments

- ASP.NET Core team for the excellent framework
- Bootstrap team for the UI components
- Font Awesome for the beautiful icons
- Entity Framework team for the ORM

---

**Made with ❤️ using ASP.NET Core 8.0**
