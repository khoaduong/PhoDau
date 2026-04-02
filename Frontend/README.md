# React + Vite Frontend

Modern React frontend with Vite build tooling, optimized for development and production.

## Features

- **React 18** — Latest version with hooks and concurrent features
- **Vite** — Lightning-fast build tool and dev server
- **Axios** — HTTP client for API calls
- **Proxy** — Dev server proxies `/api` to backend (http://localhost:5000)
- **ESLint** — Linting support

## Quick Start

### Install Dependencies
```bash
npm install
```

### Development
```bash
npm run dev
```
Starts Vite dev server at `http://localhost:5173`

Dev server proxies `/api/*` calls to the backend at `http://localhost:5000`.

### Build
```bash
npm run build
```
Outputs optimized production build to `dist/`

### Preview Production Build
```bash
npm run preview
```

## Project Structure

```
Frontend/
├── src/
│   ├── main.jsx           (Entry point)
│   ├── App.jsx            (Root component)
│   ├── App.css            (Component styles)
│   └── index.css          (Global styles)
├── public/                (Static assets)
├── index.html             (HTML template)
├── vite.config.js         (Vite configuration)
├── package.json           (Dependencies)
└── .gitignore
```

## API Integration

The dev server proxies API calls:
- Local request: `GET /api/hello`
- Forwarded to: `http://localhost:5000/api/hello`

## ESLint

```bash
npm run lint
```

## Notes

- Vite dev server runs on port 5173 (configured in `vite.config.js`)
- Backend must run on port 5000 for proxy to work
- Build output goes to `dist/` folder
