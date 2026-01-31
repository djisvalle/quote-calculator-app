import { createBrowserRouter, RouterProvider, type RouteObject } from "react-router";
import QuoteCalculator from "@/pages/QuoteCalculator";
import QuoteDetails from "@/pages/QuoteCalculator/QuoteDetails";
import StatusPage from "@/pages/QuoteCalculator/StatusPage";
import Home from "@/pages/Home";

export const routes: RouteObject[] = [
    {
        path: "/quote-calculator/initial-quote",
        element: <QuoteCalculator />,
    },
    {
        path: "/quote-calculator/quote-details",
        element: <QuoteDetails />,
    },
    {
        path: "/quote-calculator/quote-status",
        element: <StatusPage />,
    },
    {
        path: "*",
        element: <Home />,
    }
]

export function Router() {
    return (
        <RouterProvider router={createBrowserRouter(routes)} />
    )
}