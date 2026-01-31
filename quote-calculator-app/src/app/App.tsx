import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Router } from "./Router";

export default function App() {
    return (
        <QueryClientProvider client={new QueryClient()}>
            <Router />
        </QueryClientProvider>
    )
}