import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { CircleCheckBig, Loader2, AlertCircle, XCircle, FileClock } from "lucide-react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useReceiptDetailsByPublicId } from "@/hooks/useLoan";

export default function StatusPage() {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const publicId = searchParams.get("loans");

    const { data, isLoading, isError } = useReceiptDetailsByPublicId(publicId || "");

    if (isLoading) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50/50">
                <Loader2 className="h-10 w-10 text-cyan-600 animate-spin mb-4" />
                <p className="text-muted-foreground font-medium">Retrieving your application status...</p>
            </div>
        );
    }

    if (isError || !data || data.status === null || !publicId) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50/50 p-4">
                <Card className="w-full max-w-md border-red-100 shadow-lg">
                    <CardHeader className="flex flex-col items-center">
                        <AlertCircle className="h-12 w-12 text-red-500 mb-2" />
                        <CardTitle>Application Not Found</CardTitle>
                    </CardHeader>
                    <CardContent className="text-center text-muted-foreground">
                        We couldn't find an application associated with this link. It may have expired or the ID is incorrect.
                    </CardContent>
                    <CardFooter className="justify-center">
                        <Button variant="outline" onClick={() => navigate('/')}>Return to Home</Button>
                    </CardFooter>
                </Card>
            </div>
        );
    }

    const renderStatusContent = () => {
        switch (data.status) {
            case 0:
                return {
                    icon: <FileClock className="h-12 w-12 text-amber-500" />,
                    iconBg: "bg-amber-100",
                    title: "Application Incomplete",
                    message: `Hi ${data.firstName}, your application for $${data.amount.toLocaleString()} is still in draft mode.`,
                    subtext: "Please return to the previous step to finalize your submission.",
                    buttonText: "Resume Application"
                };
            case 2:
                return {
                    icon: <XCircle className="h-12 w-12 text-red-600" />,
                    iconBg: "bg-red-100",
                    title: "Application Declined",
                    message: `We've reviewed your request for $${data.amount.toLocaleString()}, ${data.firstName}.`,
                    subtext: "Unfortunately, we are unable to proceed with your application at this time. A formal notice has been sent to your email.",
                    buttonText: "Return to Home"
                };
            case 1:
            default:
                return {
                    icon: <CircleCheckBig className="h-12 w-12 text-green-600" />,
                    iconBg: "bg-green-100",
                    title: "Application Submitted!",
                    message: (
                        <>
                            Thanks <span className="text-foreground font-semibold">{data.firstName}</span>, 
                            we've received your request for <span className="text-foreground font-semibold">${data.amount.toLocaleString()}</span>.
                        </>
                    ),
                    subtext: `Our assessment team is currently reviewing your details. You will receive an update at ${data.email} within 24 hours.`,
                    buttonText: "Return to Home"
                };
        }
    };

    const status = renderStatusContent();

    return (
        <div className="min-h-screen flex flex-col items-center justify-center p-4 bg-slate-50/50">
            <Card className="w-full max-w-2xl py-2 shadow-lg">
                <CardHeader className="flex flex-col items-center justify-center gap-4 pt-10">
                    <div className={`rounded-full ${status.iconBg} p-3 dark:bg-opacity-20`}>
                        {status.icon}
                    </div>
                    <div className="space-y-1 text-center">
                        <CardTitle className="text-3xl font-bold tracking-tight">{status.title}</CardTitle>
                        {data.referenceNumber &&
                            <p className="text-sm text-muted-foreground">Reference: {data.referenceNumber}</p>
                        }
                        
                    </div>
                </CardHeader>

                <CardContent className="space-y-4 text-center">
                    <div className="text-muted-foreground text-lg">
                        {status.message}
                    </div>
                    <p className="text-sm text-muted-foreground bg-slate-100 p-4 rounded-lg">
                        {status.subtext}
                    </p>
                </CardContent>

                <CardFooter className="flex justify-center pb-10">
                    <Button 
                        className="w-full max-w-xs" 
                        onClick={() => navigate('/')}
                        variant={data.status === 2 ? "destructive" : "default"}
                    >
                        {status.buttonText}
                    </Button>
                </CardFooter>
            </Card>
        </div>
    );
}