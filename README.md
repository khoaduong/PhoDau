# PhoDau — ASP.NET Core Backend + React Frontend

## Architecture

**Backend** (API-only with Swagger)
- ASP.NET Core 8.0 minimal APIs
- Feature-based organization
- Built-in dependency injection and middleware
- Swagger/OpenAPI documentation

**Frontend** (React + Vite)
- React 19 with Hooks
- Vite dev server with hot reload
- API proxy to backend during development
- Production build to `dist/`

## Project Structure

```
PhoDau/
├── Backend/
│   ├── Features/          (Feature-based endpoints)
│   ├── Data/              (Repositories, models)
│   ├── Common/            (Constants, extensions)
│   ├── Program.cs         (API configuration)
│   ├── Backend.csproj
│   └── appsettings*.json
├── Frontend/
│   ├── src/               (React components)
│   ├── public/            (Static assets)
│   ├── vite.config.js     (Vite configuration)
│   ├── package.json
│   └── index.html
└── README.md
```

## Quick Start

### Backend
```bash
# Requires .NET SDK 8.0+
cd Backend
dotnet restore
dotnet run
# Swagger UI: http://localhost:5000/swagger
# API: http://localhost:5000/api/*
```

### Frontend
```bash
# Requires Node.js 18+
cd Frontend
npm install
npm run dev
# App: http://localhost:5173
# API default: http://localhost:5000 (configurable via VITE_API_BASE_URL)
```

## API Endpoints

- `GET /api/hello` — Greeting message with timestamp
- `GET /api/status` — Server status
- `GET /swagger` — Swagger UI (development only)

## Features Included

**Backend**
- RESTful minimal APIs
- Feature-based folder structure (scalable)
- Generic repository pattern
- Shared utilities and constants
- Swagger/OpenAPI support

**Frontend**
- React 19 with React Hooks
- Fetch-based API modules in `src/api/`
- Dev server with API proxy
- ESLint ready
- Production-ready build
