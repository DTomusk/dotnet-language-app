import { router } from "./app/AppRoutes";
import { RouterProvider } from "react-router-dom";
import { AuthProvider } from "./app/AuthProvider";

function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  );
}

export default App
