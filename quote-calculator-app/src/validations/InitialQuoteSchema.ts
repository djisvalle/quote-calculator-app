import { z } from "zod";

export const InitialQuoteSchema = z.object({
    applicantId: z.number(),
    loanApplicationPublicId: z.string(),
    productType: z.string().min(1, "Please select a product type"),
    amount: z.number().min(1, "Amount is required"),
    term: z.number().min(1, "Term is required"),
    title: z.string().min(1, "Title is required"),
    firstName: z.string().min(2, "First name must be at least 2 characters"),
    lastName: z.string().min(2, "Last name must be at least 2 characters"),
    email: z.email("Invalid email address"),
    mobileNumber: z.string().regex(/^[0-9]{10,12}$/, "Invalid mobile number"),
    dateOfBirth: z.date().refine((date) => {
        const age = new Date().getFullYear() - date.getFullYear();
        return age >= 18;
    }, "You must be at least 18 years old"),
});