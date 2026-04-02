# Copilot Workspace Instructions

## Architecture

- **Backend**: ASP.NET Core 8.0 minimal APIs (API-only, no static files)
- **Frontend**: React 18 + Vite (separate dev/build process)
- **Development**: Run backend and frontend in separate terminals
- **Production**: Build frontend to `dist/`, deploy separately or integrate with backend

## Backend
- **Run**: `dotnet run --project Backend\Backend.csproj` (requires .NET SDK 8.0+)
- **Port**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger (development only)
- **Endpoints**:
  - `GET /api/hello` → `{ message: "Hello from ASP.NET Core", timestamp: "..." }`
  - `GET /api/status` → `{ status: "running", uptime: "..." }`

## Frontend
- **Run**: `npm run dev` from `Frontend/` folder (requires Node.js 18+)
- **Port**: http://localhost:5173
- **Dev proxy**: `/api/*` routes to `http://localhost:5000`
- **Build**: `npm run build` → outputs to `Frontend/dist/`

## Project Structure
- `Backend/Features/` — Feature modules (Hello, Status)
- `Backend/Data/` — Repository pattern, base entities
- `Backend/Common/` — Shared constants, extensions
- `Frontend/src/` — React components and styles
- `Frontend/public/` — Static assets 
