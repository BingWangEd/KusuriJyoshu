import * as React from "react";
import * as ReactDOM from "react-dom/client";
import {
  createBrowserRouter,
  RouterProvider,
} from "react-router-dom";
import "./index.css";
import Chat from "./Chat";
import EditPrompt from "./EditPrompt";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Chat />,
  },
  {
    path: "/chat",
    element: <Chat />,
  },
  {
    path: "/edit",
    element: <EditPrompt />,
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);