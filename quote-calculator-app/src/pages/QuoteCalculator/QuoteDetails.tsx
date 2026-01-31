import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";

import { Loader2, AlertCircle, ChevronLeft } from "lucide-react";

import { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useForm } from "react-hook-form";
import { useFinalizeLoanApplication, useGetDetailsByPublicId, useCalculateLoanApplication } from "@/hooks/useLoan";
import { useGetAllProductTypes } from "@/hooks/useProductType";

import { formatCurrency } from "@/utils/formatters";
import type { FinalizeLoanApplicationRequest } from "@/types/LoanApplication";

import { zodResolver } from "@hookform/resolvers/zod";
import { QuoteDetailsSchema } from "@/validations/QuoteDetailsSchema";

export default function QuoteDetails() {
    const [searchParams] = useSearchParams();
    const publicId = searchParams.get("loans");
    const navigate = useNavigate();

    const { data: applicationData, isLoading: isLoadingApplication, error: applicationError } = useGetDetailsByPublicId(publicId);
    const { data: productTypes } = useGetAllProductTypes();
    const [constraints, setConstraints] = useState({ minimumAmount: 0, maximumAmount: 0, minimumTerm: 0, maximumTerm: 0 });
    const {
        register,
        handleSubmit,
        setValue,
        watch,
        reset,
        trigger,
        formState: { isSubmitting, errors }
    } = useForm({
        resolver: zodResolver(QuoteDetailsSchema)
    });

    const { firstName, lastName, email, mobileNumber, amount, term, interestFee, weeklyRepayment, monthlyRepayment, totalRepayment, establishmentFee } = watch();

    const finalizeLoanApplication = useFinalizeLoanApplication();
    const onSubmit = async (formData: FinalizeLoanApplicationRequest) => {
        const id = searchParams.get("loans") || "";
        await finalizeLoanApplication.mutateAsync({
            ...formData,
            loanApplicationPublicId: id
        });
        navigate(`/quote-calculator/quote-status?loans=${id}`, { replace: true });
    };

    const calculateLoanApplication = useCalculateLoanApplication();
    const [isInitialLoadDone, setIsInitialLoadDone] = useState(false);
    useEffect(() => {
        if (applicationData && applicationData.productType && !isInitialLoadDone) {
            const triggerInitialCalc = async () => {
                try {
                    const result = await calculateLoanApplication.mutateAsync({
                        productTypeId: applicationData.productType,
                        amount: applicationData.amount,
                        term: applicationData.term
                    });

                    reset({
                        loanApplicationPublicId: publicId || "",
                        applicantId: applicationData.applicantId,
                        title: applicationData.title,
                        firstName: applicationData.firstName,
                        lastName: applicationData.lastName,
                        email: applicationData.email,
                        mobileNumber: applicationData.mobileNumber,
                        amount: applicationData.amount,
                        term: applicationData.term,
                        establishmentFee: result.establishmentFee,
                        interestFee: result.interestFee,
                        weeklyRepayment: result.weeklyRepayment,
                        monthlyRepayment: result.monthlyRepayment,
                        totalRepayment: result.totalRepayment
                    });

                    const product = productTypes?.find(x => x.productTypeId === applicationData.productType);
                    if (product) {
                        setConstraints({
                            minimumAmount: product.minimumAmount,
                            maximumAmount: product.maximumAmount,
                            minimumTerm: product.minimumTerm,
                            maximumTerm: product.maximumTerm
                        });
                    }

                    setIsInitialLoadDone(true);
                } catch (err) {
                    console.error("Initial calculation failed", err);
                }
            };

            triggerInitialCalc();
        }
    }, [applicationData, productTypes, reset, publicId, isInitialLoadDone]);

    const [isInfoDialogOpen, setIsInfoDialogOpen] = useState(false);
    const handleValidateInformation = async () => {
        const isValid = await trigger(["firstName", "lastName", "email", "mobileNumber"]);
        if (isValid) {
            setIsInfoDialogOpen(false);
        }
    }

    const [isLoanInfoDialogOpen, setIsLoanInfoDialogOpen] = useState(false);
    const handleRecalculate = async () => {
        const isValid = await trigger(["amount", "term"]);
        if (isValid) {
            const result = await calculateLoanApplication.mutateAsync({
                productTypeId: applicationData?.productType,
                amount: watch("amount"),
                term: watch("term")
            });

            setValue("monthlyRepayment", result.monthlyRepayment);
            setValue("totalRepayment", result.totalRepayment);
            setValue("interestFee", result.interestFee);
            setValue("establishmentFee", result.establishmentFee);

            setIsLoanInfoDialogOpen(false);
        }
    };

    if (isLoadingApplication) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50/50">
                <Loader2 className="h-10 w-10 text-cyan-600 animate-spin mb-4" />
                <p className="text-muted-foreground font-medium">Calculating your personalized quote...</p>
            </div>
        );
    }

    if (applicationError || !applicationData) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-slate-50/50 p-4">
                <Card className="w-full max-w-md border-red-100">
                    <CardHeader className="flex flex-col items-center text-center">
                        <AlertCircle className="h-12 w-12 text-red-500 mb-2" />
                        <CardTitle>Something went wrong</CardTitle>
                    </CardHeader>
                    <CardContent className="text-center text-muted-foreground">
                        Please contact our support team for further assistance.
                    </CardContent>
                    <CardFooter className="justify-center">
                        <Button onClick={() => navigate("/")}>Go Back</Button>
                    </CardFooter>
                </Card>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex flex-col items-center justify-center p-3 bg-slate-50/50">
            <div className="w-full max-w-md mb-4 text-left">
                <Button
                    type="button"
                    variant="ghost"
                    size="sm"
                    className="text-muted-foreground hover:text-cyan-600 transition-colors px-0"
                    onClick={() => navigate(-1)}
                >
                    <ChevronLeft className="h-4 w-4 mr-1" />
                    Back to Calculator
                </Button>
            </div>

            <form onSubmit={handleSubmit(onSubmit)}>
                <Card className="w-full max-w-md py-10 px-1 shadow-lg">
                    <CardHeader className="justify-center">
                        <CardTitle className="text-center text-3xl">Your Quote</CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div>
                            <div className="flex justify-between">
                                <Label className="text-lg">Your information</Label>
                                <Dialog open={isInfoDialogOpen} onOpenChange={setIsInfoDialogOpen}>
                                    <DialogTrigger asChild>
                                        <Button type="button" className="text-md text-cyan-500" variant="link">
                                            Edit
                                        </Button>
                                    </DialogTrigger>

                                    <DialogContent
                                        onPointerDownOutside={(e) => e.preventDefault()}
                                        onEscapeKeyDown={(e) => e.preventDefault()}
                                        className="[&>button]:hidden">
                                        <DialogHeader>
                                            <DialogTitle>Edit Personal Information</DialogTitle>
                                        </DialogHeader>

                                        <div className="space-y-4 py-4">
                                            <div className="space-y-2">
                                                <Label className="text-sm font-medium" htmlFor="firstName">First Name</Label>
                                                <Input {...register("firstName")}
                                                    id="firstName"
                                                    name="firstName"
                                                    placeholder="John"
                                                    className="mt-2"
                                                />
                                                {errors.firstName && <p className="text-red-500 text-xs mt-1">{errors.firstName.message}</p>}
                                            </div>
                                            <div className="space-y-2">
                                                <Label className="text-sm font-medium" htmlFor="lastName">Last Name</Label>
                                                <Input {...register("lastName")}
                                                    id="lastName"
                                                    name="lastName"
                                                    placeholder="Doe"
                                                    className="mt-2"
                                                />
                                                {errors.lastName && <p className="text-red-500 text-xs mt-1">{errors.lastName.message}</p>}
                                            </div>
                                            <div className="space-y-2">
                                                <Label className="text-sm font-medium" htmlFor="email">Email Address</Label>
                                                <Input
                                                    {...register("email")}
                                                    id="email"
                                                    name="email"
                                                    placeholder="johndoe@gmail.com"
                                                    className="mt-2"
                                                />
                                                {errors.email && <p className="text-red-500 text-xs mt-1">{errors.email.message}</p>}
                                            </div>
                                            <div className="space-y-2">
                                                <Label className="text-sm font-medium" htmlFor="mobileNumber">Mobile Number</Label>
                                                <Input
                                                    {...register("mobileNumber")}
                                                    id="mobileNumber"
                                                    name="mobileNumber"
                                                    placeholder="000-000-0000"
                                                    className="mt-2"
                                                />
                                                {errors.mobileNumber && <p className="text-red-500 text-xs mt-1">{errors.mobileNumber.message}</p>}
                                            </div>
                                        </div>

                                        <DialogFooter>
                                            <Button type="button" onClick={handleValidateInformation}>Done</Button>

                                        </DialogFooter>
                                    </DialogContent>
                                </Dialog>
                            </div>
                            <div className="flex justify-between mt-2">
                                <span className="text-gray-400 font-semibold">Name</span>
                                <div className="text-right text-gray-400 font-semibold">{firstName} {lastName}</div>
                            </div>
                            <div className="flex justify-between mt-3">
                                <span className="text-gray-400 font-semibold">Mobile</span>
                                <div className="text-right text-gray-400 font-semibold">{mobileNumber}</div>
                            </div>
                            <div className="flex justify-between mt-3">
                                <span className="text-gray-400 font-semibold">Email</span>
                                <div className="text-right text-gray-400 font-semibold">{email}</div>
                            </div>
                        </div>
                        <div className="mt-7">
                            <div className="flex justify-between">
                                <Label className="text-lg">Finance Details</Label>
                                <Dialog open={isLoanInfoDialogOpen} onOpenChange={setIsLoanInfoDialogOpen}>
                                    <DialogTrigger asChild>
                                        <Button type="button" className="text-md text-cyan-500" variant="link">
                                            Edit
                                        </Button>
                                    </DialogTrigger>

                                    <DialogContent
                                        onPointerDownOutside={(e) => e.preventDefault()}
                                        onEscapeKeyDown={(e) => e.preventDefault()}
                                        className="[&>button]:hidden">
                                        <DialogHeader>
                                            <DialogTitle>Edit Loan Amount & Term</DialogTitle>
                                        </DialogHeader>

                                        <div className="w-full space-y-2 mt-5">
                                            <div className="flex items-center justify-between">
                                                <Label htmlFor="range-slider-amount">How much do you need?</Label>
                                                <span className="font-medium text-sm">${amount}</span>
                                            </div>
                                            <Input
                                                className="cursor-pointer bg-background"
                                                id="range-slider-amount"
                                                max={constraints.maximumAmount}
                                                min={constraints.minimumAmount}
                                                onChange={e => setValue("amount", Number(e.target.value))}
                                                type="range"
                                                value={amount}
                                            />
                                            {errors.amount && <p className="text-red-500 text-xs mt-1">{errors.amount.message}</p>}
                                            <div className="flex justify-between text-muted-foreground text-xs">
                                                <span>${constraints.minimumAmount}</span>
                                                <span>${constraints.maximumAmount}</span>
                                            </div>
                                        </div>
                                        <div className="w-full space-y-2 mt-5">
                                            <div className="flex items-center justify-between">
                                                <Label htmlFor="range-slider-term">Repayment Term (in months)</Label>
                                                <span className="font-medium text-sm">{term} month(s)</span>
                                            </div>
                                            <Input
                                                className="cursor-pointer bg-background"
                                                id="range-slider-term"
                                                max={constraints.maximumTerm}
                                                min={constraints.minimumTerm}
                                                onChange={e => setValue("term", Number(e.target.value))}
                                                type="range"
                                                value={term}
                                            />
                                            {errors.term && <p className="text-red-500 text-xs mt-1">{errors.term.message}</p>}
                                            <div className="flex justify-between text-muted-foreground text-xs">
                                                <span>{constraints.minimumTerm}</span>
                                                <span>{constraints.maximumTerm}</span>
                                            </div>
                                        </div>
                                        <DialogFooter>
                                            <Button type="button" onClick={handleRecalculate}>Calculate</Button>
                                        </DialogFooter>
                                    </DialogContent>
                                </Dialog>
                            </div>
                            <div className="flex justify-between items-end pb-1 border-b border-dashed border-gray-300 mt-2">
                                <span className="text-gray-400 text-md">Finance amount</span>
                                <div className="text-right">
                                    <div className="text-cyan-500 text-lg font-semibold">${formatCurrency(amount)}</div>
                                    <div className="text-xs text-gray-400">over {term} months</div>
                                </div>
                            </div>
                            <div className="flex justify-between items-end pb-1 border-b border-dashed border-gray-300 mt-5">
                                <span className="text-gray-400 text-md">Repayments from</span>
                                <div className="text-right">
                                    <div className="text-cyan-500 text-lg font-semibold">${formatCurrency(monthlyRepayment)}</div>
                                    <div className="text-xs text-gray-400">Monthly</div>
                                </div>
                            </div>
                        </div>
                    </CardContent>
                    <CardFooter className="flex-col gap-4 mt-5">
                        <Button className="w-full max-w-[280px]" type="submit" disabled={isSubmitting}>
                            {isSubmitting ? "Processing..." : "Apply Now"}
                        </Button>
                        <Label className="w-3/4 text-xs text-muted-foreground mt-3 text-center">
                            Total repayments ${formatCurrency(totalRepayment)}, made up of an establishment fee of ${formatCurrency(establishmentFee)} interest of ${formatCurrency(interestFee)}. The repayment amount is based on the variables selected, is subject to our assessment and suitability, and other important terms and conditions apply.
                        </Label>
                    </CardFooter>
                </Card>
            </form>
        </div>
    );
}