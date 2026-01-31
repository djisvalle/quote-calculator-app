import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "../../components/ui/button";
import { AlertCircle } from "lucide-react";
import { useNavigate } from "react-router-dom";

import { useLocation } from "react-router";
import InitialQuote from "./InitialQuote";
import { useGetDetailsByPublicId } from "@/hooks/useLoan";

import { Loader2 } from "lucide-react";

export default function QuoteCalculator() {
    const location = useLocation();
    const navigate = useNavigate();
    const loanQuery = new URLSearchParams(location.search).get('loans');
    const { data, isLoading, error } = useGetDetailsByPublicId(loanQuery);

    if (isLoading) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50/50">
                <Loader2 className="h-10 w-10 text-cyan-600 animate-spin mb-4" />
                <p className="text-muted-foreground font-medium">Loading your requested quote...</p>
            </div>
        );
    }
    if (error || !data) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50/50 p-4">
                <Card className="w-full max-w-md border-red-100">
                    <CardHeader className="flex flex-col items-center">
                        <AlertCircle className="h-12 w-12 text-red-500 mb-2" />
                        <CardTitle>Invalid or missing application ID.</CardTitle>
                    </CardHeader>
                    <CardContent className="text-center text-muted-foreground">
                        Please check your application ID and try again.
                    </CardContent>
                    <CardFooter className="justify-center">
                        <Button onClick={() => navigate("/")}>Go Back</Button>
                    </CardFooter>
                </Card>
            </div>
        );
    }


    return <InitialQuote loanApplication={data} />;
}