# LineUp

LineUp is a scheduling tool for when you need 100% coverage over a period of time.

## Project Structure

- **`LineUp.Backend/`** - .NET backend API
- **`lineup-client/`** - React frontend with auto-generated API client

## Quickstart

### Prerequisites
- [.NET 10](https://dotnet.microsoft.com/en-us/download)
- [Node.js](https://nodejs.org/en/download)
- [PNPM](https://pnpm.io/installation)
- [.NET Aspire CLI](https://aspire.dev/get-started/install-cli/)

### Setup and Run

1. **Install backend dependencies**
   ```bash
   dotnet restore
   ```

2. **Install frontend dependencies**
   ```bash
   cd lineup-client
   cp .env.example .env # Make sure to populate .env!
   pnpm install
   ```

3. **Start the app**
   ```bash
   aspire run
   ```

#### API Client Generation

To regenerate the API client after backend changes:
```bash
cd LineUp.Backend
GEN=true dotnet build
```

## Versions:
Postgres: 18.1