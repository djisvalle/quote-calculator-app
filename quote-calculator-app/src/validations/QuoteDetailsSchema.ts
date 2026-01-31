import { z } from "zod";

export const QuoteDetailsSchema = z.object({
    loanApplicationPublicId: z.string(),
    applicantId: z.number(),
    title: z.string().min(1, "Title is required"),
    firstName: z.string().min(2, "First name must be at least 2 characters"),
    lastName: z.string().min(2, "Last name must be at least 2 characters"),
    email: z.email("Invalid email address"),
    mobileNumber: z.string().regex(/^[0-9]{10,12}$/, "Invalid mobile number"),
    amount: z.number().min(1, "Amount is required"),
    term: z.number().min(1, "Term is required"),
    interestFee: z.number(),
    establishmentFee: z.number(),
    weeklyRepayment: z.number().min(1, "Weekly repayment is required"),
    monthlyRepayment: z.number().min(1, "Monthly repayment is required"),
    totalRepayment: z.number().min(1, "Total repayment is required"),
});