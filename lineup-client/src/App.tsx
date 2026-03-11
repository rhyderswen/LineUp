// App.tsx
import "@/App.css";
import Topbar from "@/components/Topbar";
import Availability from "@/pages/Availability";
import Error from "@/pages/Error";
import Home from "@/pages/Home";
import NewSchedule from "@/pages/NewSchedule";
import ViewEditSchedule from "@/pages/ViewEditSchedule";
import { createBrowserRouter, Navigate, Outlet, RouterProvider, type LoaderFunctionArgs } from "react-router";

// Wrap Topbar as a layout route so it wraps all pages
function Layout() {
  return (
    <Topbar>
      <Outlet />
    </Topbar>
  );
}

async function baseLoader(url: string, param: string) {
  // url should use {} for where the param should be
  const controller = new AbortController();
  const timeout = setTimeout(() => controller.abort(), 5000); // 5s timeout

  try {
    const res = await fetch(url.replace("{}", param), {
      credentials: "include",
      signal: controller.signal,
    });

    if (!res.ok) {
      throw new Response("Parameter not found", {
        status: res.status,
        statusText: res.statusText,
      });
    }

    return res.json();
  } catch (err: unknown) {
    if (err instanceof DOMException && err.name === "AbortError") {
      throw new Response("API request timed out", { status: 504, statusText: "Gateway Timeout" });
    }

    if (err instanceof Response) {
      throw err;
    }

    throw new Response("Failed to reach API", { status: 503, statusText: "Service Unavailable" });
  } finally {
    clearTimeout(timeout);
  }
}

async function availabilityLoader({ params }: LoaderFunctionArgs) {
  return baseLoader("/api/schedule/{}", params.guid!);
}

async function scheduleLoader({ params }: LoaderFunctionArgs) {
  return baseLoader("/api/schedule/{}/details", params.guid!);
}

const router = createBrowserRouter([
  {
    element: <Layout />,
    errorElement: <Error />,
    children: [
      {
        path: "/",
        element: <Home />,
      },
      {
        path: "/newschedule",
        element: <NewSchedule />,
      },
      {
        path: "/schedule",
        children: [
          {
            index: true, // matches exactly /schedule
            element: <Navigate to="/" replace />,
          },
          {
            path: ":guid", // matches /schedule/:guid
            element: <Availability />,
            loader: availabilityLoader,
          },
          {
            path: ":guid/edit", // matches /schedule/:guid/edit
            element: <ViewEditSchedule />,
            loader: scheduleLoader,
          },
        ],
      },
    ],
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
