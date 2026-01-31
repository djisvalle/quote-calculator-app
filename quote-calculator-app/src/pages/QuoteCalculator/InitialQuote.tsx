import { Card, CardContent, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "../../components/ui/button";
import { Input } from "../../components/ui/input";
import { Label } from "../../components/ui/label";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "../../components/ui/select";
import { Calendar } from "@/components/ui/calendar"
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover"
import { Field, FieldLabel } from "@/components/ui/field"
import { Separator } from "@/components/ui/separator";

import { Loader2 } from "lucide-react";

import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

import { useGetAllProductTypes } from "@/hooks/useProductType";
import { useSaveDraftLoanApplication } from "@/hooks/useLoan";
import type { LoanApplicationDetails } from "@/types/LoanApplication";

import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { InitialQuoteSchema } from "@/validations/InitialQuoteSchema";
import { useForm } from "react-hook-form";

export default function InitialQuote({ loanApplication }: { loanApplication: LoanApplicationDetails }) {

    const { data, isLoading } = useGetAllProductTypes();

    type InitialQuoteFormData = z.infer<typeof InitialQuoteSchema>;
    const {
        register,
        handleSubmit,
        setValue,
        watch,
        trigger,
        formState: { errors }
    } = useForm<InitialQuoteFormData>({
        resolver: zodResolver(InitialQuoteSchema),
        defaultValues: {
            applicantId: loanApplication.applicantId,
            loanApplicationPublicId: loanApplication.loanApplicationPublicId,
            productType: loanApplication.productType == 0 ? "" : loanApplication.productType.toString(),
            amount: 0,
            term: 0,
            title: loanApplication.title,
            firstName: loanApplication.firstName,
            lastName: loanApplication.lastName,
            email: loanApplication.email,
            mobileNumber: loanApplication.mobileNumber,
            dateOfBirth: new Date(loanApplication.dateOfBirth)
        }
    });

    const amountValue = watch("amount");
    const termValue = watch("term");
    const productType = watch("productType");
    const dateOfBirth = watch("dateOfBirth");
    const title = watch("title");

    const [constraints, setConstraints] = useState({ minimumAmount: 0, maximumAmount: 0, minimumTerm: 0, maximumTerm: 0 });

    function handleProductTypeChange(val: string) {
        const product = data?.find(x => x.productTypeId.toString() === val);
        if (product) {
            setConstraints({
                minimumAmount: product.minimumAmount,
                maximumAmount: product.maximumAmount,
                minimumTerm: product.minimumTerm,
                maximumTerm: product.maximumTerm
            });
            setValue("productType", val);
            setValue("amount", loanApplication.amount < product.minimumAmount ? product.minimumAmount : loanApplication.amount);
            setValue("term", loanApplication.term < product.minimumTerm ? product.minimumTerm : loanApplication.term);
            trigger(["amount", "term", "productType"]);
        }
    }

    const navigate = useNavigate();
    const saveDraftLoanApplication = useSaveDraftLoanApplication();
    const onSubmit = async (formData: InitialQuoteFormData) => {

        await saveDraftLoanApplication.mutateAsync({
            loanApplicationPublicId: formData.loanApplicationPublicId,
            applicantId: formData.applicantId,
            title: formData.title,
            firstName: formData.firstName,
            lastName: formData.lastName,
            email: formData.email,
            mobileNumber: formData.mobileNumber,
            amount: formData.amount,
            term: formData.term,
            productTypeId: parseInt(formData.productType)
        });

        navigate(`/quote-calculator/quote-details?loans=${loanApplication.loanApplicationPublicId}`);
    };

    useEffect(() => {
        if (data && data.length > 0 && loanApplication.productType) {

            const selectedId = loanApplication.productType.toString();
            const product = data.find(x => x.productTypeId.toString() === selectedId);

            if (product) {
                setValue("productType", selectedId);
                setValue("amount", loanApplication.amount < product.minimumAmount ? product.minimumAmount : loanApplication.amount);
                setValue("term", loanApplication.term < product.minimumTerm ? product.minimumTerm : loanApplication.term);

                setConstraints({
                    minimumAmount: product.minimumAmount,
                    maximumAmount: product.maximumAmount,
                    minimumTerm: product.minimumTerm,
                    maximumTerm: product.maximumTerm
                });

                trigger(["amount", "term", "productType"]);
            }
        }
    }, [data, loanApplication, setValue, trigger]);


    if (isLoading) {
        return (
            <div className="min-h-screen flex flex-col items-center justify-center bg-slate-50/50">
                <Loader2 className="h-10 w-10 text-cyan-600 animate-spin mb-4" />
                <p className="text-muted-foreground font-medium">Retrieving your quote details...</p>
            </div>
        );
    }

    return (
        <div className="min-h-screen flex flex-col items-center justify-center p-4 bg-slate-50/50">
            <form onSubmit={handleSubmit(onSubmit)}>
                <Card className="w-full max-w-2xl py-5 shadow-lg">
                    <CardHeader className="justify-center">
                        <CardTitle className="text-center text-3xl">Quote Calculator</CardTitle>
                    </CardHeader>
                    <CardContent>
                        <div className="mt-5">
                            <div className="w-1/2">
                                <Label className="text-sm font-medium" htmlFor="name">Product Type</Label>
                                <Select value={productType} onValueChange={e => handleProductTypeChange(e)} required>
                                    <SelectTrigger className="w-full mt-2">
                                        <SelectValue placeholder="Select Product Type" />
                                    </SelectTrigger>
                                    <SelectContent>
                                        {data?.map((p) => (
                                            <SelectItem key={p.productTypeId} value={p.productTypeId.toString()}>{p.productName}</SelectItem>
                                        ))}
                                    </SelectContent>
                                </Select>
                                {errors.productType && <p className="text-red-500 text-xs mt-1">{errors.productType.message}</p>}
                            </div>
                        </div>
                        <Separator className="my-5" />
                        <div className="w-full space-y-2 mt-5">
                            <div className="flex items-center justify-between">
                                <Label htmlFor="range-slider-amount">How much do you need?</Label>
                                <span className="font-medium text-sm">${amountValue}</span>
                            </div>
                            <Input
                                className="cursor-pointer bg-background"
                                id="range-slider-amount"
                                max={constraints.maximumAmount}
                                min={constraints.minimumAmount}
                                onChange={e => setValue("amount", Number(e.target.value))}
                                type="range"
                                value={amountValue}
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
                                <span className="font-medium text-sm">{termValue} month(s)</span>
                            </div>
                            <Input
                                className="cursor-pointer bg-background"
                                id="range-slider-term"
                                max={constraints.maximumTerm}
                                min={constraints.minimumTerm}
                                onChange={e => setValue("term", Number(e.target.value))}
                                type="range"
                                value={termValue}
                            />
                            {errors.term && <p className="text-red-500 text-xs mt-1">{errors.term.message}</p>}
                            <div className="flex justify-between text-muted-foreground text-xs">
                                <span>{constraints.minimumTerm}</span>
                                <span>{constraints.maximumTerm}</span>
                            </div>
                        </div>
                        <Separator className="my-5" />
                        <div className="flex gap-4 mt-5">
                            <div className="w-30 flex-none">
                                <Label className="text-sm font-medium" htmlFor="name">Title</Label>
                                <Select value={title} onValueChange={(t) => setValue("title", t)}>
                                    <SelectTrigger className="w-full mt-2">
                                        <SelectValue placeholder="Mr." />
                                    </SelectTrigger>
                                    <SelectContent>
                                        <SelectItem value="Mr.">Mr.</SelectItem>
                                        <SelectItem value="Mrs.">Mrs.</SelectItem>
                                        <SelectItem value="Ms.">Ms.</SelectItem>
                                    </SelectContent>
                                </Select>
                            </div>
                            <div className="w-60 flex-1">
                                <Label className="text-sm font-medium" htmlFor="firstName">First Name</Label>
                                <Input {...register("firstName")}
                                    id="firstName"
                                    name="firstName"
                                    placeholder="John"
                                    className="mt-2"
                                />
                                {errors.firstName && <p className="text-red-500 text-xs mt-1">{errors.firstName.message}</p>}
                            </div>
                            <div className="w-60 flex-1">
                                <Label className="text-sm font-medium" htmlFor="lastName">Last Name</Label>
                                <Input {...register("lastName")}
                                    id="lastName"
                                    name="lastName"
                                    placeholder="Doe"
                                    className="mt-2"
                                />
                                {errors.lastName && <p className="text-red-500 text-xs mt-1">{errors.lastName.message}</p>}
                            </div>
                        </div>
                        <div className="flex gap-4 mt-10">
                            <div className="flex-1">
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
                            <div className="flex-1">
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
                            <div className="flex-1">
                                <Field className="mx-auto gap-2">
                                    <FieldLabel htmlFor="date">Date of birth</FieldLabel>
                                    <Popover>
                                        <PopoverTrigger asChild>
                                            <Button variant="outline" id="date" className="justify-start font-normal">{dateOfBirth ? dateOfBirth.toLocaleDateString() : "Select date"}</Button>
                                        </PopoverTrigger>
                                        <PopoverContent className="w-auto overflow-hidden p-0" align="start">
                                            <Calendar
                                                mode="single"
                                                selected={dateOfBirth}
                                                captionLayout="dropdown"
                                                onSelect={(date) => date && setValue("dateOfBirth", date)}
                                            />
                                        </PopoverContent>
                                    </Popover>
                                </Field>
                                {errors.dateOfBirth && <p className="text-red-500 text-xs mt-1">{errors.dateOfBirth.message}</p>}
                            </div>
                        </div>

                    </CardContent>
                    <CardFooter className="flex-col gap-2 mt-5">
                        <Button className="w-70" type="submit">Calculate Quote</Button>
                        <Label className="text-xs text-muted-foreground mt-1">Quote does not affect your credit score</Label>
                    </CardFooter>
                </Card>
            </form>
        </div>
    )
}